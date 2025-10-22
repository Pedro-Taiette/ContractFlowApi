using ContractsMvc.Data;
using ContractsMvc.Models;
using ContractsMvc.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ContractsMvc.Services;

/// <summary>
/// Provides business operations for contracts, obligations, deliverables,
/// non‑compliances and reports. By isolating these operations in a
/// service class we keep the controllers thin and encapsulate the domain
/// logic in one place. The service uses the EF Core DbContext directly
/// without relying on repository abstractions to reduce complexity.
/// </summary>
public class ContractService
{
    private readonly ContractsDbContext _db;

    public ContractService(ContractsDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Creates a new contract based on the supplied request. Throws if the
    /// referenced supplier or organisational unit does not exist.
    /// </summary>
    public async Task<Guid> CreateContractAsync(CreateContractRequest request, CancellationToken ct)
    {
        // Verify referenced entities exist
        var supplier = await _db.Suppliers.FirstOrDefaultAsync(s => s.Id == request.SupplierId, ct);
        if (supplier is null) throw new ArgumentException("Supplier not found");
        var orgUnit = await _db.OrgUnits.FirstOrDefaultAsync(o => o.Id == request.OrgUnitId, ct);
        if (orgUnit is null) throw new ArgumentException("OrgUnit not found");

        // Create contract entity
        var contract = new Contract
        {
            OfficialNumber = request.OfficialNumber,
            AdministrativeProcess = request.AdministrativeProcess,
            SupplierId = request.SupplierId,
            Supplier = supplier,
            OrgUnitId = request.OrgUnitId,
            OrgUnit = orgUnit,
            Type = request.Type,
            Modality = request.Modality,
            Status = ContractStatus.Active,
            Term = new Period(request.TermStart, request.TermEnd),
            TotalValue = new Money(request.TotalAmount, request.Currency)
        };

        _db.Contracts.Add(contract);
        await _db.SaveChangesAsync(ct);
        return contract.Id;
    }

    /// <summary>
    /// Retrieves a contract and all nested entities by id. Returns null
    /// when the contract cannot be found.
    /// </summary>
    public async Task<ContractDetailsDto?> GetContractByIdAsync(Guid id, CancellationToken ct)
    {
        var contract = await _db.Contracts
            .AsNoTracking()
            .Include(c => c.Supplier)
            .Include(c => c.OrgUnit)
            .Include(c => c.Obligations)
                .ThenInclude(o => o.Deliverables)
            .Include(c => c.Obligations)
                .ThenInclude(o => o.NonCompliances)
                    .ThenInclude(nc => nc.Penalty)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
        if (contract is null) return null;

        // Map to DTO
        var dto = new ContractDetailsDto
        {
            Id = contract.Id,
            OfficialNumber = contract.OfficialNumber,
            AdministrativeProcess = contract.AdministrativeProcess,
            Type = contract.Type.ToString(),
            Modality = contract.Modality.ToString(),
            Status = contract.Status.ToString(),
            TermStart = contract.Term.Start,
            TermEnd = contract.Term.End,
            TotalAmount = contract.TotalValue.Amount,
            Currency = contract.TotalValue.Currency,
            SupplierId = contract.SupplierId,
            SupplierName = contract.Supplier.CorporateName,
            SupplierCnpj = contract.Supplier.Cnpj,
            OrgUnitId = contract.OrgUnitId,
            OrgUnitName = contract.OrgUnit.Name,
            OrgUnitCode = contract.OrgUnit.Code
        };

        foreach (var o in contract.Obligations)
        {
            var oDto = new ObligationDto
            {
                Id = o.Id,
                ClauseRef = o.ClauseRef,
                Description = o.Description,
                DueDate = o.DueDate,
                Status = o.Status
            };
            foreach (var d in o.Deliverables)
            {
                oDto.Deliverables.Add(new DeliverableDto
                {
                    Id = d.Id,
                    ExpectedDate = d.ExpectedDate,
                    Quantity = d.Quantity,
                    Unit = d.Unit,
                    DeliveredAt = d.DeliveredAt
                });
            }
            foreach (var nc in o.NonCompliances)
            {
                var ncDto = new NonComplianceDto
                {
                    Id = nc.Id,
                    Reason = nc.Reason,
                    Severity = nc.Severity,
                    RegisteredAt = nc.RegisteredAt
                };
                if (nc.Penalty != null)
                {
                    ncDto.Penalty = new PenaltyDto
                    {
                        Id = nc.Penalty.Id,
                        Type = nc.Penalty.Type,
                        LegalBasis = nc.Penalty.LegalBasis,
                        Amount = nc.Penalty.Amount
                    };
                }
                oDto.NonCompliances.Add(ncDto);
            }
            dto.Obligations.Add(oDto);
        }
        return dto;
    }

    /// <summary>
    /// Adds a deliverable to the specified obligation. Returns null when the
    /// obligation does not exist. The deliverable is created and persisted
    /// inside a transaction.
    /// </summary>
    public async Task<Deliverable?> AddDeliverableAsync(Guid obligationId, DateTime expectedDate, decimal quantity, string unit, CancellationToken ct)
    {
        var obligation = await _db.Obligations
            .Include(o => o.Deliverables)
            .FirstOrDefaultAsync(o => o.Id == obligationId, ct);
        if (obligation is null) return null;
        var deliverable = new Deliverable
        {
            ObligationId = obligation.Id,
            Obligation = obligation,
            ExpectedDate = expectedDate,
            Quantity = quantity,
            Unit = unit
        };
        obligation.Deliverables.Add(deliverable);
        await _db.SaveChangesAsync(ct);
        return deliverable;
    }

    /// <summary>
    /// Marks the specified deliverable as delivered by setting the delivery
    /// timestamp. Returns null if the deliverable is not found.
    /// </summary>
    public async Task<Deliverable?> MarkDeliverableDeliveredAsync(Guid deliverableId, DateTime deliveredAt, CancellationToken ct)
    {
        var deliverable = await _db.Deliverables.FirstOrDefaultAsync(d => d.Id == deliverableId, ct);
        if (deliverable is null) return null;
        deliverable.DeliveredAt = deliveredAt;
        await _db.SaveChangesAsync(ct);
        return deliverable;
    }

    /// <summary>
    /// Registers a non‑compliance against an obligation. Returns null if the
    /// obligation is not found.
    /// </summary>
    public async Task<NonCompliance?> RegisterNonComplianceAsync(Guid obligationId, string reason, string severity, CancellationToken ct)
    {
        var obligation = await _db.Obligations
            .Include(o => o.NonCompliances)
            .FirstOrDefaultAsync(o => o.Id == obligationId, ct);
        if (obligation is null) return null;
        var nc = new NonCompliance
        {
            ObligationId = obligation.Id,
            Obligation = obligation,
            Reason = reason,
            Severity = severity,
            RegisteredAt = DateTime.UtcNow
        };
        obligation.NonCompliances.Add(nc);
        await _db.SaveChangesAsync(ct);
        return nc;
    }

    /// <summary>
    /// Applies a penalty to an existing non‑compliance. Throws if the
    /// non‑compliance already has a penalty. Returns null if the record is
    /// not found.
    /// </summary>
    public async Task<Penalty?> ApplyPenaltyAsync(Guid nonComplianceId, string type, string? legalBasis, decimal? amount, CancellationToken ct)
    {
        var nc = await _db.NonCompliances
            .Include(x => x.Penalty)
            .FirstOrDefaultAsync(x => x.Id == nonComplianceId, ct);
        if (nc is null) return null;
        if (nc.Penalty != null) throw new InvalidOperationException("Penalty already applied.");
        var penalty = new Penalty
        {
            NonComplianceId = nc.Id,
            NonCompliance = nc,
            Type = type,
            LegalBasis = legalBasis,
            Amount = amount
        };
        nc.Penalty = penalty;
        _db.Penalties.Add(penalty);
        await _db.SaveChangesAsync(ct);
        return penalty;
    }

    /// <summary>
    /// Lists all deliverables that are due (expected date on or before now) and
    /// have not yet been delivered. Returns a flat list to simplify the
    /// report. Includes associated obligation and contract identifiers.
    /// </summary>
    public async Task<List<DueDeliverableDto>> GetDueDeliverablesAsync(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        return await _db.Deliverables
            .AsNoTracking()
            .Where(d => d.DeliveredAt == null && d.ExpectedDate <= now)
            .Select(d => new DueDeliverableDto
            {
                DeliverableId = d.Id,
                ExpectedDate = d.ExpectedDate,
                Quantity = d.Quantity,
                Unit = d.Unit,
                ObligationId = d.ObligationId,
                ContractId = d.Obligation.ContractId
            })
            .OrderBy(r => r.ExpectedDate)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Produces a summary of how many contracts are in each status. The
    /// enumeration values are converted to strings for ease of serialization.
    /// </summary>
    public async Task<List<ContractStatusSummaryDto>> GetContractStatusSummaryAsync(CancellationToken ct)
    {
        return await _db.Contracts
            .AsNoTracking()
            .GroupBy(c => c.Status)
            .Select(g => new ContractStatusSummaryDto
            {
                Status = g.Key.ToString(),
                Count = g.Count()
            })
            .ToListAsync(ct);
    }

    /// <summary>
    /// Retrieves a single deliverable by id and maps it to a DTO. Returns null
    /// when not found.
    /// </summary>
    public async Task<DeliverableDto?> GetDeliverableByIdAsync(Guid deliverableId, CancellationToken ct)
    {
        var d = await _db.Deliverables.AsNoTracking().FirstOrDefaultAsync(x => x.Id == deliverableId, ct);
        if (d is null) return null;
        return new DeliverableDto
        {
            Id = d.Id,
            ExpectedDate = d.ExpectedDate,
            Quantity = d.Quantity,
            Unit = d.Unit,
            DeliveredAt = d.DeliveredAt
        };
    }

    /// <summary>
    /// Retrieves all deliverables for a contract. Returns an empty list when
    /// none exist.
    /// </summary>
    public async Task<List<DeliverableDto>> GetDeliverablesForContractAsync(Guid contractId, CancellationToken ct)
    {
        return await _db.Deliverables
            .AsNoTracking()
            .Where(d => d.Obligation.ContractId == contractId)
            .Select(d => new DeliverableDto
            {
                Id = d.Id,
                ExpectedDate = d.ExpectedDate,
                Quantity = d.Quantity,
                Unit = d.Unit,
                DeliveredAt = d.DeliveredAt
            })
            .ToListAsync(ct);
    }
}