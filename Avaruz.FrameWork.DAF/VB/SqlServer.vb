Option Strict On
' ===============================================================================
' Microsoft Data Access Application Block for .NET 3.0
'
' SqlServer.cs
'
' This file contains the implementations of the AdoHelper supporting SqlServer.
'
' For more information see the Documentation. 
' ===============================================================================
' Release history
' VERSION	DESCRIPTION
'   2.0	Added support for FillDataset, UpdateDataset and "Param" helper methods
'   3.0	New abstract class supporting the same methods using ADO.NET interfaces
'
' ===============================================================================
' Copyright (C) 2000-2001 Microsoft Corporation
' All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
' FITNESS FOR A PARTICULAR PURPOSE.
' ==============================================================================

Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Xml

Namespace Belcorp.FrameWork.DAF
    ''' <summary>
    ''' The SqlServer class is intended to encapsulate high performance, scalable best practices for 
    ''' common uses of the SqlClient ADO.NET provider.  It is created using the abstract factory in AdoHelper.
    ''' </summary>
    Public Class SqlServer
        Inherits AdoHelper
        ''' <summary>
        ''' Create a SQL Helper.  Needs to be a default constructor so that the Factory can create it
        ''' </summary>
        Public Sub New()

        End Sub 'New

#Region "Overrides"
        ''' <summary>
        ''' Returns an array of SqlParameters of the specified size
        ''' </summary>
        ''' <param name="size">size of the array</param>
        ''' <returns>The array of SqlParameters</returns>
        Protected Overrides Function GetDataParameters(ByVal size As Integer) As IDataParameter()

            Return New SqlParameter(size - 1) {}
        End Function 'GetDataParameters

        ''' <summary>
        ''' Returns a SqlConnection object for the given connection string
        ''' </summary>
        ''' <param name="connectionString">The connection string to be used to create the connection</param>
        ''' <returns>A SqlConnection object</returns>
        Public Overrides Function GetConnection(ByVal connectionString As String) As IDbConnection

            Return New SqlConnection(connectionString)
        End Function 'GetConnection

        ''' <summary>
        ''' Returns a SqlDataAdapter object
        ''' </summary>
        ''' <returns>The SqlDataAdapter</returns>
        Public Overrides Function GetDataAdapter() As IDbDataAdapter

            Return New SqlDataAdapter
        End Function 'GetDataAdapter

        ''' <summary>
        ''' Calls the CommandBuilder.DeriveParameters method for the specified provider, doing any setup and cleanup necessary
        ''' </summary>
        ''' <param name="cmd">The IDbCommand referencing the stored procedure from which the parameter information is to be derived. The derived parameters are added to the Parameters collection of the IDbCommand. </param>
        Public Overrides Sub DeriveParameters(ByVal cmd As IDbCommand)

            Dim mustCloseConnection As Boolean = False

            If Not TypeOf cmd Is SqlCommand Then
                Throw New ArgumentException("The command provided is not a SqlCommand instance.", "cmd")
            End If
            If cmd.Connection.State <> ConnectionState.Open Then

                cmd.Connection.Open()
                mustCloseConnection = True
            End If

            SqlDeriveParameters.DeriveParameters(DirectCast(cmd, SqlCommand))

            If mustCloseConnection Then

                cmd.Connection.Close()
            End If
        End Sub 'DeriveParameters

        ''' <summary>
        ''' Returns a SqlParameter object
        ''' </summary>
        ''' <returns>The SqlParameter object</returns>
        Public Overloads Overrides Function GetParameter() As IDataParameter

            Return New SqlParameter
        End Function 'GetParameter

        ''' <summary>
        ''' Detach the IDataParameters from the command object, so they can be used again.
        ''' </summary>
        ''' <param name="command">command object to clear</param>
        Protected Overrides Sub ClearCommand(ByVal command As IDbCommand)

            ' HACK: There is a problem here, the output parameter values are fletched 
            ' when the reader is closed, so if the parameters are detached from the command
            ' then the IDataReader can´t set its values. 
            ' When this happen, the parameters can´t be used again in other command.
            Dim canClear As Boolean = True
            Dim commandParameter As IDataParameter
            For Each commandParameter In command.Parameters

                If commandParameter.Direction <> ParameterDirection.Input Then
                    canClear = False
                End If
            Next commandParameter
            If canClear Then

                command.Parameters.Clear()
            End If
        End Sub 'ClearCommand

        ''' <summary>
        ''' This cleans up the parameter syntax for an SQL Server call.  This was split out from PrepareCommand so that it could be called independently.
        ''' </summary>
        ''' <param name="command">An IDbCommand object containing the CommandText to clean.</param>
        Public Overrides Sub CleanParameterSyntax(ByVal command As IDbCommand)

            ' do nothing for SQL
        End Sub 'CleanParameterSyntax

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the provided SqlConnection. 
        ''' </summary>
        ''' <example>
        ''' <code>
        ''' XmlReader r = helper.ExecuteXmlReader(command);
        ''' </code></example>
        ''' <param name="command">The IDbCommand to execute</param>
        ''' <returns>An XmlReader containing the resultset generated by the command</returns>
        Public Overloads Overrides Function ExecuteXmlReader(ByVal command As IDbCommand) As XmlReader

            Dim mustCloseConnection As Boolean = False

            If command.Connection.State <> ConnectionState.Open Then

                command.Connection.Open()
                mustCloseConnection = True
            End If

            CleanParameterSyntax(command)
            ' Create the DataAdapter & DataSet
            Dim retval As XmlReader = DirectCast(command, SqlCommand).ExecuteXmlReader()

            ' Detach the SqlParameters from the command object, so they can be used again
            ' don't do this...screws up output parameters -- cjbreisch
            ' cmd.Parameters.Clear();

            If mustCloseConnection Then

                command.Connection.Close()
            End If

            Return retval
        End Function 'ExecuteXmlReader

        ''' <summary>
        ''' Provider specific code to set up the updating/ed event handlers used by UpdateDataset
        ''' </summary>
        ''' <param name="dataAdapter">DataAdapter to attach the event handlers to</param>
        ''' <param name="rowUpdatingHandler">The handler to be called when a row is updating</param>
        ''' <param name="rowUpdatedHandler">The handler to be called when a row is updated</param>
        Protected Overrides Sub AddUpdateEventHandlers(ByVal dataAdapter As IDbDataAdapter, ByVal rowUpdatingHandler As RowUpdatingHandler, ByVal rowUpdatedHandler As RowUpdatedHandler)

            If Not (rowUpdatingHandler Is Nothing) Then

                Me.m_rowUpdating = rowUpdatingHandler
                AddHandler DirectCast(dataAdapter, SqlDataAdapter).RowUpdating, AddressOf RowUpdating
            End If

            If Not (rowUpdatedHandler Is Nothing) Then

                Me.m_rowUpdated = rowUpdatedHandler
                AddHandler DirectCast(dataAdapter, SqlDataAdapter).RowUpdated, AddressOf RowUpdated
            End If
        End Sub 'AddUpdateEventHandlers

        ''' <summary>
        ''' Handles the RowUpdating event
        ''' </summary>
        ''' <param name="obj">The object that published the event</param>
        ''' <param name="e">The SqlRowUpdatingEventArgs</param>
        Protected Shadows Sub RowUpdating(ByVal obj As Object, ByVal e As SqlRowUpdatingEventArgs)

            MyBase.RowUpdating(obj, e)
        End Sub 'RowUpdating

        ''' <summary>
        ''' Handles the RowUpdated event
        ''' </summary>
        ''' <param name="obj">The object that published the event</param>
        ''' <param name="e">The SqlRowUpdatedEventArgs</param>
        Protected Shadows Sub RowUpdated(ByVal obj As Object, ByVal e As SqlRowUpdatedEventArgs)

            MyBase.RowUpdated(obj, e)
        End Sub 'RowUpdated

        ''' <summary>
        ''' Handle any provider-specific issues with BLOBs here by "washing" the IDataParameter and returning a new one that is set up appropriately for the provider.
        ''' </summary>
        ''' <param name="connection">The IDbConnection to use in cleansing the parameter</param>
        ''' <param name="p">The parameter before cleansing</param>
        ''' <returns>The parameter after it's been cleansed.</returns>
        Protected Overrides Function GetBlobParameter(ByVal connection As IDbConnection, ByVal p As IDataParameter) As IDataParameter

            ' do nothing special for BLOBs...as far as we know now.
            Return p
        End Function 'GetBlobParameter
#End Region
    End Class 'SqlServer '

#Region "Derive Parameters"
    ' We create our own class to do this because the existing ADO.NET 1.1 implementation is broken.
    Friend Class SqlDeriveParameters

        Friend Shared Sub DeriveParameters(ByVal cmd As SqlCommand)

            Dim cmdText As String
            Dim newCommand As SqlCommand
            Dim reader As SqlDataReader
            Dim parameterList As ArrayList
            Dim sqlParam As SqlParameter
            Dim cmdType As CommandType
            Dim procedureSchema As String
            Dim procedureName As String
            Dim groupNumber As Integer
            Dim trnSql As SqlTransaction = cmd.Transaction

            cmdType = cmd.CommandType

            If cmdType = CommandType.Text Then

                Throw New InvalidOperationException

            ElseIf cmdType = CommandType.TableDirect Then

                Throw New InvalidOperationException

            ElseIf cmdType <> CommandType.StoredProcedure Then

                Throw New InvalidOperationException
            End If

            procedureName = cmd.CommandText
            Dim server As String = Nothing
            Dim database As String = Nothing
            procedureSchema = Nothing

            ' split out the procedure name to get the server, database, etc.
            GetProcedureTokens(procedureName, server, database, procedureSchema)

            ' look for group numbers
            groupNumber = ParseGroupNumber(procedureName)

            newCommand = Nothing

            ' set up the command string.  We use sp_procuedure_params_rowset to get the parameters
            If Not (database Is Nothing) Then

                cmdText = String.Concat("[", database, "]..sp_procedure_params_rowset")
                If Not (server Is Nothing) Then

                    cmdText = String.Concat(server, ".", cmdText)
                End If

                ' be careful of transactions
                If Not (trnSql Is Nothing) Then

                    newCommand = New SqlCommand(cmdText, cmd.Connection, trnSql)

                Else

                    newCommand = New SqlCommand(cmdText, cmd.Connection)
                End If

            Else

                ' be careful of transactions
                If Not (trnSql Is Nothing) Then

                    newCommand = New SqlCommand("sp_procedure_params_rowset", cmd.Connection, trnSql)

                Else

                    newCommand = New SqlCommand("sp_procedure_params_rowset", cmd.Connection)
                End If
            End If

            newCommand.CommandType = CommandType.StoredProcedure
            newCommand.Parameters.Add(New SqlParameter("@procedure_name", SqlDbType.NVarChar, 255))
            newCommand.Parameters(0).Value = procedureName

            ' make sure we specify 
            If Not IsEmptyString(procedureSchema) Then

                newCommand.Parameters.Add(New SqlParameter("@procedure_schema", SqlDbType.NVarChar, 255))
                newCommand.Parameters(1).Value = procedureSchema
            End If

            ' make sure we specify the groupNumber if we were given one
            If groupNumber <> 0 Then

                newCommand.Parameters.Add(New SqlParameter("@group_number", groupNumber))
            End If

            reader = Nothing
            parameterList = New ArrayList

            Try

                ' get a reader full of our params
                reader = newCommand.ExecuteReader()
                sqlParam = Nothing

                While reader.Read()

                    ' get all the parameter properties that we can get, Name, type, length, direction, precision
                    sqlParam = New SqlParameter
                    sqlParam.ParameterName = CStr(reader("PARAMETER_NAME"))
                    sqlParam.SqlDbType = GetSqlDbType(CShort(reader("DATA_TYPE")), CStr(reader("TYPE_NAME")))

                    If Not reader("CHARACTER_MAXIMUM_LENGTH") Is DBNull.Value Then

                        sqlParam.Size = CInt(reader("CHARACTER_MAXIMUM_LENGTH"))
                    End If

                    sqlParam.Direction = GetParameterDirection(CShort(reader("PARAMETER_TYPE")))

                    If sqlParam.SqlDbType = SqlDbType.Decimal Then

                        sqlParam.Scale = CByte(CShort(reader("NUMERIC_SCALE")) And 255)
                        sqlParam.Precision = CByte(CShort(reader("NUMERIC_PRECISION")) And 255)
                    End If
                    parameterList.Add(sqlParam)
                End While

            Finally

                ' close our reader and connection when we're done
                If Not (reader Is Nothing) Then

                    reader.Close()
                End If
                newCommand.Connection = Nothing
            End Try

            ' we didn't get any parameters
            If parameterList.Count = 0 Then

                Throw New InvalidOperationException
            End If

            cmd.Parameters.Clear()

            ' add the parameters to the command object
            Dim parameter As Object
            For Each parameter In parameterList

                cmd.Parameters.Add(parameter)
            Next parameter
        End Sub 'DeriveParameters

        ''' <summary>
        ''' Checks to see if the stored procedure being called is part of a group, then gets the group number if necessary
        ''' </summary>
        ''' <param name="procedure">Stored procedure being called.  This method may change this parameter by removing the group number if it exists.</param>
        ''' <returns>the group number</returns>
        Private Shared Function ParseGroupNumber(ByRef procedure As String) As Integer

            Dim newProcName As String
            Dim groupPos As Integer = procedure.IndexOf(";"c)
            Dim groupIndex As Integer = 0

            If groupPos > 0 Then

                newProcName = procedure.Substring(0, groupPos)
                Try

                    groupIndex = Integer.Parse(procedure.Substring((groupPos + 1)))

                Catch

                    Throw New InvalidOperationException

                End Try

            Else

                newProcName = procedure
                groupIndex = 0
            End If

            procedure = newProcName
            Return groupIndex
        End Function 'ParseGroupNumber

        ''' <summary>
        ''' Tokenize the procedure string
        ''' </summary>
        ''' <param name="procedure">The procedure name</param>
        ''' <param name="server">The server name</param>
        ''' <param name="database">The database name</param>
        ''' <param name="owner">The owner name</param>
        Private Shared Sub GetProcedureTokens(ByRef procedure As String, ByRef server As String, ByRef database As String, ByRef owner As String)

            Dim spNameTokens() As String
            Dim arrIndex As Integer
            Dim nextPos As Integer
            Dim currPos As Integer
            Dim tokenCount As Integer

            server = Nothing
            database = Nothing
            owner = Nothing

            spNameTokens = New String(4) {}

            If Not IsEmptyString(procedure) Then

                arrIndex = 0
                nextPos = 0
                currPos = 0

                While arrIndex < 4

                    currPos = procedure.IndexOf("."c, nextPos)
                    If -1 = currPos Then

                        spNameTokens(arrIndex) = procedure.Substring(nextPos)
                        Exit While
                    End If
                    spNameTokens(arrIndex) = procedure.Substring(nextPos, currPos - nextPos)
                    nextPos = currPos + 1
                    If procedure.Length <= nextPos Then

                        Exit While
                    End If
                    arrIndex = arrIndex + 1
                End While

                tokenCount = arrIndex + 1

                ' based on how many '.' we found, we know what tokens we found
                Select Case tokenCount

                    Case 1
                        procedure = spNameTokens(0)

                    Case 2
                        procedure = spNameTokens(1)
                        owner = spNameTokens(0)

                    Case 3
                        procedure = spNameTokens(2)
                        owner = spNameTokens(1)
                        database = spNameTokens(0)

                    Case 4
                        procedure = spNameTokens(3)
                        owner = spNameTokens(2)
                        database = spNameTokens(1)
                        server = spNameTokens(0)

                End Select
            End If
        End Sub 'GetProcedureTokens

        ''' <summary>
        ''' Checks for an empty string
        ''' </summary>
        ''' <param name="str">String to check</param>
        ''' <returns>boolean value indicating whether string is empty</returns>
        Private Shared Function IsEmptyString(ByVal str As String) As Boolean

            If Not (str Is Nothing) Then

                Return 0 = str.Length
            End If
            Return True
        End Function 'IsEmptyString

        ''' <summary>
        ''' Convert OleDbType to SQlDbType
        ''' </summary>
        ''' <param name="paramType">The OleDbType to convert</param>
        ''' <param name="typeName">The typeName to convert for items such as Money and SmallMoney which both map to OleDbType.Currency</param>
        ''' <returns>The converted SqlDbType</returns>
        Private Shared Function GetSqlDbType(ByVal paramType As Short, ByVal typeName As String) As SqlDbType

            Dim cmdType As SqlDbType
            Dim oleDbType As OleDbType
            cmdType = SqlDbType.Variant
            oleDbType = CType(paramType, OleDbType)

            Select Case oleDbType

                Case oleDbType.SmallInt
                    cmdType = SqlDbType.SmallInt

                Case oleDbType.Integer
                    cmdType = SqlDbType.Int

                Case oleDbType.Single
                    cmdType = SqlDbType.Real

                Case oleDbType.Double
                    cmdType = SqlDbType.Float

                Case oleDbType.Currency
                    cmdType = DirectCast(IIf(typeName = "money", SqlDbType.Money, SqlDbType.SmallMoney), SqlDbType)

                Case oleDbType.Date
                    cmdType = DirectCast(IIf(typeName = "datetime", SqlDbType.DateTime, SqlDbType.SmallDateTime), SqlDbType)

                Case oleDbType.BSTR
                    cmdType = DirectCast(IIf(typeName = "nchar", SqlDbType.NChar, SqlDbType.NVarChar), SqlDbType)

                Case oleDbType.Boolean
                    cmdType = SqlDbType.Bit

                Case oleDbType.Variant
                    cmdType = SqlDbType.Variant

                Case oleDbType.Decimal
                    cmdType = SqlDbType.Decimal

                Case oleDbType.TinyInt
                    cmdType = SqlDbType.TinyInt

                Case oleDbType.UnsignedTinyInt
                    cmdType = SqlDbType.TinyInt

                Case oleDbType.UnsignedSmallInt
                    cmdType = SqlDbType.SmallInt

                Case oleDbType.BigInt
                    cmdType = SqlDbType.BigInt

                Case oleDbType.Filetime
                    cmdType = DirectCast(IIf(typeName = "datetime", SqlDbType.DateTime, SqlDbType.SmallDateTime), SqlDbType)

                Case oleDbType.Guid
                    cmdType = SqlDbType.UniqueIdentifier

                Case oleDbType.Binary
                    cmdType = DirectCast(IIf(typeName = "binary", SqlDbType.Binary, SqlDbType.VarBinary), SqlDbType)

                Case oleDbType.Char
                    cmdType = DirectCast(IIf(typeName = "char", SqlDbType.Char, SqlDbType.VarChar), SqlDbType)

                Case oleDbType.WChar
                    cmdType = DirectCast(IIf(typeName = "nchar", SqlDbType.NChar, SqlDbType.NVarChar), SqlDbType)

                Case oleDbType.Numeric
                    cmdType = SqlDbType.Decimal

                Case oleDbType.DBDate
                    cmdType = DirectCast(IIf(typeName = "datetime", SqlDbType.DateTime, SqlDbType.SmallDateTime), SqlDbType)

                Case oleDbType.DBTime
                    cmdType = DirectCast(IIf(typeName = "datetime", SqlDbType.DateTime, SqlDbType.SmallDateTime), SqlDbType)

                Case oleDbType.DBTimeStamp
                    cmdType = DirectCast(IIf(typeName = "datetime", SqlDbType.DateTime, SqlDbType.SmallDateTime), SqlDbType)

                Case oleDbType.VarChar
                    cmdType = DirectCast(IIf(typeName = "char", SqlDbType.Char, SqlDbType.VarChar), SqlDbType)

                Case oleDbType.LongVarChar
                    cmdType = SqlDbType.Text

                Case oleDbType.VarWChar
                    cmdType = DirectCast(IIf(typeName = "nchar", SqlDbType.NChar, SqlDbType.NVarChar), SqlDbType)

                Case oleDbType.LongVarWChar
                    cmdType = SqlDbType.NText

                Case oleDbType.VarBinary
                    cmdType = DirectCast(IIf(typeName = "binary", SqlDbType.Binary, SqlDbType.VarBinary), SqlDbType)

                Case oleDbType.LongVarBinary
                    cmdType = SqlDbType.Image

            End Select
            Return cmdType
        End Function 'GetSqlDbType

        ''' <summary>
        ''' Converts the OleDb parameter direction
        ''' </summary>
        ''' <param name="oledbDirection">The integer parameter direction</param>
        ''' <returns>A ParameterDirection</returns>
        Private Shared Function GetParameterDirection(ByVal oledbDirection As Short) As ParameterDirection

            Dim pd As ParameterDirection
            Select Case oledbDirection

                Case 1
                    pd = ParameterDirection.Input

                Case 2
                    pd = ParameterDirection.Output

                Case 4
                    pd = ParameterDirection.ReturnValue

                Case Else
                    pd = ParameterDirection.InputOutput

            End Select
            Return pd
        End Function 'GetParameterDirection
    End Class 'SqlDeriveParameters
#End Region
End Namespace 'GotDotNet.ApplicationBlocks.Data
