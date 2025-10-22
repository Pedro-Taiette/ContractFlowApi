using ContractsMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractsMvc.Controllers;

/// <summary>
/// Provides reporting endpoints for deliverables, contract status, and penalties.
/// Reports return flattened DTOs for easier client-side consumption.
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
    /// Returns deliverables that are due (expected date on or before now) and
    /// not yet delivered. Optional from/to query parameters filter by
    /// expected date range. Dates must be provided in ISO 8601 format
    /// (YYYY-MM-DD).
    /// </summary>
    [HttpGet("due-deliverables")]
    public async Task<IActionResult> GetDueDeliverables(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken ct)
    {
        var data = await _service.GetDueDeliverablesAsync(from, to, ct);
        return Ok(data);
    }

    /// <summary>
    /// Returns the count of contracts per status.
    /// Status is returned as a string to improve readability.
    /// </summary>
    [HttpGet("contract-status")]
    public async Task<IActionResult> GetContractStatusSummary(CancellationToken ct)
    {
        var data = await _service.GetContractStatusSummaryAsync(ct);
        return Ok(data);
    }

    /// <summary>
    /// Aggregates deliverables by supplier and returns counts for each supplier.
    /// Optional from/to query parameters filter by expected delivery date.
    /// </summary>
    [HttpGet("deliveries-by-supplier")]
    public async Task<IActionResult> GetDeliveriesBySupplier(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken ct)
    {
        var data = await _service.GetDeliveriesBySupplierAsync(from, to, ct);
        return Ok(data);
    }

    /// <summary>
    /// Aggregates deliverables by organisational unit.
    /// Optional date filtering is supported via from/to query parameters.
    /// </summary>
    [HttpGet("deliveries-by-orgunit")]
    public async Task<IActionResult> GetDeliveriesByOrgUnit(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken ct)
    {
        var data = await _service.GetDeliveriesByOrgUnitAsync(from, to, ct);
        return Ok(data);
    }

    /// <summary>
    /// Returns a list of penalties applied.
    /// Optionally filter by the registration date of the non-compliance
    /// via from/to query parameters.
    /// </summary>
    [HttpGet("penalties")]
    public async Task<IActionResult> GetPenalties(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken ct)
    {
        var data = await _service.GetPenaltiesAsync(from, to, ct);
        return Ok(data);
    }
}
