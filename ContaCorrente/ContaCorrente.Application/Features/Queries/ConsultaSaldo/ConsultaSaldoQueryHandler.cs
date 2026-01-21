using ContaCorrente.Application.Errors;
using ContaCorrente.Domain.Errors;
using ContaCorrente.Domain.Repositories;
using ContaCorrente.Domain.ValueObjects;
using Core.ApiResults;
using MediatR;
using Serilog;

namespace ContaCorrente.Application.Features.Queries.ConsultaSaldo;

public sealed class ConsultaSaldoQueryHandler(IContaCorrenteQueryRepository queryRepository) : IRequestHandler<ConsultaSaldoRequest, ApiResult<ConsultaSaldoResponse>>
{
    public async Task<ApiResult<ConsultaSaldoResponse>> Handle(
        ConsultaSaldoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await queryRepository.ConsultaSaldoAsync(request.IdContaCorrente, ct: cancellationToken);

            if (result is null)
                return ApiResult.Failure<ConsultaSaldoResponse>(DomainErrors.Account.Invalid);

            if (!FlagAtivo.IsAtivo(result.Ativo))
                return ApiResult.Failure<ConsultaSaldoResponse>(DomainErrors.Account.Inactive);

            return ApiResult.Success(
                new ConsultaSaldoResponse
                {
                    IdContaCorrente = result.IdContaCorrente,
                    Numero = result.Numero,
                    Saldo = result.Saldo
                });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao consultar saldo da conta corrente.");
            return ApiResult.Failure<ConsultaSaldoResponse>(AppErrors.Account.FailConsultBalance);
        }
    }
}
