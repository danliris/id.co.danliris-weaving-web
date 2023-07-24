using ExtCore.Data.Abstractions;
using Manufactures.Application.BeamStockUpload.DataTransferObjects;
using Manufactures.Application.Helpers;
using Manufactures.Domain.BeamStockUpload.Entities;
using Manufactures.Domain.BeamStockUpload.Queries;
using Manufactures.Domain.BeamStockUpload.Repositories;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.BeamStockUpload.QueryHandlers
{
    public class BeamStockUploadQueryHandler : IBeamStockQuery<BeamStockUploadDto>
    {
        ConverterChecker converter = new ConverterChecker();
        GeneralHelper general = new GeneralHelper();
        private readonly IStorage _storage;
        private readonly IBeamStockRepository _repository;
        public BeamStockUploadQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {

            _storage =
                storage;

            _repository =
                _storage.GetRepository<IBeamStockRepository>();
        }

        public async Task Delete(string month, string year)
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

            IConfiguration _configuration = builder.Build();
            var myConnectionString1 = _configuration.GetConnectionString("Default");
            SqlConnection conn = new SqlConnection(myConnectionString1);
            conn.Open();

            SqlCommand cmd = new SqlCommand();// Creating instance of SqlCommand  
            String sql = "";

            sql = @"delete WeavingBeamStocks where [MonthPeriode]= '" + month + "' and yearperiode='" + year + "'";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();

            conn.Close();
        }
        public async Task<bool> Upload(ExcelWorksheets sheets, string month, int year, int monthId)
        {

            var startRow = 5;
            var startCol = 1;
            BeamStock data;
            int rowIndex = 0;
            var totalRows = 0;
            string error = "";
            int saved = 0;
            int notError = 0;
            foreach (var sheet in sheets)
            {
                if (!sheet.Name.Trim().Contains("Stock"))
                {
                    notError = 0;
                }
                else
                {
                    notError = 1;
                    break;
                }
            }


            if (notError == 0)
            {
                error = "Tidak terdapat sheet Stock di File Excel";
            }
            else
            {
                foreach (var sheet in sheets)
                {
                    if (sheet.Name.Trim() == "Stock")
                    {
                        error = "";
                        totalRows = sheet.Dimension.Rows;
                        var totalColumns = sheet.Dimension.Columns;
                        try
                        {
                            for (rowIndex = startRow; rowIndex <= totalRows; rowIndex++)
                            {

                                if (Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value) > 0)
                                {
                                    if (Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value) <= DateTime.DaysInMonth(year, monthId))
                                    {
                                        data = new BeamStock(
                                        Guid.NewGuid(),
                                        Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value), //tgl
                                        year.ToString(),
                                        month,
                                        monthId,//month
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 1]),//shift
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 2]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 3]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 4]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 5]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 6]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 7])
                                        );
                                        await _repository.Update(data);
                                        saved = 1;
                                    }
                                    else
                                    {
                                        saved = 0;
                                        error = ($"Gagal memproses Sheet  pada baris ke-{rowIndex} - bulan {month} hanya sampai tanggal {DateTime.DaysInMonth(year, monthId)}");
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Gagal memproses Sheet  pada baris ke-{rowIndex} - {ex.Message}");
                        }
                    }
                }

            }
            try
            {
                if (error != "" && saved == 0)
                {
                    throw new Exception($"ERROR " + error);
                }
                else
                {
                    await Delete(month, year.ToString());
                    _storage.Save();
                    return ((rowIndex - 1) == totalRows && totalRows > 0);
                }


            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR \n" + ex.Message + "\n");
            }

        }

        public async Task<IEnumerable<BeamStockUploadDto>> GetAll()
        {
            var query = _repository
                            .Query
                            .GroupBy(r => new { r.MonthPeriodeId, r.MonthPeriode, r.YearPeriode, r.CreatedDate.Date })
                            .Select(y =>
                             new BeamStockUploadDto
                             {
                                 MonthPeriodeId = y.Key.MonthPeriodeId,
                                 MonthPeriode = y.Key.MonthPeriode,
                                 YearPeriode = y.Key.YearPeriode,
                                 CreatedDate = y.Key.Date.ToString("dd-MM-yyyy"),
                                 UploadDate = y.Max(a => a.CreatedDate)
                             }).OrderByDescending(o => o.UploadDate);

            await Task.Yield();

            return query;
        }

        public async Task<BeamStockUploadDto> GetById(Guid Id)
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .Where(s => s.Identity == Id)
                            .Select(y =>
                             new BeamStockUploadDto
                             {
                                 YearPeriode = y.YearPeriode,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy"),
                                 Date = y.Date,
                                 Periode = new DateTime(Convert.ToInt32(y.YearPeriode), y.MonthPeriodeId, y.Date)
                             }).FirstOrDefault();

            return query;
        }

        public async Task<List<BeamStockUploadDto>> GetByMonthYear(int monthId, string year)
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .Where(s => s.MonthPeriodeId == monthId && s.YearPeriode == year)
                            .Select(y =>
                             new BeamStockUploadDto
                             {
                                 YearPeriode = y.YearPeriode,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy"),
                                 Date = y.Date,
                                 Shift = y.Shift,
                                 Beam=y.Beam,
                                 Code=y.Code,
                                 Information=y.Information,
                                 InReaching=y.InReaching,
                                 Reaching=y.Reaching,
                                 MonthPeriode=y.MonthPeriode,
                                 MonthPeriodeId=y.MonthPeriodeId,
                                 Sizing=y.Sizing,
                                 Periode = new DateTime(Convert.ToInt32(y.YearPeriode), y.MonthPeriodeId, y.Date)
                             }).OrderBy(o => o.Periode);

            return query.ToList();
        }
    }
}
