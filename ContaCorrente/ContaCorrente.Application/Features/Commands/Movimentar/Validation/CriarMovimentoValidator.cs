using ContaCorrente.Domain.Errors;
using ContaCorrente.Domain.Models.Results;
using ContaCorrente.Domain.ValueObjects;
using Core.ApiResults;
using Core.ValueObjects;

namespace ContaCorrente.Application.Features.Commands.Movimentar.Validation;

public sealed class CriarMovimentoValidator : ICriarMovimentoValidator
{
    public ApiResult Validar(CriarMovimentoRequest input, ContaCorrenteResultModel? data)
    {
        if (data is null)
            return ApiResult.Failure(DomainErrors.Account.Invalid);

        if (!FlagAtivo.IsAtivo(data.Ativo))
            return ApiResult.Failure(DomainErrors.Account.Inactive);

        if (!TipoMovimento.IsValid(input.Tipo))
            return ApiResult.Failure(DomainErrors.Movement.InvalidType);

        if (input.Valor <= 0)
            return ApiResult.Failure(DomainErrors.Movement.InvalidValue);

        if (input.IdContaLogada != data?.IdContaCorrente && !TipoMovimento.IsCredito(input.Tipo))
            return ApiResult.Failure(DomainErrors.Movement.Invalid);

        return ApiResult.Success();
    }
}
