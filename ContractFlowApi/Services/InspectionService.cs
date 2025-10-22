using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContractsMvc.Data;
using ContractsMvc.Models;
using ContractsMvc.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ContractsMvc.Services
{
    /// <summary>
    /// Service providing CRUD operations for inspections. Inspections are
    /// always tied to a deliverable. This service performs minimal
    /// validation such as verifying the deliverable exists before
    /// creating an inspection.
    /// </summary>
    public class InspectionService
    {
        private readonly ContractsDbContext _db;

        public InspectionService(ContractsDbContext db)
        {
            _db = db;
        }

        public async Task<InspectionDto?> CreateAsync(Guid deliverableId, CreateInspectionRequest request, CancellationToken ct)
        {
            var deliverable = await _db.Deliverables.FirstOrDefaultAsync(d => d.Id == deliverableId, ct);
            if (deliverable == null) return null;
            var ins = new Inspection
            {
                DeliverableId = deliverable.Id,
                Deliverable = deliverable,
                Date = request.Date,
                Inspector = request.Inspector,
                Notes = request.Notes
            };
            _db.Inspections.Add(ins);
            await _db.SaveChangesAsync(ct);
            return new InspectionDto
            {
                Id = ins.Id,
                DeliverableId = deliverable.Id,
                Date = ins.Date,
                Inspector = ins.Inspector,
                Notes = ins.Notes
            };
        }

        public async Task<InspectionDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var ins = await _db.Inspections.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, ct);
            if (ins == null) return null;
            return new InspectionDto
            {
                Id = ins.Id,
                DeliverableId = ins.DeliverableId,
                Date = ins.Date,
                Inspector = ins.Inspector,
                Notes = ins.Notes
            };
        }

        public async Task<List<InspectionDto>> ListForDeliverableAsync(Guid deliverableId, CancellationToken ct)
        {
            return await _db.Inspections.AsNoTracking()
                .Where(i => i.DeliverableId == deliverableId)
                .OrderByDescending(i => i.Date)
                .Select(i => new InspectionDto
                {
                    Id = i.Id,
                    DeliverableId = i.DeliverableId,
                    Date = i.Date,
                    Inspector = i.Inspector,
                    Notes = i.Notes
                })
                .ToListAsync(ct);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateInspectionRequest request, CancellationToken ct)
        {
            var ins = await _db.Inspections.FirstOrDefaultAsync(i => i.Id == id, ct);
            if (ins == null) return false;
            ins.Date = request.Date;
            ins.Inspector = request.Inspector;
            ins.Notes = request.Notes;
            ins.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var ins = await _db.Inspections.FirstOrDefaultAsync(i => i.Id == id, ct);
            if (ins == null) return false;
            ins.IsDeleted = true;
            ins.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}