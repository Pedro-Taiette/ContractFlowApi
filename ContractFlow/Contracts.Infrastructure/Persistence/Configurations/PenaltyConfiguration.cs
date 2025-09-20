using Contracts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contracts.Infrastructure.Persistence.Configurations;

public class PenaltyConfiguration : IEntityTypeConfiguration<Penalty>
{
    public void Configure(EntityTypeBuilder<Penalty> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Type).IsRequired().HasMaxLength(60);
        b.Property(x => x.LegalBasis).HasMaxLength(500);
        b.Property(x => x.Amount).HasPrecision(18,2);

        b.HasOne(x => x.NonCompliance).WithOne(x => x.Penalty).HasForeignKey<Penalty>(x => x.NonComplianceId).OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.NonComplianceId).IsUnique();
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}
