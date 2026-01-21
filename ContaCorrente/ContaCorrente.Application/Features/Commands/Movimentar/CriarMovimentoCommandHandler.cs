using ContaCorrente.Application.Errors;
using ContaCorrente.Application.Features.Commands.Movimentar.Service;
using Core.ApiResults;
using MediatR;
using Serilog;

namespace ContaCorrente.Application.Features.Commands.Movimentar;

public sealed class CriarMovimentoCommandHandler(ICriarMovimentoService criarMovimentoService)
    : IRequestHandler<CriarMovimentoRequest, ApiResult<CriarMovimentoResponse>>
{
    public async Task<ApiResult<CriarMovimentoResponse>> Handle(
        CriarMovimentoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await criarMovimentoService.CriaMovimentoAsync(request, cancellationToken);

            if (!result.IsSuccess)
                return ApiResult.Failure<CriarMovimentoResponse>(result.Error);

            return ApiResult.SuccessCreated(result.Data!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao realizar movimentação.");
            return ApiResult.Failure<CriarMovimentoResponse>(AppErrors.Movement.FailMovement);
        }
    }
}
