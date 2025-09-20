using Contracts.Domain.Abstractions;

namespace Contracts.Domain.Entities;
public class Inspection : Entity
{
    public Guid DeliverableId { get; private set; }
    public Deliverable Deliverable { get; private set; } = null!;
    public DateTime Date { get; private set; }
    public string Inspector { get; private set; } = null!;
    public string? Notes { get; private set; }

    private readonly List<Evidence> _evidences = new();
    public IReadOnlyCollection<Evidence> Evidences => _evidences.AsReadOnly();

    private Inspection() { }
    public Inspection(Deliverable deliverable, DateTime date, string inspector, string? notes)
    {
        Deliverable = deliverable;
        DeliverableId = deliverable.Id;
        Date = date;
        Inspector = inspector;
        Notes = notes;
    }

    public Evidence AddEvidence(string fileName, string mimeType, string path, string? notes = null)
    {
        var e = new Evidence(fileName, mimeType, path, notes) { Inspection = this, InspectionId = this.Id };
        _evidences.Add(e);
        return e;
    }
}
