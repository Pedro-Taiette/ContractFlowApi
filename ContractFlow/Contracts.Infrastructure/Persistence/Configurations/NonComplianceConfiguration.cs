using Contracts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contracts.Infrastructure.Persistence.Configurations;

public class NonComplianceConfiguration : IEntityTypeConfiguration<NonCompliance>
{
    public void Configure(EntityTypeBuilder<NonCompliance> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Reason).IsRequired().HasMaxLength(1000);
        b.Property(x => x.Severity).IsRequired().HasMaxLength(20);
        b.Property(x => x.RegisteredAt).IsRequired();

        b.HasOne(x => x.Obligation).WithMany(x => x.NonCompliances).HasForeignKey(x => x.ObligationId).OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => new { x.ObligationId, x.RegisteredAt });
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}
