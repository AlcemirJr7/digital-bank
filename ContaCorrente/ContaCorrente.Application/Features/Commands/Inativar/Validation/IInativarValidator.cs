using ContaCorrente.Domain.Models.Results;
using Core.Abstractions;
using Core.ApiResults;

namespace ContaCorrente.Application.Features.Commands.Inativar.Validation;

public interface IInativarValidator : IValidator<InativarRequest, ApiResult, ContaCorrenteResultModel>
{
}
