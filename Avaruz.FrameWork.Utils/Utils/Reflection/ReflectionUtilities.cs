using System;
using System.Reflection;

namespace Avaruz.FrameWork.Utils.Reflection
{


    public static class ReflectionUtilities
    {
        private static string _rutaEnsanblado;
        public static string RutaEnsamblado
        {
            get
            {
                return _rutaEnsanblado;
            }
            set
            {
                _rutaEnsanblado = value;
            }
        }

        private static string _nombreEnsamblado;
        public static string NombreEnsamblado
        {
            get
            {
                return _nombreEnsamblado;
            }
            set
            {
                _nombreEnsamblado = value;
            }
        }




        private static string getNombreCompletoEnsamblado()
        {
            return (RutaEnsamblado + NombreEnsamblado + ".dll");
        }


        /// <summary>
        ///     ''' Busca un metodo dentro de una clase mediante Reflection
        ///     ''' </summary>
        ///     ''' <param name="Miembros"></param>
        ///     ''' <param name="metodoABuscar"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public static bool findMetodo(MemberInfo[] Miembros, string metodoABuscar)
        {
            bool resultado = false;
            // Comprobamos si tiene el método SelectByActivo
            foreach (MemberInfo Miembro in Miembros)
            {
                if (Miembro.Name.ToUpper() == metodoABuscar.ToUpper())
                {
                    resultado = true;
                    break;
                }
            }
            return resultado;
        }
        /// <summary>
        ///     ''' Obtiene una clase base dentro de un Type mediante Reflection
        ///     ''' </summary>
        ///     ''' <param name="nombreClase"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public static Type getClaseBase(string nombreClase)
        {
            Assembly ensanbladoDb = Assembly.LoadFile(getNombreCompletoEnsamblado());
            var nombreCompletoClase = ReflectionUtilities.NombreEnsamblado + "." + nombreClase + "Db";
            Type claseDb = ensanbladoDb.GetType(nombreCompletoClase);

            ensanbladoDb = null;

            return claseDb;
        }
        /// <summary>
        ///     ''' Obtiene el nombre de la clase base de un Type
        ///     ''' </summary>
        ///     ''' <param name="Tipo"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public static string getNombreClaseBase(Type Tipo)
        {
            string nombreClase = Tipo.Name;
            int tamanio = nombreClase.Length;

            try
            {
                if (nombreClase.Contains("Info"))
                {
                    nombreClase = nombreClase.Substring(0, tamanio - 4);
                }
                else
                {
                    nombreClase = nombreClase.Substring(0, tamanio - 2);
                }
            }
            catch
            {
                nombreClase = Tipo.Name;
            }

            return nombreClase;
        }
    }
}
