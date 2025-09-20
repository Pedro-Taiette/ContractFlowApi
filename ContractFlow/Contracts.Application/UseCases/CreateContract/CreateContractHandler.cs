using Contracts.Application.Abstractions;
using Contracts.Application.Contracts;
using Contracts.Domain.Entities;
using Contracts.Domain.ValueObjects;

namespace Contracts.Application.UseCases.CreateContract;

public class CreateContractHandler
{
    private readonly IContractsRepository _repo;
    public CreateContractHandler(IContractsRepository repo) => _repo = repo;

    public async Task<Guid> HandleAsync(CreateContractRequest req, CancellationToken ct)
    {
        if (!await _repo.SupplierExistsAsync(req.SupplierId, ct))
            throw new ArgumentException("Supplier not found", nameof(req.SupplierId));

        if (!await _repo.OrgUnitExistsAsync(req.OrgUnitId, ct))
            throw new ArgumentException("OrgUnit not found", nameof(req.OrgUnitId));

        var contract = new Contract(
            req.OfficialNumber, req.Type, req.Modality,
            req.SupplierId, req.OrgUnitId,
            new Period(req.TermStart, req.TermEnd),
            new Money(req.TotalAmount, req.Currency),
            req.AdministrativeProcess
        );

        if (req.Obligations is { Count: > 0 })
        {
            foreach (var o in req.Obligations)
            {
                var ob = contract.AddObligation(o.ClauseRef, o.Description, o.DueDate);
                if (o.Deliverables is { Count: > 0 })
                {
                    foreach (var d in o.Deliverables)
                        ob.AddDeliverable(d.ExpectedDate, d.Quantity, d.Unit);
                }
            }
        }

        await _repo.AddAsync(contract, ct);
        await _repo.SaveAsync(ct); 
        return contract.Id;
    }
}
