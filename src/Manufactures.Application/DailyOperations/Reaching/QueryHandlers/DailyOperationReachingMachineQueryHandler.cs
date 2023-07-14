using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.Queries;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.QueryHandlers
{
    public class DailyOperationReachingMachineQueryHandler : IDailyOperationReachingMachineQuery<DailyOperationMachineReachingDto>
    {
        ConverterChecker converter = new ConverterChecker();
        GeneralHelper general = new GeneralHelper();
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingMachineRepository _repository;
        public DailyOperationReachingMachineQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {

            _storage =
                storage;

            _repository =
                _storage.GetRepository<IDailyOperationReachingMachineRepository>();
        }

        public async Task Delete(string month, string year)
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();
            var myConnectionString1 = _configuration.GetConnectionString("Default");
            SqlConnection conn = new SqlConnection(myConnectionString1);
            //try
            //{
            conn.Open();
            SqlCommand cmd = new SqlCommand();// Creating instance of SqlCommand  

            String sql = "";
            // set the connection to instance of SqlCommand  
            sql = @"delete WeavingDailyOperationReachingMachines where [month]= '" + month + "' and yearperiode='" + year + "'";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();

            conn.Close();


        }
        public async Task<bool> Upload(ExcelWorksheets sheets, string month, int year, int monthId)
        {

            var startRow = 5;
            var startCol = 1;
            DailyOperationMachineReaching data;
            int rowIndex = 0;
            var totalRows = 0;
            string error = "";
            int saved = 0;
            int notError = 0;
            foreach (var sheet in sheets)
            {
                if (!sheet.Name.Trim().Contains("Monitor"))
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
                error = "Tidak terdapat sheet Monitor di File Excel";
            }
            else
            {
                foreach (var sheet in sheets)
                {
                    if (sheet.Name.Trim() == "Monitor")
                    {
                        error = "";
                        totalRows = sheet.Dimension.Rows;
                        var totalColumns = sheet.Dimension.Columns;
                        try
                        {
                            for (rowIndex = startRow; rowIndex <= totalRows; rowIndex++)
                            {

                                if (Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value)>0)
                                {
                                    if(Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value)< DateTime.DaysInMonth(year, monthId))
                                    {
                                        data = new DailyOperationMachineReaching(
                                            Guid.NewGuid(), //
                                            Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value), //tgl
                                            month,
                                            monthId,//month
                                            year.ToString(),
                                            year.ToString(),//year
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 1]),//shift
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 2]),//group
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 3]),//operator
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 4]),//mcNo
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 5]),//Code
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 6]),//Beam
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 7]),//reachingins
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 8]),//effins
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 9]),//cm
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 10]),//beamWidth
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 11]),//warp
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 12]),//cucukan
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 13]),
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 14]),//comb
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 15]),
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 16]),
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 17]),//doffing
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 18]),//effdoffing
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 19]),//webbing
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 20]),//strands
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 21]),//combno
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 22]),//reed
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 23]),//eff2
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 24]),//checker
                                            converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 25])//info
                                        );
                                        await _repository.Update(data);
                                        saved = 1;
                                    }
                                    else
                                    {
                                        error= ($"Gagal memproses Sheet  pada baris ke-{rowIndex} - bulan {month} hanya sampai tanggal {DateTime.DaysInMonth(year, monthId)}");
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

        public async Task<IEnumerable<DailyOperationMachineReachingDto>> GetAll()
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .GroupBy(r=> new { r.MonthId, r.Month, r.Year, r.CreatedDate.Date})
                            .Select(y =>
                             new DailyOperationMachineReachingDto
                             {
                                 MonthId = y.Key.MonthId,
                                 Month = y.Key.Month,
                                 Year = y.Key.Year,
                                 CreatedDate = y.Key.Date.ToString("dd-MM-yyyy"),
                                 UploadDate= y.Max(a => a.CreatedDate)
                             }).OrderByDescending(a=>a.UploadDate);

            await Task.Yield();

            return query;
        }

        public async Task<DailyOperationMachineReachingDto> GetById(Guid Id)
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .Where(s => s.Identity==Id)
                            .Select(y =>
                             new DailyOperationMachineReachingDto
                             {
                                 MCNo = y.MCNo,
                                 YearPeriode = y.YearPeriode,
                                 Group = y.Group,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy"),
                                 BeamNo=y.BeamNo,
                                 BeamWidth=y.BeamWidth,
                                 Checker=y.Checker,
                                 CM=y.CM,
                                 Code=y.Code,
                                 CombEfficiency=y.CombEfficiency,
                                 CombNo=y.CombNo,
                                 CombStrands=y.CombStrands,
                                 CombWidth=y.CombWidth,
                                 Date=y.Date,
                                 Doffing=y.Doffing,
                                 DoffingEfficiency=y.DoffingEfficiency,
                                 Eff2=y.Eff2,
                                 Information=y.Information,
                                 InstallEfficiency=y.InstallEfficiency,
                                 Margin=y.Margin,
                                 Month=y.Month,
                                 MonthId=y.MonthId,
                                 Operator=y.Operator,
                                 ReachingEfficiency=y.ReachingEfficiency,
                                 ReachingInstall=y.ReachingInstall,
                                 ReachingStrands=y.ReachingStrands,
                                 ReedSpace=y.ReedSpace,
                                 Shift=y.Shift,
                                 TotalWarp=y.TotalWarp,
                                 Webbing=y.Webbing,
                                 Year=y.Year
                             }).FirstOrDefault();

            return query;
        }

        public async Task<List<DailyOperationMachineReachingDto>> GetByMonthYear(int monthId, string year)
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .Where(s => s.MonthId == monthId && s.Year == year)
                            .Select(y =>
                             new DailyOperationMachineReachingDto
                             {
                                 MCNo = y.MCNo,
                                 YearPeriode = y.YearPeriode,
                                 Group = y.Group,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy"),
                                 BeamNo = y.BeamNo,
                                 BeamWidth = y.BeamWidth,
                                 Checker = y.Checker,
                                 CM = y.CM,
                                 Code = y.Code,
                                 CombEfficiency = y.CombEfficiency,
                                 CombNo = y.CombNo,
                                 CombStrands = y.CombStrands,
                                 CombWidth = y.CombWidth,
                                 Date = y.Date,
                                 Doffing = y.Doffing,
                                 DoffingEfficiency = y.DoffingEfficiency,
                                 Eff2 = y.Eff2,
                                 Information = y.Information,
                                 InstallEfficiency = y.InstallEfficiency,
                                 Margin = y.Margin,
                                 Month = y.Month,
                                 MonthId = y.MonthId,
                                 Operator = y.Operator,
                                 ReachingEfficiency = y.ReachingEfficiency,
                                 ReachingInstall = y.ReachingInstall,
                                 ReachingStrands = y.ReachingStrands,
                                 ReedSpace = y.ReedSpace,
                                 Shift = y.Shift,
                                 TotalWarp = y.TotalWarp,
                                 Webbing = y.Webbing,
                                 Year = y.Year,
                                 Periode= new DateTime(Convert.ToInt32(y.YearPeriode), y.MonthId, y.Date)
                             }).OrderBy(o=>o.Periode);

            return query.ToList();
        }


        public List<DailyOperationMachineReachingDto> GetDailyReports(DateTime fromDate, DateTime toDate, string shift, string mcNo)
        {
            var allData = from a in _repository.Query
                          where (mcNo == null || (mcNo != null && mcNo != "" && a.MCNo.Contains(mcNo))) &&
                                (shift == null || (shift != null && shift != "" && a.Shift == shift))
                          select new
                          {
                              code = a.Code,
                              beamNo = a.BeamNo,
                              Periode = new DateTime(Convert.ToInt32(a.YearPeriode), a.MonthId, a.Date).Date
                          };
            var query = (from a in allData
                         where (a.Periode.Date >= fromDate.Date && a.Periode.Date <= toDate.Date)
                         group  a by new { a.code, a.Periode }  into g
                         select new DailyOperationMachineReachingDto
                         {
                             Code= g.Key.code,
                             Periode=g.Key.Periode,
                             BeamNo=g.Count().ToString()
                         });

            return query.OrderBy(a => a.Periode).ToList();
        }
    }
}
