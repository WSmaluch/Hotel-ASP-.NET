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
            // Generowanie losowego soli
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Haszowanie hasła z użyciem PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Łączenie soli i hasza w jedną tablicę bajtów
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Konwersja na base64
            string base64Hash = Convert.ToBase64String(hashBytes);

            // Zwrócenie zahaszowanego hasła
            return base64Hash;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Konwersja base64 na bajty
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Pobranie soli z hashu
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Obliczenie hasza dla podanego hasła i soli
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Porównanie obliczonego hasza z zahashowanym hasłem
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
