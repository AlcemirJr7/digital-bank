using Core.Abstractions;
using Core.ApiResults;
using Transferencia.Application.Features.TransferenciaInterna.Services.Models;

namespace Transferencia.Application.Features.TransferenciaInterna.Services;

public interface IPrepararTransferenciaInternaService : 
    IPrepararService<TransferenciaInternaRequest, ApiResult<TransferenciaInternaResultModel>>
{
}
