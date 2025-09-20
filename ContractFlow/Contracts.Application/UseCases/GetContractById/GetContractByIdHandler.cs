using Contracts.Application.Abstractions;
using Contracts.Application.Contracts;

namespace Contracts.Application.UseCases.GetContractById;

public class GetContractByIdHandler
{
    private readonly IContractsRepository _repo;
    public GetContractByIdHandler(IContractsRepository repo) => _repo = repo;

    public Task<ContractDetailsDto?> HandleAsync(Guid id, CancellationToken ct)
        => _repo.GetDetailsByIdAsync(id, ct);
}
