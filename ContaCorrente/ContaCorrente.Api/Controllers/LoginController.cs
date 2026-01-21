using ContaCorrente.Application.Security.Login;
using Core.Infrastructure.Abstractions;
using Core.Infrastructure.Idempotencia;
using Core.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaCorrente.Api.Controllers;

public class LoginController(IMediator mediator) : AbstractApiController
{
    [HttpPost]
    [AllowAnonymous]
    [SkipIdempotency]
    [ProducesResponseType(typeof(ApiResult<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Logar(
        LoginRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);

        return Response(result);
    }
}
