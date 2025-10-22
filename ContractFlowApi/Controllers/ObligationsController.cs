using System;
using System.Threading;
using System.Threading.Tasks;
using ContractsMvc.Models.Dtos;
using ContractsMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractsMvc.Controllers
{
    /// <summary>
    /// Manages CRUD operations for obligations. Obligations belong to
    /// contracts and act as a grouping for deliverables and non‑compliance
    /// records.
    /// </summary>
    [ApiController]
    [Route("api")] // route defined per action
    public class ObligationsController : ControllerBase
    {
        private readonly ObligationService _service;

        public ObligationsController(ObligationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lists obligations for a specific contract. Returns an empty
        /// list when none exist.
        /// </summary>
        [HttpGet("contracts/{contractId:guid}/obligations")]
        public async Task<IActionResult> ListForContract(Guid contractId, CancellationToken ct)
        {
            var data = await _service.ListForContractAsync(contractId, ct);
            return Ok(data);
        }

        /// <summary>
        /// Retrieves a single obligation by id.
        /// </summary>
        [HttpGet("obligations/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _service.GetByIdAsync(id, ct);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        /// <summary>
        /// Creates a new obligation under the specified contract. Returns
        /// 404 when the contract is not found.
        /// </summary>
        [HttpPost("contracts/{contractId:guid}/obligations")]
        public async Task<IActionResult> Create(Guid contractId, [FromBody] CreateObligationRequest request, CancellationToken ct)
        {
            var dto = await _service.CreateAsync(contractId, request, ct);
            if (dto == null) return NotFound();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, new { id = dto.Id });
        }

        /// <summary>
        /// Updates an existing obligation.
        /// </summary>
        [HttpPut("obligations/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateObligationRequest request, CancellationToken ct)
        {
            var ok = await _service.UpdateAsync(id, request, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Soft deletes an obligation.
        /// </summary>
        [HttpDelete("obligations/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var ok = await _service.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}