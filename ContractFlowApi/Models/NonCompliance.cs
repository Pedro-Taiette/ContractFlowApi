using System;

namespace ContractsMvc.Models;

/// <summary>
/// Represents a non‑compliance event tied to an obligation. A non‑compliance
/// includes a reason, a severity classification and the date it was
/// registered. Optionally, a penalty can be linked to the record.
/// </summary>
public class NonCompliance : Entity
{
    public Guid ObligationId { get; set; }
    public Obligation Obligation { get; set; } = null!;
    public string Reason { get; set; } = null!;
    public string Severity { get; set; } = null!;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public Penalty? Penalty { get; set; }
}