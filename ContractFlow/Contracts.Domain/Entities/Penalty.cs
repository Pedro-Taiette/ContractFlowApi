using Contracts.Domain.Abstractions;

namespace Contracts.Domain.Entities;
public class Penalty : Entity
{
    public Guid NonComplianceId { get; private set; }
    public NonCompliance NonCompliance { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public string? LegalBasis { get; private set; }
    public decimal? Amount { get; private set; }

    private Penalty() { }
    public Penalty(NonCompliance nonCompliance, string type, string? legalBasis, decimal? amount)
    {
        NonCompliance = nonCompliance;
        NonComplianceId = nonCompliance.Id;
        Type = type;
        LegalBasis = legalBasis;
        Amount = amount;
    }
}
