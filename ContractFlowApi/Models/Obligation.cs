using System;
using System.Collections.Generic;

namespace ContractsMvc.Models;

/// <summary>
/// Represents a contractual obligation. Each obligation belongs to a
/// contract and can have multiple deliverables and non‑compliance records.
/// The clause reference and description are human readable identifiers for
/// the obligation. The status field is a free form string used to track
/// progress or completion.
/// </summary>
public class Obligation : Entity
{
    public Guid ContractId { get; set; }
    public Contract Contract { get; set; } = null!;
    public string ClauseRef { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public string Status { get; set; } = "Pending";
    public List<Deliverable> Deliverables { get; set; } = new();
    public List<NonCompliance> NonCompliances { get; set; } = new();
}