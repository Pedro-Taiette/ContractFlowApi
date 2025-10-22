using System;

namespace ContractsMvc.Models;

/// <summary>
/// Represents a penalty applied as a consequence of a non‑compliance. A
/// penalty includes a type (e.g. warning, fine), an optional legal basis
/// and a monetary amount when applicable. Penalties are always tied to a
/// specific non‑compliance and cannot exist independently.
/// </summary>
public class Penalty : Entity
{
    public Guid NonComplianceId { get; set; }
    public NonCompliance NonCompliance { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? LegalBasis { get; set; }
    public decimal? Amount { get; set; }
}