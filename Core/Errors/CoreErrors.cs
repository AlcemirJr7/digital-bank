using Core.Idempotencia;
using Core.ApiResults;

namespace Core.Errors;

public readonly record struct CoreErrors
{
    public readonly record struct Idempotency
    {
        public static readonly ErrorDetails HeaderIdempotencia =
            new("HEADER_IDEMPOTENCIA", $"Header {IdempotenciaConsts.HeaderKeyName} é obrigatório");

        public static readonly ErrorDetails InvalidLength =
            new("INVALID_IDEMPOTENCIA_LENGTH", $"{IdempotenciaConsts.HeaderKeyName} deve ter no máximo {IdempotenciaConsts.MaxLength} caracteres.");
    }
}
