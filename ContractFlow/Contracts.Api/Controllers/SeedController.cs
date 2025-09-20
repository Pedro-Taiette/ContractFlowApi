using Contracts.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/seed")]
public class SeedController : ControllerBase
{
    private readonly ContractsDbContext _db;
    public SeedController(ContractsDbContext db) => _db = db;

    [HttpGet("ids")]
    public async Task<IActionResult> GetIds()
    {
        var sup = await _db.Suppliers.AsNoTracking().Select(x => new { x.Id, x.CorporateName }).FirstOrDefaultAsync();
        var org = await _db.OrgUnits.AsNoTracking().Select(x => new { x.Id, x.Name }).FirstOrDefaultAsync();
        return Ok(new { supplierId = sup?.Id, orgUnitId = org?.Id });
    }
}
