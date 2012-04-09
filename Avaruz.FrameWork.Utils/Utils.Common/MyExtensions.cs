using System;

namespace Avaruz.FrameWork.Utils.Common
{
    public static class MyExtensions
    {

        public static DateTime ToDefaultDate(this string strfecha)
        {
            DateTime fecha = strfecha == "" ? DateTime.Parse("01/01/0001") : DateTime.Parse(strfecha);
            return fecha;
        }

        public static DateTime? ToNullDate(this DateTime fechaOriginal)
        {
            DateTime? fecha = fechaOriginal == DateTime.MinValue ? new DateTime?() : fechaOriginal;
            return fecha;
        }

        public static string TrimIfNeed(this string cadena, int maxLenght)
        {
            int cadenaLength = cadena.Length;
            string nuevaCadena = cadenaLength > maxLenght ? cadena.Substring(0, maxLenght) : cadena.Trim();
            return nuevaCadena;
        }

        public static string ToNoDefaultDate(this DateTime fecha)
        {
            string strfecha = fecha.ToShortDateString() == "01/01/0001" ? "" : fecha.ToShortDateString();
            return strfecha;
        }

        /// <returns>converted DateTime in string format</returns>
        private static DateTime ConvertTimestamp(double timestamp)
        {
            //create a new DateTime value based on the Unix Epoch
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            //add the timestamp to the value
            DateTime newDateTime = converted.AddSeconds(timestamp);

            //return the value in string format
            return newDateTime.ToLocalTime();
        }

    }

}
