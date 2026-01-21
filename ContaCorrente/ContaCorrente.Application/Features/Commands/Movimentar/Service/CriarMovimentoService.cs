using ContaCorrente.Application.Errors;
using ContaCorrente.Application.Features.Commands.Movimentar.Validation;
using ContaCorrente.Domain.Entities;
using ContaCorrente.Domain.Models.Inputs;
using ContaCorrente.Domain.Repositories;
using Core.ApiResults;
using Serilog;

namespace ContaCorrente.Application.Features.Commands.Movimentar.Service;

public sealed class CriarMovimentoService(
    IContaCorrenteQueryRepository queryRepository,
    ICriarMovimentoValidator validator,
    IContaCorrenteCommandRepository commandRepository)
    : ICriarMovimentoService
{
    public async Task<ApiResult<CriarMovimentoResponse>> CriaMovimentoAsync(
        CriarMovimentoRequest request, CancellationToken ct)
    {
        try
        {
            var buscaConta = new BuscaGenericaInputModel(numero: request.NumeroConta);

            var conta = await queryRepository.BuscaContaCorrenteAsync(buscaConta, ct);

            var validationResult = validator.Validar(request, conta);

            if (!validationResult.IsSuccess)
                return ApiResult.Failure<CriarMovimentoResponse>(validationResult.Error);

            var movimento = new MovimentoEntity(conta!.IdContaCorrente, request.Tipo, request.Valor);

            await commandRepository.MovimentarAsync(movimento, ct);

            return ApiResult.Success(new CriarMovimentoResponse
            {
                IdMovimento = movimento.IdMovimento
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao realizar movimentação.");
            return ApiResult.Failure<CriarMovimentoResponse>(AppErrors.Movement.FailMovement);
        }
    }
}
