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
    /// Service for managing contract attachments. Attachments represent
    /// documents associated with a contract. Files are stored on disk
    /// and metadata is persisted in the database.
    /// </summary>
    public class AttachmentService
    {
        private readonly ContractsDbContext _db;
        private readonly string _storageRoot;

        public AttachmentService(ContractsDbContext db, IConfiguration configuration)
        {
            _db = db;
            _storageRoot = configuration["FileStorage"] ?? "Uploads";
            if (!Directory.Exists(_storageRoot))
            {
                Directory.CreateDirectory(_storageRoot);
            }
        }

        public async Task<AttachmentDto?> CreateAsync(Guid contractId, IFormFile file, CancellationToken ct)
        {
            var contract = await _db.Contracts.FirstOrDefaultAsync(c => c.Id == contractId, ct);
            if (contract == null) return null;
            if (file.Length == 0) throw new InvalidOperationException("Empty file");
            if (file.Length > 20 * 1024 * 1024) throw new InvalidOperationException("File too large");
            var ext = Path.GetExtension(file.FileName);
            var newFileName = $"{Guid.NewGuid()}{ext}";
            var physicalPath = Path.Combine(_storageRoot, newFileName);
            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }
            var att = new Attachment
            {
                ContractId = contract.Id,
                Contract = contract,
                FileName = file.FileName,
                MimeType = file.ContentType,
                StoragePath = newFileName
            };
            _db.Attachments.Add(att);
            await _db.SaveChangesAsync(ct);
            return new AttachmentDto
            {
                Id = att.Id,
                ContractId = att.ContractId,
                FileName = att.FileName,
                MimeType = att.MimeType,
                StoragePath = att.StoragePath
            };
        }

        public async Task<List<AttachmentDto>> ListAsync(Guid contractId, CancellationToken ct)
        {
            return await _db.Attachments.AsNoTracking()
                .Where(a => a.ContractId == contractId)
                .OrderBy(a => a.FileName)
                .Select(a => new AttachmentDto
                {
                    Id = a.Id,
                    ContractId = a.ContractId,
                    FileName = a.FileName,
                    MimeType = a.MimeType,
                    StoragePath = a.StoragePath
                })
                .ToListAsync(ct);
        }

        public async Task<AttachmentDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var a = await _db.Attachments.AsNoTracking().FirstOrDefaultAsync(att => att.Id == id, ct);
            if (a == null) return null;
            return new AttachmentDto
            {
                Id = a.Id,
                ContractId = a.ContractId,
                FileName = a.FileName,
                MimeType = a.MimeType,
                StoragePath = a.StoragePath
            };
        }

        /// <summary>
        /// Returns the physical path of an attachment file. Use this in
        /// controllers to stream the file to the client. Returns null
        /// when the attachment cannot be found.
        /// </summary>
        public async Task<(string Path, string FileName, string MimeType)?> GetFileAsync(Guid id, CancellationToken ct)
        {
            var a = await _db.Attachments.AsNoTracking().FirstOrDefaultAsync(att => att.Id == id, ct);
            if (a == null) return null;
            var physicalPath = System.IO.Path.Combine(_storageRoot, a.StoragePath);
            return (physicalPath, a.FileName, a.MimeType);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var a = await _db.Attachments.FirstOrDefaultAsync(att => att.Id == id, ct);
            if (a == null) return false;
            a.IsDeleted = true;
            a.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            var physicalPath = Path.Combine(_storageRoot, a.StoragePath);
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
            return true;
        }
    }
}