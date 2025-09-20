using Contracts.Domain.Enums;

namespace Contracts.Application.Contracts;

public sealed class CreateContractRequest
{
    public string OfficialNumber { get; set; } = null!;
    public string? AdministrativeProcess { get; set; }
    public Guid SupplierId { get; set; }
    public Guid OrgUnitId { get; set; }
    public ContractType Type { get; set; }                  
    public ContractModality Modality { get; set; }          
    public DateTime TermStart { get; set; }
    public DateTime TermEnd { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "BRL";

    public List<ObligationInput>? Obligations { get; set; }

    public sealed class ObligationInput
    {
        public string ClauseRef { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? DueDate { get; set; }
        public List<DeliverableInput>? Deliverables { get; set; }
    }

    public sealed class DeliverableInput
    {
        public DateTime ExpectedDate { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;
    }
}
