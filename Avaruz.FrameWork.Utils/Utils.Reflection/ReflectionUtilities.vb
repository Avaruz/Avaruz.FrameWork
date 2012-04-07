Imports System.Reflection

Public Module ReflectionUtilities

    Private _rutaEnsanblado As String
    Public Property RutaEnsamblado() As String
        Get
            Return _rutaEnsanblado
        End Get
        Set(ByVal value As String)
            _rutaEnsanblado = value
        End Set
    End Property

    Private _nombreEnsamblado As String
    Public Property NombreEnsamblado() As String
        Get
            Return _nombreEnsamblado
        End Get
        Set(ByVal value As String)
            _nombreEnsamblado = value
        End Set
    End Property




    Private Function getNombreCompletoEnsamblado() As String
        Return (RutaEnsamblado + NombreEnsamblado + ".dll")
    End Function


    ''' <summary>
    ''' Busca un metodo dentro de una clase mediante Reflection
    ''' </summary>
    ''' <param name="Miembros"></param>
    ''' <param name="metodoABuscar"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function findMetodo(ByVal Miembros() As MemberInfo, metodoABuscar As String) As Boolean
        Dim resultado As Boolean = False
        'Comprobamos si tiene el método SelectByActivo
        For Each Miembro As MemberInfo In Miembros
            If Miembro.Name.ToUpper() = metodoABuscar.ToUpper() Then
                resultado = True
                Exit For
            End If
        Next
        Return resultado
    End Function
    ''' <summary>
    ''' Obtiene una clase base dentro de un Type mediante Reflection
    ''' </summary>
    ''' <param name="nombreClase"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getClaseBase(nombreClase As String) As Type

        Dim ensanbladoDb As Assembly = Assembly.LoadFile(getNombreCompletoEnsamblado())
        Dim nombreCompletoClase = ReflectionUtilities.NombreEnsamblado + "." + nombreClase + "Db"
        Dim claseDb As Type = ensanbladoDb.GetType(nombreCompletoClase)

        ensanbladoDb = Nothing

        Return claseDb
    End Function
    ''' <summary>
    ''' Obtiene el nombre de la clase base de un Type
    ''' </summary>
    ''' <param name="Tipo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getNombreClaseBase(ByVal Tipo As Type) As String
        Dim nombreClase As String = Tipo.Name
        Dim tamanio As Integer = nombreClase.Length

        Try
            If nombreClase.Contains("Info") Then
                nombreClase = nombreClase.Substring(0, tamanio - 4)
            Else
                nombreClase = nombreClase.Substring(0, tamanio - 2)
            End If
        Catch ex As Exception
            nombreClase = Tipo.Name
        End Try

        Return nombreClase
    End Function
End Module
