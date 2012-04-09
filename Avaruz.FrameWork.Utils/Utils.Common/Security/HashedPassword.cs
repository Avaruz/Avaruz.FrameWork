using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Avaruz.FrameWork.Utils.Common.Security
{
    public static class HashedPassword
    {
        public static bool VerifyPassword(string suppliedPassword, string passwordHash, string salt)
        {
            bool passwordMatch = false;
            // Now take the salt and the password entered by the user
            // and concatenate them together.
            string hashedPasswordAndSalt = CreatePasswordHash(suppliedPassword, salt);
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

        public static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = string.Concat(pwd, salt);
            string hashedPwd = "";
            hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "SHA1");
            return hashedPwd;
        }

        //=======================================================
        //Service provided by Telerik (www.telerik.com)
        //Conversion powered by NRefactory.
        //Twitter: @telerik, @toddanglin
        //Facebook: facebook.com/telerik
        //=======================================================

    }
}
