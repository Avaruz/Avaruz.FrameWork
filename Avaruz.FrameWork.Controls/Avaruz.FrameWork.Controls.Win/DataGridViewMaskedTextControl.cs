/**********************************
 * Title:       DataGridViewColumn Hosting MaskedTextBox
 * Author:      Juergen Thomas, Berlin (Germany)
 * Email:       post@vs-polis.de
 * Environment: WinXP, NET 2.0
 * Keywords:    DataGridViewColumn, 
 *   DataGridViewMaskedTextColumn, 
 *   DataGridViewMaskedTextCell, 
 *   DataGridViewMaskedTextControl,
 *   DataGridViewTextBoxColumn, 
 *   DataGridViewTextBoxCell, 
 *   DataGridViewTextBoxEditingControl,
 *   DataGridView, 
 *   MaskedTextBox,
 *   EditingControl,
 *   IDataGridViewEditingControl,
 *   Host Controls
 * Description: An article to host a MaskedTextBox in a DataGridViewColumn
 * Section      Desktop Development
 * SubSection   Grid & Data Controls >> .NET - DataGrid and DataView
 * 
 * Contents
 * --------
 * This solution contains the following classes:
 *   DataGridViewMaskedTextColumn, 
 *   DataGridViewMaskedTextCell, 
 *   DataGridViewMaskedTextControl,  
 * 
 * Additionally, the DataGridViewMaskedTextColumn class uses the
 * ReferencedDescriptionAttribute.
 * 
 * (C) Juergen Thomas
 * The Code Project Open License (CPOL) 
 * http://www.codeproject.com/info/EULA.aspx
 * 
 * History information is posted in DataGridViewMaskedTextColumn.cs 
 **********************************/

#region Usings
using System;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Avaruz.FrameWork.Controls.Win
{
	
	/// <summary>
	/// DataGridViewMaskedTextEditingControl is the MaskedTextBox that is hosted
	/// in a DataGridViewMaskedTextColumn.
	/// </summary>
	public class DataGridViewMaskedTextEditingControl : MaskedTextBox, IDataGridViewEditingControl
	{
		
		#region Fields
		private DataGridView dataGridView;
		private bool valueChanged;
		private int rowIndex;
		#endregion
		
		#region Constructor

		public DataGridViewMaskedTextEditingControl()
		{
			Mask = String.Empty;
		}
		
		#endregion

		#region Interface's properties

		public DataGridView EditingControlDataGridView
		{
			get	{	return dataGridView;		}
			set	{	dataGridView = value;		}
		}

		public object EditingControlFormattedValue
		{
			get	{	return Text;		}
			set	{	if (value is string)
					Text = (string)value;
			}
		}

		public int EditingControlRowIndex
		{
			get	{	return rowIndex;		}
			set	{	rowIndex = value;		}
		}

		public bool EditingControlValueChanged
		{
			get	{	return valueChanged;		}
			set	{	valueChanged = value;		}
		}

		public Cursor EditingPanelCursor
		{
			get	{	return base.Cursor;			}
		}

		public bool RepositionEditingControlOnValueChange
		{
			get	{	return false;		}
		}

		#endregion
		
		#region Interface's methods

		public void ApplyCellStyleToEditingControl(
			DataGridViewCellStyle dataGridViewCellStyle)
		{
			Font = dataGridViewCellStyle.Font;
			//	get the current cell to use the specific mask string
			DataGridViewMaskedTextCell cell 
				= dataGridView.CurrentCell as DataGridViewMaskedTextCell;
			if (cell != null) {
				Mask = cell.Mask;
			}
		}

		public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
		{
			//  Note: In a DataGridView, one could prefer to change the row using
			//	the up/down keys.
			switch (key & Keys.KeyCode)
			{
				case Keys.Left:
				case Keys.Right:
				case Keys.Home:
				case Keys.End:
					return true;
				default:
					return false;
			}
		}

		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return EditingControlFormattedValue;
		}

		public void PrepareEditingControlForEdit(bool selectAll)
		{
			if (selectAll)
				SelectAll();
			else {
				SelectionStart = 0;
				SelectionLength = 0;
			}
		}

		#endregion
		
		#region MaskedTextBox event
		
		protected override void OnTextChanged(System.EventArgs e)
		{
			base.OnTextChanged(e);
			EditingControlValueChanged = true;
			if (EditingControlDataGridView != null) {
				EditingControlDataGridView.CurrentCell.Value = Text;
			}
		}
		
		#endregion

	}

}
