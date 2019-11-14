using Barebone.Util;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects.DailyOperationReachingReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.XlsTemplates
{
    public class DailyOperationReachingReportXlsTemplate
    {
        public MemoryStream GenerateDailyOperationReachingReportXls(List<DailyOperationReachingReportListDto> dailyOperationReachingReportModel)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Mesin", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Order Produksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Konstruksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Unit Weaving", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Beam Sizing", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Operator", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Grup Sizing", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Pasang", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Waktu Terakhir Diubah", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Shift", DataType = typeof(string) });

            if (dailyOperationReachingReportModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "");
            }
            else
            {
                int index = 1;
                foreach (var item in dailyOperationReachingReportModel)
                {
                    var machineNumber = item.MachineNumber;
                    var orderProductionNumber = item.OrderProductionNumber;
                    var constructionNumber = item.ConstructionNumber;
                    var weavingUnit = item.WeavingUnit;
                    var sizingBeamNumber = item.SizingBeamNumber;
                    var operatorName = item.OperatorName;
                    var reachingOperatorGroup = item.ReachingOperatorGroup;
                    var preparationDate = item.PreparationDate;
                    var lastModifiedTime = item.LastModifiedTime;
                    var shift = item.Shift;
                    dt.Rows.Add(index++,
                                machineNumber,
                                orderProductionNumber,
                                constructionNumber,
                                weavingUnit,
                                sizingBeamNumber,
                                operatorName,
                                reachingOperatorGroup,
                                preparationDate,
                                lastModifiedTime,
                                shift);
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Daily Operation Reaching Report") }, true);
        }
    }
}
