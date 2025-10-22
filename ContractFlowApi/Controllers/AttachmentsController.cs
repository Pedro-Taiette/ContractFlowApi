using ContractFlowApi.Models.Dtos;
using ContractsMvc.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContractsMvc.Controllers
{
    /// <summary>
    /// Manages attachments associated with contracts. Allows uploading,
    /// listing, downloading and deleting attachments.
    /// </summary>
    [ApiController]
    [Route("api")] // define routes per method
    public class AttachmentsController : ControllerBase
    {
        private readonly AttachmentService _service;

        public AttachmentsController(AttachmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Uploads a new attachment to a contract. Accepts a file and
        /// persists metadata. Returns 404 if the contract does not exist.
        /// </summary>
        [HttpPost("contracts/{contractId:guid}/attachments")]
        public async Task<IActionResult> Upload(Guid id, [FromForm] FileUploadDto request, CancellationToken ct)
        {
            if (request.File == null)
                return BadRequest("No file uploaded");

            var result = await _service.CreateAsync(id, request.File, ct);
            return Ok(result);
        }

        /// <summary>
        /// Lists attachments for a contract.
        /// </summary>
        [HttpGet("contracts/{contractId:guid}/attachments")]
        public async Task<IActionResult> List(Guid contractId, CancellationToken ct)
        {
            var data = await _service.ListAsync(contractId, ct);
            return Ok(data);
        }

        /// <summary>
        /// Retrieves metadata for a single attachment.
        /// </summary>
        [HttpGet("attachments/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _service.GetByIdAsync(id, ct);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        /// <summary>
        /// Downloads an attachment file.
        /// </summary>
        [HttpGet("attachments/{id:guid}/download")]
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
        /// Deletes an attachment and its file.
        /// </summary>
        [HttpDelete("attachments/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var ok = await _service.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}