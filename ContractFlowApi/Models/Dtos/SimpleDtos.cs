using System;

namespace ContractsMvc.Models.Dtos
{
    /// <summary>
    /// A simple representation of a supplier used by API responses.
    /// </summary>
    public sealed class SupplierDto
    {
        public Guid Id { get; set; }
        public string CorporateName { get; set; } = null!;
        public string Cnpj { get; set; } = null!;
        public bool Active { get; set; }
    }

    /// <summary>
    /// Request payload for creating a supplier.
    /// </summary>
    public sealed class CreateSupplierRequest
    {
        public string CorporateName { get; init; } = null!;
        public string Cnpj { get; init; } = null!;
        public bool Active { get; init; } = true;
    }

    /// <summary>
    /// Request payload for updating a supplier.
    /// </summary>
    public sealed class UpdateSupplierRequest
    {
        public string CorporateName { get; init; } = null!;
        public string Cnpj { get; init; } = null!;
        public bool Active { get; init; } = true;
    }

    /// <summary>
    /// A simple representation of an organisational unit.
    /// </summary>
    public sealed class OrgUnitDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
    }

    /// <summary>
    /// Request payload for creating an organisational unit.
    /// </summary>
    public sealed class CreateOrgUnitRequest
    {
        public string Name { get; init; } = null!;
        public string? Code { get; init; }
    }

    /// <summary>
    /// Request payload for updating an organisational unit.
    /// </summary>
    public sealed class UpdateOrgUnitRequest
    {
        public string Name { get; init; } = null!;
        public string? Code { get; init; }
    }

    /// <summary>
    /// A simple representation of an obligation. Includes only the main
    /// fields and the parent contract identifier.
    /// </summary>
    public sealed class ObligationSummaryDto
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public string ClauseRef { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = null!;
    }

    /// <summary>
    /// Request payload for creating an obligation within a contract.
    /// </summary>
    public sealed class CreateObligationRequest
    {
        public string ClauseRef { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime? DueDate { get; init; }
        public string? Status { get; init; }
    }

    /// <summary>
    /// Request payload for updating an obligation.
    /// </summary>
    public sealed class UpdateObligationRequest
    {
        public string ClauseRef { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime? DueDate { get; init; }
        public string Status { get; init; } = null!;
    }

    /// <summary>
    /// DTO for representing an inspection.
    /// </summary>
    public sealed class InspectionDto
    {
        public Guid Id { get; set; }
        public Guid DeliverableId { get; set; }
        public DateTime Date { get; set; }
        public string Inspector { get; set; } = null!;
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Request payload for creating an inspection.
    /// </summary>
    public sealed class CreateInspectionRequest
    {
        public DateTime Date { get; init; }
        public string Inspector { get; init; } = null!;
        public string? Notes { get; init; }
    }

    /// <summary>
    /// Request payload for updating an inspection.
    /// </summary>
    public sealed class UpdateInspectionRequest
    {
        public DateTime Date { get; init; }
        public string Inspector { get; init; } = null!;
        public string? Notes { get; init; }
    }

    /// <summary>
    /// DTO for representing an evidence item.
    /// </summary>
    public sealed class EvidenceDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string StoragePath { get; set; } = null!;
        public string? Notes { get; set; }
        public Guid? DeliverableId { get; set; }
        public Guid? InspectionId { get; set; }
    }

    /// <summary>
    /// DTO for representing an attachment.
    /// </summary>
    public sealed class AttachmentDto
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public string FileName { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string StoragePath { get; set; } = null!;
    }

    /// <summary>
    /// DTO for reporting deliveries grouped by supplier or org unit.
    /// </summary>
    public sealed class DeliveryReportDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int TotalDeliverables { get; set; }
        public int DeliveredCount { get; set; }
        public int PendingCount { get; set; }
    }

    /// <summary>
    /// DTO for reporting applied penalties.
    /// </summary>
    public sealed class PenaltyReportDto
    {
        public Guid PenaltyId { get; set; }
        public Guid NonComplianceId { get; set; }
        public Guid ContractId { get; set; }
        public string Reason { get; set; } = null!;
        public string Severity { get; set; } = null!;
        public DateTime RegisteredAt { get; set; }
        public string Type { get; set; } = null!;
        public string? LegalBasis { get; set; }
        public decimal? Amount { get; set; }
    }
}