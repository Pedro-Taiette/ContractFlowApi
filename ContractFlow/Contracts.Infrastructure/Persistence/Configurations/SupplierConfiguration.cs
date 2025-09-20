using Contracts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contracts.Infrastructure.Persistence.Configurations;

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
