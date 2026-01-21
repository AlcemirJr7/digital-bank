using ContaCorrente.Domain.Errors;
using ContaCorrente.Domain.Models.Results;
using ContaCorrente.Domain.ValueObjects;
using Core.ApiResults;

namespace ContaCorrente.Application.Features.Commands.Inativar.Validation;

public sealed class InativarValidator : IInativarValidator
{
    public ApiResult Validar(InativarRequest input, ContaCorrenteResultModel? data)
    {
        if (data is null)
            return ApiResult.Failure(DomainErrors.Account.Invalid);

        if (!FlagAtivo.IsAtivo(data.Ativo))
            return ApiResult.Failure(DomainErrors.Account.Inactive);

        return ApiResult.Success();
    }
}
