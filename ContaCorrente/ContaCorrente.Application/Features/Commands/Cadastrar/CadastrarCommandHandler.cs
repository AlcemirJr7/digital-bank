using ContaCorrente.Application.Errors;
using ContaCorrente.Application.Features.Commands.Cadastrar.Validation;
using ContaCorrente.Domain.Entities;
using ContaCorrente.Domain.Repositories;
using Core.ApiResults;
using Core.Security.Crypt;
using MediatR;
using Serilog;

namespace ContaCorrente.Application.Features.Commands.Cadastrar;

public sealed class CadastrarCommandHandler(
        IContaCorrenteCommandRepository commandsRepository,
        IContaCorrenteQueryRepository queriesRepository,
        IHasher hasher,
        ICadastrarValidator validator) : IRequestHandler<CadastrarRequest, ApiResult<CadastrarResponse>>
{
    public async Task<ApiResult<CadastrarResponse>> Handle(
        CadastrarRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var conta = await queriesRepository.BuscaPeloDocumentoAsync(request.Documento);

            var validationResult = validator.Validar(request, conta);

            if (!validationResult.IsSuccess)
                return ApiResult.Failure<CadastrarResponse>(validationResult.Error);

            var hashResult = hasher.CreateHash(request.Senha);

            var numeroConta = await queriesRepository.BuscaNovoNumeroContaAsync();

            var contaCorrente = new ContaCorrenteEntity(
                numero: numeroConta,
                documento: request.Documento,
                nome: request.Nome,
                senha: hashResult.Hash,
                salt: hashResult.Salt);

            var cadastroOk = await commandsRepository.CadastrarAsync(contaCorrente);

            if (!cadastroOk)
                return ApiResult.Failure<CadastrarResponse>(AppErrors.Account.FailCreate);

            return ApiResult.SuccessCreated(
                new CadastrarResponse
                {
                    Numero = contaCorrente.Numero
                });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao cadastrar nova conta corrente.");
            return ApiResult.Failure<CadastrarResponse>(AppErrors.Account.FailCreate);
        }
    }
}
