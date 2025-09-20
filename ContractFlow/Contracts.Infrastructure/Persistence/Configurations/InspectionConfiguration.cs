using Contracts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contracts.Infrastructure.Persistence.Configurations;

public class InspectionConfiguration : IEntityTypeConfiguration<Inspection>
{
    public void Configure(EntityTypeBuilder<Inspection> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Date).IsRequired();
        b.Property(x => x.Inspector).IsRequired().HasMaxLength(120);
        b.Property(x => x.Notes).HasMaxLength(2000);

        b.HasOne(x => x.Deliverable).WithMany(x => x.Inspections).HasForeignKey(x => x.DeliverableId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Evidences).WithOne(x => x.Inspection).HasForeignKey(x => x.InspectionId).OnDelete(DeleteBehavior.NoAction);

        b.HasIndex(x => new { x.DeliverableId, x.Date });
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}
