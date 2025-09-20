namespace Contracts.Application.Abstractions;

using global::Contracts.Application.Contracts;
using global::Contracts.Domain.Entities;

public interface IContractsRepository
{
    // WRITE
    Task AddAsync(Contract contract, CancellationToken ct);
    Task SaveAsync(CancellationToken ct);

    // LOOKUPS
    Task<bool> SupplierExistsAsync(Guid id, CancellationToken ct);
    Task<bool> OrgUnitExistsAsync(Guid id, CancellationToken ct);

    // READ
    Task<ContractDetailsDto?> GetDetailsByIdAsync(Guid id, CancellationToken ct);
}
