using System;
using System.Security.Cryptography;
using System.Text;

namespace CustomFramework.BaseWebApi.Data.Utils
{
    public static class HashString
    {
        public static string GetSalt()
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string Hash(string value, string salt, int iterationCount)
        {
            var hashed = Convert.ToBase64String(Pbkdf2(
                password: value,
                salt: Encoding.ASCII.GetBytes(salt),
                iterations: iterationCount,
                outputBytes: 256 / 8));

            return hashed;
        }

        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt) { IterationCount = iterations };
            return pbkdf2.GetBytes(outputBytes);
        }
    }

}