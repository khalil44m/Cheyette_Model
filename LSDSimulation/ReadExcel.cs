using System.Diagnostics;
using System.Runtime.InteropServices;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.Office.Interop.Excel;

namespace ClassLibrary1
{
    public class ReadExcel
    {
        public static double[] readData(string name, string sheetname, int size)
        {
            Application ExcelApp = new Application();
            Workbook ExcelWorkBook = null;
            ExcelApp.Visible = false;
            ExcelWorkBook = ExcelApp.Workbooks.Open(name + ".xlsx");
            var result = new double[size];

            try
            {
                Worksheet ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Sheets[sheetname];
                Microsoft.Office.Interop.Excel.Range ExcelRange = ExcelWorkSheet.UsedRange;

                for (int i = 1; i <= size; i++)
                {
                    result[i - 1] = (double)ExcelRange.Cells[i, 2];
                }
                ExcelWorkBook.Close();
                ExcelApp.Quit();
                Marshal.ReleaseComObject(ExcelWorkSheet);
                Marshal.ReleaseComObject(ExcelWorkBook);
                Marshal.ReleaseComObject(ExcelApp);
            }
            catch (Exception exHandle)
            {
                Console.WriteLine("Exception: " + exHandle.Message);
                //Console.ReadLine();
            }
            finally
            {
                foreach (Process process in Process.GetProcessesByName("Excel"))
                    process.Kill();
            }
            return result;

        }


        public static double[][] readData(string name, int[] column, int size)
        {
            Application ExcelApp = new Application();
            Workbook ExcelWorkBook = null;
            ExcelApp.Visible = false;
            ExcelWorkBook = ExcelApp.Workbooks.Open(name + ".xlsx");

            double[][] result = new double[size][];
            for (int i = 0; i < size; i++)
                result[i] = new double[column.Length];

            try
            {
                Worksheet ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range ExcelRange = ExcelWorkSheet.UsedRange;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < column.Length; j++)
                    {
                        result[i][j] = (double)ExcelRange.Cells[i + 1, column[j]];
                    }
                }
                ExcelWorkBook.Close();
                ExcelApp.Quit();
                Marshal.ReleaseComObject(ExcelWorkSheet);
                Marshal.ReleaseComObject(ExcelWorkBook);
                Marshal.ReleaseComObject(ExcelApp);


            }
            catch (Exception exHandle)
            {
                Console.WriteLine("Exception: " + exHandle.Message);
                Console.ReadLine();
            }
            finally
            {
                foreach (Process process in Process.GetProcessesByName("Excel"))
                    process.Kill();
            }
            return result;
        }

    }

    public class WriteExcel
    {
        public static void Writedata(string name, Matrix<double> data)
        {
            Application ExcelApp = new Application();
            Workbook ExcelWorkBook = null;

            ExcelApp.Visible = false;
            ExcelWorkBook = ExcelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            // ExcelWorkBook.Worksheets.Add(); //Adding New Sheet in Excel Workbook
            try
            {
                Worksheet ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets[1];

                ExcelWorkSheet.Cells[1, 1] = "Strike";
                ExcelWorkSheet.Cells[2, 1] = "Price Model";
                ExcelWorkSheet.Cells[3, 1] = "Price Market";
                ExcelWorkSheet.Cells[4, 1] = "Impl Vol Model";
                ExcelWorkSheet.Cells[5, 1] = "Impl Vol Market";

                int n = data.RowCount;
                int m = data.ColumnCount;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        ExcelWorkSheet.Cells[i + 1, j + 2] = data[i, j];
                    }
                } //r stands for ExcelRow and c for ExcelColumn

                //ExcelWorkBook.Worksheets[1]. = "MySheet";//Renaming the Sheet1 to MySheet
                ExcelWorkBook.SaveAs(name + ".xlsx");
                ExcelWorkBook.Close();
                ExcelApp.Quit();
                Marshal.ReleaseComObject(ExcelWorkSheet);
                Marshal.ReleaseComObject(ExcelWorkBook);
                Marshal.ReleaseComObject(ExcelApp);
            }
            catch (Exception exHandle)
            {
                Console.WriteLine("Exception: " + exHandle.Message);
                Console.ReadLine();
            }
            finally
            {
                foreach (Process process in Process.GetProcessesByName("Excel"))
                    process.Kill();
            }
        }

        public static void Writedata(string name, int column, double[] data)
        {
            Application ExcelApp = new Application();
            Workbook ExcelWorkBook = null;
            ExcelApp.Visible = false;
            ExcelWorkBook = ExcelApp.Workbooks.Open(name + ".xlsx");

            try
            {
                Worksheet ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Sheets[1];
                for (int i = 0; i < data.Length; i++)
                {
                    ExcelWorkSheet.Cells[i + 1, column] = data[i];
                }
                ExcelWorkBook.SaveAs(name + ".xlsx");
                ExcelWorkBook.Close();
                ExcelApp.Quit();
                Marshal.ReleaseComObject(ExcelWorkSheet);
                Marshal.ReleaseComObject(ExcelWorkBook);
                Marshal.ReleaseComObject(ExcelApp);

            }
            catch (Exception exHandle)
            {
                Console.WriteLine("Exception: " + exHandle.Message);
                Console.ReadLine();
            }
            finally
            {
                foreach (Process process in Process.GetProcessesByName("Excel"))
                    process.Kill();
            }
        }
    }





}