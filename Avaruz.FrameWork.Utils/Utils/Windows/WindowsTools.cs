using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace Avaruz.FrameWork.Utils.Windows
{
    public static class WindowsTools
    {
        private static string separator;
        public static string SeparadorDeListas
        {
            get
            {
                return separator;
            }
            set
            {
                separator = value;
            }
        }
        public static void MostrarMensajeInformativo(string Mensaje, string Titulo = "")
        {
            Titulo = Titulo == "" ? System.Windows.Forms.Application.ProductName : Titulo;
            MessageBox.Show(Mensaje, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult MostrarMensajeSiNo(string Mensaje, string Titulo = "")
        {
            Titulo = Titulo == "" ? System.Windows.Forms.Application.ProductName : Titulo;
            return MessageBox.Show(Mensaje, Titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static void MostrarMensajeError(string Mensaje, string Titulo = "")
        {

            MessageBox.Show(Mensaje, Titulo == "" ? System.Windows.Forms.Application.ProductName : Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void MostrarMensajeAdvertencia(string Mensaje, string Titulo = "")
        {

            MessageBox.Show(Mensaje, Titulo == "" ? System.Windows.Forms.Application.ProductName : Titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MostrarMensajeExcepcion(Exception ex, [CallerMemberName]  string metodoError = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("Error en el metodo " + metodoError);
            sb.AppendLine("Exception: " + ex.GetType().ToString());
            sb.AppendLine("Source:  " + ex.Source);
            sb.AppendLine("Message:  " + ex.Message);
            sb.AppendLine("InnerException:  " + (ex.InnerException != null ? ex.InnerException.Message : ""));
            sb.AppendLine("StackTrace:" + ex.StackTrace);
            sb.AppendLine("Hora:" + DateTime.Now.ToString());

            MessageBox.Show(sb.ToString(), System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void FormatearColumnaTexto(ref DataGridViewColumn Columna, string TextoCabecera, int Indice, bool Congelada = false, bool SoloLectura = false)
        {
            string Formato = "";
            FormatearColumna(ref Columna, TextoCabecera, Indice, Formato, DataGridViewContentAlignment.TopLeft, Congelada, SoloLectura);
        }
        public static void FormatearColumna2Decimal(ref DataGridViewColumn Columna, string TextoCabecera, int Indice, bool Congelada = false, bool SoloLectura = false)
        {
            string Formato = "#,##0.00";
            FormatearColumna(ref Columna, TextoCabecera, Indice, Formato, DataGridViewContentAlignment.TopRight, Congelada, SoloLectura);
        }
        public static void FormatearColumnaEntero(ref DataGridViewColumn Columna, string TextoCabecera, int Indice, bool Congelada = false, bool SoloLectura = false)
        {
            string Formato = "#,##0";
            FormatearColumna(ref Columna, TextoCabecera, Indice, Formato, DataGridViewContentAlignment.TopRight, Congelada, SoloLectura);
        }

        /// <summary>
        ///     ''' 
        ///     ''' </summary>
        ///     ''' <param name="Columna"></param>
        ///     ''' <param name="Indice"></param>
        ///     ''' <param name="Formato"></param>
        ///     ''' <param name="Alineacion"></param>
        ///     ''' <remarks></remarks>
        public static void FormatearColumna(ref DataGridViewColumn Columna, string TextoCabecera, int Indice, string Formato, DataGridViewContentAlignment Alineacion = DataGridViewContentAlignment.NotSet, bool Congelada = false, bool SoloLectura = false)
        {

            if (TextoCabecera != "")
            {
                Columna.HeaderText = TextoCabecera;
            }

            Columna.DisplayIndex = Indice;
            Columna.ReadOnly = SoloLectura;
            Columna.Frozen = Congelada;
            Columna.DefaultCellStyle.Format = string.Format(Formato);
            Columna.DefaultCellStyle.Alignment = Alineacion;
        }

        public static void AnexarDataGridCombo<T>(ref DataGridViewComboBoxColumn ListaDesplegable, string MiembroVisible, string MiembroValor, string NombrePropiedad, string TextoCabecera, List<T> FuenteDatos, bool SoloLectura = false, FlatStyle Estilo = FlatStyle.Flat)
        {
            {
                if (NombrePropiedad != string.Empty)
                {
                    ListaDesplegable.DataPropertyName = NombrePropiedad;
                }

                ListaDesplegable.FlatStyle = Estilo;
                ListaDesplegable.HeaderText = TextoCabecera;
                if (MiembroValor != string.Empty)
                {
                    ListaDesplegable.DisplayMember = MiembroVisible;
                }

                if (MiembroValor != string.Empty)
                {
                    ListaDesplegable.ValueMember = MiembroValor;
                }

                ListaDesplegable.ReadOnly = SoloLectura;
                ListaDesplegable.DataSource = FuenteDatos;
            }
        }
        public static void AnexarFuente<T>(this ListBox ListaDesplegable, string MiembroVisible, string MiembroValor, List<T> FuenteDatos)
        {
            {
                ListaDesplegable.DisplayMember = MiembroVisible;
                ListaDesplegable.ValueMember = MiembroValor;
                ListaDesplegable.DataSource = FuenteDatos;
            }
        }

        public static void AnexarFuente<T>(this CheckedListBox ListaDesplegable, string MiembroVisible, string MiembroValor, List<T> FuenteDatos)
        {
            {
                ListaDesplegable.DisplayMember = MiembroVisible;
                ListaDesplegable.ValueMember = MiembroValor;
                ListaDesplegable.DataSource = FuenteDatos;
            }
        }
        public static void AnexarFuente<T>(this ComboBox ListaDesplegable, string MiembroVisible, string MiembroValor, List<T> FuenteDatos)
        {
            {
                ListaDesplegable.DisplayMember = MiembroVisible;
                ListaDesplegable.ValueMember = MiembroValor;
                ListaDesplegable.DataSource = FuenteDatos;
            }
        }
        public static void AnexarFuente<T>(this DataGridViewComboBoxColumn ListaDesplegable, string MiembroVisible, string MiembroValor, string NombrePropiedad, string TextoCabecera, List<T> FuenteDatos, bool SoloLectura = false, FlatStyle Estilo = FlatStyle.Flat)
        {
            {
                if (NombrePropiedad != string.Empty)
                {
                    ListaDesplegable.DataPropertyName = NombrePropiedad;
                }

                ListaDesplegable.FlatStyle = Estilo;
                ListaDesplegable.HeaderText = TextoCabecera;
                if (MiembroValor != string.Empty)
                {
                    ListaDesplegable.DisplayMember = MiembroVisible;
                }

                if (MiembroValor != string.Empty)
                {
                    ListaDesplegable.ValueMember = MiembroValor;
                }

                ListaDesplegable.ReadOnly = SoloLectura;
                ListaDesplegable.DataSource = FuenteDatos;
            }
        }
        public static void AnexarCombo<T>(ref ComboBox ListaDesplegable, string MiembroVisible, string MiembroValor, List<T> FuenteDatos)
        {
            ListaDesplegable.DisplayMember = MiembroVisible;
            ListaDesplegable.ValueMember = MiembroValor;
            ListaDesplegable.DataSource = FuenteDatos;
        }
        public static void AnexarList<T>(ref ListBox ListaDesplegable, string MiembroVisible, string MiembroValor, List<T> FuenteDatos)
        {
            ListaDesplegable.DisplayMember = MiembroVisible;
            ListaDesplegable.ValueMember = MiembroValor;
            ListaDesplegable.DataSource = FuenteDatos;
        }

        public static void ExportDGVToCSV(DataGridView DataGridView, string Titulo = "", bool blnWriteColumnHeaderNames = true, string strDelimiterType = ",")
        {
            string strExportFileName;

            using (SaveFileDialog oSaveFileDialog = new SaveFileDialog())
            {
                oSaveFileDialog.Title = "Exportando " + Titulo;
                oSaveFileDialog.AddExtension = true;
                oSaveFileDialog.OverwritePrompt = true;
                oSaveFileDialog.DefaultExt = "csv";
                oSaveFileDialog.FileName = Titulo;
                oSaveFileDialog.Filter = "Archivos CSV (*.csv)|*.csv";
                separator = string.IsNullOrEmpty(separator) ? System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator : separator;

                if (oSaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    strExportFileName = oSaveFileDialog.FileName;

                    StreamWriter sr;
                    try
                    {
                        sr = new StreamWriter(strExportFileName, false, Encoding.UTF8);
                    }
                    catch (IOException ex)
                    {
                        throw new Exception(strExportFileName + " el archivo que Ud. esta tratando de usar esta en uso");

                    }


                    string strDelimiter = strDelimiterType;

                    int intColumnCount = DataGridView.Columns.Count - 1;

                    string strRowData = "";
                    if (blnWriteColumnHeaderNames)
                    {
                        for (int intX = 0; intX <= intColumnCount; intX++)
                        {
                            strRowData += DataGridView.Columns[intX].HeaderText.Replace(strDelimiter, "") + (intX < intColumnCount ? strDelimiter : "");
                        }

                        // * Write the column header data to the CSV file.
                        sr.WriteLine(strRowData);
                    }
                    // * If blnWriteColumnHeaderNames

                    for (int intX = 0; intX <= DataGridView.Rows.Count - 1; intX++)
                    {
                        strRowData = "";
                        for (int intRowData = 0; intRowData <= intColumnCount; intRowData++)
                        {
                            strRowData += DataGridView.Rows[intX].Cells[intRowData].FormattedValue.ToString().Replace(strDelimiter, "") + (intX < intColumnCount ? strDelimiter : "");
                        }

                        sr.WriteLine(strRowData);
                    }
                    sr.Close();
                    MessageBox.Show("Proceso Terminado con éxito!", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        /// <summary>
        ///     ''' Exporta los datos de un DataGridView a CSV
        ///     ''' </summary>
        ///     ''' <param name="dgv"></param>
        ///     ''' <remarks></remarks>
        public static void ExportDataGridView(DataGridView dgv)
        {
            DataGridViewRow row;
            StreamWriter sWriter;
            string sValue;
            string Filename;
            SortedDictionary<int, string> headerList = new SortedDictionary<int, string>();
            SortedDictionary<int, string> columnsList = new SortedDictionary<int, string>();



            using (SaveFileDialog oSaveFileDialog = new SaveFileDialog())
            {
                oSaveFileDialog.AddExtension = true;
                oSaveFileDialog.OverwritePrompt = true;
                oSaveFileDialog.DefaultExt = "csv";
                oSaveFileDialog.Filter = "Archivos CSV (*.csv)|*.csv";

                if (oSaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Filename = oSaveFileDialog.FileName;


                    try
                    {
                        sWriter = new StreamWriter(Filename, false);
                    }
                    catch (IOException ex)
                    {
                        throw new Exception($"{Filename} is in Use. {ex.Message}");
                    }

                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (col.Visible)
                        {
                            headerList.Add(col.DisplayIndex, col.HeaderText);
                            columnsList.Add(col.Index, col.HeaderText);
                        }
                    }

                    int i = 0;

                    foreach (KeyValuePair<int, string> element in headerList)
                    {
                        sWriter.Write(element.Value);
                        i += 1;

                        if (i < headerList.Count)
                        {
                            sWriter.Write(separator);
                        }
                    }

                    sWriter.WriteLine();
                    foreach (DataGridViewRow row2 in dgv.Rows)
                    {
                        foreach (KeyValuePair<int, string> element in columnsList)
                        {
                            var col2 = dgv.Columns[element.Value];

                            try
                            {
                                if (row2.Cells[col2.Index].Value != null)
                                {
                                    switch (row2.Cells[col2.Index].ValueType.Name)
                                    {
                                        case "String":
                                            {
                                                if (row2.Cells[col2.Index] != null)
                                                {
                                                    sValue = "";
                                                }
                                                else
                                                {
                                                    sValue = System.Convert.ToString(row2.Cells[col2.Index].Value);
                                                }

                                                if (sValue != string.Empty)
                                                {
                                                    sValue = sValue.Replace(separator, " ");
                                                }

                                                sWriter.Write(sValue.Trim());
                                                break;
                                            }

                                        default:
                                            {
                                                sWriter.Write(row2.Cells[col2.Index].Value);
                                                break;
                                            }
                                    }
                                }
                                // row.Cells(col.Index).Value i snothing
                                if (col2.Index < dgv.Columns.Count)
                                {
                                    sWriter.Write(separator);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Something went wrong Exporting DatagridView");
                            }
                        }
                        sWriter.WriteLine();
                    }
                    // row
                    sWriter.Flush();
                    sWriter.Close();
                }
            }
        }

        /// <summary>
        ///     ''' Realiza los Databindings de todos los controles en el formulario.
        ///     ''' </summary>
        ///     ''' <remarks></remarks>
        public static void BindControls(ref Panel ParentControl, ref object Objeto, bool Activo)
        {
            foreach (Control c in ParentControl.Controls)
            {
                c.DataBindings.Clear();
                try
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Windows.Forms.TextBox":
                            {
                                c.DataBindings.Add("Text", Objeto, c.Name);
                                break;
                            }

                        case "System.Windows.Forms.DateTimePicker":
                            {
                                c.DataBindings.Add("Text", Objeto, c.Name);
                                break;
                            }

                        case "System.Windows.Forms.CheckBox":
                            {
                                c.DataBindings.Add("Checked", Objeto, c.Name);
                                break;
                            }
                    }
                    c.Enabled = Activo;
                }
                catch (Exception ex)
                {
                }
            }
        }
        /// <summary>
        ///     ''' 
        ///     ''' </summary>
        ///     ''' <param name="ClassName"></param>
        ///     ''' <param name="ParentForm"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public static bool IsFormActive(string ClassName, ref System.Windows.Forms.Form ParentForm)
        {
            bool IsActive = false;

            foreach (System.Windows.Forms.Form F in ParentForm.MdiChildren)
            {
                if (F.Name.ToString() == ClassName)
                {
                    F.Activate();
                    IsActive = true;
                    break;
                }
            }

            return IsActive;
        }
        /// <summary>
        ///     ''' 
        ///     ''' </summary>
        ///     ''' <param name="ClassName"></param>
        ///     ''' <param name="AssemblyName"></param>
        ///     ''' <param name="ParentForm"></param>
        ///     ''' <remarks></remarks>
        public static void FormActivator<T>(string ClassName, string AssemblyName, System.Windows.Forms.Form ParentForm, string rutaAssembly) where T : Form
        {
            if (!IsFormActive(ClassName, ref ParentForm))
            {
                T F = WindowsTools.GetForm<T>(AssemblyName, ClassName, rutaAssembly);
                if ((F) is Form)
                {
                    {
                        F.MdiParent = ParentForm;
                        F.StartPosition = FormStartPosition.CenterScreen;
                        F.MaximizeBox = true;
                        F.ShowInTaskbar = false;
                        F.Show();
                    }
                }
            }
        }

        /// <summary>
        ///     ''' Busca y activa un cuadro de dialogo
        ///     ''' </summary>
        ///     ''' <param name="ClassName"></param>
        ///     ''' <param name="AssemblyName"></param>
        ///     ''' <remarks></remarks>
        public static void DialogActivator(string ClassName, string AssemblyName, System.Windows.Forms.Form ParentForm, string rutaAssembly)
        {
            System.Windows.Forms.Form F = WindowsTools.GetForm<Form>(AssemblyName, ClassName, rutaAssembly);
            F.StartPosition = FormStartPosition.CenterParent;

            F.MaximizeBox = false;
            F.ShowInTaskbar = false;
            F.FormBorderStyle = FormBorderStyle.FixedDialog;
            F.ShowDialog(ParentForm);
        }
        /// <summary>
        ///     ''' 
        ///     ''' </summary>
        ///     ''' <param name="assemblyName"></param>
        ///     ''' <param name="className"></param>
        ///     ''' <param name="rutaAssembly"></param>
        ///     ''' <param name="EsDll"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        private static T GetForm<T>(string assemblyName, string className, string rutaAssembly, bool EsDll = false)
        {
            T F;
            string extension = EsDll ? ".dll" : ".exe";
            string FullPath = rutaAssembly + assemblyName + extension;
            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFile(FullPath);
            string FullClassName = string.Format("{0}.{1}", assemblyName, className);
            System.Type formObject = asm.GetType(FullClassName);
            object formActivator = Activator.CreateInstance(formObject);
            F = (T)formActivator;

            return F;
        }


        public static List<string> FormLister(string AssemblyName, string rutaAssembly)
        {
            List<string> ListadeForms = new List<string>();
            string FullPath = string.Format("{0}{1}.dll", rutaAssembly, AssemblyName);
            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFile(FullPath);

            foreach (Type tipo in asm.GetTypes())
            {
                if (tipo.BaseType.FullName == "System.Windows.Forms.Form")
                {
                    ListadeForms.Add(tipo.Name);
                }
            }
            return ListadeForms;
        }
    }

}
