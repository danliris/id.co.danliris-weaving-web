using Barebone.Util;
using Manufactures.Application.MachinesPlanning.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.XlsTemplates
{
    public class MachinePlanningReportXlsTemplate
    {
        public MemoryStream GenerateMachinePlanningReportXls(List<MachinesPlanningReportListDto> machinesPlanningReportModel)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Unit Weaving", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Mesin", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Area", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Blok", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Blok Kaizen", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Lokasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Maintenance", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Operator", DataType = typeof(string) });

            if (machinesPlanningReportModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "");
            }
            else
            {
                int index = 1;
                foreach (var item in machinesPlanningReportModel)
                {
                    var weavingUnit = item.WeavingUnit;
                    var machineNumber = item.MachineNumber;
                    var area = item.Area;
                    var block = item.Block;
                    var kaizenBlock = item.KaizenBlock;
                    var location = item.Location;
                    var maintenance = item.Maintenance;
                    var operatorName = item.OperatorName;
                    dt.Rows.Add(index++, weavingUnit, machineNumber, area, block, kaizenBlock, location, maintenance, operatorName);
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Machine Planning Report") }, true);
        }
    }
}
