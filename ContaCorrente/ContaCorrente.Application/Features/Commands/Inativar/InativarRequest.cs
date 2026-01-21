using Core.DataAnnotations;
using Core.Definitions;
using Core.ApiResults;
using MediatR;

namespace ContaCorrente.Application.Features.Commands.Inativar;

public sealed class InativarRequest : IRequest<ApiResult>
{
    [TamanhoValido(AtributosDefinitions.DocumentoMaxLength)]
    public string IdContaCorrente { get; init; } = string.Empty;

    [TamanhoValido(AtributosDefinitions.SenhaHashMaxLength)]
    public string Senha { get; init; } = string.Empty;
}
