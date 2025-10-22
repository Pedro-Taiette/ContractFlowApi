using System;

namespace ContractsMvc.Models.Dtos;

/// <summary>
/// Represents a deliverable that is due or overdue. Used by the reports
/// endpoint to surface deliveries that require attention. Includes the
/// associated obligation and contract identifiers to assist clients in
/// locating the record.
/// </summary>
public class DueDeliverableDto
{
    public Guid DeliverableId { get; set; }
    public DateTime ExpectedDate { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = null!;
    public Guid ObligationId { get; set; }
    public Guid ContractId { get; set; }
}