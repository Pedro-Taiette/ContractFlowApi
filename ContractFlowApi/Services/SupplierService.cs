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
    /// Provides CRUD operations for suppliers. Encapsulates basic
    /// validation and interacts with the DbContext directly. All
    /// operations use async/await to avoid blocking the thread.
    /// </summary>
    public class SupplierService
    {
        private readonly ContractsDbContext _db;

        public SupplierService(ContractsDbContext db)
        {
            _db = db;
        }

        public async Task<SupplierDto> CreateAsync(CreateSupplierRequest request, CancellationToken ct)
        {
            var supplier = new Supplier
            {
                CorporateName = request.CorporateName,
                Cnpj = request.Cnpj,
                Active = request.Active
            };
            _db.Suppliers.Add(supplier);
            await _db.SaveChangesAsync(ct);
            return new SupplierDto
            {
                Id = supplier.Id,
                CorporateName = supplier.CorporateName,
                Cnpj = supplier.Cnpj,
                Active = supplier.Active
            };
        }

        public async Task<SupplierDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var s = await _db.Suppliers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (s == null) return null;
            return new SupplierDto
            {
                Id = s.Id,
                CorporateName = s.CorporateName,
                Cnpj = s.Cnpj,
                Active = s.Active
            };
        }

        public async Task<List<SupplierDto>> ListAsync(CancellationToken ct)
        {
            return await _db.Suppliers.AsNoTracking()
                .OrderBy(s => s.CorporateName)
                .Select(s => new SupplierDto
                {
                    Id = s.Id,
                    CorporateName = s.CorporateName,
                    Cnpj = s.Cnpj,
                    Active = s.Active
                })
                .ToListAsync(ct);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateSupplierRequest request, CancellationToken ct)
        {
            var s = await _db.Suppliers.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (s == null) return false;
            s.CorporateName = request.CorporateName;
            s.Cnpj = request.Cnpj;
            s.Active = request.Active;
            s.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var s = await _db.Suppliers.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (s == null) return false;
            s.IsDeleted = true;
            s.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}