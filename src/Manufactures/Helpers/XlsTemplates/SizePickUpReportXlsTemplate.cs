using Barebone.Util;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects.SizePickupReport;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Manufactures.Helpers.XlsTemplates
{
    public class SizePickupReportXlsTemplate
    {
        public MemoryStream GenerateSizePickupReportXls(List<SizePickupReportListDto> sizePickupModel)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Selesai", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Grup Sizing", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Operator", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kode Resep", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kecepatan Mesin", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tek", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Visco", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Beam", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "PIS(m)", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Counter Awal", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Counter Akhir", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Netto", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Bruto", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "SPU", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Jam Doffing", DataType = typeof(string) });

            if (sizePickupModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            }
            else
            {
                int index = 1;
                foreach (var item in sizePickupModel)
                {
                    var date = item.DateTimeMachineHistory.ToString("dd MMMM yyyy");
                    var operatorGroup = item.OperatorGroup;
                    var operatorName = item.OperatorName;
                    var recipeCode = item.RecipeCode;
                    var machineSpeed = item.MachineSpeed.ToString();
                    var texSQ = item.TexSQ.ToString();
                    var visco = item.Visco.ToString();
                    var beamNumber = item.BeamNumber;
                    var pism = item.PISMeter.ToString();
                    var counterStart = item.CounterStart;
                    var counterFinish = item.CounterFinish;
                    var netto = item.WeightNetto;
                    var bruto = item.WeightBruto;
                    var spu = item.SPU.ToString();
                    var doffingTime = item.DateTimeMachineHistory.ToString("HH:mm:ss");
                    dt.Rows.Add(index++, 
                                date, 
                                operatorGroup, 
                                operatorName, 
                                recipeCode, 
                                machineSpeed,
                                texSQ, 
                                visco, 
                                beamNumber, 
                                pism, 
                                counterStart, 
                                counterFinish, 
                                netto, 
                                bruto, 
                                spu, 
                                doffingTime);
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Size Pickup Report") }, true);
        }
    }
}
