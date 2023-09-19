using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Queries;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.QueryHandlers
{
    public class DailyOperationLoomMachineQueryHandler : IDailyOperationLoomMachineQuery<DailyOperationLoomMachineDto>
    {
        ConverterChecker converter = new ConverterChecker();
        GeneralHelper general = new GeneralHelper();
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomMachineRepository _repository;
        public DailyOperationLoomMachineQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {

            _storage =
                storage;

            _repository =
                _storage.GetRepository<IDailyOperationLoomMachineRepository>();
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

            sql = @"delete WeavingDailyOperationLoomMachines where [MonthPeriode]= '" + month + "' and yearperiode='" + year + "'";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();

            conn.Close();
        }
        public async Task<bool> Upload(ExcelWorksheets sheets, string month, int year, int monthId)
        {

            var startRow = 3;
            var startCol = 2;
            DailyOperationLoomMachine data;
            int rowIndex = 0;
            var totalRows = 0;
            string error = "";
            int saved = 0;
            int notError = 0;
            foreach (var sheet in sheets)
            {
                if (!sheet.Name.Trim().Contains("MASTER"))
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
                error = "Tidak terdapat sheet MASTER di File Excel";
            }
            else
            {
                foreach (var sheet in sheets)
                {
                    if (sheet.Name.Trim() == "MASTER")
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
                                    if (Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value) <= DateTime.DaysInMonth(year, monthId))
                                    {
                                        data = new DailyOperationLoomMachine(
                                        Guid.NewGuid(),
                                        Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value), //tgl
                                        month,
                                        monthId,//month
                                        year.ToString(),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 1]),//shift
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 2]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 3]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 4]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 5]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 6]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 7]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 8]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 9]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 10]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 11]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 12]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 13]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 14]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 15]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 16]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 17]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 18]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 19]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 20]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 21]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 22]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 23]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 24]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 25]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 26]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 27]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 28]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 29]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 30]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 31]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 32]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 33]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 34]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 35]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 36]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 37]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 38]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 39]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 40]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 41]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 42]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 43]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 44]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 45]),
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 46])
                                        );
                                        await _repository.Update(data);
                                        saved = 1;
                                    }
                                    else
                                    {
                                        error = ($"Gagal memproses Sheet  pada baris ke-{rowIndex} - bulan {month} hanya sampai tanggal {DateTime.DaysInMonth(year, monthId)}");
                                        saved = 0;
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

        public async Task<IEnumerable<DailyOperationLoomMachineDto>> GetAll()
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .GroupBy(r => new { r.MonthPeriodeId, r.MonthPeriode, r.YearPeriode, r.CreatedDate.Date })
                            .Select(y =>
                             new DailyOperationLoomMachineDto
                             {
                                 MonthPeriodeId = y.Key.MonthPeriodeId,
                                 MonthPeriode = y.Key.MonthPeriode,
                                 YearPeriode = y.Key.YearPeriode,
                                 CreatedDate = y.Key.Date.ToString("dd-MM-yyyy"),
                                 UploadDate= y.Max(a=>a.CreatedDate)
                             }).OrderByDescending(o => o.UploadDate);

            await Task.Yield();

            return query;
        }

        public async Task<DailyOperationLoomMachineDto> GetById(Guid Id)
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .Where(s => s.Identity == Id)
                            .Select(y =>
                             new DailyOperationLoomMachineDto {
                                 MCNo = y.MCNo,
                                 YearPeriode = y.YearPeriode,
                                 AL = y.AL,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy"),
                                 AP = y.AP,
                                 AP2 = y.AP2,
                                 AP3 = y.AP3,
                                 Block = y.Block,
                                 BlockName = y.BlockName,
                                 T = y.T,
                                 TA = y.TA,
                                 Column1 = y.Column1,
                                 Construction = y.Construction,
                                 Date = y.Date,
                                 EFFMC = y.EFFMC,
                                 F = y.F,
                                 L = y.L,
                                 Length = y.Length,
                                 Location = y.Location,
                                 MachineNameType = y.MachineNameType,
                                 MachineType = y.MachineType,
                                 MonthId = y.MonthId,
                                 Operator = y.Operator,
                                 MC2Eff = y.MC2Eff,
                                 MCNo2 = y.MCNo2,
                                 MCRPM = y.MCRPM,
                                 MTC = y.MTC,
                                 Shift = y.Shift,
                                 MTCLock = y.MTCLock,
                                 PercentEff = y.PercentEff,
                                 Year = y.Year,
                                 MTCName = y.MTCName,
                                 Production = y.Production,
                                 Production100 = y.Production100,
                                 ProductionCMPX = y.ProductionCMPX,
                                 RPM = y.RPM,
                                 RPMProduction100 = y.RPMProduction100,
                                 SPNo = y.SPNo,
                                 SPYear = y.SPYear,
                                 Thread = y.Thread,
                                 ThreadType = y.ThreadType,
                                 WarpType = y.WarpType,
                                 W = y.W,
                                 Warp = y.Warp,
                                 Weft = y.Weft,
                                 WeftType = y.WeftType,
                                 WeftType2 = y.WeftType2,
                                 WeftType3 = y.WeftType3,
                                 Periode = new DateTime(Convert.ToInt32(y.YearPeriode), y.MonthPeriodeId, y.Date)
                             }).FirstOrDefault();

            return query;
        }

        public async Task<List<DailyOperationLoomMachineDto>> GetByMonthYear(int monthId, string year)
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .Where(s => s.MonthPeriodeId == monthId && s.YearPeriode == year)
                            .Select(y =>
                             new DailyOperationLoomMachineDto
                             {
                                 MCNo = y.MCNo,
                                 YearPeriode = y.YearPeriode,
                                 AL = y.AL,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy"),
                                 AP = y.AP,
                                 AP2 = y.AP2,
                                 AP3 = y.AP3,
                                 Block = y.Block,
                                 BlockName = y.BlockName,
                                 T = y.T,
                                 TA = y.TA,
                                 Column1 = y.Column1,
                                 Construction = y.Construction,
                                 Date = y.Date,
                                 EFFMC = y.EFFMC,
                                 F = y.F,
                                 L = y.L,
                                 Length = y.Length,
                                 Location = y.Location,
                                 MachineNameType = y.MachineNameType,
                                 MachineType = y.MachineType,
                                 MonthId = y.MonthId,
                                 Operator = y.Operator,
                                 MC2Eff = y.MC2Eff,
                                 MCNo2 = y.MCNo2,
                                 MCRPM = y.MCRPM,
                                 MTC = y.MTC,
                                 Shift = y.Shift,
                                 MTCLock = y.MTCLock,
                                 PercentEff = y.PercentEff,
                                 Year = y.Year,
                                 MTCName = y.MTCName,
                                 Production = y.Production,
                                 Production100 = y.Production100,
                                 ProductionCMPX = y.ProductionCMPX,
                                 RPM = y.RPM,
                                 RPMProduction100 = y.RPMProduction100,
                                 SPNo = y.SPNo,
                                 SPYear = y.SPYear,
                                 Thread = y.Thread,
                                 ThreadType = y.ThreadType,
                                 WarpType = y.WarpType,
                                 W = y.W,
                                 Warp = y.Warp,
                                 Weft = y.Weft,
                                 WeftType = y.WeftType,
                                 WeftType2 = y.WeftType2,
                                 WeftType3 = y.WeftType3,
                                 Periode = new DateTime(Convert.ToInt32(y.YearPeriode), y.MonthPeriodeId, y.Date)
                             }).OrderBy(o => o.Periode).ThenBy(a => a.Shift);

            return query.ToList();
        }


        //jika pake "public async" hrs pake Await
        public List<DailyOperationLoomMachineDto> GetDailyReports(DateTime fromDate, DateTime toDate, string jenisMesin, string namaBlok, string namaMtc, string operatornya, string shift, string sp)
        {
            var allData = from a in _repository.Query
                          where (jenisMesin == null || (jenisMesin != null && jenisMesin != "" && a.MachineNameType == jenisMesin)) &&
                          (namaBlok == null || (namaBlok != null && namaBlok != "" && a.BlockName == namaBlok)) &&
                          (namaMtc == null || (namaMtc != null && namaMtc != "" && a.MTCName == namaMtc)) &&
                          (operatornya == null || (operatornya != null && operatornya != "" && a.Operator == operatornya)) &&
                          (shift == null || (shift != null && shift != "" && a.Shift == shift)) &&
                          (sp == null || (sp != null && sp != "" && a.SPYear == sp)) 
                          


                          select new
                          {

                              mcNo = a.MCNo,
                              yearPeriode = a.YearPeriode,
                              al = a.AL,
                              createdDate = a.CreatedDate.ToString("dd-MM-yyyy"),
                              ap = a.AP,
                              ap2 = a.AP2,
                              ap3 = a.AP3,
                              block = a.Block,
                              blockName = a.BlockName,
                              t = a.T,
                              ta = a.TA,
                              column1 = a.Column1,
                              construction = a.Construction,
                              date = a.Date,
                              eFFMC = a.EFFMC,
                              f = a.F,
                              l = a.L,
                              length = a.Length,
                              location = a.Location,
                              machineNameType = a.MachineNameType,
                              machineType = a.MachineType,
                              monthId = a.MonthId,
                              Operator = a.Operator,
                              mc2Eff = a.MC2Eff,
                              mcNo2 = a.MCNo2,
                              mcRPM = a.MCRPM,
                              mtc = a.MTC,
                              shift = a.Shift,
                              mtCLock = a.MTCLock,
                              percentEff = a.PercentEff,
                              year = a.Year,
                              mTCName = a.MTCName,
                              production = a.Production,
                              production100 = a.Production100,
                              productionCMPX = a.ProductionCMPX,
                              rpm = a.RPM,
                              rpmProduction100 = a.RPMProduction100,
                              spNo = a.SPNo,
                              spYear = a.SPYear,
                              thread = a.Thread,
                              threadType = a.ThreadType,
                              warpType = a.WarpType,
                              w = a.W,
                              warp = a.Warp,
                              weft = a.Weft,
                              weftType = a.WeftType,
                              weftType2 = a.WeftType2,
                              weftType3 = a.WeftType3,
                              Periode = new DateTime(Convert.ToInt32(a.YearPeriode), a.MonthPeriodeId, a.Date)
                            
                          };



            var query = (from a in allData
                         where (a.Periode.Date >= fromDate.Date && a.Periode.Date <= toDate.Date)

                          select new DailyOperationLoomMachineDto
                         {
                             //besar = kecil
                             MCNo = a.mcNo,
                             YearPeriode = a.yearPeriode,
                             AL = a.al,
                             CreatedDate = a.createdDate,
                             AP = a.ap,
                             AP2 = a.ap2,
                             AP3 = a.ap3,
                             Block = a.block,
                             BlockName = a.blockName,
                             T = a.t,
                             TA = a.ta,
                             Column1 = a.column1,
                             Construction = a.construction,
                             Date = a.date,
                             EFFMC = a.eFFMC,
                             F = a.f,
                             L = a.l,
                             Length = a.length,
                             Location = a.location,
                             MachineNameType = a.machineNameType,
                             MachineType = a.machineType,
                             MonthId = a.monthId,
                             Operator = a.Operator,
                             MC2Eff = a.mc2Eff,
                             MCNo2 = a.mcNo2,
                             MCRPM = a.mcRPM,
                             MTC = a.mtc,
                             Shift = a.shift,
                             MTCLock = a.mtCLock,
                             PercentEff = a.percentEff,
                             Year = a.year,
                             MTCName = a.mTCName,
                             Production = a.production,
                             Production100 = a.production100,
                             ProductionCMPX = a.productionCMPX,
                             RPM = a.rpm,
                             RPMProduction100 = a.rpmProduction100,
                             SPNo = a.spNo,
                             SPYear = a.spYear,
                             Thread = a.thread,
                             ThreadType = a.threadType,
                             WarpType = a.warpType,
                             W = a.w,
                             Warp = a.warp,
                             Weft = a.weft,
                             WeftType = a.weftType,
                             WeftType2 = a.weftType2,
                             WeftType3 = a.weftType3,
                             Periode=a.Periode
                         }).GroupBy(r => new { r.Construction, r.ThreadType })
                         .Select(y =>
                             new DailyOperationLoomMachineDto
                             {
                                 Construction = y.Key.Construction,
                                 ThreadType = y.Key.ThreadType,
                                 TotProductionCMPX = y.Sum(z => Convert.ToDecimal(z.ProductionCMPX)),
                                 TotProduction = y.Sum(z => Convert.ToDecimal(z.Production)),
                                 TotProduction100 = y.Sum(z => Convert.ToDecimal(z.Production100)),
                                 TotPercentEff = (y.Average(z => Convert.ToDecimal(z.PercentEff))*100),
                                 TotMC2Eff = (y.Average(z => Convert.ToDecimal(z.MC2Eff))*100),
                                 TotF = y.Average(z => Convert.ToDecimal(z.F)),
                                 TotW = y.Average(z => Convert.ToDecimal(z.W)),
                                 TotRPM = y.Average(z => Convert.ToDecimal(z.RPM)),
                                 TotMCNo = y.Key.Construction.Count()
                                 
                             })
                        
                             ;

            

                 return query.OrderBy(a => a.Construction)
                 //.ThenBy(a => a.Shift)
                 .ToList();
        }
    }
}
