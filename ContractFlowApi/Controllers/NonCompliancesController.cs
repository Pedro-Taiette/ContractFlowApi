using ContractsMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractsMvc.Controllers;

/// <summary>
/// Endpoints for recording non‑compliances and applying penalties. The
/// service ensures that a penalty can only be applied once to a given
/// non‑compliance.
/// </summary>
[ApiController]
[Route("api")]
public class NonCompliancesController : ControllerBase
{
    private readonly ContractService _service;

    public NonCompliancesController(ContractService service)
    {
        _service = service;
    }

    /// <summary>
    /// Registers a non‑compliance for the specified obligation. Returns the id
    /// of the created non‑compliance. If the obligation does not exist a
    /// 404 response is returned.
    /// </summary>
    [HttpPost("obligations/{obligationId:guid}/noncompliances")]
    public async Task<IActionResult> RegisterNonCompliance(Guid obligationId, [FromBody] RegisterNonComplianceRequest request, CancellationToken ct)
    {
        var nc = await _service.RegisterNonComplianceAsync(obligationId, request.Reason, request.Severity, ct);
        if (nc is null) return NotFound();
        return Ok(new { id = nc.Id });
    }

    /// <summary>
    /// Applies a penalty to a non‑compliance. Returns the id of the penalty
    /// or a BadRequest when a penalty has already been applied. If the
    /// non‑compliance does not exist a 404 response is returned.
    /// </summary>
    [HttpPost("noncompliances/{nonComplianceId:guid}/penalties")]
    public async Task<IActionResult> ApplyPenalty(Guid nonComplianceId, [FromBody] ApplyPenaltyRequest request, CancellationToken ct)
    {
        try
        {
            var penalty = await _service.ApplyPenaltyAsync(nonComplianceId, request.Type, request.LegalBasis, request.Amount, ct);
            if (penalty is null) return NotFound();
            return Ok(new { id = penalty.Id });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Request model for registering a non‑compliance.
    /// </summary>
    public sealed class RegisterNonComplianceRequest
    {
        public string Reason { get; init; } = null!;
        public string Severity { get; init; } = null!;
    }

    /// <summary>
    /// Request model for applying a penalty.
    /// </summary>
    public sealed class ApplyPenaltyRequest
    {
        public string Type { get; init; } = null!;
        public string? LegalBasis { get; init; }
        public decimal? Amount { get; init; }
    }
}