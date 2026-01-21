using ContaCorrente.Domain.Models.Results;
using Core.Abstractions;
using Core.ApiResults;

namespace ContaCorrente.Application.Features.Commands.Cadastrar.Validation;

public interface ICadastrarValidator : IValidator<CadastrarRequest, ApiResult, ContaCorrenteResultModel>
{
}
