using LEDLuxe.Core.Interfaces;
using System.Security.Cryptography;

namespace LEDLuxe.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10000;

    public string HashPassword(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256);

        var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{Iterations}.{salt}.{key}";
    }

    public bool VerifyPasswordHash(string password, string hash)
    {
        var parts = hash.Split('.', 3);

        if (parts.Length != 3)
            throw new FormatException("Unexpected hash format. Should be formatted as `{iterations}.{salt}.{hash}`");

        var iterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);

        var keyToCheck = algorithm.GetBytes(KeySize);
        var verified = ConstantTimeComparison(keyToCheck, key);

        return verified;
    }

    private static bool ConstantTimeComparison(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            return false;

        var result = 0;

        for (int i = 0; i < a.Length; i++)
            result |= a[i] ^ b[i];

        return result == 0;
    }
}