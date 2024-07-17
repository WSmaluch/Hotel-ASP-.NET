using System.Security.Cryptography;

namespace Hotel.Intranet.Helpers
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16; // 16 bytes for salt
        private const int HashSize = 20; // 20 bytes for hash
        private const int Iterations = 10000; // 10000 iterations

        public static string HashPassword(string password)
        {
            // Generating a random salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Hashing the password using PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Combining the salt and hash into one byte array
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Converting to base64
            string base64Hash = Convert.ToBase64String(hashBytes);

            // Returning the hashed password
            return base64Hash;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Converting base64 to bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Retrieving the salt from the hash
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Computing the hash for the provided password and salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Comparing the computed hash with the hashed password
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
