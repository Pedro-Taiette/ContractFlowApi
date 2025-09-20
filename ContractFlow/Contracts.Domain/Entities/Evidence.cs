using Contracts.Domain.Abstractions;

namespace Contracts.Domain.Entities;
public class Evidence : Entity
{
    public string FileName { get; private set; } = null!;
    public string MimeType { get; private set; } = null!;
    public string StoragePath { get; private set; } = null!;
    public string? Notes { get; private set; }

    public Guid? DeliverableId { get; set; }
    public Deliverable? Deliverable { get; set; }
    public Guid? InspectionId { get; set; }
    public Inspection? Inspection { get; set; }

    private Evidence() { }
    public Evidence(string fileName, string mimeType, string storagePath, string? notes)
    {
        FileName = fileName;
        MimeType = mimeType;
        StoragePath = storagePath;
        Notes = notes;
    }
}
