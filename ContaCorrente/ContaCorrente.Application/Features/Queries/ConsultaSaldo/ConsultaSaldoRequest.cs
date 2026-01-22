using Core.Abstractions;
using Core.ApiResults;
using MediatR;

namespace ContaCorrente.Application.Features.Queries.ConsultaSaldo;

public sealed class ConsultaSaldoRequest : RequestWithContext, IRequest<ApiResult<ConsultaSaldoResponse>>
{
}
