
using Manufactures.Application.DailyOperations.Spu.DataTransferObjects;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;


namespace Manufactures.Helpers.XlsTemplates
{
    public class WeavingDailyOperationSpuReportXlsTemplate
    {
        public MemoryStream GenerateDailyOperationSpuReportXls(List<WeavingDailyOperationSpuMachineDto> dailyOperationWarpingReportModel, DateTime fromDate, DateTime toDate, string shift, string machineSizing, string groupui, string name, string code)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kode", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mc Sz", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Resep", DataType = typeof(string) });
          
            dt.Columns.Add(new DataColumn() { ColumnName = "SPU" });
          
            int index = 1;
            if (dailyOperationWarpingReportModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "");
            }
            else
            {
                foreach (var item in dailyOperationWarpingReportModel)
                {
                    var dateFormat = "dd/MM/yyyy";
                    var finalPeriode = item.Periode.ToString(dateFormat);
                    // var todec = Convert.ToDecimal(item.SPU);
                    //var exspubaru = Math.Round(Convert.ToDouble(todec) * 100, 2);
                    //var spufinalbaru = exspubaru.ToString() ;

                    //ini yg bisa k bwh
                    //var SPU = item.SPU.Replace(",", ".");
                    //double _spu = Math.Round(Convert.ToDouble(SPU) * 100, 2);
                    //var spufinalbaru = _spu.ToString() + " %";
                    //dt.Rows.Add(index++, item.MachineSizing, item.Shift, spufinalbaru);
                    //smpe sini

                    //ini bisa tp kurang sempurna
                    //var nilaiSPUawal = item.SPU;
                    //if (nilaiSPUawal == "#VALUE!")
                    //{
                    //    var nilaiMCawal = item.MachineSizing;
                    //    nilaiSPUawal = "2";
                    //    dt.Rows.Add(index++, "a", "b", "c");
                    //}
                    //else
                    //{
                    //    var SPU = item.SPU.Replace(",", ".");
                    //    double _spu = Math.Round(Convert.ToDouble(SPU) * 100, 2);
                    //    string spufinalbaru = _spu + " %";
                    //    dt.Rows.Add(index++, item.MachineSizing, item.Shift, spufinalbaru);
                    //}
                    //smpai sini

                    var nilaiSPUawal = item.SPU;
                    
                    if (nilaiSPUawal == "#VALUE!")
                    {
                        var nilaiMCawal = item.MachineSizing;
                        //nilaiSPUawal = "2";
                        dt.Rows.Add(index++, finalPeriode,item.Code,item.MachineSizing, item.Recipe, "#VALUE!");
                    }
                    else
                    {
                        var SPU = item.SPU.Replace(",", ".");
                        double _spu = Math.Round(Convert.ToDouble(SPU) * 100, 2);
                        string spufinalbaru = _spu + " %";
                        dt.Rows.Add(index++, finalPeriode, item.Code,item.MachineSizing, item.Recipe, spufinalbaru);
                    }




                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "LAPORAN SPU";
                worksheet.Cells["A2"].Value = "TANGGAL AWAL : " + fromDate.ToShortDateString() + "  TANGGAL AKHIR : " + toDate.ToShortDateString();
                worksheet.Cells["A3"].Value = "SHIFT : " + shift;
                worksheet.Cells["A4"].Value = "MESIN SIZING : " + machineSizing;
                worksheet.Cells["A5"].Value = "GROUP : " + groupui;
               

                worksheet.Cells["A" + 1 + ":G" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":G" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":G" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 4 + ":G" + 4 + ""].Merge = true;
                worksheet.Cells["A" + 5 + ":G" + 5 + ""].Merge = true;
                worksheet.Cells["A" + 6 + ":G" + 6 + ""].Merge = true;
                worksheet.Cells["A" + 7 + ":G" + 7 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":G" + 8 + ""].Style.Font.Bold = true;
                worksheet.Cells["A8"].LoadFromDataTable(dt, true);
              
                worksheet.Cells["A" + 8 + ":F" + (index + 8) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 8 + ":F" + (index + 8) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 8 + ":F" + (index + 8) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 8 + ":F" + (index + 8) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                


                var stream = new MemoryStream();
                package.SaveAs(stream);
                return stream;
            }
        }
    }
}
