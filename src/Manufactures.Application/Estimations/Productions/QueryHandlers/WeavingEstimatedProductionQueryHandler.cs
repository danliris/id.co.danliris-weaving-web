using ExtCore.Data.Abstractions;
using Manufactures.Application.Estimations.Productions.DataTransferObjects;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Estimations.WeavingEstimationProductions.Queries;
using Manufactures.Domain.Estimations.WeavingEstimationProductions.Repositories;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.Estimations.Productions.QueryHandlers
{
    public class WeavingEstimatedProductionQueryHandler : IWeavingEstimatedProductionQuery<WeavingEstimatedProductionDto>
    {
        ConverterChecker converter = new ConverterChecker();
        GeneralHelper general = new GeneralHelper();
        private readonly IStorage _storage;
        private readonly IWeavingEstimatedProductionRepository _repository;

        public WeavingEstimatedProductionQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {

            _storage =
                storage;

            _repository =
                _storage.GetRepository<IWeavingEstimatedProductionRepository>();
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
       
        public async Task<IEnumerable<WeavingEstimatedProductionDto>> GetAll()
        {
            var query = (_repository
                            .Query.OrderByDescending(s=>s.CreatedDate)
                            .Select(y =>
                             new WeavingEstimatedProductionDto
                             {
                                 Month = y.Month,
                                 YearPeriode = y.YearPeriode,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy")
                             })).ToList();

            await Task.Yield();

            return query.Distinct();
        }

        public async Task<WeavingEstimatedProductionDto> GetById(Guid id)
        {
            var query = _repository
                            .Query.OrderByDescending(o => o.CreatedDate)
                            .Where(s => s.Identity == id)
                            .Select(y =>
                             new WeavingEstimatedProductionDto
                             {
                                 Month = y.Month,
                                 YearPeriode = y.YearPeriode,
                                 CreatedDate = y.CreatedDate.ToString("dd-MM-yyyy")
                             }).FirstOrDefault();



            return query;
        }

 

        public async Task<bool> Upload(ExcelWorksheets sheets, string month, int year, int monthId)
        {

            var startRow = 2;
            var startCol = 1;
            WeavingEstimatedProduction data;
            int rowIndex = 0;
            var totalRows = 0;
            int isSave = 0;
            int saved = 0;
            string error = "";
            foreach (var sheet in sheets)
            {

                
                if (!sheet.Name.Trim().Contains("SOURCE SP"))
                {
                    error = "Tidak terdapat sheet SOURCE SP di File Excel";
                }
                else
                {
                    totalRows = sheet.Dimension.Rows;
                    var totalColumns = sheet.Dimension.Columns;
                    if (sheet.Name.Trim() == "SOURCE SP")
                    {
                        try
                        {
                            for (rowIndex = startRow; rowIndex <= totalRows; rowIndex++)
                            {

                                if (sheet.Cells[rowIndex, startCol].Value != null || sheet.Cells[rowIndex, startCol].Value != "")
                                {
                                    var tempp = converter.GenerateValueInt(sheet.Cells[rowIndex, startCol + 20]);
                                    if (converter.GenerateValueInt(sheet.Cells[rowIndex, startCol + 20]) == year && converter.GenerateValueInt(sheet.Cells[rowIndex, startCol + 19]) == monthId)
                                    {
                                        data = new WeavingEstimatedProduction(
                                      Guid.NewGuid(), //
                                      Convert.ToInt32(sheet.Cells[rowIndex, startCol + 15].Value), //tgl
                                      month,
                                      monthId,
                                      year.ToString(),//year
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol]),//yearSP
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 1]),//SPNO
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 2]),//Plait
                                      Convert.ToDouble(converter.GeneratePureDouble(sheet.Cells[rowIndex, startCol + 3])),//warpLength
                                      Convert.ToDouble(converter.GeneratePureDouble(sheet.Cells[rowIndex, startCol + 4])),//Weft
                                      Convert.ToDouble(converter.GeneratePureDouble(sheet.Cells[rowIndex, startCol + 5])),//Width
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 6]),//WarpType
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 7]),//weftType1
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 8]),//WeftType2
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 9]),//AL
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 10]),//AP1
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 11]),//AP2
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 12]),//Thread
                                      converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 13]),//Construction
                                      converter.GeneratePureString(sheet.Cells[rowIndex, startCol + 14]),//Buyer

                                      Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 16])),//NumberOfOrder

                                      converter.GeneratePureString(sheet.Cells[rowIndex, startCol + 17]),//Construction2
                                      converter.GeneratePureString(sheet.Cells[rowIndex, startCol + 18]),//weftXWarp
                                      Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 21])),//GradeA
                                      Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 22])),//GradeB
                                      Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 23])),//GradeC
                                      Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 24])),//Aval
                                      Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 25])),//WarpBale
                                      Convert.ToDouble(converter.GenerateValueDouble(sheet.Cells[rowIndex, startCol + 26])),//weftBale
                                      Convert.ToDouble(converter.GenerateValueDouble((sheet.Cells[rowIndex, startCol + 27]))),
                                       Convert.ToDouble(converter.GenerateValueDouble((sheet.Cells[rowIndex, startCol + 28])))//TotalBale
                                      );
                                        await _repository.Update(data);
                                        saved = 1;
                                    }
                                    else
                                    {
                                        isSave = 1;
                                    }

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            error = "Cek pada baris ke-{rowIndex} - {ex.Message}";
                            throw new Exception($"Gagal memproses Sheet  pada baris ke-{rowIndex} - {ex.Message}");
                        }
                    }

                }
                 

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
                }else
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
        public List<WeavingEstimatedProductionDto>   GetDataByFilter(string month, string yearPeriode)
        {
            var query = _repository
                           .Query
                           .Where(s => s.Month == month && s.YearPeriode == yearPeriode)
                           .Select(y =>
                            new WeavingEstimatedProductionDto
                            {
                                SPNo = y.SPNo,
                                Date = y.Date,
                                Construction1 = y.Construction1,
                                Thread = y.Thread,
                                GradeA = y.GradeA,
                                GradeB = y.GradeB,
                                GradeC = y.GradeC,
                                Aval = y.Aval,
                                Total= y.Total,
                                NumberOrder= y.NumberOrder,
                                WarpBale= y.WarpBale,
                                WeftBale = y.WeftBale,
                                TotalBale = y.TotalBale,
                                WarpXWeft= y.WarpXWeft

                            }).OrderByDescending(s => s.CreatedDate);
            List<WeavingEstimatedProductionDto> listData = new List<WeavingEstimatedProductionDto>();

            foreach (var  item in query)
            {
                WeavingEstimatedProductionDto weavings = new WeavingEstimatedProductionDto
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
