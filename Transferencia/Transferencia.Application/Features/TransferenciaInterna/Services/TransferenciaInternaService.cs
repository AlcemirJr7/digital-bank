using Core.ApiResults;
using Transferencia.Application.Features.TransferenciaInterna.Services;
using Transferencia.Application.Features.TransferenciaInterna.Services.Models;
using Transferencia.Application.Gateways;
using Transferencia.Application.Gateways.Models.Inputs;
using Transferencia.Domain.Repositories;
using Transferencia.Domain.ValueObjects;

namespace Transferencia.Application.Features.TransferenciaInterna.Service;

public sealed class TransferenciaInternaService(
        IContaCorrenteApiGateway contaCorrenteApiGateway,
        ITransferenciaCommandRepository commandRepository)
    : ITransferenciaInternaService
{
    public async Task<ApiResult> TransferirAsync(
        TransferenciaInternaInputModel input, CancellationToken ct)
    {
        var debitoResult = await DebitarAsync(input, ct);

        if (!debitoResult.IsSuccess)
            return debitoResult;

        var creditoResult = await CreditarAsync(input, ct);

        if (!creditoResult.IsSuccess)
        {
            var estornoResult = await EstornarAsync(input, ct);

            if (!estornoResult.IsSuccess)
            {
                await AlteraStatusTransferencia(input, StatusTransacao.ERRO_ESTORNO, ct);
                return estornoResult;
            }
        }

        await AlteraStatusTransferencia(input, StatusTransacao.PROCESSADO, ct);

        return ApiResult.Success();
    }

    private async Task<ApiResult> DebitarAsync(
        TransferenciaInternaInputModel input, CancellationToken ct)
    {
        var result = await contaCorrenteApiGateway.CriaMovimentoAsync(
            new CriaMovimentoInputModel
            {
                NumeroConta = input.DebitaInput.NumeroConta,
                Tipo = input.DebitaInput.Tipo,
                Valor = input.DebitaInput.Valor
            },
            chaveIdempotencia: $"{input.Transferencia.IdTransferencia}D",
            ct: ct);

        if (!result.IsSuccess)
        {
            await AlteraStatusTransferencia(input, StatusTransacao.ERRO_DEBITO, ct);
            return result;
        }

        return ApiResult.Success();
    }

    private async Task<ApiResult> CreditarAsync(
        TransferenciaInternaInputModel input, CancellationToken ct)
    {
        var result = await contaCorrenteApiGateway.CriaMovimentoAsync(
            new CriaMovimentoInputModel
            {
                NumeroConta = input.CreditaInput.NumeroConta,
                Tipo = input.CreditaInput.Tipo,
                Valor = input.CreditaInput.Valor
            },
            chaveIdempotencia: $"{input.Transferencia.IdTransferencia}C",
            ct: ct);

        if (!result.IsSuccess)
        {
            await AlteraStatusTransferencia(input, StatusTransacao.ERRO_CREDITO, ct);
            return result;
        }

        return ApiResult.Success();
    }

    private async Task<ApiResult> EstornarAsync(
        TransferenciaInternaInputModel input, CancellationToken ct)
    {
        var result = await contaCorrenteApiGateway.CriaMovimentoAsync(
            new CriaMovimentoInputModel
            {
                NumeroConta = input.DebitaInput.NumeroConta,
                Tipo = input.CreditaInput.Tipo,
                Valor = input.CreditaInput.Valor
            },
            chaveIdempotencia: $"{input.Transferencia.IdTransferencia}E",
            ct: ct);

        if (!result.IsSuccess)
        {
            await AlteraStatusTransferencia(input, StatusTransacao.ERRO_ESTORNO, ct);
            return result;
        }

        return ApiResult.Success();
    }

    private async Task AlteraStatusTransferencia(
        TransferenciaInternaInputModel input, string status, CancellationToken ct)
    {
        input.Transferencia.UpdateStatus(status);
        await commandRepository.AlteraStatusAsync(input.Transferencia, ct);
    }
}
