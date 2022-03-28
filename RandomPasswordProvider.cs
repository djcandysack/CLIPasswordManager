using System.Security.Cryptography;

namespace Sql;

public static class RandomPasswordProvider
{
    public static string GetRandomPassword(int length)
    {
        using var generator = RandomNumberGenerator.Create();

        var random = new byte[length - 4];
        generator.GetBytes(random);

        return Convert.ToBase64String(random);
    }
}