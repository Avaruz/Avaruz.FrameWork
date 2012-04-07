Option Strict On
' ===============================================================================
' Microsoft Data Access Application Block for .NET 3.0
'
' DaabSectionHandler.cs
'
' This file contains the implementations of the DAABSectionHandler 
' configuration section handler.
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
Imports System.Configuration
Imports System.Xml


Namespace Belcorp.FrameWork.DAF
    Public Class DAABSectionHandler
        Implements IConfigurationSectionHandler

#Region "IConfigurationSectionHandler Members"

        ''' <summary>
        ''' Evaluates the given XML section and returns a Hashtable that contains the results of the evaluation.
        ''' </summary>
        ''' <param name="parent">The configuration settings in a corresponding parent configuration section. </param>
        ''' <param name="configContext">An HttpConfigurationContext when Create is called from the ASP.NET configuration system. Otherwise, this parameter is reserved and is a null reference (Nothing in Visual Basic). </param>
        ''' <param name="section">The XmlNode that contains the configuration information to be handled. Provides direct access to the XML contents of the configuration section. </param>
        ''' <returns>A Hashtable that contains the section's configuration settings.</returns>
        Public Overridable Function Create(ByVal parent As Object, ByVal configContext As Object, ByVal section As XmlNode) As Object Implements IConfigurationSectionHandler.Create
            Dim ht As New Hashtable
            Dim list As XmlNodeList = section.SelectNodes("daabProvider")
            Dim prov As XmlNode
            For Each prov In list
                If prov.Attributes("alias") Is Nothing Then
                    Throw New InvalidOperationException("The 'daabProvider' node must contain an attribute named 'alias' with the alias name for the provider.")
                End If
                If prov.Attributes("assembly") Is Nothing Then
                    Throw New InvalidOperationException("The 'daabProvider' node must contain an attribute named 'assembly' with the name of the assembly containing the provider.")
                End If
                If prov.Attributes("type") Is Nothing Then
                    Throw New InvalidOperationException("The 'daabProvider' node must contain an attribute named 'type' with the full name of the type for the provider.")
                End If
                ht(prov.Attributes("alias").Value) = New ProviderAlias(prov.Attributes("assembly").Value, prov.Attributes("type").Value)
            Next prov
            Return ht
        End Function 'Create
#End Region
    End Class 'DAABSectionHandler
    Public Class ProviderAlias
        Private _assemblyName As String
        Private _typeName As String
        ''' <summary>
        ''' Constructor required by IConfigurationSectionHandler
        ''' </summary>
        ''' <param name="assemblyName">The Assembly where this provider can be found</param>
        ''' <param name="typeName">The type of the provider</param>-----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="assemblyName"></param>
        ''' <param name="typeName"></param>
        ''' <remarks></remarks>
        ''' <history>
        ''' 	[cbreisch] 	5/28/2004	Created
        ''' </history>
        '''-----------------------------------------------------------------------------
        Public Sub New(ByVal assemblyName As String, ByVal typeName As String)
            _assemblyName = assemblyName
            _typeName = typeName
        End Sub 'New
        ''' <summary>
        ''' Returns the Assembly name for this provider
        ''' </summary>

        Public ReadOnly Property AssemblyName() As String
            Get
                Return _assemblyName
            End Get
        End Property
        ''' <summary>
        ''' Returns the type name of this provider
        ''' </summary>

        Public ReadOnly Property TypeName() As String
            Get
                Return _typeName
            End Get
        End Property '
    End Class 'ProviderAlias
End Namespace 'GotDotNet.ApplicationBlocks.Data '
