using Core.ApiResults;
using MediatR;

namespace ContaCorrente.Application.Features.Queries.ConsultaSaldo;

public sealed class ConsultaSaldoRequest : IRequest<ApiResult<ConsultaSaldoResponse>>
{
    public string IdContaCorrente { get; set; } = string.Empty;
}
