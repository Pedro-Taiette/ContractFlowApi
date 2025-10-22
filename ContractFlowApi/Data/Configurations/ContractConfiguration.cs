using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractsMvc.Data.Configurations;

/// <summary>
/// Configurações de mapeamento para a entidade Contract.
/// Converte enums para strings, define propriedades compostas e relacionamentos.
/// </summary>
public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.OfficialNumber).IsRequired().HasMaxLength(100);
        b.HasIndex(x => x.OfficialNumber).IsUnique();

        b.Property(x => x.Type).HasConversion<string>().HasMaxLength(32).IsUnicode(false).IsRequired();
        b.Property(x => x.Modality).HasConversion<string>().HasMaxLength(32).IsUnicode(false).IsRequired();
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(32).IsUnicode(false).IsRequired();

        b.OwnsOne(x => x.Term, ov =>
        {
            ov.Property(p => p.Start).HasColumnName("TermStart").IsRequired();
            ov.Property(p => p.End).HasColumnName("TermEnd").IsRequired();
        });

        b.OwnsOne(x => x.TotalValue, ov =>
        {
            ov.Property(p => p.Amount).HasColumnName("TotalAmount").HasPrecision(18, 2);
            ov.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
        });

        b.HasOne(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierId);
        b.HasOne(x => x.OrgUnit).WithMany().HasForeignKey(x => x.OrgUnitId);

        b.HasMany(x => x.Obligations).WithOne(x => x.Contract).HasForeignKey(x => x.ContractId);
        b.HasMany(x => x.Attachments).WithOne(x => x.Contract).HasForeignKey(x => x.ContractId);

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}