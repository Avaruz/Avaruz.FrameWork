Option Strict On
' ===============================================================================
' Microsoft Data Access Application Block for .NET 3.0
'
' Oldedb.cs
'
' This file contains the implementations of the AdoHelper supporting OleDb.
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
Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.IO

Namespace Belcorp.FrameWork.DAF
    ''' <summary>
    ''' The OleDb class is intended to encapsulate high performance, scalable best practices for 
    ''' common uses of the OleDb ADO.NET provider.  It is created using the abstract factory in AdoHelper
    ''' </summary>
    Public Class OleDb
        Inherits AdoHelper
        ''' <summary>
        ''' Create an OleDb Helper.  Needs to be a default constructor so that the Factory can create it
        ''' </summary>
        Public Sub New()

        End Sub 'New

#Region "Overrides"
        ''' <summary>
        ''' Returns an array of OleDbParameters of the specified size
        ''' </summary>
        ''' <param name="size">size of the array</param>
        ''' <returns>The array of OdbcParameters</returns>
        Protected Overrides Function GetDataParameters(ByVal size As Integer) As IDataParameter()

            Return New OleDbParameter(size - 1) {}
        End Function 'GetDataParameters

        ''' <summary>
        ''' Returns an OleDbConnection object for the given connection string
        ''' </summary>
        ''' <param name="connectionString">The connection string to be used to create the connection</param>
        ''' <returns>An OleDbConnection object</returns>
        Public Overrides Function GetConnection(ByVal connectionString As String) As IDbConnection

            Return New OleDbConnection(connectionString)
        End Function 'GetConnection

        ''' <summary>
        ''' Returns an OleDbDataAdapter object
        ''' </summary>
        ''' <returns>The OleDbDataAdapter</returns>
        Public Overrides Function GetDataAdapter() As IDbDataAdapter

            Return New OleDbDataAdapter
        End Function 'GetDataAdapter

        ''' <summary>
        ''' Calls the CommandBuilder.DeriveParameters method for the specified provider, doing any setup and cleanup necessary
        ''' </summary>
        ''' <param name="cmd">The IDbCommand referencing the stored procedure from which the parameter information is to be derived. The derived parameters are added to the Parameters collection of the IDbCommand. </param>
        Public Overrides Sub DeriveParameters(ByVal cmd As IDbCommand)

            Dim mustCloseConnection As Boolean = False

            If Not TypeOf cmd Is OleDbCommand Then
                Throw New ArgumentException("The command provided is not a OleDbCommand instance.", "cmd")
            End If
            If cmd.Connection.State <> ConnectionState.Open Then

                cmd.Connection.Open()
                mustCloseConnection = True
            End If

            OleDbCommandBuilder.DeriveParameters(DirectCast(cmd, OleDbCommand))

            If mustCloseConnection Then

                cmd.Connection.Close()
            End If
        End Sub 'DeriveParameters

        ''' <summary>
        ''' Returns an OleDbParameter object
        ''' </summary>
        ''' <returns>The OleDbParameter object</returns>
        Public Overloads Overrides Function GetParameter() As IDataParameter

            Return New OleDbParameter
        End Function 'GetParameter

        ''' <summary>
        ''' This cleans up the parameter syntax for an OleDb call.  This was split out from PrepareCommand so that it could be called independently.
        ''' </summary>
        ''' <param name="command">An IDbCommand object containing the CommandText to clean.</param>
        Public Overrides Sub CleanParameterSyntax(ByVal command As IDbCommand)

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
            Dim da As New OleDbDataAdapter(DirectCast(command, OleDbCommand))
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
        ''' <param name="rowUpdatedHandler">The handler to be called when a row is updated</param>
        Protected Overrides Sub AddUpdateEventHandlers(ByVal dataAdapter As IDbDataAdapter, ByVal rowUpdatingHandler As RowUpdatingHandler, ByVal rowUpdatedHandler As RowUpdatedHandler)

            If Not (rowUpdatingHandler Is Nothing) Then

                Me.m_rowUpdating = rowUpdatingHandler
                AddHandler DirectCast(dataAdapter, OleDbDataAdapter).RowUpdating, AddressOf RowUpdating
            End If

            If Not (rowUpdatedHandler Is Nothing) Then

                Me.m_rowUpdated = rowUpdatedHandler
                AddHandler DirectCast(dataAdapter, OleDbDataAdapter).RowUpdated, AddressOf RowUpdated
            End If
        End Sub 'AddUpdateEventHandlers

        ''' <summary>
        ''' Handles the RowUpdating event
        ''' </summary>
        ''' <param name="obj">The object that published the event</param>
        ''' <param name="e">The OleDbRowUpdatingEventArgs</param>
        Protected Shadows Sub RowUpdating(ByVal obj As Object, ByVal e As OleDbRowUpdatingEventArgs)

            MyBase.RowUpdating(obj, e)
        End Sub 'RowUpdating

        ''' <summary>
        ''' Handles the RowUpdated event
        ''' </summary>
        ''' <param name="obj">The object that published the event</param>
        ''' <param name="e">The OleDbRowUpdatedEventArgs</param>
        Protected Shadows Sub RowUpdated(ByVal obj As Object, ByVal e As OleDbRowUpdatedEventArgs)

            MyBase.RowUpdated(obj, e)
        End Sub 'RowUpdated

        ''' <summary>
        ''' Handle any provider-specific issues with BLOBs here by "washing" the IDataParameter and returning a new one that is set up appropriately for the provider.
        ''' </summary>
        ''' <param name="connection">The IDbConnection to use in cleansing the parameter</param>
        ''' <param name="p">The parameter before cleansing</param>
        ''' <returns>The parameter after it's been cleansed.</returns>
        Protected Overrides Function GetBlobParameter(ByVal connection As IDbConnection, ByVal p As IDataParameter) As IDataParameter

            ' nothing special needed for OleDb...as far as we know now
            Return p
        End Function 'GetBlobParameter
#End Region
    End Class 'OleDb '
End Namespace 'GotDotNet.ApplicationBlocks.Data
