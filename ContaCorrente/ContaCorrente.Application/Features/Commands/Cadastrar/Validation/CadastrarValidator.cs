using ContaCorrente.Domain.Errors;
using ContaCorrente.Domain.Models.Results;
using Core.ApiResults;

namespace ContaCorrente.Application.Features.Commands.Cadastrar.Validation;

public sealed class CadastrarValidator : ICadastrarValidator
{
    public ApiResult Validar(CadastrarRequest input, ContaCorrenteResultModel? data)
    {
        if (data is not null)
            return ApiResult.Failure(DomainErrors.Account.AlreadyExists);

        if (!IsDocumentoValido(input.Documento))
            return ApiResult.Failure(DomainErrors.Account.InvalidDocument);

        if (!IsSenhaStrong(input.Senha))
            return ApiResult.Failure(DomainErrors.Account.WeakPassword);

        return ApiResult.Success();
    }

    private bool IsDocumentoValido(string documento) => documento.Length == 11;

    private bool IsSenhaStrong(string senha) => senha.Length >= 8;
}
