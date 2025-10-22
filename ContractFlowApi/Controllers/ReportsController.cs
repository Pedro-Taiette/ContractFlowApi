using ContractsMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractsMvc.Controllers;

/// <summary>
/// Provides simple reporting endpoints for overdue deliverables and
/// contract status distribution. Reports return flattened DTOs to simplify
/// consumption by clients.
/// </summary>
[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly ContractService _service;

    public ReportsController(ContractService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns deliverables that are due (expected date on or before now)
    /// and not yet delivered. Includes the deliverable id, expected date,
    /// quantity, unit and identifiers for the obligation and contract.
    /// </summary>
    [HttpGet("due-deliverables")]
    public async Task<IActionResult> GetDueDeliverables(CancellationToken ct)
    {
        var data = await _service.GetDueDeliverablesAsync(ct);
        return Ok(data);
    }

    /// <summary>
    /// Returns the count of contracts per status. Status is returned as a
    /// string to aid readability in clients.
    /// </summary>
    [HttpGet("contract-status")]
    public async Task<IActionResult> GetContractStatusSummary(CancellationToken ct)
    {
        var data = await _service.GetContractStatusSummaryAsync(ct);
        return Ok(data);
    }
}