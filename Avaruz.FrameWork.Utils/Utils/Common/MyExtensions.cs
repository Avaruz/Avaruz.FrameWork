using System;

namespace Avaruz.FrameWork.Utils.Common
{
    public static class MyExtensions
    {
        public static T IfChangeUpdate<T>(T value1, T value2)
        {
            return !value1.Equals(value2) ? value2 : value1;
        }

        public static string Googlelizer(this string cadenaOriginal)
        {
            //Primero partir la cadena

            string googlelizeString =
                cadenaOriginal.Replace("S", "[0]")
                              .Replace("Z", "[1]")
                              .Replace("C", "[2]")
                              .Replace("G", "[3]")
                              .Replace("Ñ", "[4]")
                              .Replace("N", "[5]")
                              .Replace("J", "[6]")
                              .Replace("B", "[7]")
                              .Replace("V", "[8]")
                              .Replace("Y", "[9]")
                              .Replace("I", "[10]");

            googlelizeString =
    googlelizeString.Replace("[0]", "[SCZ]")
                  .Replace("[1]", "[SCZ]")
                  .Replace("[2]", "[SCZ]")
                  .Replace("[3]", "[GJ]")
                  .Replace("[4]", "[NÑ]")
                  .Replace("[5]", "[ÑN]")
                  .Replace("[6]", "[GJ]")
                  .Replace("[7]", "[BV]")
                  .Replace("[8]", "[BV]")
                  .Replace("[9]", "[YI]")
                  .Replace("[10]", "[YI]");


            return googlelizeString;
        }


        public static DateTime ToDefaultDate(this string strfecha)
        {
            return strfecha == "" ? DateTime.Parse("01/01/0001") : DateTime.Parse(strfecha);
        }

        public static DateTime? ToNullDate(this DateTime fechaOriginal)
        {
            return fechaOriginal == DateTime.MinValue ? new DateTime?() : fechaOriginal;
        }

        public static string TrimIfNeed(this string cadena, int maxLenght)
        {
            var cadenaLength = cadena.Length;
            return cadenaLength > maxLenght ? cadena.Substring(0, maxLenght) : cadena.Trim();
        }

        public static string ToNoDefaultDate(this DateTime fecha)
        {
            return fecha.ToShortDateString() == "01/01/0001" ? "" : fecha.ToShortDateString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="S"></param>
        /// <param name="T"></param>
        public static void CopyTo(this object S, object T)
        {
            foreach (var pS in S.GetType().GetProperties())
            {
                foreach (var pT in T.GetType().GetProperties())
                {
                    if (pT.Name != pS.Name)
                    {
                        continue;
                    } (pT.GetSetMethod()).Invoke(T, new object[] { pS.GetGetMethod().Invoke(S, null) });
                }
            }
        }

        public static string ToSiNo(this bool valor)
        {
            return valor ? "Si" : "No";
        }

    }

}
