using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractsMvc.Data.Configurations;

/// <summary>
/// Configuration for organisational units. Ensures required fields and a
/// unique code when provided. Applies soft delete filter.
/// </summary>
public class OrgUnitConfiguration : IEntityTypeConfiguration<OrgUnit>
{
    public void Configure(EntityTypeBuilder<OrgUnit> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).IsRequired().HasMaxLength(150);
        b.Property(x => x.Code).HasMaxLength(30);
        b.HasIndex(x => x.Code)
            .IsUnique()
            .HasFilter("[Code] IS NOT NULL");

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}