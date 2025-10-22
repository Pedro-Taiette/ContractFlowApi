using ContractFlowApi.Models.Dtos;
using ContractsMvc.Models.Dtos;
using ContractsMvc.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContractsMvc.Controllers
{
    /// <summary>
    /// Manages evidence files for deliverables and inspections. Supports
    /// uploading, listing, downloading and deleting evidence.
    /// </summary>
    [ApiController]
    [Route("api")] // routes defined per method
    public class EvidencesController : ControllerBase
    {
        private readonly EvidenceService _service;

        public EvidencesController(EvidenceService service)
        {
            _service = service;
        }

        /// <summary>
        /// Uploads a piece of evidence for a deliverable. Accepts a file
        /// and optional notes. Returns 404 if the deliverable does not exist.
        /// </summary>
        [HttpPost("deliverables/{deliverableId:guid}/evidences")]
        public async Task<IActionResult> UploadForDeliverable(Guid deliverableId, [FromForm] FileUploadDto request, [FromForm] string? notes, CancellationToken ct)
        {
            if (request.File == null)
                return BadRequest("No file uploaded");

            var dto = await _service.CreateForDeliverableAsync(deliverableId, request.File, notes, ct);
            if (dto == null) return NotFound();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, new { id = dto.Id });
        }

        /// <summary>
        /// Uploads a piece of evidence for an inspection.
        /// </summary>
        [HttpPost("inspections/{inspectionId:guid}/evidences")]
        public async Task<IActionResult> UploadForInspection(Guid inspectionId, [FromForm] FileUploadDto request, [FromForm] string? notes, CancellationToken ct)
        {
            if (request.File == null)
                return BadRequest("No file uploaded");

            var dto = await _service.CreateForInspectionAsync(inspectionId, request.File, notes, ct);
            if (dto == null) return NotFound();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, new { id = dto.Id });
        }

        /// <summary>
        /// Lists evidence associated with a deliverable.
        /// </summary>
        [HttpGet("deliverables/{deliverableId:guid}/evidences")]
        public async Task<IActionResult> ListForDeliverable(Guid deliverableId, CancellationToken ct)
        {
            var data = await _service.ListForDeliverableAsync(deliverableId, ct);
            return Ok(data);
        }

        /// <summary>
        /// Lists evidence associated with an inspection.
        /// </summary>
        [HttpGet("inspections/{inspectionId:guid}/evidences")]
        public async Task<IActionResult> ListForInspection(Guid inspectionId, CancellationToken ct)
        {
            var data = await _service.ListForInspectionAsync(inspectionId, ct);
            return Ok(data);
        }

        /// <summary>
        /// Retrieves metadata about a single evidence item.
        /// </summary>
        [HttpGet("evidences/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _service.GetByIdAsync(id, ct);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        /// <summary>
        /// Downloads the file for a given evidence record.
        /// </summary>
        [HttpGet("evidences/{id:guid}/download")]
        public async Task<IActionResult> Download(Guid id, CancellationToken ct)
        {
            var fileInfo = await _service.GetFileAsync(id, ct);
            if (fileInfo == null) return NotFound();
            var (path, fileName, mimeType) = fileInfo.Value;
            if (!System.IO.File.Exists(path)) return NotFound();
            var bytes = await System.IO.File.ReadAllBytesAsync(path, ct);
            return File(bytes, mimeType, fileName);
        }

        /// <summary>
        /// Deletes an evidence record and its file.
        /// </summary>
        [HttpDelete("evidences/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var ok = await _service.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}