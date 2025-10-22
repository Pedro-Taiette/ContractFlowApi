using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractsMvc.Data.Configurations;

/// <summary>
/// Configuration for suppliers. Adds required fields, unique index on CNPJ
/// and applies soft delete filter.
/// </summary>
public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.CorporateName).IsRequired().HasMaxLength(200);
        b.Property(x => x.Cnpj).IsRequired().HasMaxLength(18);
        b.HasIndex(x => x.Cnpj).IsUnique();
        b.Property(x => x.Active).IsRequired();
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}