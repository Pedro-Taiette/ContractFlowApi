using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractsMvc.Data.Configurations;

/// <summary>
/// Configuration for the Obligation entity. It defines property lengths,
/// relationships to contract, deliverables and non‑compliances, and
/// applies a global query filter for soft deletion.
/// </summary>
public class ObligationConfiguration : IEntityTypeConfiguration<Obligation>
{
    public void Configure(EntityTypeBuilder<Obligation> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.ClauseRef).IsRequired().HasMaxLength(50);
        b.Property(x => x.Description).IsRequired().HasMaxLength(2000);
        b.Property(x => x.Status).IsRequired().HasMaxLength(30);

        b.HasOne(x => x.Contract)
            .WithMany(x => x.Obligations)
            .HasForeignKey(x => x.ContractId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(x => x.Deliverables)
            .WithOne(x => x.Obligation)
            .HasForeignKey(x => x.ObligationId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(x => x.NonCompliances)
            .WithOne(x => x.Obligation)
            .HasForeignKey(x => x.ObligationId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}