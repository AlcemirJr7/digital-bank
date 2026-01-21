using Core.ApiResults;
using Transferencia.Application.Gateways.Models.Results;
using Transferencia.Domain.Errors;

namespace Transferencia.Application.Features.TransferenciaInterna.Validation;

public class TransferenciaInternaValidator : ITransferenciaInternaValidator
{
    public ApiResult Validar(TransferenciaInternaRequest input, SaldoContaResultModel? data)
    {
        if (data is null)
            return ApiResult.Failure(DomainErrors.Transfer.InvalidAccount);

        if (input.NumeroContaDestino == data.Numero)
            return ApiResult.Failure(DomainErrors.Transfer.SameAccount);

        if (input.Valor > data.Saldo)
            return ApiResult.Failure(DomainErrors.Transfer.InsufficientBalance);

        return ApiResult.Success();
    }
}
