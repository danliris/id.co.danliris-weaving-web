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
            sql = @"delete WeavingDailyOperationMachineSizing where [periode]= '" + month + "' and year='" + year + "'";
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
            //var s = sheets.Where(x => x.Name.Trim() == "MASTER");
             
            foreach (var sheet in sheets)
            {
                // if (!sheet.Name.Trim().Contains("SOURCE SP"))
                //{
                //   error = "Tidak terdapat sheet SOURCE SP di File Excel";
                //}

                // if (sheet == null)
                //{
                //  error = "Tidak terdapat sheet MASTER SP di File Excel";

                // throw new Exception($"Tidak terdapat sheet MASTER SP di File Excel");
                //}
                //else
                //{
                //totalRows = sheet.Dimension.Rows;
                //var totalColumns = sheet.Dimension.Columns;

                if (!sheet.Name.Trim().Contains("MASTER"))
                {
                    error = "Tidak terdapat sheet MASTER di File Excel";
                }
               // if (sheet.Name.Trim() == "MASTER")
                 //   {
                    else {
                

                    totalRows = sheet.Dimension.Rows;
                    var totalColumns = sheet.Dimension.Columns;

                if (sheet.Name.Trim() == "MASTER")
                {
                    try
                        {
                            for (rowIndex = startRow; rowIndex <= totalRows; rowIndex++)
                            {

                                if (sheet.Cells[rowIndex, startCol].Value != null || sheet.Cells[rowIndex, startCol].Value != "")
                                {

                                    //hrs nya di tulis disini utk kondisi jk baris tgl >28 utk bulan feb
                                    if (Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value) <= DateTime.DaysInMonth(Convert.ToInt32(year), monthId))
                                    {
                                        //------------------
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
                                     converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 2]),// col 1,excel col =  tgl

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
                                        //tmbhn baru
                                       // error = ($"Gagal memproses Sheet  pada baris ke-{rowIndex} - bulan {month} hanya sampai tanggal {DateTime.DaysInMonth(Convert.ToInt32(year), monthId)}");
                                        saved = 0;
                                        break;
                                    }
                                    //kondisi else dr tgl >28 tulis disini
                                }
                                else
                                {
                                    //error = ($"Gagal memproses Sheet  pada baris ke-{rowIndex} - bulan {month} hanya sampai tanggal {DateTime.DaysInMonth(Convert.ToInt32(year), monthId)}");
                                    saved = 0;
                                    break;
                                }


                                //----------------

                            

                            }
                        }
                        catch (Exception ex)
                        {
                            error = "Cek pada baris ke-{rowIndex} - {ex.Message}";
                            throw new Exception($"Gagal memproses Sheet  pada baris ke-{rowIndex} - {ex.Message}");
                        }

                    //hrsnya disini    
                    await Delete(month, year.ToString());
                    _storage.Save();

                    
                 }
                    //else
                    //{
                    //    throw new Exception($"Sheet MASTER tidak ditemukan !!");

                    //}

                 }


            }
            try
            {
                if(isSave > 0 && saved == 0)
                {
                    error = "Gagal memproses Sheet  pada baris ke- " + rowIndex + "- bulan " + month + " hanya sampai tanggal " + DateTime.DaysInMonth(Convert.ToInt32(year), monthId);
                    throw new Exception($"aaaGagal memproses Sheet  pada baris ke-{rowIndex} - bulan {month} hanya sampai tanggal {DateTime.DaysInMonth(Convert.ToInt32(year), monthId)}");

                }else if(error != "" && saved ==0)
                {
                    throw new Exception($"ERROR "+ error);
                }

               

                // else
                //{
                // await Delete(month, year.ToString());
                //_storage.Save();
                // return ((rowIndex - 1) == totalRows && totalRows > 0);
                // }

                return ((rowIndex - 1) == totalRows && totalRows > 0);
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
                               
                                Date = y.Date,
                                Week = y.Week,
                                MachineSizing = y.MachineSizing,
                                Shift = y.Shift,
                                Group = y.Group,
                                Lot = y.Lot,
                                SP = y.SP,
                                YearProduction = y.YearProduction,
                                SPYear = y.SPYear,
                                WarpType = y.WarpType,
                                AL = y.AL,
                                Construction = y.Construction,
                                Code = y.Code,
                                ThreadOrigin = y.ThreadOrigin,
                                Recipe = y.Recipe,
                                Water = y.Water,
                                BeamNo = y.BeamNo,
                                BeamWidth = y.BeamWidth,
                                TekSQ = y.TekSQ,
                                ThreadCount = y.ThreadCount,
                                Ne = y.Ne,
                                TempSD1 = y.TempSD1,
                                TempSD2 = y.TempSD2,
                                VisCoseSD1 = y.VisCoseSD1,
                                VisCoseSD2 = y.VisCoseSD2,
                                F1 = y.F1,
                                F2 = y.F2,
                                FDS = y.FDS,
                                FW = y.FW,
                                FP = y.FP,
                                A12 = y.A12,
                                A34 = y.A34,
                                B12 = y.B12,
                                B34 = y.B34,
                                C1234 = y.C1234,
                                Pis = y.Pis,
                                AddedLength = y.AddedLength,
                                Length = y.Length,
                                EmptyBeamWeight = y.EmptyBeamWeight,
                                Bruto = y.Bruto,
                                Netto = y.Netto,
                                Teoritis = y.Teoritis,
                                SPU = y.SPU,
                                WarpingLenght = y.WarpingLenght,
                                FinalCounter = y.FinalCounter,
                                Draft = y.Draft,
                                Speed = y.Speed,
                                Information = y.Information,
                                SpeedMin = y.SpeedMin,
                                Capacity = y.Capacity,
                                Efficiency = y.Efficiency


                            }).OrderBy(s => s.Date)
                              .ThenBy(a => a.Shift)
                              .ThenBy(a => a.MachineSizing);
            List<WeavingDailyOperationMachineSizingDto> listData = new List<WeavingDailyOperationMachineSizingDto>();

            foreach (var  item in query)
            {
                WeavingDailyOperationMachineSizingDto weavings = new WeavingDailyOperationMachineSizingDto
                {
                    

                    Date = item.Date,
                    Week = item.Week,
                    MachineSizing = item.MachineSizing,
                    Shift = item.Shift,
                    Group = item.Group,
                    Lot = item.Lot,
                    SP = item.SP,
                    YearProduction = item.YearProduction,
                    SPYear = item.SPYear,
                    WarpType = item.WarpType,
                    AL = item.AL,
                    Construction = item.Construction,
                    Code = item.Code,
                    ThreadOrigin = item.ThreadOrigin,
                    Recipe = item.Recipe,
                    Water = item.Water,
                    BeamNo = item.BeamNo,
                    BeamWidth = item.BeamWidth,
                    TekSQ = item.TekSQ,
                    ThreadCount = item.ThreadCount,
                    Ne = item.Ne,
                    TempSD1 = item.TempSD1,
                    TempSD2 = item.TempSD2,
                    VisCoseSD1 = item.VisCoseSD1,
                    VisCoseSD2 = item.VisCoseSD2,
                    F1 = item.F1,
                    F2 = item.F2,
                    FDS = item.FDS,
                    FW = item.FW,
                    FP = item.FP,
                    A12 = item.A12,
                    A34 = item.A34,
                    B12 = item.B12,
                    B34 = item.B34,
                    C1234 = item.C1234,
                    Pis = item.Pis,
                    AddedLength = item.AddedLength,
                    Length = item.Length,
                    EmptyBeamWeight = item.EmptyBeamWeight,
                    Bruto = item.Bruto,
                    Netto = item.Netto,
                    Teoritis = item.Teoritis,
                    SPU = item.SPU,
                    WarpingLenght = item.WarpingLenght,
                    FinalCounter = item.FinalCounter,
                    Draft = item.Draft,
                    Speed = item.Speed,
                    Information = item.Information,
                    SpeedMin = item.SpeedMin,
                    Capacity = item.Capacity,
                    Efficiency = item.Efficiency
                };
                listData.Add(weavings);

            }
            return listData;
        }

    }
}
