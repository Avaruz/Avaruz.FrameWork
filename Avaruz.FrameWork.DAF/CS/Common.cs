using System.Configuration;

namespace Avaruz.FrameWork.DAF
{
	/// <summary>
	/// Obtiene el Assembly necesario para la conexión
	/// </summary>
	public sealed class Common
	{
        
		/// <summary>
		/// Devuelve un assemby necesario para la conexión
		/// </summary>
		/// <param name="strHelper">Este parametro debe coincidir con el que esta en su archivo .config</param>
		/// <returns>AdoHelper</returns>
        /// 

		public static AdoHelper GetAdoHelper(string strHelper)
		{
			string assembly = null;
			string type = null;
			
			
			

			switch (strHelper.ToUpper())
			{
				case "SQLSERVER":
					assembly = ConfigurationManager.AppSettings["SqlServerHelperAssembly"];
					type = ConfigurationManager.AppSettings["SqlServerHelperType"];
					break;
				case "OLEDB":
					assembly = ConfigurationManager.AppSettings["OleDbHelperAssembly"];
					type = ConfigurationManager.AppSettings["OleDbHelperType"];
					break;
				case "ODBC":
					assembly = ConfigurationManager.AppSettings["OdbcHelperAssembly"];
					type = ConfigurationManager.AppSettings["OdbcHelperType"];
					break;
				case "Firebird":
					assembly = ConfigurationManager.AppSettings["FirebirdHelperAssembly"];
					type = ConfigurationManager.AppSettings["FirebirdHelperType"];
					break;
			}
			return AdoHelper.CreateHelper(assembly, type);
		}
	}
}