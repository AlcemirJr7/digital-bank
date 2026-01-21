using Transferencia.Domain.Entities;

namespace Transferencia.Application.Features.TransferenciaInterna.Services.Models;

public record TransferenciaInternaInputModel
{
    public TransferenciaEntity Transferencia { get; init; }
    public DebitaInputModel DebitaInput { get; }
    public CreditaInputModel CreditaInput { get; }

    public TransferenciaInternaInputModel(
        TransferenciaEntity transferencia, 
        int contaOrigem, 
        int contaDestino, 
        decimal valor)
    {
        Transferencia = transferencia;
        DebitaInput = new DebitaInputModel(contaOrigem, valor);
        CreditaInput = new CreditaInputModel(contaDestino, valor);
    }
}
