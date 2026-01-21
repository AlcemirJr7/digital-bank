using Core.ApiResults;
using MediatR;

namespace ContaCorrente.Application.Features.Queries.ConsultaId;

public sealed class ConsultaIdRequest : IRequest<ApiResult<ConsultaIdResponse>>
{
    public string? IdContaCorrente { get; set; } = string.Empty;
    public int? NumeroConta { get; set; } = 0;
}
