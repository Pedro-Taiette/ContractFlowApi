using System;
using System.Collections.Generic;

namespace ContractsMvc.Models;

/// <summary>
/// Represents a specific delivery expected for an obligation. A deliverable
/// tracks the expected date, quantity and unit, and records when it was
/// delivered. Inspections and evidence can be associated with a deliverable.
/// </summary>
public class Deliverable : Entity
{
    public Guid ObligationId { get; set; }
    public Obligation Obligation { get; set; } = null!;
    public DateTime ExpectedDate { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = null!;
    public DateTime? DeliveredAt { get; set; }
    public List<Inspection> Inspections { get; set; } = new();
    public List<Evidence> Evidences { get; set; } = new();
}