using Core.Idempotencia;
using Core.Infrastructure.Idempotencia;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Extensions;

public static class IdempotenciaExtensions
{
    public static IServiceCollection AddIdempotencia(this IServiceCollection services)
    {
        services.AddTransient<IIdempotenciaRepository, IdempotenciaRepository>();

        return services;
    }
}
