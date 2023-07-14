using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.XlsTemplates
{
    public class ReachingDailyOperationReportXlsTemplate
    {
        public MemoryStream GenerateXls(List<DailyOperationMachineReachingDto> machineDtos, DateTime fromDate, DateTime toDate, string shift, string mcNo)
        {

            var reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(int) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Code", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Beam", DataType = typeof(double) });
            int idx = 1;
            double sumBeam = 0;
            foreach (var item in machineDtos)
            {
                var dateFormat = "dd MMM yyyy";
                var date = item.Periode.ToString(dateFormat);
                reportDataTable.Rows.Add(idx, date, item.Code, item.BeamNo);
                sumBeam += Convert.ToDouble( item.BeamNo);
                idx++;
            };

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "LAPORAN OPERASIONAL HARIAN REACHING";
                worksheet.Cells["A2"].Value = "TANGGAL AWAL : " + fromDate.ToShortDateString() + "  TANGGAL AKHIR : " + toDate.ToShortDateString();
                worksheet.Cells["A3"].Value = "SHIFT : " + shift;
                worksheet.Cells["A4"].Value = "NO MC : " + mcNo;
                worksheet.Cells["A" + 1 + ":D" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":D" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":D" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 4 + ":D" + 4 + ""].Merge = true;
                worksheet.Cells["A" + 5 + ":D" + 5 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":D" + 7 + ""].Style.Font.Bold = true;
                //worksheet.Cells["A6" + ":A7"].Merge = true;
                //worksheet.Cells["B6" + ":B7"].Merge = true;
                //worksheet.Cells["C6" + ":D6"].Merge = true;

                //worksheet.Cells["A6"].Value = "No";
                //worksheet.Cells["B6"].Value = "Tanggal";
                //worksheet.Cells["C6"].Value = "EFISIENSI REACHING PER OPERATOR";
                //worksheet.Cells["E6"].Value = "MONITORING PRODUKSI REACHING";
                worksheet.Cells["A7"].LoadFromDataTable(reportDataTable, true);

                worksheet.Cells["A" + (idx + 7)].Value = "TOTAL BEAM";
                worksheet.Cells["D" + (idx + 7)].Value = sumBeam;
                worksheet.Cells["A" + (idx + 7) + ":C" + (idx + 7) + ""].Merge = true;
                worksheet.Cells["A" + (idx + 7)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["A" + (idx + 7) + ":D" + (idx + 7) + ""].Style.Font.Bold = true;

                worksheet.Cells["A" + 7 + ":D" + (idx + 7) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 7 + ":D" + (idx + 7) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 7 + ":D" + (idx + 7) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 7 + ":D" + (idx + 7) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                worksheet.Cells["A" + 1 + ":D" + (idx + 7) + ""].AutoFitColumns();


                var stream = new MemoryStream();
                package.SaveAs(stream);
                return stream;
            }
        }
    }
}
