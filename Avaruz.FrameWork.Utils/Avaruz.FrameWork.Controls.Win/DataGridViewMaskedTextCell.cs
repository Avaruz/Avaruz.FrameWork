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
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
#endregion

namespace Avaruz.FrameWork.Controls.Win
{

    /// <summary>
    /// DataGridViewMaskedTextCell is derived from DGV-TextBoxCell using all TextBox
    /// properties and containing the Mask property to host a MaskedTextBox.
    /// </summary>
    public class DataGridViewMaskedTextCell : DataGridViewTextBoxCell
    {
        #region Fields
        private static readonly Type valueType = typeof(string);
        private static readonly Type editorType = typeof(DataGridViewMaskedTextEditingControl);
        #endregion

        #region Constructor, Clone, ToString

        public DataGridViewMaskedTextCell()
            : base()
        {
            Mask = String.Empty;
        }

        /// <summary>
        /// Creates a copy of a DGV-MaskedTextCell containing the DGV-Cell properties.
        /// </summary>
        /// <returns>Instance of a DGV-MaskedTextCell using the Mask string.</returns>
        public override object Clone()
        {
            DataGridViewMaskedTextCell cell = base.Clone() as DataGridViewMaskedTextCell;
            cell.Mask = this.Mask;
            return cell;
        }

        /// <summary>
        /// Converting the current DGV-MaskedTextCell instance to a string value.
        /// </summary>
        /// <returns>String value of the instance containing the type name,
        /// column index, and row index.</returns>
        public override string ToString()
        {
            StringBuilder builder = new(0x40);
            builder.Append("DataGridViewMaskedTextCell { ColumnIndex=");
            builder.Append(base.ColumnIndex);
            builder.Append(", RowIndex=");
            builder.Append(base.RowIndex);
            builder.Append(" }");
            return builder.ToString();
        }

        #endregion

        #region Derived methods

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void DetachEditingControl()
        {
            DataGridView dataGridView = base.DataGridView;
            if ((dataGridView == null) || (dataGridView.EditingControl == null))
            {
                throw new InvalidOperationException();
            }
            if (dataGridView.EditingControl is MaskedTextBox editingControl)
            {
                editingControl.ClearUndo();
            }
            base.DetachEditingControl();
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            if (base.DataGridView.EditingControl is DataGridViewMaskedTextEditingControl editingControl)
            {
                if (Value == null || Value is DBNull)
                {
                    editingControl.Text = (string)DefaultNewRowValue;
                }
                else
                {
                    editingControl.Text = (string)Value;
                }
            }
        }

        #endregion

        #region Derived properties

        public override Type EditType
        {
            get { return editorType; }
        }

        public override Type ValueType
        {
            get { return valueType; }
        }

        #endregion

        #region Additional Mask property

        private string mask;

        /// <summary>
        /// Input String that rules the possible input values in the current cell of the column.
        /// </summary>
        public string Mask
        {
            get { return mask ?? String.Empty; }
            set { mask = value; }
        }

        #endregion

    }

}
