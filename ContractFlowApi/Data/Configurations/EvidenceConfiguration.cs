using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractsMvc.Data.Configurations;

/// <summary>
/// EF Core configuration for the Evidence entity. It defines property lengths,
/// relationships to Deliverable and Inspection and adds a global query filter
/// for soft deletion. The cascade behaviours mirror those in the original
/// infrastructure layer.
/// </summary>
public class EvidenceConfiguration : IEntityTypeConfiguration<Evidence>
{
    public void Configure(EntityTypeBuilder<Evidence> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.FileName).IsRequired().HasMaxLength(255);
        b.Property(x => x.MimeType).IsRequired().HasMaxLength(100);
        b.Property(x => x.StoragePath).IsRequired().HasMaxLength(500);
        b.Property(x => x.Notes).HasMaxLength(1000);

        b.HasOne(x => x.Deliverable)
            .WithMany(x => x.Evidences)
            .HasForeignKey(x => x.DeliverableId)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasOne(x => x.Inspection)
            .WithMany(x => x.Evidences)
            .HasForeignKey(x => x.InspectionId)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasIndex(x => x.DeliverableId);
        b.HasIndex(x => x.InspectionId);

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}