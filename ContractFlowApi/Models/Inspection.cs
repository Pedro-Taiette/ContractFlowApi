using System;
using System.Collections.Generic;

namespace ContractsMvc.Models;

/// <summary>
/// Represents an inspection of a deliverable. An inspection records the
/// date, the inspector's name and optional notes, and can gather multiple
/// pieces of evidence such as photos or documents.
/// </summary>
public class Inspection : Entity
{
    public Guid DeliverableId { get; set; }
    public Deliverable Deliverable { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Inspector { get; set; } = null!;
    public string? Notes { get; set; }
    public List<Evidence> Evidences { get; set; } = new();
}