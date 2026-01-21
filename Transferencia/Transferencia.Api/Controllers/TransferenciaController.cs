using Core.Infrastructure.Abstractions;
using Core.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transferencia.Application.Features.TransferenciaInterna;

namespace Transferencia.Api.Controllers;

public class TransferenciaController(IMediator mediator) : AbstractApiController
{
    [HttpPost("TransferirInterno")]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TransferirInterno(
        TransferenciaInternaRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);

        return Response(result);
    }
}
