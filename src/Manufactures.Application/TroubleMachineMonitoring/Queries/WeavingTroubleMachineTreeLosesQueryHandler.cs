using ExtCore.Data.Abstractions;
using Manufactures.Application.Helpers;
using Manufactures.Application.TroubleMachineMonitoring.DTOs;
using Manufactures.Domain.TroubleMachineMonitoring.Entities;
using Manufactures.Domain.TroubleMachineMonitoring.Queries;
using Manufactures.Domain.TroubleMachineMonitoring.Repositories;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.TroubleMachineMonitoring.Queries
{
    public class WeavingTroubleMachineTreeLosesQueryHandler : IWeavingTroubleMachineTreeLosesQuery<WeavingTroubleMachingTreeLosesDto>
    {
        ConverterChecker converter = new ConverterChecker();
        GeneralHelper general = new GeneralHelper();
        private readonly IStorage _storage;
        private readonly IWeavingTroubleMachineTreeLosesRepository _repository;
        public WeavingTroubleMachineTreeLosesQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {

            _storage =
                storage;

            _repository =
                _storage.GetRepository<IWeavingTroubleMachineTreeLosesRepository>();
        }
        public async Task<bool> Upload(ExcelWorksheets sheets, string month, int year, int monthId)
        {

            var startRow = 4;
            var startCol = 1;
            WeavingTroubleMachineTreeLoses data;
            int rowIndex = 0;
            var totalRows = 0;
            string error = "";
            int isSave = 0;
            int saved = 0;
            foreach (var sheet in sheets)
            {
                if (!sheet.Name.Trim().Contains("Tree Loses"))
                {
                    error = "Tidak terdapat sheet Tree Loses di File Excel";
                }
                else
                {

                    if (sheet.Name.Trim() == "Tree Loses")
                    {
                        try
                        {
                            totalRows = sheet.Dimension.Rows;
                            var totalColumns = sheet.Dimension.Columns;
                            for (rowIndex = startRow; rowIndex <= totalRows; rowIndex++)
                            {

                                if (sheet.Cells[rowIndex, startCol].Value != null || Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value) != 0)
                                {

                                    data = new WeavingTroubleMachineTreeLoses(
                                    Guid.NewGuid(), //
                                    Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value), //tgl
                                    month,
                                    monthId,
                                    year.ToString(),//year
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 4]),//shift
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 10]),//description
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 2]),//warpingMachineNo
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 3]),//Group
                                    converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 9]),//Code

                                    Convert.ToDateTime(converter.GeneratePureTime(sheet.Cells[rowIndex, startCol + 7])),//DownTImeMC
                                    Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 8])),//TimePerMinute
                                    Convert.ToDateTime(converter.GeneratePureTime(sheet.Cells[rowIndex, startCol + 5])),//Start
                                    Convert.ToDateTime(converter.GeneratePureTime(sheet.Cells[rowIndex, startCol + 6])),//Finish
                                    Convert.ToInt32(converter.GenerateValueInt(sheet.Cells[rowIndex, startCol + 1]))//Week

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
                throw new Exception($"ERROR \n" + error +"\n");
            }

        }
        public async Task Delete(string month, string year)
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();
            var myConnectionString1 = _configuration.GetConnectionString("Default");
            SqlConnection conn = new SqlConnection(myConnectionString1);
           {
                conn.Open();
                SqlCommand cmd = new SqlCommand();// Creating instance of SqlCommand  

                String sql = "";
                // set the connection to instance of SqlCommand  
                sql = @"delete WeavingTroubleMachineTreeLoses where [month]= '" + month + "' and yearperiode='" + year + "'";
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                conn.Close();

            }
            
        }

         

        public async Task<IEnumerable<WeavingTroubleMachingTreeLosesDto>> GetAll()
        {
            var query = _repository
                           .Query.OrderByDescending(o => o.CreatedDate)
                           .Select(y =>
                            new WeavingTroubleMachingTreeLosesDto
                            {
                                Month = y.Month,
                                YearPeriode = y.YearPeriode,
                                Group = y.Group,
                                CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy")
                            });

            await Task.Yield();

            return query;
        }

        public async Task<WeavingTroubleMachingTreeLosesDto> GetById(Guid id)
        {
            var query = _repository
                           .Query.OrderByDescending(o => o.CreatedDate)
                           .Where(s => s.Identity == id)
                           .Select(y =>
                            new WeavingTroubleMachingTreeLosesDto
                            {
                                Month = y.Month,
                                YearPeriode = y.YearPeriode,
                                Group = y.Group,
                                CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy")
                            }).FirstOrDefault();



            return query;
        }
        public List<WeavingTroubleMachingTreeLosesDto> GetDataByFilter(string month, string yearPeriode)
        {
            var query = _repository
                           .Query
                           .Where(s => s.Month == month && s.YearPeriode == yearPeriode)
                           .Select(y =>
                            new WeavingTroubleMachingTreeLosesDto
                            {
                                Code= y.Code,
                                CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy"),
                                Date= y.Date,
                                Description = y.Description,
                                DownTimeMC=  y.DownTimeMC.ToShortTimeString(),
                                Group=  y.Group,
                                Shift = y.Shift,
                                Start = y.Start.ToShortTimeString(),
                                Finish = y.Finish.ToShortTimeString(),
                                TimePerMinutes = Math.Round( y.TimePerMinutes),
                                WarpingMachineNo = y.WarpingMachineNo,
                                Week=y.Week
                            }).OrderBy(s=>s.Date);
            List<WeavingTroubleMachingTreeLosesDto> listData = new List<WeavingTroubleMachingTreeLosesDto>();

            foreach (var item in query)
            {
                WeavingTroubleMachingTreeLosesDto weavings = new WeavingTroubleMachingTreeLosesDto
                {
                    Code = item.Code,
                    CreatedDate = item.CreatedDate,
                    Date = item.Date,
                    Description = item.Description,
                    DownTimeMC = item.DownTimeMC,
                    Group = item.Group,
                    Shift = item.Shift,
                    Start = item.Start,
                    Finish = item.Finish,
                    TimePerMinutes = item.TimePerMinutes,
                    WarpingMachineNo = item.WarpingMachineNo,
                    Week=item.Week
                };
                listData.Add(weavings);

            }
            return listData;
        }
    }
}
 
