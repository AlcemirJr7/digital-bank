using Core.Messengers.Models;
using Core.ApiResults;
using MediatR;
using Transferencia.Application.Errors;
using Transferencia.Application.Features.TransferenciaInterna.Services;
using Transferencia.Application.Features.TransferenciaInterna.Services.Models;
using Transferencia.Application.Messengers;
using Transferencia.Domain.Entities;
using Transferencia.Domain.Repositories;
using Transferencia.Domain.ValueObjects;

namespace Transferencia.Application.Features.TransferenciaInterna;

public sealed class TransferenciaInternaCommandHandler(
        IPrepararTransferenciaInternaService prepararTransferenciaInternaService,
        ITransferenciaCommandRepository commandRepository,
        ITransferenciaInternaService transferenciaInternaService,
        ITransferenciasRealizadasProducerMessenger transferenciasRealizadasMessenger) 
    : IRequestHandler<TransferenciaInternaRequest, ApiResult>
{
    public async Task<ApiResult> Handle(TransferenciaInternaRequest request, CancellationToken cancellationToken)
    {
        var prepararResult = await prepararTransferenciaInternaService.PrepararAsync(request, cancellationToken);

        if (!prepararResult.IsSuccess)
            return prepararResult;

        var transferenciaModel = prepararResult.Data;

        var transferencia = new TransferenciaEntity(
                request.IdContaLogada,
                transferenciaModel.IdContaCorrenteDestino,
                transferenciaModel.Valor,
                StatusTransacao.PENDENTE);

        var criaTransferenciaResult = await commandRepository.CriarAsync(transferencia, cancellationToken);

        if (!criaTransferenciaResult)
            return ApiResult.Failure(AppErrors.Transfer.FailCreateTransfer);

        var transferenciaResult = await transferenciaInternaService.TransferirAsync(
            new TransferenciaInternaInputModel(
                transferencia,
                transferenciaModel.NumeroContaOrigem,
                transferenciaModel.NumeroContaDestino,
                transferenciaModel.Valor
            ), cancellationToken);

        if (!transferenciaResult.IsSuccess)
            return transferenciaResult;

        await transferenciasRealizadasMessenger.EnviarAsync(
                new TransferenciasRealizadasMessage
                {
                    IdTransferencia = transferencia.IdTransferencia,
                    IdContaCorrente = request.IdContaLogada,
                    Valor = request.Valor
                });

        return ApiResult.Success();
    }
}
