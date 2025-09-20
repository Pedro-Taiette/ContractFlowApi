using Contracts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contracts.Infrastructure.Persistence.Configurations;

public class ObligationConfiguration : IEntityTypeConfiguration<Obligation>
{
    public void Configure(EntityTypeBuilder<Obligation> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.ClauseRef).IsRequired().HasMaxLength(50);
        b.Property(x => x.Description).IsRequired().HasMaxLength(2000);
        b.Property(x => x.Status).IsRequired().HasMaxLength(30);
        b.HasMany(x => x.Deliverables).WithOne(x => x.Obligation).HasForeignKey(x => x.ObligationId);
        b.HasMany(x => x.NonCompliances).WithOne(x => x.Obligation).HasForeignKey(x => x.ObligationId);
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}
