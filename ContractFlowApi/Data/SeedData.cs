using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace ContractsMvc.Data;

/// <summary>
/// Populates the database with a minimal set of data if it is empty. This
/// implementation mirrors the original seeding behavior: it creates a demo
/// supplier and org unit, and adds a sample contract with one obligation and
/// one deliverable when no contracts exist. Keeping the seed simple helps
/// developers explore the API without first creating entities manually.
/// </summary>
public static class SeedData
{
    public static async Task InitializeAsync(ContractsDbContext db)
    {
        // Ensure supplier and org unit records exist
        if (!await db.Suppliers.AnyAsync())
        {
            db.Suppliers.Add(new Supplier
            {
                CorporateName = "Fornecedor Demo Ltda",
                Cnpj = "00.000.000/0001-00",
                Active = true
            });
        }
        if (!await db.OrgUnits.AnyAsync())
        {
            db.OrgUnits.Add(new OrgUnit
            {
                Name = "Secretaria de Obras",
                Code = "SO-01"
            });
        }
        await db.SaveChangesAsync();

        // Create a sample contract with an obligation and deliverable if none exists
        if (!await db.Contracts.AnyAsync())
        {
            var supplier = await db.Suppliers.FirstAsync();
            var org = await db.OrgUnits.FirstAsync();
            var contract = new Contract
            {
                OfficialNumber = "CT-0001",
                Type = ContractType.Servico,
                Modality = ContractModality.Pregao,
                SupplierId = supplier.Id,
                Supplier = supplier,
                OrgUnitId = org.Id,
                OrgUnit = org,
                Term = new Period(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddYears(1)),
                TotalValue = new Money(10000m, "BRL"),
                AdministrativeProcess = "PROC-001"
            };
            // Create an obligation and a deliverable manually since methods were removed
            var obligation = new Obligation
            {
                Contract = contract,
                ContractId = contract.Id,
                ClauseRef = "1.1",
                Description = "Fornecimento mensal de uniformes",
                DueDate = DateTime.UtcNow.Date.AddMonths(1),
                Status = "Pending"
            };
            var deliverable = new Deliverable
            {
                Obligation = obligation,
                ObligationId = obligation.Id,
                ExpectedDate = DateTime.UtcNow.Date.AddMonths(1),
                Quantity = 100m,
                Unit = "un"
            };
            obligation.Deliverables.Add(deliverable);
            contract.Obligations.Add(obligation);
            db.Contracts.Add(contract);
            await db.SaveChangesAsync();
        }
    }
}