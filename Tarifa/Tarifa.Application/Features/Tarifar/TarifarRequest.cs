using Core.DataAnnotations;
using Core.Definitions;
using Core.ApiResults;
using MediatR;

namespace Tarifa.Application.Features.Tarifar;

public sealed class TarifarRequest : IRequest<ApiResult>
{
    [TamanhoValido(AtributosDefinitions.PrimaryKeyMaxLength)]
    public string IdContaCorrete { get; set; } = string.Empty;

    [DataValida]
    public string DataMovimento { get; set; } = string.Empty;

    [ValorMaiorZero]
    public decimal Valor { get; set; }
}
