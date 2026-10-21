using System;
using System.Globalization;
using System.Security.Cryptography;

namespace ProjectSecurity
{
    public static class PasswordSecurity
    {
        private const int Iterations = 600000;
        private const string Prefix = "pbkdf2-sha256";

        public static string Hash(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Password is required.");
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);
            using (var kdf = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                return Prefix + "$" + Iterations.ToString(CultureInfo.InvariantCulture) + "$" +
                    Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(kdf.GetBytes(32));
        }

        public static bool IsHash(string stored)
        {
            int iterations; byte[] salt, expected;
            return TryParse(stored, out iterations, out salt, out expected);
        }

        public static bool Verify(string password, string stored)
        {
            int iterations; byte[] salt, expected;
            if (password == null || !TryParse(stored, out iterations, out salt, out expected)) return false;
            using (var kdf = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] actual = kdf.GetBytes(expected.Length);
                int difference = 0;
                for (int i = 0; i < actual.Length; i++) difference |= actual[i] ^ expected[i];
                return difference == 0;
            }
        }

        private static bool TryParse(string stored, out int iterations, out byte[] salt, out byte[] expected)
        {
            iterations = 0; salt = null; expected = null;
            if (string.IsNullOrEmpty(stored)) return false;
            string[] parts = stored.Split('$');
            if (parts.Length != 4 || parts[0] != Prefix ||
                !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out iterations) ||
                iterations < Iterations || iterations > 2000000) return false;
            try
            {
                salt = Convert.FromBase64String(parts[2]); expected = Convert.FromBase64String(parts[3]);
                return salt.Length == 16 && expected.Length == 32;
            }
            catch (FormatException) { return false; }
        }
    }
}
