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
    public class WeavingDailyOperationWarpingMachineReportXlsTemplate
    {
        public MemoryStream GenerateXls(List<WeavingDailyOperationWarpingMachineDto> machineDtos, DateTime fromDate, DateTime toDate,string shift,string mcNo,string sp,string threadNo,string code)
        {
           
            var reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(int) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nama Operator", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sum Of Putus Benang", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sum Of Panjang ( Meter )", DataType = typeof(double) });
            int idx = 1;
            double sumLength = 0;
            double sumTHreadCut = 0;
            foreach (var item in machineDtos)
            {
                reportDataTable.Rows.Add(idx, item.Name, item.ThreadCut, item.Length);
                sumLength += item.Length;
                sumTHreadCut += item.ThreadCut;
                idx++;
            };

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "LAPORAN PRODUKSI WARPING PER OPERATOR";
                worksheet.Cells["A2"].Value = "TANGGAL AWAL : " + fromDate.ToShortDateString()+ "  TANGGAL AKHIR : " + toDate.ToShortDateString();
                worksheet.Cells["A3"].Value = "SHIFT : " + shift;
                worksheet.Cells["A4"].Value = "NO MC : " + mcNo;
                worksheet.Cells["A5"].Value = "SP : " + sp;
                worksheet.Cells["A6"].Value = "NO BENANG : " + threadNo;
                worksheet.Cells["A7"].Value = "CODE : " + code;
                worksheet.Cells["A" + 1 + ":D" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":D" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":D" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 4 + ":D" + 4 + ""].Merge = true;
                worksheet.Cells["A" + 5 + ":D" + 5 + ""].Merge = true;
                worksheet.Cells["A" + 6 + ":D" + 6 + ""].Merge = true;
                worksheet.Cells["A" + 7 + ":D" + 7 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":D" + 10 + ""].Style.Font.Bold = true;
                worksheet.Cells["A10"].LoadFromDataTable(reportDataTable, true);
                worksheet.Cells["A" + (idx + 10)].Value = "GRAND TOTAL";
                worksheet.Cells["C" + (idx + 10)].Value = sumTHreadCut;
                worksheet.Cells["D" + (idx + 10)].Value = sumLength;
                worksheet.Cells["A" + 10 + ":D" + (idx + 10) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 10 + ":D" + (idx + 10) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 10 + ":D" + (idx + 10) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 10 + ":D" + (idx + 10) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + (idx + 10) + ":D" + (idx + 10) + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + (idx + 10) + ":B" + (idx + 10) + ""].Merge = true;

                worksheet.Cells["A" + 1 + ":D" + (idx + 10) + ""].AutoFitColumns();


                var stream = new MemoryStream();
                package.SaveAs(stream);
                return stream;
            }
        }
    }
}