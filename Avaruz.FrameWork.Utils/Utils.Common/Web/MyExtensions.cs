using System.Web.SessionState;

namespace Avaruz.FrameWork.Utils.Common.Web
{
    public static class MyExtensions
    {
        /// <summary>
        /// Si el objeto ya existe no lo actualiza, sino lo crea.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddIfNull(this HttpSessionState session, string name, object value)
        {
            //El objeto existe?
            if (session[name] != null)
                //Bien si no existe hay que añadirlo
                session.Add(name, value);
        }

    }
}
