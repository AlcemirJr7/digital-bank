using Core.ApiResults;
using Transferencia.Application.Features.TransferenciaInterna.Services.Models;

namespace Transferencia.Application.Features.TransferenciaInterna.Services;

public interface ITransferenciaInternaService
{
    Task<ApiResult> TransferirAsync(TransferenciaInternaInputModel input, CancellationToken ct);
}
