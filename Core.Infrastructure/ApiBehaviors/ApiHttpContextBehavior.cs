using Core.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Infrastructure.ApiBehaviors;

public class ApiHttpContextBehavior<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is RequestWithContext requestWithContext)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                // Extrai IdContaLogada dos claims do JWT
                requestWithContext.IdContaLogada = httpContext.User.FindFirst("idContaCorrente")?.Value ?? string.Empty;

                // Extrai ChaveIdempotencia de um cabeçalho HTTP (ex: "X-Idempotency-Key")
                if (httpContext.Request.Headers.TryGetValue("X-Idempotency-Key", out var idempotencyKey))
                {
                    requestWithContext.ChaveIdempotencia = idempotencyKey.ToString();
                }
            }
        }

        return await next();
    }
}
