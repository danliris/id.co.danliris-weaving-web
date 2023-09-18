
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects;
using Manufactures.Application.DailyOperations.Spu.DataTransferObjects;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Manufactures.Helpers.XlsTemplates
{
    public class WeavingDailyOperationLoomReportXlsTemplate
    {
        public MemoryStream GenerateDailyOperationLoomReportXls(List<DailyOperationLoomMachineDto> dailyOperationWarpingReportModel, DateTime fromDate, DateTime toDate, string jenisMesin, string namaBlok, string namaMtc, string operatornya, string shift, string sp)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kontruksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Jenis Benang", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "CMPX", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Frm", DataType = typeof(string) });//5
            dt.Columns.Add(new DataColumn() { ColumnName = "Produksi Meter", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "100 % Produksi" });
            dt.Columns.Add(new DataColumn() { ColumnName = "% EFF" });
            dt.Columns.Add(new DataColumn() { ColumnName = "EFF MC 2" });
            dt.Columns.Add(new DataColumn() { ColumnName = "Fill" });//10
            dt.Columns.Add(new DataColumn() { ColumnName = "Warp" });
            dt.Columns.Add(new DataColumn() { ColumnName = "RPM" });//13
          


            int index = 1;
            if (dailyOperationWarpingReportModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "");
            }
            else
            {
                //grand total
                double GrandTotProductionCMPX = 0;
                double GrandTotMCNo = 0;
                double GrandTotProduction = 0;
                double GrandTotProduction100 = 0;
                double GrandTotPercentEff = 0;
                double GrandTotMC2Eff = 0;
                double GrandTotF = 0;
                double GrandTotW = 0;
                double GrandTotRPM = 0;


                foreach (var item in dailyOperationWarpingReportModel)
                {
                    var dateFormat = "dd/MM/yyyy";
                    var finalPeriode = item.Periode.ToString(dateFormat);

                   



                    dt.Rows.Add(index++, item.Construction,item.ThreadType, Math.Round(item.TotProductionCMPX,2),item.TotMCNo, Math.Round(item.TotProduction,2), Math.Round(item.TotProduction100,2), Math.Round(item.TotPercentEff,2), Math.Round(item.TotMC2Eff,2), Math.Round(item.TotF,2), Math.Round(item.TotW,2), Math.Round(item.TotRPM,2));



                    //grand total
                    GrandTotProductionCMPX += Convert.ToDouble(item.TotProductionCMPX);
                    GrandTotMCNo += Convert.ToDouble(item.TotMCNo);
                    GrandTotProduction += Convert.ToDouble(item.TotProduction);
                    GrandTotProduction100 += Convert.ToDouble(item.TotProduction100);
                    GrandTotPercentEff += Convert.ToDouble(item.TotPercentEff);
                    GrandTotMC2Eff += Convert.ToDouble(item.TotMC2Eff);
                    GrandTotF += Convert.ToDouble(item.TotF);
                    GrandTotW += Convert.ToDouble(item.TotW);
                    GrandTotRPM += Convert.ToDouble(item.TotRPM);


                }
                //penak iki
                //var total = dailyOperationWarpingReportModel.Sum(x => x.TotProductionCMPX);
                //query pake linq
                //dt.Rows.Add("GRAND TOTAL","","", GrandTotProductionCMPX, dailyOperationWarpingReportModel.Sum(x=>x.TotProductionCMPX));
                dt.Rows.Add("GRAND TOTAL", "", "", Math.Round(GrandTotProductionCMPX,2), Math.Round(GrandTotMCNo,2), Math.Round(GrandTotProduction,2), Math.Round(GrandTotProduction100,2), Math.Round(GrandTotPercentEff,2), Math.Round(GrandTotMC2Eff,2), Math.Round(GrandTotF,2), Math.Round(GrandTotW,2), Math.Round(GrandTotRPM,2));

            }


            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "LAPORAN LOOM";
                worksheet.Cells["A2"].Value = "TANGGAL AWAL : " + fromDate.ToShortDateString() + "  TANGGAL AKHIR : " + toDate.ToShortDateString();
                worksheet.Cells["A3"].Value = "SHIFT : " + shift;
                worksheet.Cells["A4"].Value = "JENIS MESIN : " + jenisMesin;
                worksheet.Cells["A5"].Value = "NAMA BLOK : " + namaBlok;
                worksheet.Cells["A6"].Value = "NAMA MTC : " + namaMtc;
                worksheet.Cells["A7"].Value = "OPERATOR : " + operatornya;
                worksheet.Cells["A8"].Value = "NO SP / TAHUN : " + sp;


                worksheet.Cells["A" + 1 + ":G" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":G" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":G" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 4 + ":G" + 4 + ""].Merge = true;
                worksheet.Cells["A" + 5 + ":G" + 5 + ""].Merge = true;
                worksheet.Cells["A" + 6 + ":G" + 6 + ""].Merge = true;
                worksheet.Cells["A" + 7 + ":G" + 7 + ""].Merge = true;
                worksheet.Cells["A" + 8 + ":G" + 8 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":L" + 8 + ""].Style.Font.Bold = true;

                worksheet.Cells["A" + 11 + ":L" + 11 + ""].Style.Font.Bold = true;
                worksheet.Cells["A11"].LoadFromDataTable(dt, true);
              
                worksheet.Cells["A" + 11 + ":L" + (index + 11) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 11 + ":L" + (index + 11) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 11 + ":L" + (index + 11) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 11 + ":L" + (index + 11) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                //sni jg bs
                //worksheet.Cells["A" + (index + 14)].Value = "TOTAL BEAM";

                var stream = new MemoryStream();
                package.SaveAs(stream);
                return stream;
            }
        }
    }
}
