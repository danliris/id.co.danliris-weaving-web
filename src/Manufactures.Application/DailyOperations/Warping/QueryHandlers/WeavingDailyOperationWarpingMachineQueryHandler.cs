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
            int isSave = 0;
            int saved = 0;
            foreach (var sheet in sheets)
            {
                if (!sheet.Name.Trim().Contains("Produksi WP"))
                {
                    error = "Tidak terdapat sheet Produksi WP di File Excel";
                }
                else
                {
                    error = null;
                    totalRows = sheet.Dimension.Rows;
                    var totalColumns = sheet.Dimension.Columns;

                    if (sheet.Name.Trim() == "Produksi WP")
                    {
                        try
                        {
                            for (rowIndex = startRow; rowIndex <= totalRows; rowIndex++)
                            {

                                if (sheet.Cells[rowIndex, startCol].Value != null || sheet.Cells[rowIndex, startCol].Value != "")
                                {
                                    //if (converter.GenerateValueInt(sheet.Cells[rowIndex, startCol + 7]) == year)
                                    //{
                                    data = new WeavingDailyOperationWarpingMachine(
                                    Guid.NewGuid(), //
                                    Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value), //tgl
                                    month,
                                    monthId,//month
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 7]),
                                    year.ToString(),//year
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 1]),//shift
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 2]),//mcNo
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 3]),//Name
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 4]),//Group
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 5]),//Lot
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 6]),//SP
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 8]),//YearSP
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 9]),//WarpType
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 10]),//AL
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 11]),//COnstruction
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 12]),//Code
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 13]),//BeamNo
                                    Convert.ToInt32(converter.GenerateValueInt(sheet.Cells[rowIndex, startCol + 14])),//TotalCone
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 15]),//ThreadNo
                                    Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 16])),//Length
                                    "MT",//uom
                                    Convert.ToDateTime(converter.GeneratePureTime(sheet.Cells[rowIndex, startCol + 17])),//start
                                    Convert.ToDateTime(converter.GeneratePureTime(sheet.Cells[rowIndex, startCol + 18])),//Doff
                                    Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 19])),//HNLeft
                                    Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 20])),//HNMiddle
                                    Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 21])),//HNRight
                                    Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 22])),//Speed
                                    Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 23])),//ThreadCut
                                    Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 24])),//Capacity
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 25])//Eff
                                    );
                                    await _repository.Update(data);
                                    saved = 1;
                                    
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
                if (isSave > 0 && saved == 0)
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
                throw new Exception($"ERROR \n" + error + "\n");
            }

        }

        public async Task<IEnumerable<WeavingDailyOperationWarpingMachineDto>> GetAll()
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .Select(y =>
                             new WeavingDailyOperationWarpingMachineDto
                             {
                                 MCNo = y.MCNo,
                                 YearPeriode = y.YearPeriode,
                                 Group = y.Group,
                                 Name = y.Name,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy")
                             });

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
                              Periode = new DateTime(Convert.ToInt32(a.YearPeriode), a.MonthId, a.Date)
                          };
            var query = (from a in allData
                        where 
                        ((mcNo == null || (mcNo != null && mcNo != "" && a.mcNo == mcNo)) &&
                        (shift == null || (shift != null && shift != "" && a.shift == shift)) &&
                        (sp == null || (sp != null && sp != "" && a.sp.Contains(sp))) &&
                        (threadNo == null || (threadNo != null && threadNo != "" && a.threadNo.Contains(threadNo))) &&
                        (code == null || (code != null && code != "" && a.code.Contains( code))) &&
                        (a.Periode.Date  >= fromDate.Date && a.Periode.Date<= toDate.Date))
                        select new { Name = a.name , ThreadCut  = a.threadCut, Length  =a.length}).GroupBy(l => l.Name)
                            .Select(cl => new
                            {
                                Name = cl.First().Name,
                                ThreadCut = cl.Sum(c => c.ThreadCut),
                                Length = cl.Sum(c => c.Length),
                            }).ToList(); ;
            List<WeavingDailyOperationWarpingMachineDto> list = new List<WeavingDailyOperationWarpingMachineDto>();
            foreach (var item in query)
            {
                WeavingDailyOperationWarpingMachineDto dto = new WeavingDailyOperationWarpingMachineDto
                {
                    ThreadCut = item.ThreadCut,
                    Length = item.Length,
                    Name = item.Name
                };
                list.Add(dto);
            }

            return list;

        }
    }
}
