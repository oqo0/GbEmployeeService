using System.Security.Cryptography;
using System.Text;

namespace AspWebApiGb.Utils;

public static class PasswordUtils
{
    private const string SecretKey = "9LKl6RF12YF9XuRrLsBV9KIJYf1u0N9O";

    public static (string passwordSalt, string passwordHash) CreatePasswordHash(string password)
    {
        // generate random salt
        byte[] buffer = new byte[16];
        RNGCryptoServiceProvider secureRandom = new RNGCryptoServiceProvider();
        secureRandom.GetBytes(buffer);

        // create hash
        string passwordSalt = Convert.ToBase64String(buffer);
        string passwordHash = GetPasswordHash(password, passwordSalt);

        return (passwordSalt, passwordHash);
    }

    public static bool VerifyPassword(string password, string passwordSalt, string passwordHash)
        => GetPasswordHash(password, passwordSalt) == passwordHash;

    private static string GetPasswordHash(string password, string passwordSalt)
    {
        password = $"{password}-{passwordSalt}-{SecretKey}";
        byte[] buffer = Encoding.UTF8.GetBytes(password);

        SHA512 sha512 = new SHA512Managed();
        byte[] passwordHash = sha512.ComputeHash(buffer);

        return Convert.ToBase64String(passwordHash);
    }
}