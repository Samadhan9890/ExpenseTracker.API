using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming;
using System.Data;

namespace ExpenseTracker.Services.Utilities
{
    public static class ExcelGenerateHelper
    {
        public static void SaveExcelToPath(string outputPath, DataTable dtExcelData)
        {
            try
            {
                //T1

                // Create a new workbook
                IWorkbook workbook = new SXSSFWorkbook();

                // Create a new worksheet
                ISheet worksheet = workbook.CreateSheet("Sheet1");

                IRow headerRow = worksheet.CreateRow(0);

                int totalCols = dtExcelData.Columns.Count;
                for (int col = 0; col < dtExcelData.Columns.Count; col++)
                {
                    headerRow.CreateCell(col).SetCellValue(dtExcelData.Columns[col].ColumnName);

                }

                var allDataRows = dtExcelData.Select().AsEnumerable();

                IRow dataRow = null;

                int rowCount = 1;
                foreach (var record in allDataRows)
                {
                    dataRow = worksheet.CreateRow(rowCount);

                    for (int i = 0; i < totalCols; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(record[i] == null ? string.Empty : record[i].ToString());

                    }
                    rowCount++;
                }


                // Save the workbook to a file
                using (FileStream stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(stream);
                }
                workbook.Close();
                dtExcelData.Clear();


            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
