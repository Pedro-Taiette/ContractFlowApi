using System;
using System.Collections.Generic;

namespace ContractsMvc.Models.Dtos;

/// <summary>
/// Detailed view of a contract, including its obligations and deliverables.
/// This DTO is returned by the GET /api/contracts/{id} endpoint. Nested
/// objects are flattened into DTOs to avoid exposing EF entities directly.
/// </summary>
public class ContractDetailsDto
{
    public Guid Id { get; set; }
    public string OfficialNumber { get; set; } = null!;
    public string? AdministrativeProcess { get; set; }
    public string Type { get; set; } = null!;
    public string Modality { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime TermStart { get; set; }
    public DateTime TermEnd { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = null!;
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; } = null!;
    public string SupplierCnpj { get; set; } = null!;
    public Guid OrgUnitId { get; set; }
    public string OrgUnitName { get; set; } = null!;
    public string? OrgUnitCode { get; set; }
    public IList<ObligationDto> Obligations { get; set; } = new List<ObligationDto>();
}

public class ObligationDto
{
    public Guid Id { get; set; }
    public string ClauseRef { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public string Status { get; set; } = null!;
    public IList<DeliverableDto> Deliverables { get; set; } = new List<DeliverableDto>();
    public IList<NonComplianceDto> NonCompliances { get; set; } = new List<NonComplianceDto>();
}

public class DeliverableDto
{
    public Guid Id { get; set; }
    public DateTime ExpectedDate { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = null!;
    public DateTime? DeliveredAt { get; set; }
}

public class NonComplianceDto
{
    public Guid Id { get; set; }
    public string Reason { get; set; } = null!;
    public string Severity { get; set; } = null!;
    public DateTime RegisteredAt { get; set; }
    public PenaltyDto? Penalty { get; set; }
}

public class PenaltyDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public string? LegalBasis { get; set; }
    public decimal? Amount { get; set; }
}