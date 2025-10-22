using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContractsMvc.Data;
using ContractsMvc.Models;
using ContractsMvc.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ContractsMvc.Services
{
    /// <summary>
    /// Service responsible for managing evidence files. Evidence can be
    /// associated with either a deliverable or an inspection. Files are
    /// stored on the local file system at a configurable root path. This
    /// service performs minimal validation and persists metadata in the
    /// database.
    /// </summary>
    public class EvidenceService
    {
        private readonly ContractsDbContext _db;
        private readonly string _storageRoot;

        public EvidenceService(ContractsDbContext db, IConfiguration configuration)
        {
            _db = db;
            _storageRoot = configuration["FileStorage"] ?? "Uploads";
            // Ensure the directory exists
            if (!Directory.Exists(_storageRoot))
            {
                Directory.CreateDirectory(_storageRoot);
            }
        }

        /// <summary>
        /// Saves an evidence file for a deliverable. Returns null if the
        /// deliverable does not exist.
        /// </summary>
        public async Task<EvidenceDto?> CreateForDeliverableAsync(Guid deliverableId, IFormFile file, string? notes, CancellationToken ct)
        {
            var deliverable = await _db.Deliverables.FirstOrDefaultAsync(d => d.Id == deliverableId, ct);
            if (deliverable == null) return null;
            return await SaveEvidenceAsync(deliverableId, null, file, notes, ct);
        }

        /// <summary>
        /// Saves an evidence file for an inspection. Returns null if the
        /// inspection does not exist.
        /// </summary>
        public async Task<EvidenceDto?> CreateForInspectionAsync(Guid inspectionId, IFormFile file, string? notes, CancellationToken ct)
        {
            var inspection = await _db.Inspections.FirstOrDefaultAsync(i => i.Id == inspectionId, ct);
            if (inspection == null) return null;
            return await SaveEvidenceAsync(null, inspectionId, file, notes, ct);
        }

        private async Task<EvidenceDto> SaveEvidenceAsync(Guid? deliverableId, Guid? inspectionId, IFormFile file, string? notes, CancellationToken ct)
        {
            if (file.Length == 0)
                throw new InvalidOperationException("Empty file");
            if (file.Length > 10 * 1024 * 1024)
                throw new InvalidOperationException("File too large");

            var fileExt = Path.GetExtension(file.FileName);
            var newFileName = $"{Guid.NewGuid()}{fileExt}";
            var physicalPath = Path.Combine(_storageRoot, newFileName);
            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }
            var evidence = new Evidence
            {
                FileName = file.FileName,
                MimeType = file.ContentType,
                StoragePath = newFileName,
                Notes = notes,
                DeliverableId = deliverableId,
                InspectionId = inspectionId
            };
            _db.Evidences.Add(evidence);
            await _db.SaveChangesAsync(ct);
            return new EvidenceDto
            {
                Id = evidence.Id,
                FileName = evidence.FileName,
                MimeType = evidence.MimeType,
                StoragePath = evidence.StoragePath,
                Notes = evidence.Notes,
                DeliverableId = evidence.DeliverableId,
                InspectionId = evidence.InspectionId
            };
        }

        public async Task<EvidenceDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var e = await _db.Evidences.AsNoTracking().FirstOrDefaultAsync(ev => ev.Id == id, ct);
            if (e == null) return null;
            return new EvidenceDto
            {
                Id = e.Id,
                FileName = e.FileName,
                MimeType = e.MimeType,
                StoragePath = e.StoragePath,
                Notes = e.Notes,
                DeliverableId = e.DeliverableId,
                InspectionId = e.InspectionId
            };
        }

        public async Task<List<EvidenceDto>> ListForDeliverableAsync(Guid deliverableId, CancellationToken ct)
        {
            return await _db.Evidences.AsNoTracking()
                .Where(ev => ev.DeliverableId == deliverableId)
                .OrderBy(ev => ev.FileName)
                .Select(ev => new EvidenceDto
                {
                    Id = ev.Id,
                    FileName = ev.FileName,
                    MimeType = ev.MimeType,
                    StoragePath = ev.StoragePath,
                    Notes = ev.Notes,
                    DeliverableId = ev.DeliverableId,
                    InspectionId = ev.InspectionId
                })
                .ToListAsync(ct);
        }

        public async Task<List<EvidenceDto>> ListForInspectionAsync(Guid inspectionId, CancellationToken ct)
        {
            return await _db.Evidences.AsNoTracking()
                .Where(ev => ev.InspectionId == inspectionId)
                .OrderBy(ev => ev.FileName)
                .Select(ev => new EvidenceDto
                {
                    Id = ev.Id,
                    FileName = ev.FileName,
                    MimeType = ev.MimeType,
                    StoragePath = ev.StoragePath,
                    Notes = ev.Notes,
                    DeliverableId = ev.DeliverableId,
                    InspectionId = ev.InspectionId
                })
                .ToListAsync(ct);
        }

        /// <summary>
        /// Deletes an evidence record and its underlying file. Returns false
        /// if the record was not found.
        /// </summary>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var ev = await _db.Evidences.FirstOrDefaultAsync(e => e.Id == id, ct);
            if (ev == null) return false;
            ev.IsDeleted = true;
            ev.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            var physicalPath = Path.Combine(_storageRoot, ev.StoragePath);
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
            return true;
        }

        /// <summary>
        /// Returns the physical path, file name and MIME type of an evidence
        /// record. Returns null when the record does not exist. The caller
        /// is responsible for reading the file from disk.
        /// </summary>
        public async Task<(string Path, string FileName, string MimeType)?> GetFileAsync(Guid id, CancellationToken ct)
        {
            var ev = await _db.Evidences.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);
            if (ev == null) return null;
            var physicalPath = System.IO.Path.Combine(_storageRoot, ev.StoragePath);
            return (physicalPath, ev.FileName, ev.MimeType);
        }
    }
}