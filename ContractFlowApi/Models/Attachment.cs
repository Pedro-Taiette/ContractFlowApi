using System;

namespace ContractsMvc.Models;

/// <summary>
/// Represents a file attached to a contract. Attachments store metadata
/// about the file but not the file contents themselves. The actual
/// storage location is defined by the StoragePath property. An attachment
/// belongs to exactly one contract.
/// </summary>
public class Attachment : Entity
{
    public Guid ContractId { get; set; }
    public Contract Contract { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public string StoragePath { get; set; } = null!;
}