using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractsMvc.Data.Configurations;

/// <summary>
/// Configuration for NonCompliance records. Defines required fields,
/// indexes and the relationship to an obligation. The penalty relationship
/// is configured in PenaltyConfiguration.
/// </summary>
public class NonComplianceConfiguration : IEntityTypeConfiguration<NonCompliance>
{
    public void Configure(EntityTypeBuilder<NonCompliance> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Reason).IsRequired().HasMaxLength(1000);
        b.Property(x => x.Severity).IsRequired().HasMaxLength(20);
        b.Property(x => x.RegisteredAt).IsRequired();

        b.HasOne(x => x.Obligation)
            .WithMany(x => x.NonCompliances)
            .HasForeignKey(x => x.ObligationId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => new { x.ObligationId, x.RegisteredAt });

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}