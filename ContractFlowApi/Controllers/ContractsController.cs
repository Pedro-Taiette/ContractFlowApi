using ContractsMvc.Data;
using ContractsMvc.Models.Dtos;
using ContractsMvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContractsMvc.Controllers;

/// <summary>
/// Manages contract creation and retrieval. This controller demonstrates
/// minimal endpoints required for the MVP. The heavy lifting is delegated
/// to the ContractService to keep the controller thin.
/// </summary>
[ApiController]
[Route("api/contracts")]
public class ContractsController : ControllerBase
{
    private readonly ContractService _service;
    private readonly ContractsDbContext _db;

    public ContractsController(ContractService service, ContractsDbContext db)
    {
        _service = service;
        _db = db;
    }

    /// <summary>
    /// Creates a new contract. Returns the generated identifier in the
    /// Location header. Throws when referenced entities are not found.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest request, CancellationToken ct)
    {
        var id = await _service.CreateContractAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    /// <summary>
    /// Retrieves a contract by id. Includes obligations, deliverables and
    /// non‑compliances. Returns 404 when not found.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _service.GetContractByIdAsync(id, ct);
        if (dto is null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// Lists recently created contracts with minimal details. Limited to the
    /// 20 most recent records to avoid returning a large dataset.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var data = await _db.Contracts.AsNoTracking()
            .Select(c => new
            {
                c.Id,
                c.OfficialNumber,
                Status = c.Status.ToString(),
                c.IsDeleted
            })
            .OrderByDescending(c => c.Id)
            .Take(20)
            .ToListAsync(ct);
        return Ok(data);
    }
}