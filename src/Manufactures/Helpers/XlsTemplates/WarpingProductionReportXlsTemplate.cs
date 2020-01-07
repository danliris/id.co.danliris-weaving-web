using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Manufactures.Helpers.XlsTemplates
{
    public class WarpingProductionReportXlsTemplate
    {
        public MemoryStream GenerateWarpingProductionReportXls(WarpingProductionReportListDto warpingProductionReportListDto)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Produksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "A", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "B", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "C", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "D", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "E", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "F", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "G", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Total", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Paraf", DataType = typeof(string) });

            if (warpingProductionReportListDto.PerOperatorList.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "", "");
            }
            else
            {
                int index = 1;
                foreach (var item in warpingProductionReportListDto.PerOperatorList)
                {
                    var aGroup = item.AGroup;
                    var bGroup = item.BGroup;
                    var cGroup = item.CGroup;
                    var dGroup = item.DGroup;
                    var eGroup = item.EGroup;
                    var fGroup = item.FGroup;
                    var gGroup = item.GGroup;
                    var total = item.Total;
                    dt.Rows.Add(index++,
                                aGroup,
                                bGroup,
                                cGroup,
                                dGroup,
                                eGroup,
                                fGroup,
                                gGroup,
                                total);
                }
            }

            return CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Laporan Produksi Warping Per Operator") }, warpingProductionReportListDto);
        }

        public  MemoryStream CreateExcel(List<KeyValuePair<DataTable, String>> dtSourceList, WarpingProductionReportListDto warpingProductionReportListDto)
        {
            ExcelPackage package = new ExcelPackage();

            foreach (KeyValuePair<DataTable, String> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                //sheet.Cells["A1:J2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                sheet.Cells["A1:A2"].Merge = true;
                sheet.Cells["A1:A2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["A1"].Value = "Tgl";
                sheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                sheet.Cells["B1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["B1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["B1"].Value = "A";

                sheet.Cells["B2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["B2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["B2"].Value = warpingProductionReportListDto.AGroupTotal;

                sheet.Cells["C1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["C1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["C1"].Value = "B";

                sheet.Cells["C2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["C2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["C2"].Value = warpingProductionReportListDto.BGroupTotal;

                sheet.Cells["D1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["D1"].Value = "C";

                sheet.Cells["D2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["D2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["D2"].Value = warpingProductionReportListDto.CGroupTotal;

                sheet.Cells["E1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["E1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["E1"].Value = "D";

                sheet.Cells["E2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["E2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["E2"].Value = warpingProductionReportListDto.DGroupTotal;

                sheet.Cells["F1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["F1"].Value = "E";

                sheet.Cells["F2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["F2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["F2"].Value = warpingProductionReportListDto.EGroupTotal;

                sheet.Cells["G1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["G1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["G1"].Value = "F";

                sheet.Cells["G2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["G2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["G2"].Value = warpingProductionReportListDto.FGroupTotal;

                sheet.Cells["H1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["H1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["H1"].Value = "G";

                sheet.Cells["H2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["H2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["H2"].Value = warpingProductionReportListDto.GGroupTotal;

                sheet.Cells["I1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["I1"].Value = "Total";

                sheet.Cells["I2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["I2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["I2"].Value = warpingProductionReportListDto.TotalAll;

                sheet.Cells["J1:J2"].Merge = true;
                sheet.Cells["J1:J2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["J1"].Value = "Paraf";
                sheet.Cells["J1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells["J1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                sheet.Cells["A3"].LoadFromDataTable(item.Key, false, OfficeOpenXml.Table.TableStyles.Light8);
                sheet.Cells["A3:A33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["B3:B33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["C3:C33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["D3:D33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["E3:E33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["F3:F33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["G3:G33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["H3:H33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["I3:I33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells["J3:J33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }
    }
}
