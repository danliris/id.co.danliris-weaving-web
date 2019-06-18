using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.XlsTemplates
{
    public class SizePickupReportXlsTemplate
    {
        //private MemoryStream GenerateSizePickupReportXls(List<SizePickupListDto> data)
        //{
        //    DataTable dt = new DataTable();

        //    dt.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
        //    dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(string) });
        //    dt.Columns.Add(new DataColumn() { ColumnName = "Unit", DataType = typeof(string) });
        //    dt.Columns.Add(new DataColumn() { ColumnName = "Jenis Proses", DataType = typeof(string) });
        //    dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Mesin", DataType = typeof(string) });
        //    dt.Columns.Add(new DataColumn() { ColumnName = "Merk Mesin", DataType = typeof(string) });
        //    dt.Columns.Add(new DataColumn() { ColumnName = "Tipe Benang", DataType = typeof(string) });
        //    dt.Columns.Add(new DataColumn() { ColumnName = "Output", DataType = typeof(string) });
        //    dt.Columns.Add(new DataColumn() { ColumnName = "UOM", DataType = typeof(string) });


        //    if (data.Count == 0)
        //    {
        //        dt.Rows.Add("", "", "", "", "", "", "", "", "");
        //    }
        //    else
        //    {
        //        int index = 1;
        //        foreach (var item in data)
        //        {
        //            dt.Rows.Add(index++, item.Date.Value.Date.ToString("dd/MM/yyyy"), item.UnitDepartment.Name, item.ProcessType, item.MachineSpinning.No, item.MachineSpinning.Name,
        //                item.MaterialType.Code, item.Total.ToString("0.00"), item.MachineSpinning.UomUnit);
        //        }
        //    }

        //    return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Machine Input") }, true);
        //}
    }
}
