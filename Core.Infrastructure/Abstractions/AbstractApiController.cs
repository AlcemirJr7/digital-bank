using Asp.Versioning;
using Core.ApiResults;
using Core.Idempotencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Infrastructure.Abstractions;

[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class AbstractApiController : ControllerBase
{
    // Método de extensão Response (já existente)
    // A implementação deve garantir que o StatusCode do ApiResponse seja usado para o StatusCode HTTP.
    protected IActionResult Response<TData>(ApiResult<TData> apiResponse)
    {
        if (apiResponse.IsSuccess)
        {
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }
        else
        {
            // Para falhas, usamos o StatusCode fornecido no ApiResponse
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }
    }

    protected IActionResult Response(ApiResult apiResponse)
    {
        if (apiResponse.IsSuccess)
        {
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }
        else
        {
            // Para falhas, usamos o StatusCode fornecido no ApiResponse
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }
    }
}