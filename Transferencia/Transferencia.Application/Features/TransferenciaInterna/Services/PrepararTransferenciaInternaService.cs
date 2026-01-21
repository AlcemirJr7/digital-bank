using Core.ApiResults;
using Transferencia.Application.Features.TransferenciaInterna.Services.Models;
using Transferencia.Application.Features.TransferenciaInterna.Validation;
using Transferencia.Application.Gateways;

namespace Transferencia.Application.Features.TransferenciaInterna.Services;

public sealed class PrepararTransferenciaInternaService(
        IContaCorrenteApiGateway contaCorrenteApiGateway,
        ITransferenciaInternaValidator transferenciaInternaValidator) 
    : IPrepararTransferenciaInternaService
{
    public async Task<ApiResult<TransferenciaInternaResultModel>> PrepararAsync(
        TransferenciaInternaRequest request, CancellationToken ct)
    {
        var contaOrigem = await contaCorrenteApiGateway.ConsultaSaldoAsync(request.IdContaLogada, ct);

        var validationResult = transferenciaInternaValidator.Validar(request, contaOrigem.Data);

        if (!validationResult.IsSuccess)
            return ApiResult.Failure<TransferenciaInternaResultModel>(validationResult.Error);

        // Busca a identificação da conta corrente destino para gravar a transferencia
        var contaDestino = await contaCorrenteApiGateway.ConsultaIdAsync(
            numeroConta: request.NumeroContaDestino,
            ct: ct);

        if (!contaDestino.IsSuccess)
            return ApiResult.Failure<TransferenciaInternaResultModel>(contaDestino.Error);

        var transferenciaResult = new TransferenciaInternaResultModel
        {
            NumeroContaOrigem = contaOrigem.Data.Numero,
            NumeroContaDestino = request.NumeroContaDestino,
            IdContaCorrenteDestino = contaDestino.Data.IdContaCorrente,
            Valor = request.Valor
        };

        return ApiResult.Success(transferenciaResult);
    }
}
