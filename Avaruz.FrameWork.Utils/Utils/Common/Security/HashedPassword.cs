using System;
using System.Collections.Generic;
using System.Linq;
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
            bool passwordMatch = false;
            // Now take the salt and the password entered by the user
            // and concatenate them together.
            string hashedPasswordAndSalt = CreatePasswordHash(suppliedPassword, salt, Algoritmo);
            // Now verify them.
            passwordMatch = hashedPasswordAndSalt.Equals(passwordHash);
            //passwordMatch = True
            return passwordMatch;
        }

        public static string CreateSalt(int size)
        {
            // Generate a cryptographic random number using the cryptographic
            // service provider
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size + 1];
            rng.GetBytes(buff);
            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        public static string CreatePasswordHash(string pwd, string salt, string algorithName)
        {
            string saltAndPwd = string.Concat(pwd, salt);
            string hashedPwd = CreateHash(saltAndPwd, algorithName);
            return hashedPwd;
        }


        public static string CreateHash(string saltAndPassword, string algorithName)
        {

            dynamic algorithm = HashAlgorithm.Create(algorithName);
            byte[] Data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            string Hashed = "";

            for (int i = 0; i <= Data.Length - 1; i++)
            {
                Hashed += Data[i].ToString("x2").ToUpperInvariant();
            }

            return Hashed;
        }

        //=======================================================
        //Service provided by Telerik (www.telerik.com)
        //Conversion powered by NRefactory.
        //Twitter: @telerik, @toddanglin
        //Facebook: facebook.com/telerik
        //=======================================================

    }
}
