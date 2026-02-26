namespace Core.Security.Auth;

public interface ILogin
{
    bool ValidaSenha(ReadOnlySpan<char> senha, ReadOnlySpan<char> hash, ReadOnlySpan<char> salt);
}
