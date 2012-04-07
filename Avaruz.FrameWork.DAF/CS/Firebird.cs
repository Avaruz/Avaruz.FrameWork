
// ===============================================================================
// Microsoft Data Access Application Block for .NET 3.0
//
// FbServer.cs
//
// This file contains the implementations of the AdoHelper supporting FbServer.
//
// For more information see the Documentation. 
// ===============================================================================
// Release history
// VERSION	DESCRIPTION
//   2.0	Added support for FillDataset, UpdateDataset and "Param" helper methods
//   3.0	New abstract class supporting the same methods using ADO.NET interfaces
//
// ===============================================================================
// Copyright (C) 2000-2001 Microsoft Corporation
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================

using System;
using System.Collections;
using System.Data;
using FirebirdSql.Data.Firebird;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Text;

namespace Belcorp.FrameWork.DAF
{
	/// <summary>
	/// The FbServer class is intended to encapsulate high performance, scalable best practices for 
	/// common uses of the FbClient ADO.NET provider.  It is created using the abstract factory in AdoHelper.
	/// </summary>
	public class Firebird : AdoHelper
	{
		/// <summary>
		/// Create a Fb Helper.  Needs to be a default constructor so that the Factory can create it
		/// </summary>
		public Firebird()
		{
		}

		#region Overrides
		/// <summary>
		/// Returns an array of FbParameters of the specified size
		/// </summary>
		/// <param name="size">size of the array</param>
		/// <returns>The array of FbParameters</returns>
		protected override IDataParameter[] GetDataParameters(int size)
		{
			return new FbParameter[size];
		}

		/// <summary>
		/// Returns a FbConnection object for the given connection string
		/// </summary>
		/// <param name="connectionString">The connection string to be used to create the connection</param>
		/// <returns>A FbConnection object</returns>
		public override IDbConnection GetConnection( string connectionString )
		{
			return new FbConnection( connectionString );
		}

		/// <summary>
		/// Returns a FbDataAdapter object
		/// </summary>
		/// <returns>The FbDataAdapter</returns>
		public override IDbDataAdapter GetDataAdapter()
		{
			return new FbDataAdapter();
		}

		/// <summary>
		/// Calls the CommandBuilder.DeriveParameters method for the specified provider, doing any setup and cleanup necessary
		/// </summary>
		/// <param name="cmd">The IDbCommand referencing the stored procedure from which the parameter information is to be derived. The derived parameters are added to the Parameters collection of the IDbCommand. </param>
		public override void DeriveParameters( IDbCommand cmd )
		{
			bool mustCloseConnection = false;

			if( !( cmd is FbCommand ) )
				throw new ArgumentException( "The command provided is not a FbCommand instance.", "cmd" );
			
			if (cmd.Connection.State != ConnectionState.Open) 
			{
				cmd.Connection.Open();
				mustCloseConnection = true;
			}
			
			FirebirdSql.Data.FirebirdClient.FbCommandBuilder.DeriveParameters((FbCommand)cmd );
			
			if (mustCloseConnection)
			{
				cmd.Connection.Close();
			}
		}

		/// <summary>
		/// Returns a FbParameter object
		/// </summary>
		/// <returns>The FbParameter object</returns>
		public override IDataParameter GetParameter()
		{
			return new FbParameter(); 
		}

		/// <summary>
		/// Detach the IDataParameters from the command object, so they can be used again.
		/// </summary>
		/// <param name="command">command object to clear</param>
		protected override void ClearCommand( IDbCommand command )
		{
			// HACK: There is a problem here, the output parameter values are fletched 
			// when the reader is closed, so if the parameters are detached from the command
			// then the IDataReader can´t set its values. 
			// When this happen, the parameters can´t be used again in other command.
			bool canClear = true;
	
			foreach(IDataParameter commandParameter in command.Parameters)
			{
				if (commandParameter.Direction != ParameterDirection.Input)
					canClear = false;
			
			}
			if (canClear)
			{
				command.Parameters.Clear();
			}
		}

		/// <summary>
		/// This cleans up the parameter syntax for an Fb Server call.  This was split out from PrepareCommand so that it could be called independently.
		/// </summary>
		/// <param name="command">An IDbCommand object containing the CommandText to clean.</param>
		public override void CleanParameterSyntax(IDbCommand command)
		{
			// do nothing for Fb
		}

		/// <summary>
		/// Execute a FbCommand (that returns a resultset) against the provided FbConnection. 
		/// </summary>
		/// <example>
		/// <code>
		/// XmlReader r = helper.ExecuteXmlReader(command);
		/// </code></example>
		/// <param name="command">The IDbCommand to execute</param>
		/// <returns>An XmlReader containing the resultset generated by the command</returns>
		public override XmlReader ExecuteXmlReader(IDbCommand command)
		{
			bool mustCloseConnection = false;

			if (command.Connection.State != ConnectionState.Open) 
			{
				command.Connection.Open();
				mustCloseConnection = true;
			}

			CleanParameterSyntax(command);

			FbDataAdapter da = new FbDataAdapter((FbCommand)command);
			DataSet ds = new DataSet();

			da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
			da.Fill(ds);

			StringReader stream = new StringReader(ds.GetXml());
			if (mustCloseConnection)
			{
				command.Connection.Close();
			}

			return new XmlTextReader(stream);
		}

		/// <summary>
		/// Provider specific code to set up the updating/ed event handlers used by UpdateDataset
		/// </summary>
		/// <param name="dataAdapter">DataAdapter to attach the event handlers to</param>
		/// <param name="rowUpdatingHandler">The handler to be called when a row is updating</param>
		/// <param name="rowUpdatedHandler">The handler to be called when a row is updated</param>
		protected override void AddUpdateEventHandlers(IDbDataAdapter dataAdapter, RowUpdatingHandler rowUpdatingHandler, RowUpdatedHandler rowUpdatedHandler)
		{
			if (rowUpdatingHandler != null)
			{
				this.m_rowUpdating = rowUpdatingHandler;
				((FbDataAdapter)dataAdapter).RowUpdating += new FbRowUpdatingEventHandler(RowUpdating);
			}

			if (rowUpdatedHandler != null)
			{
				this.m_rowUpdated = rowUpdatedHandler;
				((FbDataAdapter)dataAdapter).RowUpdated += new FbRowUpdatedEventHandler(RowUpdated);
			}
		}

		/// <summary>
		/// Handles the RowUpdating event
		/// </summary>
		/// <param name="obj">The object that published the event</param>
		/// <param name="e">The FbRowUpdatingEventArgs</param>
		protected void RowUpdating(object obj, FbRowUpdatingEventArgs e)
		{
			base.RowUpdating(obj, e);
		}

		/// <summary>
		/// Handles the RowUpdated event
		/// </summary>
		/// <param name="obj">The object that published the event</param>
		/// <param name="e">The FbRowUpdatedEventArgs</param>
		protected void RowUpdated(object obj, FbRowUpdatedEventArgs e)
		{
			base.RowUpdated(obj, e);
		}
		
		/// <summary>
		/// Handle any provider-specific issues with BLOBs here by "washing" the IDataParameter and returning a new one that is set up appropriately for the provider.
		/// </summary>
		/// <param name="connection">The IDbConnection to use in cleansing the parameter</param>
		/// <param name="p">The parameter before cleansing</param>
		/// <returns>The parameter after it's been cleansed.</returns>
		protected override IDataParameter GetBlobParameter(IDbConnection connection, IDataParameter p)
		{
			// do nothing special for BLOBs...as far as we know now.
			return p;
		}
		#endregion

	}
}
