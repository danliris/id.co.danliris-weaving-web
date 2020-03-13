using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Manufactures.Helpers.XlsTemplates
{
    public class WarpingBrokenReportXlsTemplate
    {
        public MemoryStream GenerateWarpingBrokenReportXls(WarpingBrokenThreadsReportListDto warpingBrokenThreadsReportListDto)
        {
            int manyColumn = warpingBrokenThreadsReportListDto.GroupedItems[0].ItemsValue[0].Count; //get all column for merging title document
            List<string> titleRow = new List<string>
            {
                "Laporan Putus Warping",
                $"Periode TGL 01 S/D 31",
                $"Bulan : {warpingBrokenThreadsReportListDto.Month} {warpingBrokenThreadsReportListDto.Year}",
                warpingBrokenThreadsReportListDto.WeavingUnitName
            };
            string datePlace = $"Sukoharjo, {DateTime.Now.ToString("DD MMMM YYYY", CultureInfo.CreateSpecificCulture("id-ID"))}";

            return CreateExcel("Sheet 1", titleRow, warpingBrokenThreadsReportListDto);
            //    DataTable dt = new DataTable();

            //    dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Produksi", DataType = typeof(string) });
            //    dt.Columns.Add(new DataColumn() { ColumnName = "A", DataType = typeof(string) });
            //    dt.Columns.Add(new DataColumn() { ColumnName = "B", DataType = typeof(string) });
            //    dt.Columns.Add(new DataColumn() { ColumnName = "C", DataType = typeof(string) });
            //    dt.Columns.Add(new DataColumn() { ColumnName = "D", DataType = typeof(string) });
            //    dt.Columns.Add(new DataColumn() { ColumnName = "E", DataType = typeof(string) });
            //    dt.Columns.Add(new DataColumn() { ColumnName = "F", DataType = typeof(string) });
            //    dt.Columns.Add(new DataColumn() { ColumnName = "G", DataType = typeof(string) });
            //    dt.Columns.Add(new DataColumn() { ColumnName = "Total", DataType = typeof(string) });
            //    dt.Columns.Add(new DataColumn() { ColumnName = "Paraf", DataType = typeof(string) });

            //    if (warpingProductionReportListDto.PerOperatorList.Count == 0)
            //    {
            //        dt.Rows.Add("", "", "", "", "", "", "", "", "", "");
            //    }
            //    else
            //    {
            //        int index = 1;
            //        foreach (var item in warpingProductionReportListDto.PerOperatorList)
            //        {
            //            var aGroup = item.AGroup;
            //            var bGroup = item.BGroup;
            //            var cGroup = item.CGroup;
            //            var dGroup = item.DGroup;
            //            var eGroup = item.EGroup;
            //            var fGroup = item.FGroup;
            //            var gGroup = item.GGroup;
            //            var total = item.Total;
            //            dt.Rows.Add(index++,
            //                        aGroup,
            //                        bGroup,
            //                        cGroup,
            //                        dGroup,
            //                        eGroup,
            //                        fGroup,
            //                        gGroup,
            //                        total);
            //        }
            //    }

            //    return CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Laporan Produksi Warping Per Operator") }, warpingProductionReportListDto);
            //}

            //public  MemoryStream CreateExcel(List<KeyValuePair<DataTable, String>> dtSourceList, WarpingProductionReportListDto warpingProductionReportListDto)
            //{
            //    ExcelPackage package = new ExcelPackage();

            //    foreach (KeyValuePair<DataTable, String> item in dtSourceList)
            //    {
            //        var sheet = package.Workbook.Worksheets.Add(item.Value);

            //        //sheet.Cells["A1:J2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            //        sheet.Cells["A1:A2"].Merge = true;
            //        sheet.Cells["A1:A2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["A1"].Value = "Tgl";
            //        sheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            //        sheet.Cells["B1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["B1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["B1"].Value = "A";

            //        sheet.Cells["B2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["B2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["B2"].Value = warpingProductionReportListDto.AGroupOperatorName;

            //        sheet.Cells["C1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["C1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["C1"].Value = "B";

            //        sheet.Cells["C2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["C2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["C2"].Value = warpingProductionReportListDto.BGroupOperatorName;

            //        sheet.Cells["D1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["D1"].Value = "C";

            //        sheet.Cells["D2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["D2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["D2"].Value = warpingProductionReportListDto.CGroupOperatorName;

            //        sheet.Cells["E1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["E1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["E1"].Value = "D";

            //        sheet.Cells["E2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["E2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["E2"].Value = warpingProductionReportListDto.DGroupOperatorName;

            //        sheet.Cells["F1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["F1"].Value = "E";

            //        sheet.Cells["F2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["F2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["F2"].Value = warpingProductionReportListDto.EGroupOperatorName;

            //        sheet.Cells["G1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["G1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["G1"].Value = "F";

            //        sheet.Cells["G2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["G2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["G2"].Value = warpingProductionReportListDto.FGroupOperatorName;

            //        sheet.Cells["H1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["H1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["H1"].Value = "G";

            //        sheet.Cells["H2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["H2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["H2"].Value = warpingProductionReportListDto.GGroupOperatorName;

            //        sheet.Cells["I1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["I1"].Value = "Total";

            //        sheet.Cells["I2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["I2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["I2"].Value = warpingProductionReportListDto.TotalAll;

            //        sheet.Cells["J1:J2"].Merge = true;
            //        sheet.Cells["J1:J2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["J1"].Value = "Paraf";
            //        sheet.Cells["J1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        sheet.Cells["J1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            //        sheet.Cells["A3"].LoadFromDataTable(item.Key, false, OfficeOpenXml.Table.TableStyles.Light8);
            //        sheet.Cells["A3:A33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["B3:B33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["C3:C33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["D3:D33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["E3:E33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["F3:F33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["G3:G33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["H3:H33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["I3:I33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //        sheet.Cells["J3:J33"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            //        sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            //    }
            //    MemoryStream stream = new MemoryStream();
            //    package.SaveAs(stream);
            //    return stream;
            //return null;
        }
        public MemoryStream CreateExcel(string nameSheet, List<string> titleList, WarpingBrokenThreadsReportListDto warpingBrokenThreadsReportListDto)
        {
            List<string> ignoredKeys = new List<string>
            {
                "SupplierName", "YarnName", "Total", "Max", "Min", "Average"
            };

            ExcelPackage package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add(nameSheet);
            int defaultRowIndex = 1;
            int defaultColIndex = 1;
            int totalCol = warpingBrokenThreadsReportListDto.GroupedItems[0].ItemsValue[0].Count;
            int rowIndex = titleList.Count+1;// start row index table
            int colIndex = defaultColIndex;// start col index table 1 means from A

            #region title sheet
            var i = 0;
            foreach(var title in titleList)
            {
                sheet.Cells[defaultRowIndex + i, defaultColIndex].Value = title;
                sheet.Cells[defaultRowIndex + i, defaultColIndex].Style.Font.Bold = true;
                sheet.Cells[defaultRowIndex + i, defaultColIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[defaultRowIndex + i, defaultColIndex,defaultRowIndex+i,totalCol].Merge = true;
                i++;
            }
            #endregion

            sheet.Cells[rowIndex, colIndex].Value = "Penyebab";
            sheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
            sheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment= OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[rowIndex, colIndex, rowIndex, ++colIndex].Merge = true;
            sheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            sheet.Cells[++rowIndex, --colIndex].Value = "Asal Benang";
            sheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
            sheet.Cells[rowIndex, colIndex].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            sheet.Cells[rowIndex, ++colIndex].Value = "Jenis & No. Benang";
            sheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
            sheet.Cells[rowIndex, colIndex].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            --rowIndex;
            foreach (var key in warpingBrokenThreadsReportListDto.GroupedItems[0].ItemsValue[0])
            {
                if (!ignoredKeys.Contains(key.Key))
                {
                    var localTemp = rowIndex + 1;
                    sheet.Cells[rowIndex, ++colIndex].Value = key.Key;
                    sheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, colIndex].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    sheet.Cells[rowIndex, colIndex, localTemp, colIndex].Merge = true;
                    sheet.Cells[rowIndex, colIndex, localTemp, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                }
            }
            sheet.Cells[rowIndex, ++colIndex].Value = "Total";
            sheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
            sheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            var temp = rowIndex + 1;
            sheet.Cells[rowIndex, colIndex, temp, colIndex].Merge = true;
            sheet.Cells[rowIndex, colIndex, temp, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            sheet.Cells[rowIndex, ++colIndex].Value = "Max";
            sheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
            sheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            sheet.Cells[rowIndex, colIndex, temp, colIndex].Merge = true;
            sheet.Cells[rowIndex, colIndex, temp, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            sheet.Cells[rowIndex, ++colIndex].Value = "Min";
            sheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
            sheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            sheet.Cells[rowIndex, colIndex, temp, colIndex].Merge = true;
            sheet.Cells[rowIndex, colIndex, temp, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            sheet.Cells[rowIndex, ++colIndex].Value = "Average";
            sheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
            sheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            sheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            sheet.Cells[rowIndex, colIndex, temp, colIndex].Merge = true;
            sheet.Cells[rowIndex, colIndex, temp, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            rowIndex += 2;
            colIndex = defaultColIndex;
            #region binding Data
            foreach (var groupedItem in warpingBrokenThreadsReportListDto.GroupedItems)
            {
                int startRowData = rowIndex;
                foreach (var item in groupedItem.ItemsValue.Select((data,index)=>new { Index = index, Data = data }))
                {
                    if (item.Index == 0)
                    {
                        foreach (var data in item.Data)
                        {
                            sheet.Cells[rowIndex, colIndex].Value = data.Value;
                            sheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            if (data.Key == "SupplierName")
                            {
                                sheet.Cells[rowIndex, colIndex, rowIndex + groupedItem.ItemsValueLength - 1, colIndex].Merge = true;
                            }
                            colIndex++;
                        }
                    }
                    else
                    {
                        foreach (var data in item.Data)
                        {
                            if (data.Key != "SupplierName")
                            {
                                sheet.Cells[rowIndex, colIndex].Value = data.Value;
                                sheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                colIndex++;
                            }
                            else
                            {
                                colIndex++;
                            }
                        }
                    }
                    rowIndex++;
                    colIndex = defaultColIndex;
                }
                colIndex = defaultColIndex;
                int endRowData = rowIndex + groupedItem.ItemsValueLength - 1;
            }
            #endregion
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);

            return stream;
        }
    }
}
