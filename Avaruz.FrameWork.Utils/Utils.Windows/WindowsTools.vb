Imports System.Windows.Forms
Imports System.IO
Imports System.Text


Public Module WindowsTools
    Sub MostrarMensajeInformativo(ByVal Mensaje As String, Optional ByVal Titulo As String = "")
        Titulo = IIf(Titulo = "", My.Application.Info.ProductName, Titulo)
        MessageBox.Show(Mensaje, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Function MostrarMensajeSiNo(ByVal Mensaje As String, Optional ByVal Titulo As String = "") As DialogResult
        Titulo = IIf(Titulo = "", My.Application.Info.ProductName, Titulo)
        Return _
            MessageBox.Show(Mensaje, My.Application.Info.ProductName, MessageBoxButtons.YesNo, _
                             MessageBoxIcon.Question)
    End Function

    Sub MostrarMensajeError(ByVal Mensaje As String)
        MessageBox.Show(Mensaje, My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
    Sub FormatearColumnaTexto(ByRef Columna As DataGridViewColumn, ByVal TextoCabecera As String, _
                      ByVal Indice As Integer, Optional ByVal Congelada As Boolean = False, _
                      Optional ByVal SoloLectura As Boolean = False)


        Dim Formato As String = ""
        FormatearColumna(Columna, TextoCabecera, Indice, Formato, DataGridViewContentAlignment.TopLeft, Congelada, SoloLectura)
    End Sub
    Sub FormatearColumna2Decimal(ByRef Columna As DataGridViewColumn, ByVal TextoCabecera As String, _
                          ByVal Indice As Integer, Optional ByVal Congelada As Boolean = False, _
                          Optional ByVal SoloLectura As Boolean = False)


        Dim Formato As String = "#,##0.00"
        FormatearColumna(Columna, TextoCabecera, Indice, Formato, DataGridViewContentAlignment.TopRight, Congelada, SoloLectura)
    End Sub
    Sub FormatearColumnaEntero(ByRef Columna As DataGridViewColumn, ByVal TextoCabecera As String, _
                      ByVal Indice As Integer, Optional ByVal Congelada As Boolean = False, _
                      Optional ByVal SoloLectura As Boolean = False)


        Dim Formato As String = "#,##0"
        FormatearColumna(Columna, TextoCabecera, Indice, Formato, DataGridViewContentAlignment.TopRight, Congelada, SoloLectura)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Columna"></param>
    ''' <param name="Indice"></param>
    ''' <param name="Formato"></param>
    ''' <param name="Alineacion"></param>
    ''' <remarks></remarks>
    Sub FormatearColumna(ByRef Columna As DataGridViewColumn, ByVal TextoCabecera As String, _
                          ByVal Indice As Integer, ByVal Formato As String, _
                          Optional ByVal Alineacion As DataGridViewContentAlignment = _
                             DataGridViewContentAlignment.NotSet, Optional ByVal Congelada As Boolean = False, _
                          Optional ByVal SoloLectura As Boolean = False)
        With Columna
            If TextoCabecera <> "" Then
                .HeaderText = TextoCabecera
            End If
            .DisplayIndex = Indice
            .ReadOnly = SoloLectura
            .Frozen = Congelada
            .DefaultCellStyle.Format = String.Format(Formato)
            .DefaultCellStyle.Alignment = Alineacion
        End With

    End Sub

    Public Sub AnexarDataGridCombo(Of T)(ByRef ListaDesplegable As DataGridViewComboBoxColumn, _
                                           ByVal MiembroVisible As String, ByVal MiembroValor As String, _
                                           ByVal NombrePropiedad As String, ByVal TextoCabecera As String, _
                                           ByVal FuenteDatos As List(Of T), _
                                           Optional ByVal SoloLectura As Boolean = False, _
                                           Optional ByVal Estilo As FlatStyle = FlatStyle.Flat)
        With ListaDesplegable
            If NombrePropiedad <> String.Empty Then
                .DataPropertyName = NombrePropiedad
            End If
            .FlatStyle = Estilo
            .HeaderText = TextoCabecera
            If MiembroValor <> String.Empty Then
                .DisplayMember = MiembroVisible
            End If
            If MiembroValor <> String.Empty Then
                .ValueMember = MiembroValor
            End If
            .ReadOnly = SoloLectura
            .DataSource = FuenteDatos
        End With
    End Sub

    Public Sub AnexarCombo(Of T)(ByRef ListaDesplegable As ComboBox, ByVal MiembroVisible As String, _
                                   ByVal MiembroValor As String, ByVal FuenteDatos As List(Of T))
        With ListaDesplegable
            .DisplayMember = MiembroVisible
            .ValueMember = MiembroValor
            .DataSource = FuenteDatos
        End With
    End Sub

    Public Sub ExportDGVToCSV(ByVal DataGridView As DataGridView, Optional ByVal Titulo As String = "", _
                               Optional ByVal blnWriteColumnHeaderNames As Boolean = True, _
                               Optional ByVal strDelimiterType As String = ",")

        Dim strExportFileName As String

        Using oSaveFileDialog As New SaveFileDialog()
            oSaveFileDialog.Title = "Exportando " & Titulo
            oSaveFileDialog.AddExtension = True
            oSaveFileDialog.OverwritePrompt = True
            oSaveFileDialog.DefaultExt = "csv"
            oSaveFileDialog.FileName = Titulo
            oSaveFileDialog.Filter = "Archivos CSV (*.csv)|*.csv"

            If oSaveFileDialog.ShowDialog() = DialogResult.OK Then
                strExportFileName = oSaveFileDialog.FileName

                Dim sr As StreamWriter
                Try

                    sr = New StreamWriter(strExportFileName, False)
                Catch ex As IOException
                    Throw _
                        New Exception(strExportFileName & " el archivo que Ud. esta tratando de usar esta en uso")
                    'probably
                    Return
                End Try


                Dim strDelimiter As String = strDelimiterType

                Dim intColumnCount As Integer = DataGridView.Columns.Count - 1

                Dim strRowData As String = ""
                If blnWriteColumnHeaderNames Then

                    For intX As Integer = 0 To intColumnCount
                        strRowData += Replace(DataGridView.Columns(intX).HeaderText, strDelimiter, "") & _
                                      IIf(intX < intColumnCount, strDelimiter, "")

                    Next intX

                    '* Write the column header data to the CSV file.
                    sr.WriteLine(strRowData)

                End If
                '* If blnWriteColumnHeaderNames

                For intX As Integer = 0 To DataGridView.Rows.Count - 1
                    strRowData = ""
                    For intRowData As Integer = 0 To intColumnCount
                        strRowData += _
                            Replace(DataGridView.Rows(intX).Cells(intRowData).FormattedValue, strDelimiter, "") & _
                            IIf(intRowData < intColumnCount, strDelimiter, "")

                    Next intRowData
                    sr.WriteLine(strRowData)
                Next intX
                sr.Close()
                MsgBox("Proceso Terminado con éxito!", MsgBoxStyle.Information, "Exportar")
            End If
        End Using


    End Sub


    ''' <summary>
    ''' Exporta los datos de un DataGridView a CSV
    ''' </summary>
    ''' <param name="dgv"></param>
    ''' <remarks></remarks>
    Sub ExportDataGridView(ByVal dgv As DataGridView)
        Dim row As DataGridViewRow
        Dim col As DataGridViewColumn
        Dim sWriter As StreamWriter
        Dim sValue As String
        Dim Filename As String
        Dim headerList As New SortedDictionary(Of Integer, String)
        Dim columnsList As New SortedDictionary(Of Integer, String)


        Using oSaveFileDialog As New SaveFileDialog()
            oSaveFileDialog.AddExtension = True
            oSaveFileDialog.OverwritePrompt = True
            oSaveFileDialog.DefaultExt = "csv"
            oSaveFileDialog.Filter = "Archivos CSV (*.csv)|*.csv"

            If oSaveFileDialog.ShowDialog() = DialogResult.OK Then
                Filename = oSaveFileDialog.FileName


                Try
                    sWriter = New StreamWriter(Filename, False)
                Catch ex As IOException
                    Throw New Exception(Filename & " is in Use")
                    'probably
                    Return
                End Try

                For Each col In dgv.Columns
                    If col.Visible Then
                        headerList.Add(col.DisplayIndex, col.HeaderText)
                        columnsList.Add(col.Index, col.HeaderText)
                    End If
                Next

                Dim i As Integer = 0

                For Each element As KeyValuePair(Of Integer, String) In headerList
                    sWriter.Write(element.Value)
                    i = i + 1

                    If i < headerList.Count Then
                        sWriter.Write(",")
                    End If
                Next

                sWriter.WriteLine()
                For Each row In dgv.Rows
                    For Each element As KeyValuePair(Of Integer, String) In columnsList
                        col = dgv.Columns(element.Value)

                        Try
                            If Not row.Cells(col.Index).Value Is Nothing Then
                                Select Case row.Cells(col.Index).ValueType.Name
                                    Case "String"
                                        If IsNothing(row.Cells(col.Index)) Then
                                            sValue = ""
                                        Else
                                            sValue = CType(row.Cells(col.Index).Value, String)
                                        End If
                                        If sValue <> String.Empty Then
                                            sValue = sValue.Replace(",", " ")
                                            'lose the commas
                                        End If

                                        sWriter.Write(sValue.Trim)
                                        'Case Additional Formatting Not Implemented
                                    Case Else
                                        sWriter.Write(row.Cells(col.Index).Value)
                                End Select
                            End If
                            'row.Cells(col.Index).Value i snothing
                            If col.Index < dgv.Columns.Count Then sWriter.Write(",")
                        Catch ex As Exception
                            Throw New Exception("Something went wrong Exporting DatagridView")
                        End Try

                    Next
                    sWriter.WriteLine()
                Next
                'row
                sWriter.Flush()
                sWriter.Close()
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Realiza los Databindings de todos los controles en el formulario.
    ''' </summary>
    ''' <remarks></remarks>
    Sub BindControls(ByRef ParentControl As Panel, ByRef Objeto As Object, Activo As Boolean)
        Dim c As Control
        For Each c In ParentControl.Controls
            c.DataBindings.Clear()
            Try
                Select Case c.GetType().ToString()
                    Case "System.Windows.Forms.TextBox"
                        c.DataBindings.Add("Text", Objeto, c.Name)
                    Case "System.Windows.Forms.DateTimePicker"
                        c.DataBindings.Add("Text", Objeto, c.Name)
                    Case "System.Windows.Forms.CheckBox"
                        c.DataBindings.Add("Checked", Objeto, c.Name)
                End Select
                c.Enabled = Activo
            Catch ex As Exception
                'no hago nada
            End Try
        Next
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ClassName"></param>
    ''' <param name="ParentForm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsFormActive(ByVal ClassName As String, ByRef ParentForm As System.Windows.Forms.Form) As Boolean
        Dim IsActive As Boolean = False

        For Each F As System.Windows.Forms.Form In ParentForm.MdiChildren
            If F.Name.ToString() = ClassName Then
                F.Activate()
                IsActive = True
                Exit For
            End If
        Next

        Return IsActive
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ClassName"></param>
    ''' <param name="AssemblyName"></param>
    ''' <param name="ParentForm"></param>
    ''' <remarks></remarks>
    Public Sub FormActivator(ByVal ClassName As String, ByVal AssemblyName As String, ParentForm As System.Windows.Forms.Form, ByVal rutaAssembly As String)
        If Not IsFormActive(ClassName, ParentForm) Then
            Dim F As System.Windows.Forms.Form = WindowsTools.GetForm(AssemblyName, ClassName, rutaAssembly)
            F.MdiParent = ParentForm
            F.StartPosition = FormStartPosition.CenterScreen
            F.MaximizeBox = True
            F.ShowInTaskbar = False
            F.FormBorderStyle = FormBorderStyle.Sizable
            F.Show()
        End If
    End Sub

    ''' <summary>
    ''' Busca y activa un cuadro de dialogo
    ''' </summary>
    ''' <param name="ClassName"></param>
    ''' <param name="AssemblyName"></param>
    ''' <remarks></remarks>
    Public Sub DialogActivator(ByVal ClassName As String, ByVal AssemblyName As String, ParentForm As System.Windows.Forms.Form, ByVal rutaAssembly As String)
        Dim F As System.Windows.Forms.Form = WindowsTools.GetForm(AssemblyName, ClassName, rutaAssembly)
        F.StartPosition = FormStartPosition.CenterParent

        F.MaximizeBox = False
        F.ShowInTaskbar = False
        F.FormBorderStyle = FormBorderStyle.FixedDialog
        F.ShowDialog(ParentForm)

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="assemblyName"></param>
    ''' <param name="className"></param>
    ''' <param name="rutaAssembly"></param>
    ''' <param name="EsDll"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetForm(assemblyName As String, className As String, rutaAssembly As String, Optional ByVal EsDll As Boolean = False) As Form
        Dim F As System.Windows.Forms.Form
        Dim extension As String = IIf(EsDll, ".dll", ".exe")
        Dim FullPath As String = rutaAssembly + assemblyName + extension
        Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.LoadFile(FullPath)
        Dim FullClassName As String = String.Format("{0}.{1}", assemblyName, className)
        Dim formObject As System.Type = asm.GetType(FullClassName)
        Dim formActivator As Object = Activator.CreateInstance(formObject)
        F = CType(formActivator, Form)

        Return F
    End Function


    ''' <summary>
    ''' Busca y activa un cuadro de dialogo
    ''' </summary>
    ''' <param name="ClassName"></param>
    ''' <param name="AssemblyName"></param>
    ''' <remarks></remarks>
    Public Function DialogActivator(ByVal ClassName As String, ByVal AssemblyName As String, RutaAssembly As String) As Form

        Dim F As Form = GetForm(AssemblyName, ClassName, RutaAssembly)

        F.StartPosition = FormStartPosition.CenterParent

        F.MaximizeBox = False
        F.ShowInTaskbar = False
        F.FormBorderStyle = FormBorderStyle.FixedDialog
        Return F

    End Function

    Public Function FormLister(ByVal AssemblyName As String, rutaAssembly As String) As List(Of String)
        Dim ListadeForms As New List(Of String)
        Dim FullPath As String = String.Format("{0}{1}.dll", rutaAssembly, AssemblyName)
        Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.LoadFile(FullPath)

        For Each tipo As Type In asm.GetTypes
            If tipo.BaseType.FullName = "System.Windows.Forms.Form" Then
                ListadeForms.Add(tipo.Name)
            End If
        Next
        Return ListadeForms
    End Function
End Module