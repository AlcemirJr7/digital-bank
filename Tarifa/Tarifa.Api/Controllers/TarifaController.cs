using Core.Infrastructure.Abstractions;
using Core.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tarifa.Application.Features.Tarifar;

namespace Tarifa.Api.Controllers;

public class TarifaController(IMediator mediator) : AbstractApiController
{
    [HttpPost("Tarifar")]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Tarifar(
        TarifarRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);

        return Response(result);
    }
}
