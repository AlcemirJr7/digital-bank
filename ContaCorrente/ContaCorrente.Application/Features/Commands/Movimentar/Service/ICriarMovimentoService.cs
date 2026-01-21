using Core.ApiResults;

namespace ContaCorrente.Application.Features.Commands.Movimentar.Service;

public interface ICriarMovimentoService
{
    Task<ApiResult<CriarMovimentoResponse>> CriaMovimentoAsync(CriarMovimentoRequest request, CancellationToken ct);
}
