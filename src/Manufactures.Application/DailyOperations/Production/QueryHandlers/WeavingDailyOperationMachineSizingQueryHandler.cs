using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Production.DataTransferObjects;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Productions.ReadModels;
using Manufactures.Domain.DailyOperations.WeavingDailyOperationMachineSizing.Queries;
using Manufactures.Domain.DailyOperations.WeavingDailyOperationMachineSizing.Repositories;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Production.QueryHandlers
{
    public class WeavingDailyOperationMachineSizingQueryHandler : IWeavingDailyOperationMachineSizingQuery<WeavingDailyOperationMachineSizingDto>
    {
        ConverterChecker converter = new ConverterChecker();
        GeneralHelper general = new GeneralHelper();
        private readonly IStorage _storage;
        private readonly IWeavingDailyOperationMachineSizingRepository _repository;

        public WeavingDailyOperationMachineSizingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {

            _storage =
                storage;

            _repository =
                _storage.GetRepository<IWeavingDailyOperationMachineSizingRepository>();
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
            // set the connection to instance of SqlCommand  
            sql = @"delete WeavingEstimatedProductions where [month]= '" + month + "' and yearperiode='" + year + "'";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();

            conn.Close();

        }

        public async Task<IEnumerable<WeavingDailyOperationMachineSizingDto>> GetAll()
        {
            var query = (_repository
                            .Query.OrderByDescending(s=>s.CreatedDate)
                            .Select(y =>
                             new WeavingDailyOperationMachineSizingDto
                             {
                                 Month = y.Periode,
                                 YearPeriode = y.Year,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy")
                             })).ToList();

            await Task.Yield();

            return query.Distinct();
        }

        public async Task<WeavingDailyOperationMachineSizingDto> GetById(Guid id)
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .Where(s => s.Identity == id)
                            .Select(y =>
                             new WeavingDailyOperationMachineSizingDto
                             {
                                 Month = y.Periode,
                                 YearPeriode = y.Year,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy")
                             }).FirstOrDefault();



            return query;
        }

 

        public async Task<bool> Upload(ExcelWorksheets sheets, string month, string year, int monthId)
        {

            var startRow = 5;
            var startCol = 1;
            WeavingDailyOperationMachineSizings data;
            int rowIndex = 0;
            var totalRows = 0;
            int isSave = 0;
            int saved = 0;
            string error = "";
            var s = sheets.Where(x => x.Name.Trim() == "MASTER");
            foreach (var sheet in s)
            {

                
                //if (!sheet.Name.Trim().Contains("MASTER"))
                //{
                //   error = "Tidak terdapat sheet MASTER SP di File Excel";
                //}
                //else
                //{
                    //totalRows = sheet.Dimension.Rows;
                    //var totalColumns = sheet.Dimension.Columns;
                    if (sheet.Name.Trim() == "MASTER")
                    {

                    totalRows = sheet.Dimension.Rows;
                    var totalColumns = sheet.Dimension.Columns;
                    try
                        {
                            for (rowIndex = startRow; rowIndex <= totalRows; rowIndex++)
                            {

                                if (sheet.Cells[rowIndex, startCol].Value != null || sheet.Cells[rowIndex, startCol].Value != "")
                                {
                                    // var tempp = converter.GenerateValueInt(sheet.Cells[rowIndex, startCol + 20]);
                                    // if (converter.GenerateValueInt(sheet.Cells[rowIndex, startCol + 20]) == year && converter.GenerateValueInt(sheet.Cells[rowIndex, startCol + 19]) == monthId)
                                    //{
                                    var col1 = Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value);
                                        data = new WeavingDailyOperationMachineSizings(
                                        DateTimeOffset.MinValue,
                                        DateTimeOffset.MinValue,

                                      Guid.NewGuid(), //
                                      monthId,
                                      month,
                                      year,

                                     Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value),
                                     //Convert.ToInt32(sheet.Cells[rowIndex, startCol]),
                                     //Convert.ToInt32(sheet.Cells[rowIndex, startCol + 15].Value), //col 16,tgl
                                     // Convert.ToInt32(sheet.Cells[rowIndex, startCol+1]),// col 1,excel col =  tgl
                                     converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 1]),
                                     converter.GenerateValueString(sheet.Cells[rowIndex, startCol +2]),// col 1,excel col =  tgl
                                   
                                      // Convert.ToInt32(sheet.Cells[rowIndex, startCol+3]),// col 1,excel col =  tgl
                                    
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 3]),//col 3,excel col =  mesin sizing
                                     converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 4]),//shift
                                     converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 5]),//LOT
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 6]),//SP
                                     converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 7]),//WarpType
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 8]),//weftType1
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 9]),//WeftType2
                                     converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 10]),//AL
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 11]),//AP1
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 12]),//AP2
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 13]),//Thread
                                     converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 14]),//Construction
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 15]),//Buyer

                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 16]),//NumberOfOrder

                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 17]),//Construction2
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 18]),//weftXWarp
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 19]),//GradeA
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 20]),//GradeB
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 21]),//GradeC
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 22]),//Aval
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 23]),//WarpBale
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 24]),//weftBale
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 25]),
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 26]),//TotalBale
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
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 46]),
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 47]),
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 48]),
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 49]),
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 50])




                                       );
                                        await _repository.Update(data);
                                        saved = 1;
                            }
                            else
                            {
                                isSave = 1;
                            }

                            //}

                        }
                        }
                        catch (Exception ex)
                        {
                            error = "Cek pada baris ke-{rowIndex} - {ex.Message}";
                            throw new Exception($"Gagal memproses Sheet  pada baris ke-{rowIndex} - {ex.Message}");
                        }
                    }

               // }
                 

            }
            try
            {
                if(isSave > 0 && saved == 0)
                {
                    error = "Tahun dan bulan tidak sesuai";
                    throw new Exception($"Tahun dan Bulan tidak sesuai");

                }else if(error != "" && saved ==0)
                {
                    throw new Exception($"ERROR "+ error);
                }
                else
                {
                    //await Delete(month, year.ToString());
                    _storage.Save();
                    return ((rowIndex - 1) == totalRows && totalRows > 0);
                }
                

            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR \n" + error + "\n");
            }

        }
        public List<WeavingDailyOperationMachineSizingDto>   GetDataByFilter(string month, string yearPeriode)
        {
            var query = _repository
                           .Query
                           .Where(s => s.Periode == month && s.Year == yearPeriode)
                           .Select(y =>
                            new WeavingDailyOperationMachineSizingDto
                            {
                               // SPNo = y.SPNo,
                                Date = y.Date,
                                //Construction1 = y.Construction1,
                                //Thread = y.Thread,
                                //GradeA = y.GradeA,
                                //GradeB = y.GradeB,
                                //GradeC = y.GradeC,
                                //Aval = y.Aval,
                                //Total= y.Total,
                                //NumberOrder= y.NumberOrder,
                                //WarpBale= y.WarpBale,
                                //WeftBale = y.WeftBale,
                                //TotalBale = y.TotalBale,
                                //WarpXWeft= y.WarpXWeft

                            }).OrderBy(s => s.Date);
            List<WeavingDailyOperationMachineSizingDto> listData = new List<WeavingDailyOperationMachineSizingDto>();

            foreach (var  item in query)
            {
                WeavingDailyOperationMachineSizingDto weavings = new WeavingDailyOperationMachineSizingDto
                {
                    SPNo = item.SPNo,
                    Date = item.Date,
                    Construction1 = item.Construction1,
                    Thread = item.Thread,
                    GradeA = item.GradeA,
                    GradeB = item.GradeB,
                    GradeC = item.GradeC,
                    Aval = item.Aval,
                    Total = item.Total,
                    WarpXWeft = item.WarpXWeft,
                    NumberOrder = item.NumberOrder,
                    WarpBale = item.WarpBale,
                    WeftBale = item.WeftBale,
                    TotalBale = item.TotalBale
                };
                listData.Add(weavings);

            }
            return listData;
        }

    }
}
