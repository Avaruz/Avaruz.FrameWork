Option Strict On

' ===============================================================================
' Microsoft Data Access Application Block for .NET 3.0
'
' Odbc.cs
'
' This file contains the implementations of the AdoHelper supporting Odbc.
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
Imports System.Data.Common
Imports System.Data.Odbc
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.IO


Namespace Belcorp.FrameWork.DAF

    ''' <summary>
    ''' The Odbc class is intended to encapsulate high performance, scalable best practices for 
    ''' common uses of the Odbc ADO.NET provider.  It is created using the abstract factory in AdoHelper
    ''' </summary>
    Public Class Odbc
        Inherits AdoHelper
        ' used for correcting Call syntax for stored procedures in ODBC
        Private Shared _regExpr As New Regex("\{.*call|CALL\s\w+.*}", RegexOptions.Compiled)

        ''' <summary>
        ''' Create an Odbc Helper.  Needs to be a default constructor so that the Factory can create it
        ''' </summary>
        Public Sub New()

        End Sub 'New

#Region "Overrides"
        ''' <summary>
        ''' Returns an array of OdbcParameters of the specified size
        ''' </summary>
        ''' <param name="size">size of the array</param>
        ''' <returns>The array of OdbcParameters</returns>
        Protected Overrides Function GetDataParameters(ByVal size As Integer) As IDataParameter()

            Return New OdbcParameter(size - 1) {}
        End Function 'GetDataParameters

        ''' <summary>
        ''' Returns an OdbcConnection object for the given connection string
        ''' </summary>
        ''' <param name="connectionString">The connection string to be used to create the connection</param>
        ''' <returns>An OdbcConnection object</returns>
        Public Overrides Function GetConnection(ByVal connectionString As String) As IDbConnection
            Return New OdbcConnection(connectionString)
        End Function 'GetConnection


        ''' <summary>
        ''' Returns an OdbcDataAdapter object
        ''' </summary>
        ''' <returns>The OdbcDataAdapter</returns>
        Public Overrides Function GetDataAdapter() As IDbDataAdapter

            Return New OdbcDataAdapter
        End Function 'GetDataAdapter

        ''' <summary>
        ''' Calls the CommandBuilder.DeriveParameters method for the specified provider, doing any setup and cleanup necessary
        ''' </summary>
        ''' <param name="cmd">The IDbCommand referencing the stored procedure from which the parameter information is to be derived. The derived parameters are added to the Parameters collection of the IDbCommand. </param>
        Public Overrides Sub DeriveParameters(ByVal cmd As IDbCommand)

            Dim mustCloseConnection As Boolean = False

            If Not TypeOf cmd Is OdbcCommand Then
                Throw New ArgumentException("The command provided is not a OdbcCommand instance.", "cmd")
            End If
            If cmd.Connection.State <> ConnectionState.Open Then
                cmd.Connection.Open()
                mustCloseConnection = True
            End If

            OdbcCommandBuilder.DeriveParameters(DirectCast(cmd, OdbcCommand))

            If mustCloseConnection Then

                cmd.Connection.Close()
            End If
        End Sub 'DeriveParameters

        ''' <summary>
        ''' Returns an OdbcParameter object
        ''' </summary>
        ''' <returns>The OdbcParameter object</returns>
        Public Overloads Overrides Function GetParameter() As IDataParameter

            Return New OdbcParameter
        End Function 'GetParameter

        ''' <summary>
        ''' This cleans up the parameter syntax for an ODBC call.  This was split out from PrepareCommand so that it could be called independently.
        ''' </summary>
        ''' <param name="command">An IDbCommand object containing the CommandText to clean.</param>
        Public Overrides Sub CleanParameterSyntax(ByVal command As IDbCommand)

            Dim [call] As String = " call "

            If command.CommandType = CommandType.StoredProcedure Then

                If Not _regExpr.Match(command.CommandText).Success AndAlso _
                    command.CommandText.Trim().IndexOf(" ") = -1 Then  ' It does not like like { call sp_name() }

                    ' If there's only a stored procedure name
                    Dim par As New StringBuilder
                    If command.Parameters.Count <> 0 Then

                        Dim isFirst As Boolean = True
                        Dim hasParameters As Boolean = False
                        Dim i As Integer
                        For i = 0 To command.Parameters.Count - 1

                            Dim p As OdbcParameter = DirectCast(command.Parameters(i), OdbcParameter)
                            If p.Direction <> ParameterDirection.ReturnValue Then

                                If isFirst Then

                                    isFirst = False
                                    par.Append("(?")

                                Else

                                    par.Append(",?")
                                End If
                                hasParameters = True

                            Else

                                [call] = " ? = call "
                            End If
                        Next i
                        If hasParameters Then

                            par.Append(")")
                        End If
                    End If
                    command.CommandText = "{" + [call] + command.CommandText + par.ToString() + " }"
                End If
            End If
        End Sub 'CleanParameterSyntax

        ''' <summary>
        ''' Execute an IDbCommand (that returns a resultset) against the provided IDbConnection. 
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
            Dim da As New OdbcDataAdapter(DirectCast(command, OdbcCommand))
            Dim ds As New DataSet

            da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            da.Fill(ds)

            Dim stream As New StringReader(ds.GetXml())

            If mustCloseConnection Then

                command.Connection.Close()
            End If

            Return New XmlTextReader(stream)
        End Function 'ExecuteXmlReader


        ''' <summary>
        ''' Provider specific code to set up the updating/ed event handlers used by UpdateDataset
        ''' </summary>
        ''' <param name="dataAdapter">DataAdapter to attach the event handlers to</param>
        ''' <param name="rowUpdatingHandler">The handler to be called when a row is updating</param>
        ''' <param name="rowUpdatedHandler">The handler to be called when a row is updated</param>-----------------------------------------------------------------------------
        Protected Overrides Sub AddUpdateEventHandlers(ByVal dataAdapter As IDbDataAdapter, ByVal rowUpdatingHandler As RowUpdatingHandler, ByVal rowUpdatedHandler As RowUpdatedHandler)

            If Not (rowUpdatingHandler Is Nothing) Then

                Me.m_rowUpdating = rowUpdatingHandler
                AddHandler DirectCast(dataAdapter, OdbcDataAdapter).RowUpdating, AddressOf RowUpdating
            End If

            If Not (rowUpdatedHandler Is Nothing) Then

                Me.m_rowUpdated = rowUpdatedHandler
                AddHandler DirectCast(dataAdapter, OdbcDataAdapter).RowUpdated, AddressOf RowUpdated
            End If
        End Sub 'AddUpdateEventHandlers

        ''' <summary>
        ''' Handles the RowUpdating event
        ''' </summary>
        ''' <param name="obj">The object that published the event</param>
        ''' <param name="e">The OdbcRowUpdatingEventArgs</param>
        Protected Shadows Sub RowUpdating(ByVal obj As Object, ByVal e As OdbcRowUpdatingEventArgs)

            MyBase.RowUpdating(obj, e)
        End Sub 'RowUpdating

        ''' <summary>
        ''' Handles the RowUpdated event
        ''' </summary>
        ''' <param name="obj">The object that published the event</param>
        ''' <param name="e">The OdbcRowUpdatedEventArgs</param>
        Protected Shadows Sub RowUpdated(ByVal obj As Object, ByVal e As OdbcRowUpdatedEventArgs)

            MyBase.RowUpdated(obj, e)
        End Sub 'RowUpdated

        ''' <summary>
        ''' Handle any provider-specific issues with BLOBs here by "washing" the IDataParameter and returning a new one that is set up appropriately for the provider.
        ''' </summary>
        ''' <param name="connection">The IDbConnection to use in cleansing the parameter</param>
        ''' <param name="p">The parameter before cleansing</param>
        ''' <returns>The parameter after it's been cleansed.</returns>
        Protected Overrides Function GetBlobParameter(ByVal connection As IDbConnection, ByVal p As IDataParameter) As IDataParameter

            ' nothing special needed for ODBC...so far as we know now.
            Return p
        End Function 'GetBlobParameter
#End Region
    End Class
End Namespace
