using Barebone.Util;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.DailyOperationWarpingReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.XlsTemplates
{
    public class DailyOperationWarpingReportXlsTemplate
    {
        public MemoryStream GenerateDailyOperationWarpingReportXls(List<DailyOperationWarpingReportListDto> dailyOperationWarpingReportModel)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Order Produksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Konstruksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Unit Weaving", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Jenis Material", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Jumlah Cone", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Warna Cone", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Operator", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Grup Warping", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Pasang", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Shift", DataType = typeof(string) });

            if (dailyOperationWarpingReportModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "");
            }
            else
            {
                int index = 1;
                foreach (var item in dailyOperationWarpingReportModel)
                {
                    var orderProductionNumber = item.OrderProductionNumber;
                    var constructionNumber = item.ConstructionNumber;
                    var weavingUnit = item.WeavingUnit;
                    var materialType = item.MaterialType;
                    var amountOfCones = item.AmountOfCones;
                    var colourOfCones = item.ColourOfCones;
                    var operatorName = item.OperatorName;
                    var warpingOperatorGroup = item.WarpingOperatorGroup;
                    var preparationDate = item.PreparationDate;
                    var shift = item.Shift;
                    dt.Rows.Add(index++, 
                                orderProductionNumber, 
                                constructionNumber, 
                                weavingUnit, 
                                materialType, 
                                amountOfCones, 
                                colourOfCones, 
                                operatorName, 
                                warpingOperatorGroup, 
                                preparationDate, 
                                shift);
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Daily Operation Warping Report") }, true);
        }
    }
}
