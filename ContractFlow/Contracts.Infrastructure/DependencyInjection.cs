using Contracts.Application.Abstractions;
using Contracts.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IContractsRepository, ContractsRepository>();
        return services;
    }
}
