using System.Security.Cryptography;
using System.Text;

namespace Logic
{
    public static class PasswordHash
    {
        const int keySize = 64;
        const int saltSize = 8;
        const int iterations = 1200;
        static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize
            );

            return $"SHA512${Convert.ToHexString(salt)}${iterations}${Convert.ToHexString(hash)}";
        }

        public static bool Verify(string password, string hash)
        {
            string[] hashParts = hash.Split("$");
            if (hashParts.Length < 4)
            {
                throw new ArgumentException("Unrecognized hash format.");
            }

            if (hashParts[0] != "SHA512")
            {
                throw new ArgumentException("Unknown hash algoritm.");
            }

            byte[] salt = Convert.FromHexString(hashParts[1]);
            int iterations = int.Parse(hashParts[2]);

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize
            );

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hashParts[3]));
        }
    }
}
