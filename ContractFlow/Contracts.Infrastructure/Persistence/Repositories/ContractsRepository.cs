using Contracts.Application.Abstractions;
using Contracts.Application.Contracts;
using Contracts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contracts.Infrastructure.Persistence.Repositories;

public class ContractsRepository : IContractsRepository
{
    private readonly ContractsDbContext _db;
    public ContractsRepository(ContractsDbContext db) => _db = db;

    // WRITE
    public Task AddAsync(Contract contract, CancellationToken ct)
        => _db.Contracts.AddAsync(contract, ct).AsTask();

    public Task SaveAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);

    // LOOKUPS
    public Task<bool> SupplierExistsAsync(Guid id, CancellationToken ct)
        => _db.Suppliers.AnyAsync(x => x.Id == id, ct);

    public Task<bool> OrgUnitExistsAsync(Guid id, CancellationToken ct)
        => _db.OrgUnits.AnyAsync(x => x.Id == id, ct);

    // READ (projeção direta pra DTO, sem tracking)
    public async Task<ContractDetailsDto?> GetDetailsByIdAsync(Guid id, CancellationToken ct)
    {
        var q =
            from c in _db.Contracts.AsNoTracking()
            where c.Id == id
            select new ContractDetailsDto
            {
                Id = c.Id,
                OfficialNumber = c.OfficialNumber,
                AdministrativeProcess = c.AdministrativeProcess,
                Type = c.Type,
                Modality = c.Modality,
                Status = c.Status,
                TermStart = c.Term.Start,
                TermEnd = c.Term.End,
                TotalAmount = c.TotalValue.Amount,
                Currency = c.TotalValue.Currency,
                Supplier = new ContractDetailsDto.PartyDto
                {
                    Id = c.Supplier.Id,
                    Name = c.Supplier.CorporateName,
                    Cnpj = c.Supplier.Cnpj
                },
                OrgUnit = new ContractDetailsDto.OrgUnitDto
                {
                    Id = c.OrgUnit.Id,
                    Name = c.OrgUnit.Name,
                    Code = c.OrgUnit.Code
                },
                Attachments = c.Attachments.Select(a => new ContractDetailsDto.AttachmentDto
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    MimeType = a.MimeType,
                    StoragePath = a.StoragePath
                }).ToList(),
                Obligations = c.Obligations.Select(o => new ContractDetailsDto.ObligationDto
                {
                    Id = o.Id,
                    ClauseRef = o.ClauseRef,
                    Description = o.Description,
                    DueDate = o.DueDate,
                    Status = o.Status,
                    Deliverables = o.Deliverables.Select(d => new ContractDetailsDto.DeliverableDto
                    {
                        Id = d.Id,
                        ExpectedDate = d.ExpectedDate,
                        Quantity = d.Quantity,
                        Unit = d.Unit,
                        DeliveredAt = d.DeliveredAt,
                        Inspections = d.Inspections.Select(i => new ContractDetailsDto.InspectionDto
                        {
                            Id = i.Id,
                            Date = i.Date,
                            Inspector = i.Inspector,
                            Notes = i.Notes,
                            Evidences = i.Evidences.Select(ev => new ContractDetailsDto.EvidenceDto
                            {
                                Id = ev.Id,
                                FileName = ev.FileName,
                                MimeType = ev.MimeType,
                                StoragePath = ev.StoragePath,
                                Notes = ev.Notes
                            }).ToList()
                        }).ToList(),
                        Evidences = d.Evidences.Select(ev => new ContractDetailsDto.EvidenceDto
                        {
                            Id = ev.Id,
                            FileName = ev.FileName,
                            MimeType = ev.MimeType,
                            StoragePath = ev.StoragePath,
                            Notes = ev.Notes
                        }).ToList()
                    }).ToList(),
                    NonCompliances = o.NonCompliances.Select(nc => new ContractDetailsDto.NonComplianceDto
                    {
                        Id = nc.Id,
                        Reason = nc.Reason,
                        Severity = nc.Severity,
                        RegisteredAt = nc.RegisteredAt,
                        Penalty = nc.Penalty == null ? null : new ContractDetailsDto.PenaltyDto
                        {
                            Id = nc.Penalty.Id,
                            Type = nc.Penalty.Type,
                            LegalBasis = nc.Penalty.LegalBasis,
                            Amount = nc.Penalty.Amount
                        }
                    }).ToList()
                }).ToList()
            };

        return await q.FirstOrDefaultAsync(ct);
    }
}
