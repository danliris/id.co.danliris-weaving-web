using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport;
using Manufactures.Domain.BrokenCauses.Warping.Repositories;
using Manufactures.Domain.DailyOperations.Warping.Queries.WarpingBrokenThreadsReport;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Suppliers.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static Manufactures.Application.Helpers.BrokenWarpingConstants;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers.WarpingBrokenThreadsReport
{
    public class WarpingBrokenReportQueryHandler : IWarpingBrokenThreadsReportQuery<WarpingBrokenThreadsReportListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IDailyOperationWarpingBeamProductRepository
            _dailyOperationWarpingBeamProductRepository;
        private readonly IDailyOperationWarpingBrokenCauseRepository
            _dailyOperationWarpingBrokenCauseRepository;
        private readonly IOrderRepository
            _orderRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IConstructionYarnDetailRepository
            _constructionYarnDetailRepository;
        private readonly IWarpingBrokenCauseRepository
            _warpingBrokenCauseRepository;
        private readonly IYarnDocumentRepository
            _yarnRepository;
        private readonly IWeavingSupplierRepository
            _supplierRepository;

        public WarpingBrokenReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _yarnRepository =
                _storage.GetRepository<IYarnDocumentRepository>();
            _constructionYarnDetailRepository =
                _storage.GetRepository<IConstructionYarnDetailRepository>();
            _supplierRepository =
                _storage.GetRepository<IWeavingSupplierRepository>();
            _orderRepository =
                _storage.GetRepository<IOrderRepository>();
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _dailyOperationWarpingBeamProductRepository =
                _storage.GetRepository<IDailyOperationWarpingBeamProductRepository>();
            _dailyOperationWarpingBrokenCauseRepository =
                _storage.GetRepository<IDailyOperationWarpingBrokenCauseRepository>();
            _warpingBrokenCauseRepository =
                _storage.GetRepository<IWarpingBrokenCauseRepository>();

            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
        }

        private SingleUnitResult GetUnit(int id)
        {
            var masterUnitUri = MasterDataSettings.Endpoint + $"master/units/{id}";
            var unitResponse = _http.GetAsync(masterUnitUri).Result;

            if (unitResponse.IsSuccessStatusCode)
            {
                SingleUnitResult unitResult = JsonConvert.DeserializeObject<SingleUnitResult>(unitResponse.Content.ReadAsStringAsync().Result);
                return unitResult;
            }
            else
            {
                return new SingleUnitResult();
            }
        }

        private SingleUomResult GetUom(int id)
        {
            var masterUomUri = MasterDataSettings.Endpoint + $"master/uoms/{id}";
            var uomResponse = _http.GetAsync(masterUomUri).Result;

            if (uomResponse.IsSuccessStatusCode)
            {
                SingleUomResult uomResult = JsonConvert.DeserializeObject<SingleUomResult>(uomResponse.Content.ReadAsStringAsync().Result);
                return uomResult;
            }
            else
            {
                return new SingleUomResult();
            }
        }

        public WarpingBrokenThreadsReportListDto GetReports(int month, int year, int weavingUnitId)
        {
            try
            {
                //Add Shell (result) for Daily Operation Warping Broken Report Dto
                var result = new WarpingBrokenThreadsReportListDto();

                if (month == 0)
                {
                    return result;
                }

                //Preparing Order Query
                var orderQuery =
                    _orderRepository
                        .Query
                        .AsQueryable();

                //Validation for weavingId from UI, if exist parse to weavingGuid, else return empty result
                if (weavingUnitId != 0)
                {
                    orderQuery = orderQuery.Where(o => o.UnitId.Equals(weavingUnitId));
                }
                else
                {
                    return result;
                }

                //Create DateTime from Params (month and year)
                var dateTime = new DateTime(year, month, 1);

                //Get MonthName and LastMonthName
                var monthName = dateTime.ToString("MMMM", CultureInfo.CreateSpecificCulture("id-ID"));
                var lastMonthName = "";
                if (dateTime.AddMonths(-1).Year != dateTime.Year)
                {
                    lastMonthName = dateTime.ToString("MMMM", CultureInfo.CreateSpecificCulture("id-ID"));
                }
                else
                {
                    lastMonthName = dateTime.AddMonths(-1).ToString("MMMM", CultureInfo.CreateSpecificCulture("id-ID"));
                }

                //Get Unit Name from Master
                SingleUnitResult unitData = GetUnit(weavingUnitId);
                var weavingUnitName = unitData.data.Name;

                //Get ReportQueryDto from Join Result using LINQ
                var reportQueries = GetWarpingBrokenReportQueries(year, weavingUnitId);
                if (reportQueries == null)
                {
                    return result;
                }
                var filteredResult = reportQueries.Where(reportQuery => reportQuery.DateTimeProductBeam.HasValue && reportQuery.DateTimeProductBeam.Value.Month == month);

                // Get Broken Causes
                var brokenCauses = _warpingBrokenCauseRepository
                    .Query
                    .Select(brokenCause => new
                    {
                        brokenCause.Identity,
                        brokenCause.WarpingBrokenCauseName
                    });

                var tempResult = new List<Dictionary<string, string>>();
                foreach (var element in filteredResult)
                {
                    var existingDictionary = tempResult.FirstOrDefault(dictionary => dictionary["SupplierName"] == element.SupplierName && dictionary["YarnName"] == element.YarnName);

                    if (existingDictionary == null)
                    {
                        var dictionary = new Dictionary<string, string>
                        {
                            ["SupplierName"] = element.SupplierName,
                            ["YarnName"] = element.YarnName
                        };

                        foreach (var brokenCause in brokenCauses)
                        {
                            dictionary[brokenCause.WarpingBrokenCauseName] = "0";

                            dictionary[FooterBroken.Total.ToString()] = "0";
                            dictionary[FooterBroken.Max.ToString()] = "0";
                            dictionary[FooterBroken.Min.ToString()] = "0";
                            dictionary[FooterBroken.Average.ToString()] = "0";
                        }

                        dictionary[element.BrokenCauseName] = element.BrokenEachYarn.ToString();

                        dictionary[FooterBroken.Total.ToString()] = GetBrokenTotalAndAverage(element.BeamProductUomUnit, 
                                                                                                    element.BeamLength, 
                                                                                                    element.AmountOfCones, 
                                                                                                    month, 
                                                                                                    year,
                                                                                                    weavingUnitId).ToString();
                        dictionary[FooterBroken.Max.ToString()] = GetBrokenMax(element.BeamProductUomUnit, 
                                                                                      element.BeamLength,
                                                                                      element.AmountOfCones,
                                                                                      year,
                                                                                      weavingUnitId).ToString();
                        dictionary[FooterBroken.Min.ToString()] = GetBrokenMin(element.BeamProductUomUnit,
                                                                                      element.BeamLength,
                                                                                      element.AmountOfCones,
                                                                                      year,
                                                                                      weavingUnitId).ToString();
                        dictionary[FooterBroken.Average.ToString()] = GetBrokenTotalAndAverage(element.BeamProductUomUnit, 
                                                                                                      element.BeamLength, 
                                                                                                      element.AmountOfCones,

                                                                                                      element.DateTimeProductBeam.GetValueOrDefault().Year !=
                                                                                                      element.DateTimeProductBeam.GetValueOrDefault().AddMonths(-1).Year ?
                                                                                                      element.DateTimeProductBeam.GetValueOrDefault().Month :
                                                                                                      element.DateTimeProductBeam.GetValueOrDefault().AddMonths(-1).Month,

                                                                                                      element.DateTimeProductBeam.GetValueOrDefault().Year !=
                                                                                                      element.DateTimeProductBeam.GetValueOrDefault().AddMonths(-1).Year ?
                                                                                                      element.DateTimeProductBeam.GetValueOrDefault().Year :
                                                                                                      element.DateTimeProductBeam.GetValueOrDefault().AddMonths(-1).Year,
                                                                                                      
                                                                                                      weavingUnitId).ToString();
                        tempResult.Add(dictionary);
                    }
                    else
                    {
                        int.TryParse(existingDictionary[element.BrokenCauseName], out var totalBroken);
                        existingDictionary[element.BrokenCauseName] = (totalBroken + element.BrokenEachYarn).ToString();
                    }
                }

                var groupedResult = (from o in tempResult
                                     group o by o["SupplierName"] into groupedTemp
                                     //select groupedTemp
                                     select new WarpingBrokenThreadsGroupedItemsDto
                                     {
                                         SupplierName = groupedTemp.Key,
                                         ItemsValue = groupedTemp.ToList(),
                                         ItemsValueLength = groupedTemp.ToList().Count()
                                     }
                                    ).ToList();

                result.Month = monthName;
                result.Year = year.ToString();
                result.WeavingUnitName = weavingUnitName;
                result.LastMonth = lastMonthName;
                result.GroupedItems = groupedResult;
                result.TotalItems = tempResult.Count();

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private double CalculateWarpingBroken(string uomUnit, double totalWarpingBeamLength, int amountOfCones, int totalBroken)
        {
            double result = 0;
            double convertedResult = 0;

            if (uomUnit == "YARD")
            {
                result = CalculateWarpingBrokenYard(totalWarpingBeamLength, amountOfCones, totalBroken);
            }
            else if (uomUnit == "MTR")
            {
                result = CalculateWarpingBrokenMeter(totalWarpingBeamLength, amountOfCones, totalBroken);
            }
            else
            {
                result = 0;
            }

            convertedResult = Math.Round(result, 4);
            return convertedResult;
        }

        private double CalculateWarpingBrokenYard(double totalWarpingBeamLength, int amountOfCones, int totalBroken)
        {
            double result = 0;

            double lengthCones = totalWarpingBeamLength * amountOfCones;
            double constantPerLengthCones = BROKEN_WARPING_DEFAULT_CONST / lengthCones;
            result = constantPerLengthCones * totalBroken;

            return result;
        }

        private double CalculateWarpingBrokenMeter(double totalWarpingBeamLength, int amountOfCones, int totalBroken)
        {
            double result = 0;

            double lengthCones = totalWarpingBeamLength * amountOfCones;
            double constantMeter = 0.9144 * BROKEN_WARPING_DEFAULT_CONST;
            double constantMeterPerLengthCones = constantMeter / lengthCones;
            result = constantMeterPerLengthCones * totalBroken;

            return result;
        }

        private List<WarpingBrokenThreadsReportQueryDto> GetWarpingBrokenReportQueries(int year, int weavingUnitId)
        {
            try
            {
                var reportQueries = (from brokenCause in _warpingBrokenCauseRepository.Query

                                     join warpingBrokenCause in _dailyOperationWarpingBrokenCauseRepository.Query
                                                             on brokenCause.Identity equals warpingBrokenCause.BrokenCauseId
                                                             into joinWarpingBrokenCause
                                     from brokenCauseJoinWarpingBrokenCause in joinWarpingBrokenCause.DefaultIfEmpty()

                                     join warpingBeamProduct in _dailyOperationWarpingBeamProductRepository.Query.Where(o => o.LatestDateTimeBeamProduct.Year == year)
                                                             on brokenCauseJoinWarpingBrokenCause.DailyOperationWarpingBeamProductId equals warpingBeamProduct.Identity
                                                             into joinWarpingBeamProduct
                                     from brokenCauseJoinBeamProduct in joinWarpingBeamProduct.DefaultIfEmpty()

                                     join warpingDocument in _dailyOperationWarpingRepository.Query
                                                          on brokenCauseJoinBeamProduct.DailyOperationWarpingDocumentId equals warpingDocument.Identity
                                                          into joinWarpingDocument
                                     from beamProductJoinWarpingDocument in joinWarpingDocument.DefaultIfEmpty()

                                     join orderDocument in _orderRepository.Query.Where(o => o.UnitId == weavingUnitId)
                                                        on beamProductJoinWarpingDocument.OrderDocumentId equals orderDocument.Identity
                                                        into joinOrderDocument
                                     from warpingDocumentJoinOrder in joinOrderDocument.DefaultIfEmpty()

                                     join supplierDocument in _supplierRepository.Query
                                                           on warpingDocumentJoinOrder.WarpOriginIdOne equals supplierDocument.Identity
                                                           into joinSupplierDocument
                                     from orderDocumentJoinSupplier in joinSupplierDocument.DefaultIfEmpty()

                                     join constructionYarnDetail in _constructionYarnDetailRepository.Query.Where(o => o.Type == "Warp")
                                                                 on warpingDocumentJoinOrder.ConstructionDocumentId equals constructionYarnDetail.FabricConstructionDocumentId
                                                                 into joinConstructionYarnDetail
                                     from orderJoinConstructionYarnDetail in joinConstructionYarnDetail.DefaultIfEmpty()

                                     join yarnDocument in _yarnRepository.Query
                                                       on orderJoinConstructionYarnDetail.YarnId equals yarnDocument.Identity
                                                       into joinYarnDocument
                                     from constructionYarnDetailJoinYarn in joinYarnDocument.DefaultIfEmpty()

                                     select new
                                     {
                                         DateTimeProduceBeam = brokenCauseJoinBeamProduct.LatestDateTimeBeamProduct,
                                         WeavingUnitId = warpingDocumentJoinOrder.UnitId,
                                         SupplierName = orderDocumentJoinSupplier.Name,
                                         BrokenCauseName = brokenCause.WarpingBrokenCauseName,
                                         YarnName = constructionYarnDetailJoinYarn.Name,

                                         YarnId = constructionYarnDetailJoinYarn.YarnNumberId,
                                         BeamLength = brokenCauseJoinBeamProduct.WarpingTotalBeamLength,
                                         beamProductJoinWarpingDocument.AmountOfCones,
                                         BrokenEachYarn = brokenCauseJoinWarpingBrokenCause.TotalBroken,
                                         BeamProductUomId = brokenCauseJoinBeamProduct.WarpingTotalBeamLengthUomId,
                                         BeamProductUomUnit = brokenCauseJoinBeamProduct.WarpingTotalBeamLengthUomUnit

                                         //DateTimeProduceBeam = brokenCauseJoinBeamProduct != null ? brokenCauseJoinBeamProduct.LatestDateTimeBeamProduct : DateTimeOffset.UtcNow,
                                         //WeavingUnitId = warpingDocumentJoinOrder != null ? warpingDocumentJoinOrder.UnitId : 0,
                                         //SupplierName = orderDocumentJoinSupplier != null ? orderDocumentJoinSupplier.Name : "",
                                         //BrokenCauseName = brokenCause != null ? brokenCause.WarpingBrokenCauseName : "",
                                         //YarnName = constructionYarnDetailJoinYarn != null ? constructionYarnDetailJoinYarn.Name : "",

                                         //YarnId = constructionYarnDetailJoinYarn != null ? constructionYarnDetailJoinYarn.YarnNumberId.Value : Guid.Empty,
                                         //BeamLength = brokenCauseJoinBeamProduct != null ? brokenCauseJoinBeamProduct.WarpingTotalBeamLength : 0,
                                         //AmountOfCones = beamProductJoinWarpingDocument != null ? beamProductJoinWarpingDocument.AmountOfCones : 0,
                                         //BrokenEachYarn = brokenCauseJoinWarpingBrokenCause != null ? brokenCauseJoinWarpingBrokenCause.TotalBroken : 0,
                                         //BeamPerOperatorUom = brokenCauseJoinBeamProduct != null ? brokenCauseJoinBeamProduct.WarpingTotalBeamLengthUomId : 0
                                     })
                                     .ToList();

                var warpingBrokenReport = new List<WarpingBrokenThreadsReportQueryDto>();
                foreach (var report in reportQueries)
                {
                    var brokenReportDto = new WarpingBrokenThreadsReportQueryDto(report.DateTimeProduceBeam,
                                                                                 report.WeavingUnitId,
                                                                                 report.BrokenCauseName,
                                                                                 report.SupplierName,
                                                                                 report.YarnName,
                                                                                 report.YarnId,
                                                                                 report.BeamLength,
                                                                                 report.AmountOfCones,
                                                                                 report.BrokenEachYarn,
                                                                                 report.BeamProductUomId,
                                                                                 report.BeamProductUomUnit);
                    warpingBrokenReport.Add(brokenReportDto);
                }

                return warpingBrokenReport;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private double GetBrokenTotalAndAverage(string uomUnit, double totalWarpingBeamLength, int amountOfCones, int month, int year, int weavingId)
        {
            var reportQueries = GetWarpingBrokenReportQueries(year, weavingId);

            var totalBroken =
                reportQueries
                    .GroupBy(o => new { o.YarnId })
                    .Sum(s => s.Sum(sum => sum.BrokenEachYarn));

            var calculateTotal = CalculateWarpingBroken(uomUnit, totalWarpingBeamLength, amountOfCones, totalBroken);

            return calculateTotal;
        }

        private double GetBrokenMax(string uomUnit, double totalWarpingBeamLength, int amountOfCones, int year, int weavingId)
        {
            var reportQueries = GetWarpingBrokenReportQueries(year, weavingId);

            var filteredQueries =
                reportQueries
                    .Where(o => o.DateTimeProductBeam.GetValueOrDefault().Year == year && o.WeavingUnitId == weavingId)
                    .ToList();

            var maxBroken =
                filteredQueries
                    .GroupBy(o => new { o.YarnId })
                    .Sum(s => s.Max(max => max.BrokenEachYarn));

            var calculateMax = CalculateWarpingBroken(uomUnit, totalWarpingBeamLength, amountOfCones, maxBroken);

            return calculateMax;
        }

        private double GetBrokenMin(string uomUnit, double totalWarpingBeamLength, int amountOfCones, int year, int weavingId)
        {
            var reportQueries = GetWarpingBrokenReportQueries(year, weavingId);

            var filteredQueries =
                reportQueries
                    .Where(o => o.DateTimeProductBeam.GetValueOrDefault().Year == year && o.WeavingUnitId == weavingId)
                    .ToList();

            var minBroken =
                filteredQueries
                    .GroupBy(o => new { o.YarnId })
                    .Sum(s => s.Min(min => min.BrokenEachYarn));

            var calculateMin = CalculateWarpingBroken(uomUnit, totalWarpingBeamLength, amountOfCones, minBroken);

            return calculateMin;
        }
    }
}
