using Core.Abstractions;
using Core.DataAnnotations;
using Core.ApiResults;
using MediatR;

namespace Transferencia.Application.Features.TransferenciaInterna;

public sealed class TransferenciaInternaRequest : RequestWithContext, IRequest<ApiResult>
{
    public int NumeroContaDestino { get; init; } = 0;

    [ValorMaiorZero]
    public decimal Valor { get; init; } = 0;
}
