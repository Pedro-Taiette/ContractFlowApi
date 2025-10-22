using System;
using System.Threading;
using System.Threading.Tasks;
using ContractsMvc.Models.Dtos;
using ContractsMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractsMvc.Controllers
{
    /// <summary>
    /// Manages CRUD operations for inspections. Inspections are created
    /// under deliverables and can be retrieved, updated or deleted.
    /// </summary>
    [ApiController]
    [Route("api")] // route defined on each action
    public class InspectionsController : ControllerBase
    {
        private readonly InspectionService _service;

        public InspectionsController(InspectionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new inspection for a deliverable. Returns 404 if the
        /// deliverable does not exist.
        /// </summary>
        [HttpPost("deliverables/{deliverableId:guid}/inspections")]
        public async Task<IActionResult> Create(Guid deliverableId, [FromBody] CreateInspectionRequest request, CancellationToken ct)
        {
            var dto = await _service.CreateAsync(deliverableId, request, ct);
            if (dto == null) return NotFound();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, new { id = dto.Id });
        }

        /// <summary>
        /// Lists all inspections for a deliverable ordered by date descending.
        /// </summary>
        [HttpGet("deliverables/{deliverableId:guid}/inspections")]
        public async Task<IActionResult> ListForDeliverable(Guid deliverableId, CancellationToken ct)
        {
            var data = await _service.ListForDeliverableAsync(deliverableId, ct);
            return Ok(data);
        }

        /// <summary>
        /// Retrieves a single inspection by id.
        /// </summary>
        [HttpGet("inspections/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _service.GetByIdAsync(id, ct);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        /// <summary>
        /// Updates an inspection. Returns 404 when not found.
        /// </summary>
        [HttpPut("inspections/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInspectionRequest request, CancellationToken ct)
        {
            var ok = await _service.UpdateAsync(id, request, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Soft deletes an inspection.
        /// </summary>
        [HttpDelete("inspections/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var ok = await _service.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}