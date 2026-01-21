using Core.Abstractions;
using Core.ApiResults;
using Transferencia.Application.Gateways.Models.Results;

namespace Transferencia.Application.Features.TransferenciaInterna.Validation;

public interface ITransferenciaInternaValidator 
    : IValidator<TransferenciaInternaRequest, ApiResult, SaldoContaResultModel>
{}
