using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractsMvc.Data.Configurations;

/// <summary>
/// Configuration for penalties. Ensures required properties and configures
/// one‑to‑one relationship with NonCompliance. Applies soft delete filter.
/// </summary>
public class PenaltyConfiguration : IEntityTypeConfiguration<Penalty>
{
    public void Configure(EntityTypeBuilder<Penalty> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Type).IsRequired().HasMaxLength(60);
        b.Property(x => x.LegalBasis).HasMaxLength(500);
        b.Property(x => x.Amount).HasPrecision(18, 2);

        b.HasOne(x => x.NonCompliance)
            .WithOne(x => x.Penalty)
            .HasForeignKey<Penalty>(x => x.NonComplianceId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.NonComplianceId).IsUnique();

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}