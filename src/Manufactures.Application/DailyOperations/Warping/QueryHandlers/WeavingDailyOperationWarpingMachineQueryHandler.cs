using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Queries.WeavingDailyOperationWarpingMachines;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers
{
    public class WeavingDailyOperationWarpingMachineQueryHandler : IWeavingDailyOperationWarpingMachineQuery<WeavingDailyOperationWarpingMachineDto>
    {
        ConverterChecker converter = new ConverterChecker();
        GeneralHelper general = new GeneralHelper();
        private readonly IStorage _storage;
        private readonly IWeavingDailyOperationWarpingMachineRepository _repository;
        public WeavingDailyOperationWarpingMachineQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {

            _storage =
                storage;

            _repository =
                _storage.GetRepository<IWeavingDailyOperationWarpingMachineRepository>();
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
                sql = @"delete WeavingDailyOperationWarpingMachines where [month]= '" + month + "' and yearperiode='" + year + "'";
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                conn.Close();

            //}
            //catch (Exception ex)
            //{

            //}

        }
        public async Task<bool> Upload(ExcelWorksheets sheets, string month, int year, int monthId)
        {

            var startRow = 5;
            var startCol = 1;
            WeavingDailyOperationWarpingMachine data;
            int rowIndex = 0;
            var totalRows = 0;
            string error = "";
            int saved = 0;
            int notError = 0;
            foreach (var sheet in sheets)
            {
                if (!sheet.Name.Trim().Contains("Produksi WP") )
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
                    error = "Tidak terdapat sheet Produksi WP di File Excel";
                }
                else  
                {
                    foreach (var sheet in sheets)
                    {
                        if (sheet.Name.Trim() == "Produksi WP")
                        {
                            error = "";
                            totalRows = sheet.Dimension.Rows;
                            var totalColumns = sheet.Dimension.Columns;
                        try
                        {
                            for (rowIndex = startRow; rowIndex <= totalRows; rowIndex++)
                            {
                                var a = converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 16]);
                                if (Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value)>0)
                                {
                                    if (Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value) <= DateTime.DaysInMonth(year, monthId))
                                    {
                                        data = new WeavingDailyOperationWarpingMachine(
                                        Guid.NewGuid(), //
                                        Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value), //tgl
                                        month,
                                        monthId,//month
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 8]),
                                        year.ToString(),//year
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 2]),//shift
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 3]),//mcNo
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 4]),//Name
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 5]),//Group
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 6]),//Lot
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 7]),//SP
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 9]),//YearSP
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 10]),//WarpType
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 11]),//AL
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 12]),//COnstruction
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 13]),//Code
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 14]),//BeamNo
                                        Convert.ToInt32(converter.GenerateValueInt(sheet.Cells[rowIndex, startCol + 15])),//TotalCone
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 16]),//ThreadNo
                                        Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 17])),//Length
                                        "MT",//uom
                                        Convert.ToDateTime(converter.GeneratePureTime(sheet.Cells[rowIndex, startCol + 18])),//start
                                        Convert.ToDateTime(converter.GeneratePureTime(sheet.Cells[rowIndex, startCol + 19])),//Doff
                                        Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 20])),//HNLeft
                                        Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 21])),//HNMiddle
                                        Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 22])),//HNRight
                                        Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 23])),//Speed
                                        Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 24])),//ThreadCut
                                        Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 25])),//Capacity
                                        converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 26]),//Eff
                                        Convert.ToInt32(converter.GenerateValueInt(sheet.Cells[rowIndex, startCol + 1]))//Week
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
                if (saved == 0)
                {
                    error = "Tahun tidak sesuai";
                    throw new Exception($"Tahun dan Bulan tidak sesuai");

                }
                else if (error != "" && saved == 0)
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

        public async Task<IEnumerable<WeavingDailyOperationWarpingMachineDto>> GetAll()
        {
            var query = (from a in _repository.Query
                        group a by new { a.YearPeriode, a.MonthId, a.Month, a.CreatedDate.Date } into g
                        select new WeavingDailyOperationWarpingMachineDto
                        {
                            YearPeriode = g.Key.YearPeriode,
                            Month = g.Key.Month,
                            MonthId = g.Key.MonthId,
                            CreatedDate = g.Key.Date.ToString("dd MMM yyyy"),
                            UploadDate = g.Max(a => a.CreatedDate)
                        }).OrderByDescending(a=>a.UploadDate);

            await Task.Yield();
            return query;
        }

        public async Task<WeavingDailyOperationWarpingMachineDto> GetById(Guid id)
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .Where(s=>s.Identity == id)
                            .Select(y =>
                             new WeavingDailyOperationWarpingMachineDto
                             {
                                 MCNo = y.MCNo,
                                 YearPeriode = y.YearPeriode,
                                 Group = y.Group,
                                 Name = y.Name,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy")
                             }).FirstOrDefault();
            return query;
        }

        public async Task<List<WeavingDailyOperationWarpingMachineDto>> GetByMonthYear(int monthId, string year)
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .Where(s => s.MonthId == monthId && s.YearPeriode == year)
                            .Select(y =>
                             new WeavingDailyOperationWarpingMachineDto
                             {
                                 Day=y.Date,
                                 MCNo = y.MCNo,
                                 YearPeriode = y.YearPeriode,
                                 Group = y.Group,
                                 Name = y.Name,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy"),
                                 Week=y.Week,
                                 Shift=y.Shift,
                                 Lot=y.Lot,
                                 SP=y.SP,
                                 Year=y.Year,
                                 YearSP=y.YearSP,
                                 WarpType=y.WarpType,
                                 AL=y.AL,
                                 Construction=y.Construction,
                                 Code=y.Code,
                                 BeamNo=y.BeamNo,
                                 TotalCone=y.TotalCone,
                                 ThreadNo=y.ThreadNo,
                                 Length=y.Length,
                                 Start=y.Start,
                                 Doff=y.Doff,
                                 HNLeft=y.HNLeft,
                                 HNMiddle=y.HNMiddle,
                                 HNRight=y.HNRight,
                                 SpeedMeterPerMinute=y.SpeedMeterPerMinute,
                                 ThreadCut=y.ThreadCut,
                                 Capacity=y.Capacity,
                                 Eff= Convert.ToDecimal(y.Eff),
                                 MonthId=y.MonthId
                             }).OrderBy(a=>a.YearPeriode).ThenBy(a=>a.MonthId).ThenBy(a=>a.Day).ThenBy(a=>a.Week).ThenBy(a=>a.Shift).ThenBy(a=>a.MCNo);
            return query.ToList();
        }

        public List<WeavingDailyOperationWarpingMachineDto> GetReports(DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string threadNo, string code)
        {
            var allData = from a in _repository.Query
                          select new
                          {
                              code = a.Code,
                              threadNo = a.ThreadNo,
                              shift = a.Shift,
                              sp = a.SP,
                              threadCut = a.ThreadCut,
                              length = a.Length,
                              mcNo = a.MCNo,
                              month = a.Month,
                              yearPeriode = a.YearPeriode,
                              day = a.Date,
                              name = a.Name,
                              warpType= a.WarpType,
                              al= a.AL,
                              Periode = new DateTime(Convert.ToInt32(a.YearPeriode), a.MonthId, a.Date)
                          };
            var query = (from a in allData
                         where
                         ((mcNo == null || (mcNo != null && mcNo != "" && a.mcNo.Contains(mcNo))) &&
                         (shift == null || (shift != null && shift != "" && a.shift == shift)) &&
                         (sp == null || (sp != null && sp != "" && a.sp==sp)) &&
                         (threadNo == null || (threadNo != null && threadNo != "" && a.threadNo.Contains(threadNo))) &&
                         (code == null || (code != null && code != "" && a.code.Contains(code))) &&
                         (a.Periode.Date >= fromDate.Date && a.Periode.Date <= toDate.Date))
                         select new { a.name, a.threadCut, Length = a.length, a.code, a.threadNo, a.al, a.Periode })

                        .GroupBy(l => new { l.Periode, l.code, l.threadNo, l.al })

                            .Select(cl => new
                            {
                                AL = cl.Key.al,
                                Code = cl.Key.code,
                                Periode = cl.Key.Periode,
                                ThreadNo= cl.Key.threadNo,
                                ThreadCut=cl.Sum(a=>a.threadCut)

                            }).ToList().OrderBy(a=>a.Periode).ThenBy(a=>a.Code).ThenBy(a=>a.ThreadNo).ThenBy(a=>a.AL);
            List<WeavingDailyOperationWarpingMachineDto> list = new List<WeavingDailyOperationWarpingMachineDto>();
            foreach (var item in query)
            {
                WeavingDailyOperationWarpingMachineDto dto = new WeavingDailyOperationWarpingMachineDto
                {
                    ThreadCut = item.ThreadCut,
                    AL= item.AL,
                    ThreadNo=item.ThreadNo,
                    Date=item.Periode,
                    Code=item.Code
                };
                list.Add(dto);
            }

            return list;

        }

        public List<WeavingDailyOperationWarpingMachineDto> GetDailyReports(DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string name, string code)
        {
            var allData = from a in _repository.Query
                          where (mcNo == null || (mcNo != null && mcNo != "" && a.MCNo.Contains(mcNo))) &&
                        (shift == null || (shift != null && shift != "" && a.Shift == shift)) &&
                        (sp == null || (sp != null && sp != "" && a.SP.Contains(sp))) &&
                        (name == null || (name != null && name != "" && a.Name.Contains(name))) &&
                        (code == null || (code != null && code != "" && a.Code.Contains(code))) 
                          select new
                          {
                              code = a.Code,
                              threadNo = a.ThreadNo,
                              shift = a.Shift,
                              sp = a.SP,
                              threadCut = a.ThreadCut,
                              length = a.Length,
                              mcNo = a.MCNo,
                              day = a.Date,
                              name = a.Name,
                              Periode = new DateTime(Convert.ToInt32(a.YearPeriode), a.MonthId, a.Date),
                              a.Length,
                              efficiency=a.Eff
                          };
            var query = (from a in allData
                         where (a.Periode.Date >= fromDate.Date && a.Periode.Date <= toDate.Date)
                         group a by new { a.shift, a.Periode, a.mcNo } into g
                         select new WeavingDailyOperationWarpingMachineDto
                         {
                             MCNo= g.Key.mcNo,
                             Date= g.Key.Periode,
                             Shift=g.Key.shift,
                             Length=g.Sum(z=>z.Length),
                             Eff= g.Sum(z => Convert.ToDecimal(z.efficiency)),
                             ThreadCut= g.Sum(z => z.threadCut)
                         });

            return query.OrderBy(a=>a.Date).ThenBy(b=>b.Shift).ThenBy(c=>c.MCNo).ToList();
        }
    }
}
