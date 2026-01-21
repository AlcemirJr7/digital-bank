namespace Core.Security.Auth;

public sealed record AutenticaInputModel
{
    public string Senha { get; init; } = string.Empty;
    public string Hash { get; init; } = string.Empty;
    public string Salt { get; init; } = string.Empty;
}
