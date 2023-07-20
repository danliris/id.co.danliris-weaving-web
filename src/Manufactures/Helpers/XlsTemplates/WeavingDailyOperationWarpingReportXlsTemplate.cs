using Barebone.Util;
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
    public class WeavingDailyOperationWarpingReportXlsTemplate
    {
        public MemoryStream GenerateDailyOperationWarpingReportXls(List<WeavingDailyOperationWarpingMachineDto> dailyOperationWarpingReportModel, DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string name, string code)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Shift", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No MC", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Panjang", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Effisiensi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Putus Benang", DataType = typeof(double) });

            int index = 1;
            if (dailyOperationWarpingReportModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", 0, "");
            }
            else
            {
                foreach (var item in dailyOperationWarpingReportModel)
                {
                    var dateFormat = "dd/MM/yyyy";
                    var date = item.Date.ToString(dateFormat);
                    double ef;
                    var eff = Math.Round(Convert.ToDouble(item.Eff) * 100,2);
                    var efficiency = eff.ToString() + "%";
                    dt.Rows.Add(index++,date, item.Shift, item.MCNo, item.Length, efficiency, item.ThreadCut);
                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "LAPORAN OPERASIONAL HARIAN WARPING";
                worksheet.Cells["A2"].Value = "TANGGAL AWAL : " + fromDate.ToShortDateString() + "  TANGGAL AKHIR : " + toDate.ToShortDateString();
                worksheet.Cells["A3"].Value = "SHIFT : " + shift;
                worksheet.Cells["A4"].Value = "NO MC : " + mcNo;
                worksheet.Cells["A5"].Value = "SP : " + sp;
                worksheet.Cells["A6"].Value = "CODE : " + code;
                worksheet.Cells["A7"].Value = "NAMA : " + name;
                worksheet.Cells["A" + 1 + ":G" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":G" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":G" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 4 + ":G" + 4 + ""].Merge = true;
                worksheet.Cells["A" + 5 + ":G" + 5 + ""].Merge = true;
                worksheet.Cells["A" + 6 + ":G" + 6 + ""].Merge = true;
                worksheet.Cells["A" + 7 + ":G" + 7 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":G" + 10 + ""].Style.Font.Bold = true;
                worksheet.Cells["A10"].LoadFromDataTable(dt, true);
                //worksheet.Cells["A" + (idx + 10)].Value = "GRAND TOTAL";
                //worksheet.Cells["C" + (idx + 10)].Value = sumTHreadCut;
                //worksheet.Cells["D" + (idx + 10)].Value = sumLength;
                worksheet.Cells["A" + 10 + ":G" + (index + 10) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 10 + ":G" + (index + 10) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 10 + ":G" + (index + 10) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 10 + ":G" + (index + 10) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //worksheet.Cells["A" + (idx + 10) + ":D" + (idx + 10) + ""].Style.Font.Bold = true;
                //worksheet.Cells["A" + (idx + 10) + ":B" + (idx + 10) + ""].Merge = true;

                //worksheet.Cells["A" + 1 + ":D" + (idx + 10) + ""].AutoFitColumns();


                var stream = new MemoryStream();
                package.SaveAs(stream);
                return stream;
            }
        }
    }
}
