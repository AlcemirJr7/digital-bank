using Core.Security.Crypt;
using Microsoft.Extensions.Configuration;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace Core.Infrastructure.Security.Crypt;

public sealed class CryptService : ICryptService, IDisposable
{
    private byte[]? _key; // Removido readonly para permitir o clear total
    private const int NonceSize = 12;
    private const int TagSize = 16;
    private const int StackAllocThreshold = 1024; // Limite seguro para stackalloc

    public CryptService(IConfiguration configuration)
    {
        var keyBase64 = configuration["Encryption:Key"] ?? throw new ArgumentNullException("Encryption:Key");
        _key = Convert.FromBase64String(keyBase64);

        if (_key.Length != 32) throw new ArgumentException("A chave AES deve ter 256 bits (32 bytes).");
    }

    public string EncryptAES(ReadOnlySpan<char> plainText)
    {
        ObjectDisposedException.ThrowIf(_key is null, this);
        if (plainText.IsEmpty) return string.Empty;

        int plainTextByteCount = Encoding.UTF8.GetByteCount(plainText);
        int totalSize = NonceSize + TagSize + plainTextByteCount;

        // Gerenciamento inteligente de memória: Stack para pequenos, Pool para grandes
        byte[]? arrayFromPool = null;
        Span<byte> buffer = totalSize <= StackAllocThreshold
            ? stackalloc byte[totalSize]
            : (arrayFromPool = ArrayPool<byte>.Shared.Rent(totalSize));

        try
        {
            Span<byte> nonce = buffer.Slice(0, NonceSize);
            Span<byte> tag = buffer.Slice(NonceSize, TagSize);
            Span<byte> cipherText = buffer.Slice(NonceSize + TagSize, plainTextByteCount);

            RandomNumberGenerator.Fill(nonce);

            // Converte texto para bytes diretamente no buffer temporário
            byte[]? plainTextArrayFromPool = null;
            Span<byte> plainTextBytes = plainTextByteCount <= StackAllocThreshold
                ? stackalloc byte[plainTextByteCount]
                : (plainTextArrayFromPool = ArrayPool<byte>.Shared.Rent(plainTextByteCount));

            try
            {
                int written = Encoding.UTF8.GetBytes(plainText, plainTextBytes);
                using var aesGcm = new AesGcm(_key, TagSize);
                aesGcm.Encrypt(nonce, plainTextBytes[..written], cipherText, tag);

                return Convert.ToBase64String(buffer[..totalSize]);
            }
            finally
            {
                if (plainTextArrayFromPool != null) ArrayPool<byte>.Shared.Return(plainTextArrayFromPool, clearArray: true);
            }
        }
        finally
        {
            if (arrayFromPool != null) ArrayPool<byte>.Shared.Return(arrayFromPool, clearArray: true);
        }
    }

    public string DecryptAES(ReadOnlySpan<char> base64CipherText)
    {
        ObjectDisposedException.ThrowIf(_key is null, this);
        if (base64CipherText.IsEmpty) return string.Empty;

        // Estima tamanho do buffer Base64
        int maxBufferSize = (base64CipherText.Length * 3) / 4;
        byte[]? arrayFromPool = null;
        Span<byte> fullBuffer = maxBufferSize <= StackAllocThreshold
            ? stackalloc byte[maxBufferSize]
            : (arrayFromPool = ArrayPool<byte>.Shared.Rent(maxBufferSize));

        try
        {
            if (!Convert.TryFromBase64Chars(base64CipherText, fullBuffer, out int bytesWritten))
                throw new CryptographicException("Base64 inválido.");

            var data = fullBuffer[..bytesWritten];
            Span<byte> nonce = data.Slice(0, NonceSize);
            Span<byte> tag = data.Slice(NonceSize, TagSize);
            Span<byte> cipherText = data.Slice(NonceSize + TagSize);

            // Buffer para o resultado decriptografado
            byte[]? decryptArrayFromPool = null;
            Span<byte> decryptedBytes = cipherText.Length <= StackAllocThreshold
                ? stackalloc byte[cipherText.Length]
                : (decryptArrayFromPool = ArrayPool<byte>.Shared.Rent(cipherText.Length));

            try
            {
                using var aesGcm = new AesGcm(_key, TagSize);
                aesGcm.Decrypt(nonce, cipherText, tag, decryptedBytes);

                return Encoding.UTF8.GetString(decryptedBytes[..cipherText.Length]);
            }
            finally
            {
                if (decryptArrayFromPool != null) ArrayPool<byte>.Shared.Return(decryptArrayFromPool, clearArray: true);
            }
        }
        finally
        {
            if (arrayFromPool != null) ArrayPool<byte>.Shared.Return(arrayFromPool, clearArray: true);
        }
    }

    public void Dispose()
    {
        if (_key != null)
        {
            CryptographicOperations.ZeroMemory(_key);
            _key = null;
        }
    }
}