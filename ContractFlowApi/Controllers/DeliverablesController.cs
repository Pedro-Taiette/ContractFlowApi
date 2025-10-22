using ContractsMvc.Models.Dtos;
using ContractsMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractsMvc.Controllers;

/// <summary>
/// Provides endpoints to manage deliverables for obligations. Allows
/// creating new deliverables, listing deliverables for a contract, querying
/// a single deliverable and marking a deliverable as delivered.
/// </summary>
[ApiController]
[Route("api")]
public class DeliverablesController : ControllerBase
{
    private readonly ContractService _service;

    public DeliverablesController(ContractService service)
    {
        _service = service;
    }

    /// <summary>
    /// Creates a new deliverable for the specified obligation. Returns the id of the
    /// created deliverable. When the obligation is not found, a 404 is
    /// returned.
    /// </summary>
    [HttpPost("obligations/{obligationId:guid}/deliverables")]
    public async Task<IActionResult> CreateDeliverable(Guid obligationId, [FromBody] CreateDeliverableRequest request, CancellationToken ct)
    {
        var deliverable = await _service.AddDeliverableAsync(obligationId, request.ExpectedDate, request.Quantity, request.Unit, ct);
        if (deliverable is null) return NotFound();
        return CreatedAtAction(nameof(GetDeliverable), new { deliverableId = deliverable.Id }, new { id = deliverable.Id });
    }

    /// <summary>
    /// Retrieves a single deliverable by its id. Returns 404 when not found.
    /// </summary>
    [HttpGet("deliverables/{deliverableId:guid}")]
    public async Task<IActionResult> GetDeliverable(Guid deliverableId, CancellationToken ct)
    {
        var dto = await _service.GetDeliverableByIdAsync(deliverableId, ct);
        if (dto is null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// Lists all deliverables associated with a contract. Useful for
    /// visualising the contract's delivery schedule.
    /// </summary>
    [HttpGet("contracts/{contractId:guid}/deliverables")]
    public async Task<IActionResult> GetDeliverablesForContract(Guid contractId, CancellationToken ct)
    {
        var data = await _service.GetDeliverablesForContractAsync(contractId, ct);
        return Ok(data);
    }

    /// <summary>
    /// Marks a deliverable as delivered by recording the date. Returns 404
    /// when the deliverable is not found.
    /// </summary>
    [HttpPut("deliverables/{deliverableId:guid}/delivered")]
    public async Task<IActionResult> MarkDelivered(Guid deliverableId, [FromBody] MarkDeliveredRequest request, CancellationToken ct)
    {
        var d = await _service.MarkDeliverableDeliveredAsync(deliverableId, request.DeliveredAt, ct);
        if (d is null) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Request model for creating a deliverable.
    /// </summary>
    public sealed class CreateDeliverableRequest
    {
        public DateTime ExpectedDate { get; init; }
        public decimal Quantity { get; init; }
        public string Unit { get; init; } = null!;
    }

    /// <summary>
    /// Request model for marking a deliverable as delivered.
    /// </summary>
    public sealed class MarkDeliveredRequest
    {
        public DateTime DeliveredAt { get; init; }
    }
}