using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR/FluentValidation talvez
        return services;
    }
}
