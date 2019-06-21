using Barebone.Controllers;
using Barebone.Util;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Dtos;
using Microsoft.EntityFrameworkCore;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.XlsTemplates
{
    public class SizePickupReportXlsTemplate
    {
        public MemoryStream GenerateSizePickupReportXls(List<SizePickupDto> sizePickupModel)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Grup Sizing", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Operator", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kode Resep", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kecepatan Mesin", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "TexSQ", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Visco", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Beam", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "PIS", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Counter Awal", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Counter Akhir", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Netto", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Bruto", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "SPU", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Jam Doffing", DataType = typeof(string) });


            if (sizePickupModel.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "");
            }
            else
            {
                int index = 1;
                foreach (var item in sizePickupModel)
                {
                    dt.Rows.Add(index++, item.DateTimeMachineHistory.Date.ToString("dd MMMM yyyy"), item.OperatorGroup, item.OperatorName, item.RecipeCode, item.MachineSpeed,
                        item.TexSQ, item.Visco, item.BeamNumber, item.PIS, item.CounterStart, item.CounterFinish, item.WeightNetto, item.WeightBruto, item.SPU, item.DateTimeMachineHistory.TimeOfDay.ToString("HH:mm:ss"));
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Size Pickup") }, true);
        }
    }
}
