using Manufactures.Application.DailyOperations.Warping.DataTransferObjects;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.XlsTemplates
{
    public class WeavingWarpingBrokenXlsTemplate
    {
        public MemoryStream GenerateXls(List<WeavingDailyOperationWarpingMachineDto> machineDtos, DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string threadNo, string code)
        {

            var reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(int) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Code", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jenis Benang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Asal Benang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Putus Benang", DataType = typeof(double) });
            int idx = 1;
            double sumTHreadCut = 0;
            foreach (var item in machineDtos)
            {
                var dateFormat = "dd/MM/yyyy";
                var date = item.Date.ToString(dateFormat);
                reportDataTable.Rows.Add(idx, date, item.Code, item.WarpType, item.AL, item.ThreadCut);
                sumTHreadCut += item.ThreadCut;
                idx++;
            };

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "LAPORAN PUTUS BENANG WARPING";
                worksheet.Cells["A2"].Value = "TANGGAL AWAL : " + fromDate.ToShortDateString() + "  TANGGAL AKHIR : " + toDate.ToShortDateString();
                worksheet.Cells["A3"].Value = "SHIFT : " + shift;
                worksheet.Cells["A4"].Value = "NO MC : " + mcNo;
                worksheet.Cells["A5"].Value = "SP : " + sp;
                //worksheet.Cells["A6"].Value = "NO BENANG : " + threadNo;
                worksheet.Cells["A6"].Value = "CODE : " + code;
                worksheet.Cells["A" + 1 + ":F" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":F" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":F" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 4 + ":F" + 4 + ""].Merge = true;
                worksheet.Cells["A" + 5 + ":F" + 5 + ""].Merge = true;
                worksheet.Cells["A" + 6 + ":F" + 6 + ""].Merge = true;
                worksheet.Cells["A" + 7 + ":F" + 7 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":F" + 10 + ""].Style.Font.Bold = true;
                worksheet.Cells["A10"].LoadFromDataTable(reportDataTable, true);
                worksheet.Cells["A" + 10 + ":F" + (idx + 10) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 10 + ":F" + (idx + 10) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 10 + ":F" + (idx + 10) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 10 + ":F" + (idx + 10) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                worksheet.Cells["A" + 1 + ":F" + (idx + 10) + ""].AutoFitColumns();


                var stream = new MemoryStream();
                package.SaveAs(stream);
                return stream;
            }
        }
    }
}
