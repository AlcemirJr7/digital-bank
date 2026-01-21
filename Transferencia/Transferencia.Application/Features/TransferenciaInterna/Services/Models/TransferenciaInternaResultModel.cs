namespace Transferencia.Application.Features.TransferenciaInterna.Services.Models;

public record TransferenciaInternaResultModel
{
    public int NumeroContaOrigem { get; init; }
    public int NumeroContaDestino { get; init; }
    public string IdContaCorrenteDestino { get; init; } = null!;
    public decimal Valor { get; init; }
}
