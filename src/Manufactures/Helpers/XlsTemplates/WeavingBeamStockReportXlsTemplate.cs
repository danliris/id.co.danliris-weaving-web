
using Manufactures.Application.BeamStockUpload.DataTransferObjects;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;


namespace Manufactures.Helpers.XlsTemplates
{
    public class WeavingBeamStockReportXlsTemplate
    {
        public MemoryStream GenerateBeamStockReportXls(List<BeamStockUploadDto> dailyOperationWarpingReportModel,int monthId, string year, int datestart, int datefinish, string shift)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Shift", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Beam", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Code" });
            dt.Columns.Add(new DataColumn() { ColumnName = "Sizing" });
            dt.Columns.Add(new DataColumn() { ColumnName = "Di Reaching" });
            dt.Columns.Add(new DataColumn() { ColumnName = "Reaching" });
            dt.Columns.Add(new DataColumn() { ColumnName = "Ket" });



            DateTime tglAwal = new DateTime(Convert.ToInt32(year), monthId, datestart);
            var dateFormat = "dd/MM/yyyy";
            var finalTglAwal = tglAwal.ToString(dateFormat);
            //--------
            DateTime tglAkhir = new DateTime(Convert.ToInt32(year), monthId, datefinish);
            var dateFormat2 = "dd/MM/yyyy";
            var finalTglAkhir = tglAkhir.ToString(dateFormat2);

            int index = 1;
            if (dailyOperationWarpingReportModel.Count == 0)
            {
                dt.Rows.Add("","", "", "", "", "", "", "","");
            }
            else
            {
                foreach (var item in dailyOperationWarpingReportModel)
                {
                   
                        dt.Rows.Add(index++, item.Date, item.Shift, item.Beam,item.Code,item.Sizing,item.InReaching,item.Reaching,item.Information);
                   
                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "LAPORAN VISUALISASI STOCK BEAM";
                worksheet.Cells["A2"].Value = "TANGGAL AWAL : " + finalTglAwal;
                worksheet.Cells["A3"].Value = "TANGGAL AKHIR : " + finalTglAkhir;
                worksheet.Cells["A4"].Value = "SHIFT : " + shift;

               

                worksheet.Cells["A" + 1 + ":I" + 8 + ""].Style.Font.Bold = true;
                worksheet.Cells["A8"].LoadFromDataTable(dt, true);

                worksheet.Cells["A" + 8 + ":I" + (index + 8) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 8 + ":I" + (index + 8) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 8 + ":I" + (index + 8) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 8 + ":I" + (index + 8) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                var stream = new MemoryStream();
                package.SaveAs(stream);
                return stream;
            }
        }
    }
}
