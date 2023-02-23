using Barebone.Util;
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects.DailyOperationLoomReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.XlsTemplates
{
    public class DailyOperationLoomReportXlsTemplate
    {
        public MemoryStream GenerateDailyOperationLoomReportXls(List<DailyOperationLoomReportListDto> dailyOperationLoomReportModel)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Order Produksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Konstruksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Unit Weaving", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Asal Lusi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Asal Pakan", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Pasang", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Waktu Terakhir Diubah", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Status Operasi", DataType = typeof(string) });

            if (dailyOperationLoomReportModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "");
            }
            else
            {
                int index = 1;
                foreach (var item in dailyOperationLoomReportModel)
                {
                    var orderProductionNumber = item.OrderProductionNumber;
                    var constructionNumber = item.ConstructionNumber;
                    var weavingUnit = item.WeavingUnit;
                    var warpOrigin = item.WarpOrigin;
                    var weftOrigin = item.WeftOrigin;
                    var preparationDate = item.PreparationDate;
                    var lastModifiedTime = item.LastModifiedTime;
                    var operationStatus = item.OperationStatus;
                    dt.Rows.Add(index++,
                                orderProductionNumber,
                                constructionNumber,
                                weavingUnit,
                                warpOrigin,
                                weftOrigin,
                                preparationDate,
                                lastModifiedTime,
                                operationStatus);
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Daily Operation Loom Report") }, true);
        }
    }
}
