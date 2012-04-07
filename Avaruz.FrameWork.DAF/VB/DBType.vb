Namespace Belcorp.FrameWork.DAF
    Public Structure DBType
        Public ReadOnly Property FoxPro() As String
            Get
                Return "FoxPro"
            End Get
        End Property
        Public ReadOnly Property MSSQL() As String
            Get
                Return "MSSQL"
            End Get
        End Property
        Public ReadOnly Property Firebird() As String
            Get
                Return "Firebird"
            End Get
        End Property
        Public ReadOnly Property MySQL() As String
            Get
                Return "MySQL"
            End Get
        End Property
        Public ReadOnly Property Access() As String
            Get
                Return "Access"
            End Get
        End Property
        Public ReadOnly Property Excel() As String
            Get
                Return "Excel"
            End Get
        End Property
    End Structure
End Namespace