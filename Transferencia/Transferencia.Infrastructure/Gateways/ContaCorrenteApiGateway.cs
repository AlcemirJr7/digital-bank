using Core.Idempotencia;
using Core.Infrastructure.Extensions;
using Core.ApiResults;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using System.Net.Http.Json;
using Transferencia.Application.Errors;
using Transferencia.Application.Gateways;
using Transferencia.Application.Gateways.Models.Inputs;
using Transferencia.Application.Gateways.Models.Results;

namespace Transferencia.Infrastructure.Gateways;

public sealed class ContaCorrenteApiGateway : IContaCorrenteApiGateway
{
    private readonly HttpClient _httpClient;

    private const string ENDPOINT_MOVIMENTAR = "/api/v1/ContaCorrente/Movimentar";
    private const string ENDPOINT_CONSULTA_SALDO = "/api/v1/ContaCorrente/ConsultaSaldo";
    private const string ENDPOINT_CONSULTA_ID = "/api/v1/ContaCorrente/ConsultaId";

    public ContaCorrenteApiGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResult<SaldoContaResultModel>> ConsultaSaldoAsync(string idContaCorrente, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(ENDPOINT_CONSULTA_SALDO, ct);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResult<SaldoContaResultModel>>(ct);

            if (apiResponse is null)
            {
                Log.Error(@"Falha ao desserializar resposta da API de Conta Corrente ao consultar saldo. 
                            Status: {StatusCode}", response.StatusCode);
                return ApiResult.Failure<SaldoContaResultModel>(GatewayErros.ContaCorrenteApi.FailConsultBalanceRequest);
            }

            if (!apiResponse.IsSuccess)
                return ApiResult.Failure<SaldoContaResultModel>(apiResponse.Error ?? GatewayErros.ContaCorrenteApi.FailConsultBalanceRequest);

            if (!response.IsSuccessStatusCode)
                Log.Warning(@"API de Conta Corrente retornou status {StatusCode} mas corpo indica sucesso.", response.StatusCode);

            return apiResponse;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Erro ao fazer requisição HTTP para consultar saldo: {ENDPOINT_CONSULTA_SALDO}");
            return ApiResult.Failure<SaldoContaResultModel>(GatewayErros.ContaCorrenteApi.FailConsultBalanceRequest);
        }
    }

    public async Task<ApiResult<IdContaResultModel>> ConsultaIdAsync(
        string? idContaCorrente = null, int? numeroConta = null, CancellationToken ct = default)
    {
        try
        {
            var query = new QueryBuilder
            {
                { "IdContaCorrente", idContaCorrente ?? string.Empty },
                { "numeroConta", numeroConta.ToString() ?? string.Empty }
            };

            var response = await _httpClient.GetAsync($"{ENDPOINT_CONSULTA_ID}{query}", ct);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResult<IdContaResultModel>>(ct);

            if (apiResponse is null)
            {
                Log.Error(@"Falha ao desserializar resposta da API de Conta Corrente ao consultar identificação. 
                            Status: {StatusCode}", response.StatusCode);
                return ApiResult.Failure<IdContaResultModel>(GatewayErros.ContaCorrenteApi.FailConsultIdRequest);
            }

            if (!apiResponse.IsSuccess)
                return ApiResult.Failure<IdContaResultModel>(apiResponse.Error ?? GatewayErros.ContaCorrenteApi.FailConsultIdRequest);

            if (!response.IsSuccessStatusCode)
                Log.Warning(@"API de Conta Corrente retornou status {StatusCode} mas corpo indica sucesso.", response.StatusCode);

            return ApiResult.Success(
                new IdContaResultModel
                {
                    IdContaCorrente = apiResponse.Data!.IdContaCorrente,
                    Numero = apiResponse.Data?.Numero ?? 0
                });
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Erro ao fazer requisição HTTP para consultar identificação da conta: {ENDPOINT_CONSULTA_ID}");
            return ApiResult.Failure<IdContaResultModel>(GatewayErros.ContaCorrenteApi.FailConsultIdRequest);
        }
    }

    public async Task<ApiResult<CriaMovimentoResultModel>> CriaMovimentoAsync(
        CriaMovimentoInputModel input, string chaveIdempotencia, CancellationToken ct = default)
    {
        try
        {
            var requestData = new
            {
                input.NumeroConta,
                input.Valor,
                input.Tipo
            };

            var headers = new Dictionary<string, string>
            {
                { IdempotenciaConsts.HeaderKeyName, chaveIdempotencia }
            };

            var response = await _httpClient.PostAsJsonWithHeadersAsync(ENDPOINT_MOVIMENTAR, requestData, headers, ct);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResult<CriaMovimentoResultModel>>(ct);

            if (apiResponse is null)
            {
                Log.Error(@"Falha ao desserializar resposta da API de Conta Corrente para movimentação. 
                            Status: {StatusCode}", response.StatusCode);
                return ApiResult.Failure<CriaMovimentoResultModel>(GatewayErros.ContaCorrenteApi.FailMovementRequest);
            }

            if (!apiResponse.IsSuccess)
                return ApiResult.Failure<CriaMovimentoResultModel>(apiResponse.Error ?? GatewayErros.ContaCorrenteApi.FailMovementRequest);

            if (!response.IsSuccessStatusCode)
                Log.Warning(@"API de Conta Corrente retornou status {StatusCode} mas corpo indica sucesso para movimentação. 
                              IdMovimento: {IdMovimento}", response.StatusCode, apiResponse.Data?.IdMovimento);

            return apiResponse;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Erro ao fazer requisição HTTP para creditar: {ENDPOINT_MOVIMENTAR}");
            return ApiResult.Failure<CriaMovimentoResultModel>(GatewayErros.ContaCorrenteApi.FailMovementRequest);
        }
    }
}
