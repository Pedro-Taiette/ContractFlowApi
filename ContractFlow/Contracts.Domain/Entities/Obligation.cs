using Contracts.Domain.Abstractions;

namespace Contracts.Domain.Entities;
public class Obligation : Entity
{
    public Guid ContractId { get; private set; }
    public Contract Contract { get; private set; } = null!;
    public string ClauseRef { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime? DueDate { get; private set; }
    public string Status { get; private set; } = "Pending";

    private readonly List<Deliverable> _deliverables = new();
    public IReadOnlyCollection<Deliverable> Deliverables => _deliverables.AsReadOnly();

    private readonly List<NonCompliance> _nonCompliances = new();
    public IReadOnlyCollection<NonCompliance> NonCompliances => _nonCompliances.AsReadOnly();

    private Obligation() { }
    public Obligation(Contract contract, string clauseRef, string description, DateTime? dueDate)
    {
        Contract = contract;
        ContractId = contract.Id;
        ClauseRef = clauseRef;
        Description = description;
        DueDate = dueDate;
    }

    public Deliverable AddDeliverable(DateTime expectedDate, decimal quantity, string unit)
    {
        var d = new Deliverable(this, expectedDate, quantity, unit);
        _deliverables.Add(d);
        return d;
    }

    public NonCompliance RegisterNonCompliance(string reason, string severity)
    {
        var nc = new NonCompliance(this, reason, severity);
        _nonCompliances.Add(nc);
        return nc;
    }
}
