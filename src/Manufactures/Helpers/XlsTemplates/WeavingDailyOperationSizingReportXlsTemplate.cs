
using Manufactures.Application.DailyOperations.Spu.DataTransferObjects;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;


namespace Manufactures.Helpers.XlsTemplates
{
    public class WeavingDailyOperationSizingReportXlsTemplate
    {
        public MemoryStream GenerateDailyOperationSizingReportXls(List<WeavingDailyOperationSpuMachineDto> dailyOperationWarpingReportModel, DateTime fromDate, DateTime toDate, string shift, string machineSizing, string groupui, string sp, string code)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
        
            dt.Columns.Add(new DataColumn() { ColumnName = "Mesin Sizing", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Shift", DataType = typeof(string) });
          
            dt.Columns.Add(new DataColumn() { ColumnName = "SPU" });
            dt.Columns.Add(new DataColumn() { ColumnName = "PANJANG" });
            dt.Columns.Add(new DataColumn() { ColumnName = "EFISIENSI" });

            int index = 1;
            if (dailyOperationWarpingReportModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "","","");
            }
            else
            {
                foreach (var item in dailyOperationWarpingReportModel)
                {
                  
                    var nilaiSPUawal = item.SPU;
                    var nilaiEfficiencyawal = item.Efficiency;

                    if (nilaiSPUawal == "#VALUE!" || nilaiEfficiencyawal == "#VALUE!")
                    {
                        
                        dt.Rows.Add(index++, item.MachineSizing, item.Shift, "#VALUE!",item.Length, "#VALUE!");
                    }
                
                    else
                    {
                        //efisiensi
                        var efisiensi = item.Efficiency.Replace(",", ".");
                        double _efisiensi = Math.Round(Convert.ToDouble(efisiensi) * 100, 2);
                        string efisiensifinal = _efisiensi + " %";

                        //SPU
                        var SPU = item.SPU.Replace(",", ".");
                        double _spu = Math.Round(Convert.ToDouble(SPU) * 100, 2);
                        string spufinalbaru = _spu + " %";
                        dt.Rows.Add(index++, item.MachineSizing, item.Shift, spufinalbaru,item.Length, efisiensifinal);
                    }




                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "LAPORAN OPERASIONAL HARIAN SIZING";
                worksheet.Cells["A2"].Value = "TANGGAL AWAL : " + fromDate.ToShortDateString() + "  TANGGAL AKHIR : " + toDate.ToShortDateString();
                worksheet.Cells["A3"].Value = "SHIFT : " + shift;
                worksheet.Cells["A4"].Value = "MESIN SIZING : " + machineSizing;
                worksheet.Cells["A5"].Value = "GROUP : " + groupui;
                worksheet.Cells["A6"].Value = "SP : " + sp;
                worksheet.Cells["A7"].Value = "CODE : " + code;


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
