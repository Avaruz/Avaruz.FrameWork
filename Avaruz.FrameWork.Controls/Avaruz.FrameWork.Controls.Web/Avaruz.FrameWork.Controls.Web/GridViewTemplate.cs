using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Avaruz.FrameWork.Controls.Web
{
    public enum ControlType
    {
        LabelControl = 0,
        EditControl = 1,
        DropDownControl = 2,
        CheckBoxControl = 3,
        TextAreaControl = 4

    }
    public class GridViewTemplate : ITemplate
    {

        private ListItemType _templateType;
        private string _columnName;
        private string _col;
        private string _dataType;
        private ControlType _typeControl;
        private bool _isEnable;
        private int _widthInPixel;
        private bool _verticalHeader;
        private string _nombreBase;

        public List<ListItem> DatosDdl = new List<ListItem>();
        private bool _useDefaultCSSClass;
        private bool _useBorder;

        public GridViewTemplate(ListItemType type,
            string colname, string col, string dataType, ControlType typeControl, bool isEnable, int widthInPixel,
            bool verticalHeader = false, string nombreBase = "", bool useDefaultCSSClass = true, bool useBorder = true)
        {
            _templateType = type;
            _columnName = colname;
            _dataType = dataType;
            _col = col;
            _typeControl = typeControl;
            _isEnable = isEnable;
            _widthInPixel = widthInPixel;
            _verticalHeader = verticalHeader;
            _nombreBase = nombreBase;
            _useDefaultCSSClass = useDefaultCSSClass;
            _useBorder = useBorder;
        }

        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            Label lbl = new Label();
            switch (_templateType)
            {
                case ListItemType.Header:
                    container.Controls.Add(lbl);
                    lbl.Text = _columnName;


                    if (_verticalHeader)
                    {
                        lbl.Font.Size = FontUnit.Small;
                        lbl.Font.Bold = false;
                        lbl.Style.Add("writing-mode", "tb-rl");
                        lbl.Style.Add("filter", "flipv fliph");
                    }
                    break;

                case ListItemType.Item:
                    switch (_typeControl)
                    {
                        case ControlType.LabelControl:
                            Label lblc = new Label();
                            lblc.ID = String.IsNullOrWhiteSpace(_nombreBase) ? "lbl" + _col : _nombreBase + _col;
                            if (_widthInPixel > 0)
                                lblc.Width = Unit.Pixel(_widthInPixel);
                            lblc.DataBinding += new EventHandler(lbl_DataBinding);
                            container.Controls.Add(lblc);
                            break;
                        case ControlType.EditControl:

                            TextBox edt = new TextBox();
                            edt.ID = String.IsNullOrWhiteSpace(_nombreBase) ? "edt" + _col : _nombreBase + _col;
                            if (this._useBorder)
                            {
                                edt.BorderStyle = BorderStyle.Solid;
                                edt.BorderWidth = Unit.Pixel(1);
                            }
                            else
                            {
                                edt.BorderStyle = BorderStyle.None;
                                edt.BorderWidth = Unit.Pixel(0);
                            }
                            edt.Enabled = _isEnable;
                            edt.CssClass = this._useDefaultCSSClass ? "text ui-widget ui-corner-all" : "";
                            edt.Width = Unit.Pixel(_widthInPixel);
                            edt.Style["text-align"] = "right";
                            edt.Attributes.Add("colName", this._columnName);
                            container.Controls.Add(edt);
                            edt.DataBinding += new EventHandler(edt_DataBinding);
                            edt.Columns = 1;
                            break;
                        case ControlType.CheckBoxControl:

                            CheckBox chb = new CheckBox();
                            chb.ID = String.IsNullOrWhiteSpace(_nombreBase) ? "chb" + _col : _nombreBase + _col;
                            chb.Attributes.Add("colName", this._columnName);
                            chb.Attributes.Add("colIndex", this._col);

                            chb.Enabled = _isEnable;
                            container.Controls.Add(chb);
                            chb.DataBinding += new EventHandler(chb_DataBinding);
                            break;

                        case ControlType.DropDownControl:

                            DropDownList ddl = new DropDownList();
                            ddl.ID = String.IsNullOrWhiteSpace(_nombreBase) ? "ddl" + _col : _nombreBase + _col;
                            if (this._useBorder)
                            {
                                ddl.BorderStyle = BorderStyle.Solid;
                                ddl.BorderWidth = Unit.Pixel(1);
                            }
                            else
                            {
                                ddl.BorderStyle = BorderStyle.None;
                                ddl.BorderWidth = Unit.Pixel(0);
                            }
                            ddl.Width = Unit.Pixel(_widthInPixel);
                            ddl.Enabled = _isEnable;
                            ddl.DataSource = DatosDdl;
                            ddl.DataTextField = "Text";
                            ddl.DataValueField = "Value";

                            ddl.DataBind();
                            ddl.CssClass = this._useDefaultCSSClass ? "text ui-widget ui-corner-all" : "";
                            container.Controls.Add(ddl);
                            ddl.DataBinding += new EventHandler(ddl_DataBinding);

                            break;


                        case ControlType.TextAreaControl:

                            TextBox eda = new TextBox();
                            eda.ID = String.IsNullOrWhiteSpace(_nombreBase) ? "eda" + _col : _nombreBase + _col;
                            eda.BorderStyle = BorderStyle.Solid;
                            eda.BorderWidth = Unit.Pixel(1);
                            eda.Enabled = _isEnable;
                            eda.TextMode = TextBoxMode.MultiLine;
                            eda.Attributes.Add("maxLength", _widthInPixel.ToString(CultureInfo.InvariantCulture));
                            eda.Height = Unit.Pixel(30);
                            //eda.CssClass = "ui-widget ui-corner-all";
                            eda.Width = Unit.Pixel(_widthInPixel / 2);
                            eda.Attributes.Add("colName", this._columnName);
                            container.Controls.Add(eda);
                            eda.DataBinding += new EventHandler(eda_DataBinding);
                            eda.Columns = 1;
                            break;


                    }
                    break;
            }

        }

        public static void anexarCombo<T>(ref DropDownList listaDesplegable, string campoTexto, string campoValor, List<T> fuenteDatos)
        {

            listaDesplegable.DataTextField = campoTexto;
            listaDesplegable.DataValueField = campoValor;
            listaDesplegable.DataSource = fuenteDatos;
        }

        // Databind an edit box in the grid
        void edt_DataBinding(object sender, EventArgs e)
        {
            TextBox txtdata = (TextBox)sender;
            GridViewRow container = (GridViewRow)txtdata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            // Add JavaScript function sav(row,col,hours) which will save changes
            //txtdata.Attributes.Add("onchange", "sav(" + container.RowIndex.ToString() + "," + _columnName + ",this.value)");
            if (dataValue != DBNull.Value)
                txtdata.Text = dataValue.ToString();
        }

        // Databind an edit box in the grid
        void eda_DataBinding(object sender, EventArgs e)
        {
            TextBox txtdata = (TextBox)sender;
            GridViewRow container = (GridViewRow)txtdata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            // Add JavaScript function sav(row,col,hours) which will save changes
            int maximunLength = _widthInPixel == 0 ? 100 : _widthInPixel;
            txtdata.Attributes.Add("onkeypress", "return isMaxLength(this," + maximunLength.ToString(CultureInfo.InvariantCulture) + ");");
            // Add JavaScript function sav(row,col,hours) which will save changes
            txtdata.Attributes.Add("onchange", "sav(" + container.RowIndex.ToString(CultureInfo.InvariantCulture) + ",'" + _columnName + "',this.value)");
            if (dataValue != DBNull.Value)
                txtdata.Text = dataValue.ToString();
        }

        // Databind an check box in the grid
        void chb_DataBinding(object sender, EventArgs e)
        {
            CheckBox chbdata = (CheckBox)sender;
            GridViewRow container = (GridViewRow)chbdata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            // Add JavaScript function sav(row,col,hours) which will save changes
            chbdata.Attributes.Add("rowIndex", container.RowIndex.ToString(CultureInfo.InvariantCulture));
            if (dataValue != DBNull.Value)
                chbdata.Checked = Convert.ToBoolean(dataValue);

        }

        // Databind an edit box in the grid
        void ddl_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddldata = (DropDownList)sender;
            GridViewRow container = (GridViewRow)ddldata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            // Add JavaScript function sav(row,col,hours) which will save changes
            ddldata.Attributes.Add("onchange", "sav(" + container.RowIndex.ToString(CultureInfo.InvariantCulture) + ",'" + _columnName + "',this.value)");
            if (dataValue != DBNull.Value)
            {
                ddldata.Text = dataValue.ToString();
            }
        }


        // Databind a label 
        void lbl_DataBinding(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            GridViewRow container = (GridViewRow)lbl.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            if (dataValue != DBNull.Value)
                lbl.Text = dataValue.ToString();
        }
    }

}