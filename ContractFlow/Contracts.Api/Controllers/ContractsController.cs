using Contracts.Application.Contracts;
using Contracts.Application.UseCases.CreateContract;
using Contracts.Application.UseCases.GetContractById; 
using Microsoft.AspNetCore.Mvc;
using Contracts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Contracts.Api.Controllers;

[ApiController]
[Route("api/contracts")]
public class ContractsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateContractHandler handler,
        [FromBody] CreateContractRequest request,
        CancellationToken ct)
    {
        var id = await handler.HandleAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] GetContractByIdHandler handler,
        CancellationToken ct)
    {
        var dto = await handler.HandleAsync(id, ct);
        if (dto is null) return NotFound();
        return Ok(dto);
    }

    [HttpGet] 
    public async Task<IActionResult> List([FromServices] ContractsDbContext db, CancellationToken ct)
    {
        var data = await db.Contracts.AsNoTracking()
            .Select(c => new { c.Id, c.OfficialNumber, c.Status, c.IsDeleted })
            .OrderByDescending(c => c.Id)
            .Take(20)
            .ToListAsync(ct);
        return Ok(data);
    }
}
