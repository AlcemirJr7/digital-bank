namespace Core.Security.Crypt;

public readonly record struct CryptConsts
{
    public const int KeySize = 32;
    public const int Iterations = 750_000;
    public const int SaltSize = 32;
}
