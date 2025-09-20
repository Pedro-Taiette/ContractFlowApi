using Contracts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contracts.Infrastructure.Persistence.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.FileName).IsRequired().HasMaxLength(255);
        b.Property(x => x.MimeType).IsRequired().HasMaxLength(100);
        b.Property(x => x.StoragePath).IsRequired().HasMaxLength(500);

        b.HasOne(x => x.Contract).WithMany(x => x.Attachments).HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.ContractId);
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}
