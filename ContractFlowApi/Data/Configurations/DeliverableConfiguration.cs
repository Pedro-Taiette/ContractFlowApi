using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractsMvc.Data.Configurations;

/// <summary>
/// Configuração da entidade Deliverable. Define campos obrigatórios e relações.
/// </summary>
public class DeliverableConfiguration : IEntityTypeConfiguration<Deliverable>
{
    public void Configure(EntityTypeBuilder<Deliverable> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.ExpectedDate).IsRequired();
        b.Property(x => x.Quantity).HasPrecision(18, 2).IsRequired();
        b.Property(x => x.Unit).IsRequired().HasMaxLength(20);
        b.Property(x => x.DeliveredAt);

        b.HasOne(x => x.Obligation).WithMany(x => x.Deliverables).HasForeignKey(x => x.ObligationId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Inspections).WithOne(x => x.Deliverable).HasForeignKey(x => x.DeliverableId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Evidences).WithOne(x => x.Deliverable).HasForeignKey(x => x.DeliverableId).OnDelete(DeleteBehavior.NoAction);

        b.HasIndex(x => new { x.ObligationId, x.ExpectedDate });

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}