
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects;
using Manufactures.Application.DailyOperations.Spu.DataTransferObjects;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;


namespace Manufactures.Helpers.XlsTemplates
{
    public class WeavingDailyOperationLoomReportXlsTemplate
    {
        public MemoryStream GenerateDailyOperationLoomReportXls(List<DailyOperationLoomMachineDto> dailyOperationWarpingReportModel, DateTime fromDate, DateTime toDate, string jenisMesin, string namaBlok, string namaMtc, string operatornya, string shift, string sp)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kode", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No Beam", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mesin Sizing", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Resep", DataType = typeof(string) });
          
            dt.Columns.Add(new DataColumn() { ColumnName = "SPU" });
          
            int index = 1;
            if (dailyOperationWarpingReportModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "");
            }
            else
            {
                foreach (var item in dailyOperationWarpingReportModel)
                {
                    var dateFormat = "dd/MM/yyyy";
                    var finalPeriode = item.Periode.ToString(dateFormat);
                
                    dt.Rows.Add(index++, finalPeriode, item.AL, item.BlockName, item.EFFMC, item.MTC, item.Operator);
                    




                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "LAPORAN LOOM";
                worksheet.Cells["A2"].Value = "TANGGAL AWAL : " + fromDate.ToShortDateString() + "  TANGGAL AKHIR : " + toDate.ToShortDateString();
                worksheet.Cells["A3"].Value = "SHIFT : " + shift;
                worksheet.Cells["A4"].Value = "MESIN : ";
                worksheet.Cells["A5"].Value = "GROUP : " ;
               

                worksheet.Cells["A" + 1 + ":G" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":G" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":G" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 4 + ":G" + 4 + ""].Merge = true;
                worksheet.Cells["A" + 5 + ":G" + 5 + ""].Merge = true;
                worksheet.Cells["A" + 6 + ":G" + 6 + ""].Merge = true;
                worksheet.Cells["A" + 7 + ":G" + 7 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":G" + 8 + ""].Style.Font.Bold = true;
                worksheet.Cells["A8"].LoadFromDataTable(dt, true);
              
                worksheet.Cells["A" + 8 + ":G" + (index + 8) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 8 + ":G" + (index + 8) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 8 + ":G" + (index + 8) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 8 + ":G" + (index + 8) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                


                var stream = new MemoryStream();
                package.SaveAs(stream);
                return stream;
            }
        }
    }
}
