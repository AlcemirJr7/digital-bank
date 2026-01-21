using Core.DataAnnotations;
using Core.Definitions;
using Core.ApiResults;
using MediatR;

namespace ContaCorrente.Application.Features.Commands.Cadastrar;

public sealed class CadastrarRequest : IRequest<ApiResult<CadastrarResponse>>
{
    [TamanhoValido(AtributosDefinitions.DocumentoMaxLength)]
    public string Documento { get; init; } = string.Empty;

    [TamanhoValido(AtributosDefinitions.NomeMaxLength)]
    public string Nome { get; init; } = string.Empty;

    [TamanhoValido(AtributosDefinitions.SenhaSaltMaxLength)]
    public string Senha { get; init; } = string.Empty;
}
