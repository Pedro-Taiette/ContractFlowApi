using Contracts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contracts.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(ContractsDbContext db, CancellationToken ct = default)
    {
        if (!await db.Suppliers.AnyAsync(ct))
        {
            db.Suppliers.Add(new Supplier("Fornecedor Demo Ltda", "00.000.000/0001-00"));
        }
        if (!await db.OrgUnits.AnyAsync(ct))
        {
            db.OrgUnits.Add(new OrgUnit("Secretaria de Obras", "SO-01"));
        }
        await db.SaveChangesAsync(ct);
    }
}
