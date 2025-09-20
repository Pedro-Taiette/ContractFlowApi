using Contracts.Domain.Abstractions;

namespace Contracts.Domain.Entities;
public class Deliverable : Entity
{
    public Guid ObligationId { get; private set; }
    public Obligation Obligation { get; private set; } = null!;
    public DateTime ExpectedDate { get; private set; }
    public decimal Quantity { get; private set; }
    public string Unit { get; private set; } = null!;
    public DateTime? DeliveredAt { get; private set; }

    private readonly List<Inspection> _inspections = new();
    public IReadOnlyCollection<Inspection> Inspections => _inspections.AsReadOnly();

    private readonly List<Evidence> _evidences = new();
    public IReadOnlyCollection<Evidence> Evidences => _evidences.AsReadOnly();

    private Deliverable() { }
    public Deliverable(Obligation obligation, DateTime expectedDate, decimal quantity, string unit)
    {
        Obligation = obligation;
        ObligationId = obligation.Id;
        ExpectedDate = expectedDate;
        Quantity = quantity;
        Unit = unit;
    }

    public void MarkDelivered(DateTime when) { DeliveredAt = when; MarkUpdated(); }

    public Inspection AddInspection(DateTime date, string inspector, string? notes = null)
    {
        var i = new Inspection(this, date, inspector, notes);
        _inspections.Add(i);
        return i;
    }

    public Evidence AddEvidence(string fileName, string mimeType, string path, string? notes = null)
    {
        var e = new Evidence(fileName, mimeType, path, notes) { Deliverable = this, DeliverableId = this.Id };
        _evidences.Add(e);
        return e;
    }
}
