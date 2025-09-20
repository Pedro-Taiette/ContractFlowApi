using Contracts.Domain.Abstractions;

namespace Contracts.Domain.Entities;
public class NonCompliance : Entity
{
    public Guid ObligationId { get; private set; }
    public Obligation Obligation { get; private set; } = null!;
    public string Reason { get; private set; } = null!;
    public string Severity { get; private set; } = null!;
    public DateTime RegisteredAt { get; private set; } = DateTime.UtcNow;

    public Penalty? Penalty { get; private set; }

    private NonCompliance() { }
    public NonCompliance(Obligation obligation, string reason, string severity)
    {
        Obligation = obligation;
        ObligationId = obligation.Id;
        Reason = reason;
        Severity = severity;
    }

    public void ApplyPenalty(string type, string? legalBasis, decimal? amount)
    {
        if (Penalty != null) throw new InvalidOperationException("Penalty already applied.");
        Penalty = new Penalty(this, type, legalBasis, amount);
    }
}
