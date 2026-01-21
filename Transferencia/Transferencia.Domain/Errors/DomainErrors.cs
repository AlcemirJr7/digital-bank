using Core.ApiResults;

namespace Transferencia.Domain.Errors;
public readonly record struct DomainErrors
{
    public readonly record struct Transfer
    {
        public static readonly ErrorDetails InvalidAccount =
            new("INVALID_ACCOUNT", "Conta de transferência inválida.");

        public static readonly ErrorDetails InsufficientBalance =

            new("INSUFFICIENT_BALANCE", "Saldo insuficiente para a transferência.");

        public static readonly ErrorDetails SameAccount =
            new("SAME_ACCOUNT", "Conta de origem e destino não podem ser iguais.");
    }
}