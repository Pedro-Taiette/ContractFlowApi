using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ContractsMvc.Models.Dtos;
using ContractsMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractsMvc.Controllers
{
    /// <summary>
    /// Provides CRUD operations for suppliers. Suppliers represent the
    /// companies that provide goods or services in a contract. All
    /// endpoints follow RESTful conventions and return simple DTOs.
    /// </summary>
    [ApiController]
    [Route("api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly SupplierService _service;

        public SuppliersController(SupplierService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lists all suppliers ordered by corporate name.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> List(CancellationToken ct)
        {
            var data = await _service.ListAsync(ct);
            return Ok(data);
        }

        /// <summary>
        /// Retrieves a single supplier by id. Returns 404 when not found.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _service.GetByIdAsync(id, ct);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        /// <summary>
        /// Creates a new supplier. Returns the identifier of the new
        /// supplier in the response body and the location header.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupplierRequest request, CancellationToken ct)
        {
            var dto = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, new { id = dto.Id });
        }

        /// <summary>
        /// Updates an existing supplier. Returns 404 when the supplier is
        /// not found.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSupplierRequest request, CancellationToken ct)
        {
            var ok = await _service.UpdateAsync(id, request, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Soft deletes a supplier. Returns 404 when not found.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var ok = await _service.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}