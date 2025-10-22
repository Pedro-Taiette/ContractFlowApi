using System;

namespace ContractsMvc.Models;

/// <summary>
/// Stores files or notes gathered during inspections or deliveries. Evidence
/// can belong either to a deliverable or to an inspection, and includes
/// optional notes for contextual information.
/// </summary>
public class Evidence : Entity
{
    public string FileName { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public string StoragePath { get; set; } = null!;
    public string? Notes { get; set; }
    public Guid? DeliverableId { get; set; }
    public Deliverable? Deliverable { get; set; }
    public Guid? InspectionId { get; set; }
    public Inspection? Inspection { get; set; }
}