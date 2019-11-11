using Barebone.Util;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects.DailyOperationSizingReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.XlsTemplates
{
    public class DailyOperationSizingReportXlsTemplate
    {
        public MemoryStream GenerateDailyOperationSizingReportXls(List<DailyOperationSizingReportListDto> dailyOperationSizingReportModel)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Mesin", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Order Produksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Konstruksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Unit Weaving", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kode Resep", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Ne Real", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Operator", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Grup Sizing", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Pasang", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Shift", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Total Helai Benang", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Total Berat Kosong Beam Warping", DataType = typeof(string) });

            if (dailyOperationSizingReportModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "");
            }
            else
            {
                int index = 1;
                foreach (var item in dailyOperationSizingReportModel)
                {
                    var machineNumber = item.MachineNumber;
                    var orderProductionNumber = item.OrderProductionNumber;
                    var constructionNumber = item.ConstructionNumber;
                    var weavingUnit = item.WeavingUnit;
                    var recipeCode = item.RecipeCode;
                    var neReal = item.NeReal;
                    var operatorName = item.OperatorName;
                    var sizingOperatorGroup = item.SizingOperatorGroup;
                    var preparationDate = item.PreparationDate;
                    var shift = item.Shift;
                    var yarnStrands = item.YarnStrands;
                    var emptyWeight = item.EmptyWeight;
                    dt.Rows.Add(index++,
                                orderProductionNumber,
                                constructionNumber,
                                weavingUnit,
                                recipeCode,
                                neReal,
                                operatorName,
                                sizingOperatorGroup,
                                preparationDate,
                                shift,
                                yarnStrands,
                                emptyWeight);
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Daily Operation Sizing Report") }, true);
        }
    }
}
