using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractsMvc.Data.Configurations
{
    /// <summary>
    /// EF Core configuration for the Alert entity. Defines key,
    /// relationships and soft delete query filter.
    /// </summary>
    public class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Message).IsRequired().HasMaxLength(500);
            b.Property(x => x.TargetDate).IsRequired();

            b.HasOne(x => x.Contract)
                .WithMany()
                .HasForeignKey(x => x.ContractId)
                .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(x => x.Deliverable)
                .WithMany()
                .HasForeignKey(x => x.DeliverableId)
                .OnDelete(DeleteBehavior.NoAction);

            b.HasIndex(x => x.ContractId);
            b.HasIndex(x => x.DeliverableId);
            b.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}