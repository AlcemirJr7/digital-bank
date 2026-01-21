using Core.ApiResults;
using Transferencia.Application.Gateways.Models.Inputs;
using Transferencia.Application.Gateways.Models.Results;

namespace Transferencia.Application.Gateways;

public interface IContaCorrenteApiGateway
{
    Task<ApiResult<CriaMovimentoResultModel>> CriaMovimentoAsync(
        CriaMovimentoInputModel input, string chaveIdempotencia, CancellationToken ct = default);
    Task<ApiResult<SaldoContaResultModel>> ConsultaSaldoAsync(string idContaCorrente, CancellationToken ct = default);
    Task<ApiResult<IdContaResultModel>> ConsultaIdAsync(string? idContaCorrente = null, int? numeroConta = null, CancellationToken ct = default);
}
