using Core.ApiResults;
using MediatR;
using Serilog;
using Tarifa.Application.Errors;
using Tarifa.Domain.Entities;
using Tarifa.Domain.Repositories;

namespace Tarifa.Application.Features.Tarifar;

public sealed class TarifarCommandHandler(
    ICommandRepository commandRepository)
    : IRequestHandler<TarifarRequest, ApiResult>
{
    public async Task<ApiResult> Handle(TarifarRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var tarifa = new TarifaEntity(request.IdContaCorrete, request.DataMovimento, request.Valor);

            var result = await commandRepository.CreateAsync(tarifa, cancellationToken);

            if (!result)
                return ApiResult.Failure(AppErrors.Fee.FailCreate);

            return ApiResult.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao processar tarifa.");
            return ApiResult.Failure(AppErrors.Fee.FailFee);
        }
    }
}
