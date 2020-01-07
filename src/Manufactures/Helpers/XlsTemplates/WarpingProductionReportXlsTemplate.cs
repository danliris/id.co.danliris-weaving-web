using Barebone.Util;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

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

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Laporan Produksi Warping Per Operator") }, true);
        }
    }
}
