using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport;
using Manufactures.Application.Helpers;
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
        //private readonly IDailyOperationWarpingBeamProductRepository
        //    _dailyOperationWarpingBeamProductRepository;
        //private readonly IDailyOperationWarpingBrokenCauseRepository
        //    _dailyOperationWarpingBrokenCauseRepository;
        private readonly IOrderRepository
            _orderRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
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
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _orderRepository =
                _storage.GetRepository<IOrderRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _warpingBrokenCauseRepository =
                _storage.GetRepository<IWarpingBrokenCauseRepository>();
            _yarnRepository =
                _storage.GetRepository<IYarnDocumentRepository>();
            _supplierRepository =
                _storage.GetRepository<IWeavingSupplierRepository>();
        }

        protected SingleUnitResult GetUnit(int id)
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

                //Transform month and year from UI to DateTimeOffset
                DateTimeOffset dateTimeFilter =
                    new DateTimeOffset(year, month, 1, 0, 0, 0, new TimeSpan(+7, 0, 0));
                var monthName = dateTimeFilter.ToString("MMMM", CultureInfo.CreateSpecificCulture("id-ID"));
                var daysOfMonth = DateTime.DaysInMonth(year, month);

                //Get order which used (weavingId) same as parameter from UI
                var filteredOrderDocuments = _orderRepository.Find(orderQuery);
                var filteredOrderDocumentIds = filteredOrderDocuments.Select(o => o.Identity);

                //Preparing Daily Operation Warping Query w/ Filter Identity param equals (filteredOrderDocumentIds)
                var dailyOperationWarpingQuery =
                    _dailyOperationWarpingRepository
                        .Query
                        //.Include(h => h.WarpingHistories)
                        //.Include(p => p.WarpingBeamProducts)
                        //.ThenInclude(b => b.WarpingBrokenThreadsCauses)
                        .Where(o => filteredOrderDocumentIds.Contains(o.OrderDocumentId))
                        .AsQueryable();
                var warpingDocuments = _dailyOperationWarpingRepository.Find(dailyOperationWarpingQuery);
                
                //var documentLength = warpingDocuments.Count();

                //Add Shell for (BodyBrokenDto)
                var bodyBrokenList = new List<WarpingBrokenThreadsReportBodyBrokenDto>();
                foreach (var document in warpingDocuments)
                {
                    //Get Warping Beam Products w/ Filter date and year param from UI
                    var groupedBeamProducts =
                        document.WarpingBeamProducts.Where(item => item.LatestDateTimeBeamProduct.Month == month &&
                                                                   item.LatestDateTimeBeamProduct.Year == year)
                                                    .ToList();

                    var unitId = _orderRepository.Find(o => o.Identity == document.OrderDocumentId.Value).FirstOrDefault().UnitId.Value;

                    SingleUnitResult unitData = GetUnit(unitId);
                    var weavingUnitName = unitData.data.Name;

                    //Get ConstructionId from OrderId from Warping Document (document)
                    var constructionId =
                        _orderRepository
                            .Find(o => o.Identity.Equals(document.OrderDocumentId.Value))
                            .FirstOrDefault().ConstructionId.Value;

                    //Get ConstructionDocument using ConstructionId from above, to get List of Warp Id (warpIds)
                    var constructionDocument =
                        _fabricConstructionRepository
                            .Find(c => c.Identity.Equals(constructionId))
                            .FirstOrDefault();

                    //Get WarpName using List of Warp Id (warpIds)
                    var warpIds = constructionDocument.ConstructionWarpsDetail.Select(o => o.YarnId.Value);
                    var warpNames = _yarnRepository.Find(o => warpIds.Contains(o.Identity)).Select(y => y.Name);

                    var warpOriginId = _orderRepository.Find(o => o.Identity == document.OrderDocumentId.Value).FirstOrDefault().WarpOrigin;
                    var supplierName = _supplierRepository.Find(o => o.Identity.ToString() == warpOriginId).FirstOrDefault().Name;

                    var resultBodyBroken = new WarpingBrokenThreadsReportBodyBrokenDto();
                    foreach (var warpName in warpNames)
                    {
                        foreach (var beamProduct in groupedBeamProducts)
                        {
                            foreach (var broken in beamProduct.BrokenCauses)
                            {
                                var brokenName = _warpingBrokenCauseRepository.Find(o => o.Identity == broken.BrokenCauseId).FirstOrDefault().WarpingBrokenCauseName;

                                //Calculate BrokenTotal based on UomId(136 = YARD or 195 = MTR)
                                double brokenValue = CalculateWarpingBroken(beamProduct.WarpingTotalBeamLengthUomId, beamProduct.WarpingTotalBeamLength, document.AmountOfCones, broken.TotalBroken);

                                resultBodyBroken = new WarpingBrokenThreadsReportBodyBrokenDto(brokenName,
                                                                                               //supplierName, 
                                                                                               warpName,
                                                                                               brokenValue);
                                bodyBrokenList.Add(resultBodyBroken);
                            }
                        }
                    }

                    var headerWarps =
                        bodyBrokenList
                            .GroupBy(g => new { supplierName, g.BrokenName, g.WarpName })
                            .Select(s => new WarpingBrokenThreadsReportHeaderWarpDto(s.Key.supplierName, s.Key.BrokenName, s.Key.WarpName))
                            .ToList();

                    var headerSuppliers =
                        headerWarps
                            .GroupBy(o => new { o.SpinningUnit, o.WarpName })
                            .Select(s => new WarpingBrokenThreadsReportHeaderSupplierDto(s.Key.SpinningUnit, s.Count()))
                            .ToList();

                    var totalValue =
                        bodyBrokenList.GroupBy(o => new { o.WarpName }).Select(s => new WarpingBrokenThreadsReportFooterTotalDto(s.Key.WarpName, s.Sum(sum => sum.BrokenValue))).ToList();

                    var maxValue =
                        bodyBrokenList.GroupBy(o => new { o.WarpName }).Select(s => s.Max(max => max.BrokenValue)).ToList();

                    var minValue =
                        bodyBrokenList.GroupBy(o => new { o.WarpName }).Select(s => s.Min(min => min.BrokenValue)).ToList();

                    var previousDateTime = dateTimeFilter.AddMonths(-1);
                    var lastMonthTotalValue = GetLastMonthTotal(previousDateTime.Month, previousDateTime.Year, weavingUnitId);
                    var lastMonthAverageValue = new List<double>();

                    foreach (var value in totalValue)
                    {
                        var selectedLastMonthTotal = lastMonthTotalValue.FirstOrDefault(o => o.WarpName == value.WarpName);
                        if (selectedLastMonthTotal != null)
                        {
                            lastMonthAverageValue.Add(selectedLastMonthTotal.TotalValue);
                        }
                        else
                        {
                            lastMonthAverageValue.Add(0);
                        }
                    }

                    var footer = new WarpingBrokenThreadsReportFooterDto(totalValue, maxValue, minValue, lastMonthAverageValue);

                    result.Month = monthName;
                    result.Year = year.ToString();
                    result.WeavingUnitName = weavingUnitName;
                    result.HeaderSuppliers = headerSuppliers;
                    result.HeaderWarps = headerWarps;
                    result.BodyBrokens = bodyBrokenList;
                    result.Footers = footer;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private double CalculateWarpingBroken(int uomId, double totalWarpingBeamLength, int amountOfCones, int totalBroken)
        {
            double result = 0;
            if (uomId == 136)
            {
                result = BrokenWarpingConstants.BROKEN_WARPING_DEFAULT_CONST / totalWarpingBeamLength * amountOfCones * totalBroken;
            }
            else if (uomId == 195)
            {
                result = BrokenWarpingConstants.MTR_CONST * BrokenWarpingConstants.BROKEN_WARPING_DEFAULT_CONST / totalWarpingBeamLength * amountOfCones * totalBroken;
            }

            return result;
        }

        private List<WarpingBrokenThreadsReportFooterTotalDto> GetLastMonthTotal(int month, int year, int weavingId)
        {
            //Transform month and year from UI to DateTimeOffset
            DateTimeOffset dateTimeFilter =
                new DateTimeOffset(year, month, 1, 0, 0, 0, new TimeSpan(+7, 0, 0));
            var monthName = dateTimeFilter.ToString("MMMM", CultureInfo.CreateSpecificCulture("id-ID"));
            var daysOfMonth = DateTime.DaysInMonth(year, month);

            //Get order which used (weavingId) same as parameter from UI
            var orderQuery =
                _orderRepository
                    .Query
                    .AsQueryable();
            var filteredOrderDocuments = _orderRepository.Find(orderQuery);
            var filteredOrderDocumentIds = filteredOrderDocuments.Select(o => o.Identity);

            //Preparing Daily Operation Warping Query w/ Filter Identity param equals (filteredOrderDocumentIds)
            
            var warpingDocuments = _dailyOperationWarpingRepository.Find(x => filteredOrderDocumentIds.Contains(x.OrderDocumentId));

            //var documentLength = warpingDocuments.Count();

            //Add Shell for (BodyBrokenDto)
            var bodyBrokenList = new List<WarpingBrokenThreadsReportBodyBrokenDto>();
            var totalValue = new List<WarpingBrokenThreadsReportFooterTotalDto>();
            foreach (var document in warpingDocuments)
            {
                //Get Warping Beam Products w/ Filter date and year param from UI
                var groupedBeamProducts =
                    document.WarpingBeamProducts.Where(item => item.LatestDateTimeBeamProduct.Month == month &&
                                                               item.LatestDateTimeBeamProduct.Year == year)
                                                .ToList();

                var weavingUnitId = _orderRepository.Find(o => o.Identity == document.OrderDocumentId.Value).FirstOrDefault().UnitId.Value;

                SingleUnitResult unitData = GetUnit(weavingUnitId);
                var weavingUnitName = unitData.data.Name;

                //Get ConstructionId from OrderId from Warping Document (document)
                var constructionId =
                    _orderRepository
                        .Find(o => o.Identity.Equals(document.OrderDocumentId.Value))
                        .FirstOrDefault().ConstructionId.Value;

                //Get ConstructionDocument using ConstructionId from above, to get List of Warp Id (warpIds)
                var constructionDocument =
                    _fabricConstructionRepository
                        .Find(c => c.Identity.Equals(constructionId))
                        .FirstOrDefault();

                //Get WarpName using List of Warp Id (warpIds)
                var warpIds = constructionDocument.ConstructionWarpsDetail.Select(o => o.YarnId.Value);
                var warpNames = _yarnRepository.Find(o => warpIds.Contains(o.Identity)).Select(y => y.Name);

                var warpOriginId = _orderRepository.Find(o => o.Identity == document.OrderDocumentId.Value).FirstOrDefault().WarpOrigin;
                var supplierName = _supplierRepository.Find(o => o.Identity.ToString() == warpOriginId).FirstOrDefault().Name;

                var resultBodyBroken = new WarpingBrokenThreadsReportBodyBrokenDto();
                foreach (var warpName in warpNames)
                {
                    foreach (var beamProduct in groupedBeamProducts)
                    {
                        foreach (var broken in beamProduct.BrokenCauses)
                        {
                            var brokenName = _warpingBrokenCauseRepository.Find(o => o.Identity == broken.BrokenCauseId).FirstOrDefault().WarpingBrokenCauseName;

                            //Calculate BrokenTotal based on UomId(136 = YARD or 195 = MTR)
                            double brokenValue = CalculateWarpingBroken(beamProduct.WarpingTotalBeamLengthUomId, beamProduct.WarpingTotalBeamLength, document.AmountOfCones, broken.TotalBroken);

                            resultBodyBroken = new WarpingBrokenThreadsReportBodyBrokenDto(brokenName,
                                                                                           //supplierName, 
                                                                                           warpName,
                                                                                           brokenValue);
                            bodyBrokenList.Add(resultBodyBroken);
                        }
                    }
                }

                var headerWarps =
                    bodyBrokenList
                        .GroupBy(g => new { supplierName, g.BrokenName, g.WarpName })
                        .Select(s => new WarpingBrokenThreadsReportHeaderWarpDto(s.Key.supplierName,
                                                                                 s.Key.BrokenName,
                                                                                 s.Key.WarpName))
                        .ToList();

                totalValue =
                    bodyBrokenList.GroupBy(o => new { o.WarpName }).Select(s => new WarpingBrokenThreadsReportFooterTotalDto(s.Key.WarpName, s.Sum(sum => sum.BrokenValue))).ToList();
            }
            return totalValue;
        }
    }
}
