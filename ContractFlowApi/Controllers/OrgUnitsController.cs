using System;
using System.Threading;
using System.Threading.Tasks;
using ContractsMvc.Models.Dtos;
using ContractsMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractsMvc.Controllers
{
    /// <summary>
    /// Provides CRUD endpoints for organisational units. Units represent
    /// departments or areas within the organisation responsible for
    /// contracts.
    /// </summary>
    [ApiController]
    [Route("api/orgunits")]
    public class OrgUnitsController : ControllerBase
    {
        private readonly OrgUnitService _service;

        public OrgUnitsController(OrgUnitService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> List(CancellationToken ct)
        {
            var data = await _service.ListAsync(ct);
            return Ok(data);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _service.GetByIdAsync(id, ct);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrgUnitRequest request, CancellationToken ct)
        {
            var dto = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, new { id = dto.Id });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrgUnitRequest request, CancellationToken ct)
        {
            var ok = await _service.UpdateAsync(id, request, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var ok = await _service.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}