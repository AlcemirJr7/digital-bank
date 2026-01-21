using ContaCorrente.Application.Errors;
using ContaCorrente.Domain.Errors;
using ContaCorrente.Domain.Models.Inputs;
using ContaCorrente.Domain.Repositories;
using ContaCorrente.Domain.ValueObjects;
using Core.ApiResults;
using MediatR;
using Serilog;

namespace ContaCorrente.Application.Features.Queries.ConsultaId;

public sealed class ConsultaIdQueryHandler(
    IContaCorrenteQueryRepository queryRepository) : IRequestHandler<ConsultaIdRequest, ApiResult<ConsultaIdResponse>>
{
    public async Task<ApiResult<ConsultaIdResponse>> Handle(ConsultaIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var buscaConta = new BuscaGenericaInputModel
            {
                idContaCorrente = request.IdContaCorrente,
                numero = request.NumeroConta
            };

            var result = await queryRepository.ConsultaIdAsync(buscaConta, ct: cancellationToken);

            if (result is null)
                return ApiResult.Failure<ConsultaIdResponse>(DomainErrors.Account.Invalid);

            if (!FlagAtivo.IsAtivo(result.Ativo))
                return ApiResult.Failure<ConsultaIdResponse>(DomainErrors.Account.Inactive);

            return ApiResult.Success(new ConsultaIdResponse
            {
                IdContaCorrente = result.IdContaCorrente,
                Numero = result.Numero
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao consultar identificação da conta corrente.");
            return ApiResult.Failure<ConsultaIdResponse>(AppErrors.Account.FailConsultId);
        }
    }
}
