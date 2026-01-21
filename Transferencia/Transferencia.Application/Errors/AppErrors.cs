using Core.ApiResults;

namespace Transferencia.Application.Errors;

public readonly record struct AppErrors
{
    public readonly record struct Transfer
    {
        public static readonly ErrorDetails InsufficientBalance =
            new("INSUFFICIENT_BALANCE", "Saldo insuficiente.");

        public static readonly ErrorDetails FailTransfer =
            new("FAIL_TRANSFER", "Falha ao efetuar transferencia.");

        public static readonly ErrorDetails FailValidation =
            new("FAIL_TRANSFER_VALIDATION", "Falha ao validar dados da conta.");

        public static readonly ErrorDetails FailCreateTransfer =
            new("FAIL_CREATE_TRANSFER", "Falha ao criar transferência.");
    }
}
