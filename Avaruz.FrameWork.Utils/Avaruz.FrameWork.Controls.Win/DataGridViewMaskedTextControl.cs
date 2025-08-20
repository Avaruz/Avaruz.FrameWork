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

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Avaruz.FrameWork.Controls.Win
{
    /// <summary>
    /// DataGridViewMaskedTextEditingControl is the MaskedTextBox that is hosted
    /// in a DataGridViewMaskedTextColumn.
    /// </summary>
    public class DataGridViewMaskedTextEditingControl : MaskedTextBox, IDataGridViewEditingControl
    {
        public DataGridViewMaskedTextEditingControl()
        {
            Mask = String.Empty;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DataGridView EditingControlDataGridView { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public object EditingControlFormattedValue
        {
            get { return Text; }
            set
            {
                if (value is string x)
                {
                    Text = x;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int EditingControlRowIndex { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool EditingControlValueChanged { get; set; }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            Font = dataGridViewCellStyle.Font;
            //	get the current cell to use the specific mask string
            if (EditingControlDataGridView.CurrentCell is DataGridViewMaskedTextCell cell)
            {
                Mask = cell.Mask;
            }
        }

        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            //  Note: In a DataGridView, one could prefer to change the row using
            //	the up/down keys.
            return (key & Keys.KeyCode) switch
            {
                Keys.Left or Keys.Right or Keys.Home or Keys.End => true,
                _ => false,
            };
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
            {
                SelectAll();
            }
            else
            {
                SelectionStart = 0;
                SelectionLength = 0;
            }
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            EditingControlValueChanged = true;
            if (EditingControlDataGridView != null)
            {
                EditingControlDataGridView.CurrentCell.Value = Text;
            }
        }
    }
}