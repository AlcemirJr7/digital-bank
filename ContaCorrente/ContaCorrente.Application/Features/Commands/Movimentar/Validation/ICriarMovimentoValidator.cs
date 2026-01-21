using ContaCorrente.Domain.Models.Results;
using Core.Abstractions;
using Core.ApiResults;

namespace ContaCorrente.Application.Features.Commands.Movimentar.Validation;

public interface ICriarMovimentoValidator : IValidator<CriarMovimentoRequest, ApiResult, ContaCorrenteResultModel>
{
}
