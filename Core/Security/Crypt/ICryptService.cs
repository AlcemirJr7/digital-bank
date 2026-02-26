namespace Core.Security.Crypt;

public interface ICryptService
{
    string EncryptAES(ReadOnlySpan<char> value);
    string DecryptAES(ReadOnlySpan<char> value);
}
