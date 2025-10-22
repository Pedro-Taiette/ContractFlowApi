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
    /// Provides CRUD operations for organisational units. The service
    /// encapsulates persistence logic and returns DTOs for API
    /// consumption.
    /// </summary>
    public class OrgUnitService
    {
        private readonly ContractsDbContext _db;

        public OrgUnitService(ContractsDbContext db)
        {
            _db = db;
        }

        public async Task<OrgUnitDto> CreateAsync(CreateOrgUnitRequest request, CancellationToken ct)
        {
            var ou = new OrgUnit
            {
                Name = request.Name,
                Code = request.Code
            };
            _db.OrgUnits.Add(ou);
            await _db.SaveChangesAsync(ct);
            return new OrgUnitDto
            {
                Id = ou.Id,
                Name = ou.Name,
                Code = ou.Code
            };
        }

        public async Task<OrgUnitDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var ou = await _db.OrgUnits.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (ou == null) return null;
            return new OrgUnitDto
            {
                Id = ou.Id,
                Name = ou.Name,
                Code = ou.Code
            };
        }

        public async Task<List<OrgUnitDto>> ListAsync(CancellationToken ct)
        {
            return await _db.OrgUnits.AsNoTracking()
                .OrderBy(o => o.Name)
                .Select(o => new OrgUnitDto { Id = o.Id, Name = o.Name, Code = o.Code })
                .ToListAsync(ct);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateOrgUnitRequest request, CancellationToken ct)
        {
            var ou = await _db.OrgUnits.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (ou == null) return false;
            ou.Name = request.Name;
            ou.Code = request.Code;
            ou.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var ou = await _db.OrgUnits.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (ou == null) return false;
            ou.IsDeleted = true;
            ou.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}