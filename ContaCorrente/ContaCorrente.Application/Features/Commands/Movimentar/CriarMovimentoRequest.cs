using Core.Abstractions;
using Core.DataAnnotations;
using Core.ApiResults;
using MediatR;

namespace ContaCorrente.Application.Features.Commands.Movimentar;

public sealed class CriarMovimentoRequest : RequestWithContext, IRequest<ApiResult<CriarMovimentoResponse>>
{
    public int NumeroConta { get; init; } = 0;

    [ValorMaiorZero]
    public decimal Valor { get; init; } = 0;

    [TipoMovimentoValido]
    public string Tipo { get; init; } = string.Empty;
}
