using System;
using System.Security.Cryptography;
using System.Text;


namespace Avaruz.FrameWork.Utils.Common.Security
{
    public static class HashedPassword
    {
        /// <summary>
        /// Verifica el password guardado con algun salt
        /// </summary>
        /// <param name="suppliedPassword">Password en texto plano</param>
        /// <param name="passwordHash">El password en forma de Hash</param>
        /// <param name="salt">el Salt que esta guardado</param>
        /// <param name="Algoritmo">El Algoritmo</param>
        /// <returns></returns>
        public static bool VerifyPassword(string suppliedPassword, string passwordHash, string salt, string Algoritmo)
        {
            // Now take the salt and the password entered by the user
            // and concatenate them together.
            string hashedPasswordAndSalt = CreatePasswordHash(suppliedPassword, salt, Algoritmo);
            // Now verify them.

            //passwordMatch = True
            return hashedPasswordAndSalt.Equals(passwordHash);
        }

        // Pseudocode Plan:
        // - Validate input size: must be >= 0 (to preserve existing behavior where size+1 bytes are produced)
        // - Allocate a byte array of size + 1 to maintain original output length behavior
        // - Use RandomNumberGenerator.GetBytes to fill the array with cryptographically strong random bytes
        // - Convert to Base64 and return

        public static string CreateSalt(int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Salt size must be non-negative.");
            }

            byte[] buff = RandomNumberGenerator.GetBytes(size + 1);
            return Convert.ToBase64String(buff);
        }

        public static string CreatePasswordHash(string pwd, string salt, string algorithName)
        {
            string saltAndPwd = string.Concat(pwd, salt);
            return CreateHash(saltAndPwd, algorithName);
        }


        public static string CreateHash(string saltAndPassword, string algorithName)
        {
            HashAlgorithm algorithm = algorithName switch
            {
                "SHA256" => SHA256.Create(),
                "SHA384" => SHA384.Create(),
                "SHA512" => SHA512.Create(),
                "MD5" => MD5.Create(),
                "SHA1" => SHA1.Create(),
                _ => throw new ArgumentException($"Unsupported hash algorithm: {algorithName}", nameof(algorithName))
            };

            byte[] Data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            string Hashed = "";

            for (int i = 0; i <= Data.Length - 1; i++)
            {
                Hashed += Data[i].ToString("x2").ToUpperInvariant();
            }

            return Hashed;
        }


    }
}
