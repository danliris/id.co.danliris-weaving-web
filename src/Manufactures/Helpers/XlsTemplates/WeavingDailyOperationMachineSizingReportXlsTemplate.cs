using Manufactures.Application.DailyOperations.Production.DataTransferObjects;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Manufactures.Helpers.XlsTemplates
{
    public class WeavingDailyOperationMachineSizingReportXlsTemplate
    {
        public MemoryStream GenerateXls(List<WeavingDailyOperationMachineSizingDto> machineDtos, string month, string yearPeriode)
        {

            var reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO SP", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "KONSTRUKSI", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "JENIS BENANG", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "ESTIMASI PRODUKSI", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "ESTIMASI PRODUKSI1", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "ESTIMASI PRODUKSI2", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "ESTIMASI PRODUKSI3", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "ESTIMASI PRODUKSI4", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "ORDER BARU ALL", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "KEBUTUHAN BARANG", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "KEBUTUHAN BARANG1", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "KEBUTUHAN BARANG2", DataType = typeof(string) });
            reportDataTable.Rows.Add("", "", "", "", "GRADE A", "GRADE B", "GRADE C", "AVAL", "TOTAL","", "LUSI", "PAKAN", "TOTAL");
            int idx = 1;
            
            foreach (var item in machineDtos)
            {
                reportDataTable.Rows.Add(item.SPNo, item.Date.ToString(), item.Construction1,item.WarpXWeft, item.GradeA,item.GradeB,item.GradeC,item.Aval,item.Total,item.NumberOrder,item.WarpBale,item.WeftBale,item.TotalBale);
                
            };

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "LAPORAN ESTIMASI PRODUKSI";
                worksheet.Cells["A2"].Value = month + " " + yearPeriode;
                
                worksheet.Cells["A" + 1 + ":M" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":M" + 2 + ""].Merge = true;
                worksheet.Cells["E" + 5 + ":I" + 5 + ""].Merge = true;
                worksheet.Cells["K" + 5 + ":M" + 5 + ""].Merge = true;

                worksheet.Cells["A" + 5 + ":A" + 6 + ""].Merge = true;
                worksheet.Cells["B" + 5 + ":B" + 6 + ""].Merge = true;
                worksheet.Cells["J" + 5 + ":J" + 6 + ""].Merge = true;

                worksheet.Cells["A" + 1 + ":M" + 6 + ""].Style.Font.Bold = true;
                worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);

                worksheet.Cells["A" + 5 + ":M" + (6 + machineDtos.Count())+""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":M" + (6 + machineDtos.Count()) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":M" + (6 + machineDtos.Count()) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":M" + (6 + machineDtos.Count()) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":M" +  6+ ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                
                worksheet.Cells["A" + 5 + ":M" +5 + ""].AutoFitColumns();
             
                var stream = new MemoryStream();
                package.SaveAs(stream);
                return stream;
            }
        }

    }
}
