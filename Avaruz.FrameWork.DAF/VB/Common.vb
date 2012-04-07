Imports System.Configuration
Imports Belcorp.FrameWork.DAF

Namespace Belcorp.FrameWork.DAF
    Public Module Common
        Public Function GetAdoHelper(ByVal strHelper As String) As AdoHelper
            Dim [assembly] As String = Nothing
            Dim type As String = Nothing

            Select Case strHelper.ToUpper
                Case "SQLSERVER"
                    [assembly] = ConfigurationManager.AppSettings("SqlServerHelperAssembly")
                    type = ConfigurationManager.AppSettings("SqlServerHelperType")
                Case "OLEDB"
                    [assembly] = ConfigurationManager.AppSettings("OleDbHelperAssembly")
                    type = ConfigurationManager.AppSettings("OleDbHelperType")
                Case "ODBC"
                    [assembly] = ConfigurationManager.AppSettings("OdbcHelperAssembly")
                    type = ConfigurationManager.AppSettings("OdbcHelperType")
            End Select

            Return AdoHelper.CreateHelper([assembly], type)
        End Function
    End Module
End Namespace