using Contracts.Domain.Abstractions;
using Contracts.Domain.Enums;
using Contracts.Domain.ValueObjects;

namespace Contracts.Domain.Entities;

public class Contract : AggregateRoot
{
    public string OfficialNumber { get; private set; } = null!;
    public string? AdministrativeProcess { get; private set; }

    public Guid SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = null!;
    public Guid OrgUnitId { get; private set; }
    public OrgUnit OrgUnit { get; private set; } = null!;

    public ContractType Type { get; private set; }
    public ContractModality Modality { get; private set; }
    public ContractStatus Status { get; private set; } = ContractStatus.Active;

    public Period Term { get; private set; } = null!;
    public Money TotalValue { get; private set; } = null!;

    private readonly List<Obligation> _obligations = new();
    public IReadOnlyCollection<Obligation> Obligations => _obligations.AsReadOnly();

    private readonly List<Attachment> _attachments = new();
    public IReadOnlyCollection<Attachment> Attachments => _attachments.AsReadOnly();

    private Contract() { } // EF

    public Contract(
        string officialNumber,
        ContractType type,
        ContractModality modality,
        Guid supplierId,
        Guid orgUnitId,
        Period term,
        Money totalValue,
        string? administrativeProcess = null)
    {
        OfficialNumber = officialNumber;
        Type = type;
        Modality = modality;
        SupplierId = supplierId;
        OrgUnitId = orgUnitId;
        AdministrativeProcess = administrativeProcess;
        Term = term;
        TotalValue = totalValue;
    }

    public void SetStatus(ContractStatus status) { Status = status; MarkUpdated(); }

    public Obligation AddObligation(string clauseRef, string description, DateTime? dueDate = null)
    {
        var o = new Obligation(this, clauseRef, description, dueDate);
        _obligations.Add(o);
        MarkUpdated();
        return o;
    }

    public Attachment AddAttachment(string fileName, string mimeType, string storagePath)
    {
        var a = new Attachment(this, fileName, mimeType, storagePath);
        _attachments.Add(a);
        MarkUpdated();
        return a;
    }

    public void Close() { Status = ContractStatus.Closed; MarkUpdated(); }
    public void Activate() { Status = ContractStatus.Active; MarkUpdated(); }
    public void Suspend() { Status = ContractStatus.Suspended; MarkUpdated(); }
    public void Terminate() { Status = ContractStatus.Terminated; MarkUpdated(); }
    public void Cancel() { Status = ContractStatus.Cancelled; MarkUpdated(); }
}
