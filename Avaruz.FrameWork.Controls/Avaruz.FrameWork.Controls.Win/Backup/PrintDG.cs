using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Data;
using System.Text;

namespace Avaruz.FrameWork.Controls.Win
{
    public class PrintDG
    {
        private static StringFormat StrFormat;  // Holds content of a TextBox Cell to write by DrawString
        private static CheckBox ChkBox;         // Holds content of a Boolean Cell to write by DrawImage 

        private static int TotalWidth;          // Summation of Columns widths
        private static int RowPos;              // Position of currently printing row 
        private static bool NewPage;            // Indicates if a new page reached
        private static int PageNo;              // Number of pages to print
        private static ArrayList ColumnLefts = new ArrayList();  // Left Coordinate of Columns
        private static ArrayList ColumnWidths = new ArrayList(); // Width of Columns
        private static ArrayList ColumnTypes = new ArrayList();  // DataType of Columns
        private static int nHeight;             // Height of DataGrid Cell
        private static int RowsPerPage;         // Number of Rows per Page
        private static System.Drawing.Printing.PrintDocument printDoc =
                       new System.Drawing.Printing.PrintDocument();  // PrintDocumnet Object used for printing

        private static string PrintTitle = "";  // Header of pages
        private static DataGrid dg;             // Holds DataGrid Object to print its contents
        private static List<string> SelectedColumns = new List<string>();   // The Columns Selected by user to print.
        private static List<string> AvailableColumns = new List<string>();  // All Columns avaiable in DataGrid 
        private static Font PrintFont;           // Font to use in the printing of DataGrid contents
        private static Color PrintFontColor;     // Font Color to use in the printing of DataGrid contents
        private static bool PrintAllRows = true; // True = print all rows,  False = print selected rows    

        public static void Print_DataGrid(DataGrid dg1, string printTitle, bool printAllRows)
        {
            PrintPreviewDialog ppvw;
            try
            {
                // Save DataGrid attributes
                dg = dg1;
                PrintFont = dg.Font;
                PrintFontColor = dg.ForeColor;

                // Get all Coulmns Names in the DataGrid
                AvailableColumns.Clear();
                foreach (DataGridColumnStyle c in dg.TableStyles[0].GridColumnStyles)
                    AvailableColumns.Add(c.HeaderText);

                // Show PrintOption Form
                PrintTitle = printTitle;
                PrintAllRows = printAllRows;
                SelectedColumns = AvailableColumns;
                //if (dlg.PrintFont != null) PrintFont = dlg.PrintFont;
                //if (dlg.PrintFontColor.Name != "" & dlg.PrintFontColor.Name != "0")
                //    PrintFontColor = dlg.PrintFontColor;

                RowsPerPage = 0;

                ppvw = new PrintPreviewDialog();
                ppvw.Document = printDoc;

                // Show Print Preview Page
                printDoc.BeginPrint += new System.Drawing.Printing.PrintEventHandler(printDoc_BeginPrint);
                printDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDoc_PrintPage);
                if (ppvw.ShowDialog() != DialogResult.OK)
                {
                    printDoc.BeginPrint -= new System.Drawing.Printing.PrintEventHandler(printDoc_BeginPrint);
                    printDoc.PrintPage -= new System.Drawing.Printing.PrintPageEventHandler(printDoc_PrintPage);
                    return;
                }

                // Print the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= new System.Drawing.Printing.PrintEventHandler(printDoc_BeginPrint);
                printDoc.PrintPage -= new System.Drawing.Printing.PrintPageEventHandler(printDoc_PrintPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
        }

        private static void printDoc_BeginPrint(object sender,
                    System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                // Formatting the Content of Text Cell to print
                StrFormat = new StringFormat();
                StrFormat.Alignment = StringAlignment.Near;
                StrFormat.LineAlignment = StringAlignment.Center;
                StrFormat.Trimming = StringTrimming.EllipsisCharacter;

                ColumnLefts.Clear();
                ColumnWidths.Clear();
                ColumnTypes.Clear();
                nHeight = 0;
                RowsPerPage = 0;

                ChkBox = new CheckBox();

                // Calculating Total Widths
                TotalWidth = 0;
                foreach (DataGridColumnStyle oColumn in dg.TableStyles[0].GridColumnStyles)
                {
                    if (!PrintDG.SelectedColumns.Contains(oColumn.HeaderText)) continue;
                    TotalWidth += oColumn.Width;
                }
                PageNo = 1;
                NewPage = true;
                RowPos = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void printDoc_PrintPage(object sender,
                    System.Drawing.Printing.PrintPageEventArgs e)
        {
            int i;
            int nWidth;
            int nTop = e.MarginBounds.Top;
            int nLeft = e.MarginBounds.Left;
            if (dg.DataSource == null) return;
            DataView dv;
            if (dg.DataSource.GetType().Name == "DataTable")
                dv = ((DataTable)dg.DataSource).DefaultView;
            else
                dv = (DataView)dg.DataSource;

            try
            {
                if (PrintFont == null) PrintFont = dg.Font;

                // Before starting first page, it saves Width & Height of Headers and CoulmnType
                if (PageNo == 1)
                {
                    foreach (DataGridColumnStyle GridCol in dg.TableStyles[0].GridColumnStyles)
                    {
                        // Skip if the current column not selected
                        if (!PrintDG.SelectedColumns.Contains(GridCol.HeaderText)) continue;

                        // Calculate width & height of headres
                        nWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                                 (double)TotalWidth * (double)TotalWidth *
                                 ((double)e.MarginBounds.Width / (double)TotalWidth))));
                        nHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText, PrintFont, nWidth).Height) + 11;

                        // Save width & height of headres and ColumnType
                        ColumnLefts.Add(nLeft);
                        ColumnWidths.Add(nWidth);
                        ColumnTypes.Add(GridCol.GetType());
                        nLeft += nWidth;
                    }
                }

                // Print Current Page, Row by Row
                while (RowPos <= dv.Count - 1)
                {
                    if (!PrintAllRows && !dg.IsSelected(RowPos))
                    {
                        RowPos++;
                        continue;
                    }

                    DataRowView oRow = dv[RowPos];
                    if (nTop + nHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                    {
                        DrawFooter(e, RowsPerPage);
                        NewPage = true;
                        PageNo++;
                        e.HasMorePages = true;
                        return;
                    }
                    else
                    {
                        if (NewPage)
                        {
                            // Draw Header
                            e.Graphics.DrawString(PrintTitle, new Font(PrintFont, FontStyle.Bold), Brushes.Black,
                              e.MarginBounds.Left, e.MarginBounds.Top -
                              e.Graphics.MeasureString(PrintTitle, new Font(PrintFont, FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            string s = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();

                            e.Graphics.DrawString(s, new Font(PrintFont, FontStyle.Bold), Brushes.Black,
                               e.MarginBounds.Left +
                              (e.MarginBounds.Width - e.Graphics.MeasureString(s, new Font(PrintFont, FontStyle.Bold), e.MarginBounds.Width).Width),
                               e.MarginBounds.Top -
                               e.Graphics.MeasureString(PrintTitle, new Font(new Font(PrintFont, FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            // Draw Columns
                            nTop = e.MarginBounds.Top;
                            i = 0;
                            foreach (DataGridColumnStyle GridCol in dg.TableStyles[0].GridColumnStyles)
                            {
                                if (!PrintDG.SelectedColumns.Contains(GridCol.HeaderText)) continue;

                                e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                                    new Rectangle((int)ColumnLefts[i], nTop,
                                    (int)ColumnWidths[i], nHeight));
                                e.Graphics.DrawRectangle(Pens.Black,
                                    new Rectangle((int)ColumnLefts[i], nTop,
                                    (int)ColumnWidths[i], nHeight));
                                e.Graphics.DrawString(GridCol.HeaderText, PrintFont, new SolidBrush(PrintFontColor),
                                    new RectangleF((int)ColumnLefts[i], nTop, (int)ColumnWidths[i], nHeight), StrFormat);
                                i++;
                            }
                            NewPage = false;
                        }
                        nTop += nHeight;
                        i = 0;

                        // Draw Columns Contents
                        foreach (DataGridColumnStyle oColumn in dg.TableStyles[0].GridColumnStyles)
                        {
                            if (!PrintDG.SelectedColumns.Contains(oColumn.HeaderText)) continue;

                            string cellval = oRow.Row[oColumn.MappingName].ToString().Trim();
                            if (ColumnTypes[i].ToString() == "System.Windows.Forms.DataGridTextBoxColumn")
                            {
                                // For the TextBox Column

                                // Draw Content of TextBox Cell
                                e.Graphics.DrawString(cellval, PrintFont, new SolidBrush(PrintFontColor),
                                    new RectangleF((int)ColumnLefts[i], nTop, (int)ColumnWidths[i], nHeight),
                                    StrFormat);
                            }
                            else if (ColumnTypes[i].ToString() == "System.Windows.Forms.DataGridBoolColumn")
                            {
                                // For the CheckBox Column

                                // Draw Content of CheckBox Cell
                                ChkBox.Size = new Size(14, 14);
                                ChkBox.Checked = (bool)(oRow.Row[oColumn.MappingName]);
                                Bitmap oBitmap = new Bitmap((int)ColumnWidths[i], nHeight);
                                Graphics oTempGraphics = Graphics.FromImage(oBitmap);
                                oTempGraphics.FillRectangle(Brushes.White,
                                    new Rectangle(0, 0, oBitmap.Width, oBitmap.Height));
                                ChkBox.DrawToBitmap(oBitmap,
                                    new Rectangle((int)((oBitmap.Width - ChkBox.Width) / 2),
                                    (int)((oBitmap.Height - ChkBox.Height) / 2),
                                    ChkBox.Width, ChkBox.Height));
                                e.Graphics.DrawImage(oBitmap, new Point((int)ColumnLefts[i], nTop));
                            }

                            e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)ColumnLefts[i],
                                nTop, (int)ColumnWidths[i], nHeight));
                            i++;
                        }
                    }

                    RowPos++;
                    // For the first page it calculates Rows per Page
                    if (PageNo == 1) RowsPerPage++;
                }

                if (RowsPerPage == 0) return;
                // Write Footer (Page Number)
                DrawFooter(e, RowsPerPage);
                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void DrawFooter(System.Drawing.Printing.PrintPageEventArgs e,
                    int RowsPerPage)
        {
            int cnt = 0;
            if (dg.DataSource == null) return;
            DataView dv;
            if (dg.DataSource.GetType().Name == "DataTable")
                dv = ((DataTable)dg.DataSource).DefaultView;
            else dv = (DataView)dg.DataSource;

            if (PrintAllRows)
            {
                cnt = dv.Count - 1;
            }
            else
            {
                for (int i = 0; i < dv.Count; i++)
                    if (dg.IsSelected(i)) cnt++;
            }

            // Write Page Number in the Bottom of Page
            string sPageNo = PageNo.ToString() + " of " +
                Math.Ceiling((double)(cnt / RowsPerPage)).ToString();

            e.Graphics.DrawString(sPageNo, dg.Font, Brushes.Black,
                e.MarginBounds.Left + (e.MarginBounds.Width -
                e.Graphics.MeasureString(sPageNo, dg.Font,
                e.MarginBounds.Width).Width) / 2, e.MarginBounds.Top +
                e.MarginBounds.Height + 31);

        }
    }
}
