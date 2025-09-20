using Contracts.Domain.Enums;

namespace Contracts.Application.Contracts;

public sealed class ContractDetailsDto
{
    public Guid Id { get; init; }
    public string OfficialNumber { get; init; } = null!;
    public string? AdministrativeProcess { get; init; }
    public ContractType Type { get; init; }                 
    public ContractModality Modality { get; init; }         
    public ContractStatus Status { get; init; }             
    public DateTime TermStart { get; init; }
    public DateTime TermEnd { get; init; }
    public decimal TotalAmount { get; init; }
    public string Currency { get; init; } = "BRL";

    public PartyDto Supplier { get; init; } = null!;
    public OrgUnitDto OrgUnit { get; init; } = null!;

    public List<AttachmentDto> Attachments { get; init; } = [];
    public List<ObligationDto> Obligations { get; init; } = [];

    public sealed class PartyDto { public Guid Id { get; init; } public string Name { get; init; } = null!; public string Cnpj { get; init; } = null!; }
    public sealed class OrgUnitDto { public Guid Id { get; init; } public string Name { get; init; } = null!; public string? Code { get; init; } }
    public sealed class AttachmentDto { public Guid Id { get; init; } public string FileName { get; init; } = null!; public string MimeType { get; init; } = null!; public string StoragePath { get; init; } = null!; }

    public sealed class ObligationDto
    {
        public Guid Id { get; init; }
        public string ClauseRef { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime? DueDate { get; init; }
        public string Status { get; init; } = null!;
        public List<DeliverableDto> Deliverables { get; init; } = [];
        public List<NonComplianceDto> NonCompliances { get; init; } = [];
    }

    public sealed class DeliverableDto
    {
        public Guid Id { get; init; }
        public DateTime ExpectedDate { get; init; }
        public decimal Quantity { get; init; }
        public string Unit { get; init; } = null!;
        public DateTime? DeliveredAt { get; init; }
        public List<InspectionDto> Inspections { get; init; } = [];
        public List<EvidenceDto> Evidences { get; init; } = [];
    }

    public sealed class InspectionDto
    {
        public Guid Id { get; init; }
        public DateTime Date { get; init; }
        public string Inspector { get; init; } = null!;
        public string? Notes { get; init; }
        public List<EvidenceDto> Evidences { get; init; } = [];
    }

    public sealed class EvidenceDto
    {
        public Guid Id { get; init; }
        public string FileName { get; init; } = null!;
        public string MimeType { get; init; } = null!;
        public string StoragePath { get; init; } = null!;
        public string? Notes { get; init; }
    }

    public sealed class NonComplianceDto
    {
        public Guid Id { get; init; }
        public string Reason { get; init; } = null!;
        public string Severity { get; init; } = null!;
        public DateTime RegisteredAt { get; init; }
        public PenaltyDto? Penalty { get; init; }
    }

    public sealed class PenaltyDto
    {
        public Guid Id { get; init; }
        public string Type { get; init; } = null!;
        public string? LegalBasis { get; init; }
        public decimal? Amount { get; init; }
    }
}
