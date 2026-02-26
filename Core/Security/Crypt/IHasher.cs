namespace Core.Security.Crypt;

public interface IHasher
{
    HashResult CreateHash(ReadOnlySpan<char> value);
}
