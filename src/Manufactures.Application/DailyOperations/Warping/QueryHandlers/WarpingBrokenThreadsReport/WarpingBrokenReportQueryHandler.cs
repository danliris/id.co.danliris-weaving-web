using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
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
                var reportQueries = from brokenCause in _warpingBrokenCauseRepository.Query

                                  join warpingBrokenCause in _dailyOperationWarpingBrokenCauseRepository.Query on brokenCause.Identity
                                    equals warpingBrokenCause.BrokenCauseId
                                    into joinBrokenCause
                                  from warpingBrokenCauseJoinCause in joinBrokenCause.DefaultIfEmpty()

                                  join warpingBeamProduct in _dailyOperationWarpingBeamProductRepository.Query on warpingBrokenCauseJoinCause.DailyOperationWarpingBeamProductId
                                    equals warpingBeamProduct.Identity
                                    into joinBeamProduct
                                  from brokenCauseJoinBeamProduct in joinBeamProduct.DefaultIfEmpty()

                                  join warpingDocument in _dailyOperationWarpingRepository.Query on brokenCauseJoinBeamProduct.DailyOperationWarpingDocumentId
                                    equals warpingDocument.Identity
                                    into joinWarpingDocument
                                  from warpingDocumentJoinBeamProduct in joinWarpingDocument.DefaultIfEmpty()

                                  join orderDocument in _orderRepository.Query on warpingDocumentJoinBeamProduct.OrderDocumentId
                                    equals orderDocument.Identity
                                    into joinOrderDocument
                                  from orderDocumentJoinWarpingDocument in joinOrderDocument.DefaultIfEmpty()

                                  join supplierDocument in _supplierRepository.Query on orderDocumentJoinWarpingDocument.WarpOriginId
                                    equals supplierDocument.Identity
                                    into joinSupplierDocument
                                  from supplierDocumentJoinOrderDocument in joinSupplierDocument.DefaultIfEmpty()

                                  join constructionYarnDetails in _constructionYarnDetailRepository.Query on orderDocumentJoinWarpingDocument.ConstructionDocumentId
                                    equals constructionYarnDetails.FabricConstructionDocumentId
                                    into joinConstructionDetail
                                  from constructionDetailJoinSupplierDocument in joinConstructionDetail.DefaultIfEmpty()

                                  join yarnDocument in _yarnRepository.Query on constructionDetailJoinSupplierDocument.YarnId
                                    equals yarnDocument.Identity
                                    into joinYarnDocument
                                  from yarnDocumentJoinConstructionDetail in joinYarnDocument.DefaultIfEmpty()

                                  where constructionDetailJoinSupplierDocument.Type == "Warp"

                                  select new
                                  {
                                      BrokenCauseId = brokenCause != null ? brokenCause.Identity : Guid.Empty,
                                      BrokenCauseName = brokenCause != null ? brokenCause.WarpingBrokenCauseName : "",
                                      BrokenEachCause = warpingBrokenCauseJoinCause != null ? warpingBrokenCauseJoinCause.TotalBroken : 0,
                                      OrderDocumentId = orderDocumentJoinWarpingDocument != null ? orderDocumentJoinWarpingDocument.Identity : Guid.Empty,
                                      WarpOriginId = orderDocumentJoinWarpingDocument != null ? orderDocumentJoinWarpingDocument.WarpOriginId : Guid.Empty,
                                      SupplierName = supplierDocumentJoinOrderDocument != null ? supplierDocumentJoinOrderDocument.Name : "",
                                      YarnId = constructionDetailJoinSupplierDocument != null ? constructionDetailJoinSupplierDocument.YarnId : Guid.Empty,
                                      YarnName = yarnDocumentJoinConstructionDetail != null ? yarnDocumentJoinConstructionDetail.Name : "",
                                      DateTimeProduceBeam = brokenCauseJoinBeamProduct != null ? brokenCauseJoinBeamProduct.LatestDateTimeBeamProduct : DateTimeOffset.Now,
                                      WeavingUnitId = orderDocumentJoinWarpingDocument != null ? orderDocumentJoinWarpingDocument.UnitId : 0,
                                      BeamPerOperatorUom = brokenCauseJoinBeamProduct != null ? brokenCauseJoinBeamProduct.WarpingTotalBeamLengthUomId : 0,
                                      TotalBeamLength = brokenCauseJoinBeamProduct != null ? brokenCauseJoinBeamProduct.WarpingTotalBeamLength : 0,
                                      AmountOfCones = warpingDocumentJoinBeamProduct != null ? warpingDocumentJoinBeamProduct.AmountOfCones : 0
                                  };
                var filteredReportQueries = reportQueries.Where(o => o.DateTimeProduceBeam.Month == month && o.DateTimeProduceBeam.Year == year && o.WeavingUnitId == weavingUnitId);

                var groupedReportQueries = filteredReportQueries.GroupBy(item => new { item.BrokenCauseName, item.SupplierName, item.YarnName, item.BeamPerOperatorUom, item.TotalBeamLength, item.AmountOfCones })
                    .Select(entity => new
                    {
                        entity.Key.BrokenCauseName,
                        entity.Key.SupplierName,
                        entity.Key.YarnName,
                        TotalBrokenCause = entity.Sum(sum => sum.BrokenEachCause),
                        entity.Key.BeamPerOperatorUom,
                        entity.Key.TotalBeamLength,
                        entity.Key.AmountOfCones
                    })
                    .ToList();

                var headerSupplier =
                    groupedReportQueries
                        .GroupBy(item => new { item.SupplierName })
                        .Select(o => new WarpingBrokenThreadsReportHeaderSupplierDto(o.Key.SupplierName, o.Count()));

                var headerYarn =
                    groupedReportQueries
                        .GroupBy(item => new { item.SupplierName, item.BrokenCauseName, item.YarnName })
                        .Select(o => new WarpingBrokenThreadsReportHeaderYarnDto(o.Key.SupplierName, o.Key.BrokenCauseName, o.Key.YarnName));

                var bodyBrokens =
                    groupedReportQueries.GroupBy(o => o.BrokenCauseName)
                        .Select(r => new WarpingBrokenThreadsReportBodyBrokenDto()
                        {
                            BrokenName = 
                                r.First().BrokenCauseName,
                            ListBroken = 
                                r.Select(b => new WarpingListBrokenDto(b.SupplierName, b.YarnName, CalculateWarpingBroken(b.BeamPerOperatorUom, b.TotalBeamLength, b.AmountOfCones, b.TotalBrokenCause)))
                                 .ToList()
                        })
                        .ToList();
                //new List<WarpingBrokenThreadsReportBodyBrokenDto>();
                foreach (var reportQuery in groupedReportQueries)
                {
                    var bodyBroken = new WarpingBrokenThreadsReportBodyBrokenDto();
                    var listBroken = new WarpingListBrokenDto(reportQuery.SupplierName, reportQuery.YarnName, reportQuery.TotalBrokenCause);
                }

                //-----------------------------------------------------------------

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
                        _dailyOperationWarpingBeamProductRepository
                            .Find(item => item.DailyOperationWarpingDocumentId == document.Identity &&
                                          item.LatestDateTimeBeamProduct.Month == month &&
                                          item.LatestDateTimeBeamProduct.Year == year)
                            .ToList();

                    var unitId = _orderRepository.Find(o => o.Identity == document.OrderDocumentId.Value).FirstOrDefault().UnitId.Value;

                    SingleUnitResult unitData = GetUnit(unitId);
                    var weavingUnitName = unitData.data.Name;

                    //Get ConstructionId from OrderId from Warping Document (document)
                    var constructionId =
                        _orderRepository
                            .Find(o => o.Identity.Equals(document.OrderDocumentId.Value))
                            .FirstOrDefault().ConstructionDocumentId.Value;

                    //Get ConstructionDocument using ConstructionId from above, to get List of Warp Id (warpIds)
                    var constructionYarnDetail =
                        _constructionYarnDetailRepository
                            .Find(d=>d.FabricConstructionDocumentId == constructionId);

                    //Get WarpName using List of Warp Id (warpIds)
                    var warpIds = constructionYarnDetail.Select(o => o.YarnId.Value);
                    var warpNames = _yarnRepository.Find(o => warpIds.Contains(o.Identity)).Select(y => y.Name);

                    var warpOriginId = _orderRepository.Find(o => o.Identity == document.OrderDocumentId.Value).FirstOrDefault().WarpOriginId;
                    var supplierName = _supplierRepository.Find(o => o.Identity == warpOriginId.Value).FirstOrDefault().Name;

                    var resultBodyBroken = new WarpingBrokenThreadsReportBodyBrokenDto();
                    foreach (var warpName in warpNames)
                    {
                        foreach (var beamProduct in groupedBeamProducts)
                        {
                            foreach (var broken in beamProduct.BrokenCauses)
                            {
                                var brokenName = _warpingBrokenCauseRepository.Find(o => o.Identity == broken.BrokenCauseId).FirstOrDefault().WarpingBrokenCauseName;

                                //Calculate BrokenTotal based on UomId(136 = YARD or 195 = MTR)
                                double brokenValue = CalculateWarpingBroken(beamProduct.WarpingTotalBeamLengthUomId.Value, beamProduct.WarpingTotalBeamLength, document.AmountOfCones, broken.TotalBroken);

                                //resultBodyBroken = new WarpingBrokenThreadsReportBodyBrokenDto(brokenName,
                                //                                                               //supplierName, 
                                //                                                               warpName,
                                //                                                               brokenValue);
                                //bodyBrokenList.Add(resultBodyBroken);
                            }
                        }
                    }

                    //var headerWarps =
                    //    bodyBrokenList
                    //        .GroupBy(g => new { supplierName, g.BrokenName, g.WarpName })
                    //        .Select(s => new WarpingBrokenThreadsReportHeaderWarpDto(s.Key.supplierName, s.Key.BrokenName, s.Key.WarpName))
                    //        .ToList();

                    //var headerSuppliers =
                    //    headerWarps
                    //        .GroupBy(o => new { o.SpinningUnit, o.WarpName })
                    //        .Select(s => new WarpingBrokenThreadsReportHeaderSupplierDto(s.Key.SpinningUnit, s.Count()))
                    //        .ToList();

                    //var totalValue =
                    //    bodyBrokenList.GroupBy(o => new { o.WarpName }).Select(s => new WarpingBrokenThreadsReportFooterTotalDto(s.Key.WarpName, s.Sum(sum => sum.BrokenValue))).ToList();

                    //var maxValue =
                    //    bodyBrokenList.GroupBy(o => new { o.WarpName }).Select(s => s.Max(max => max.BrokenValue)).ToList();

                    //var minValue =
                    //    bodyBrokenList.GroupBy(o => new { o.WarpName }).Select(s => s.Min(min => min.BrokenValue)).ToList();

                    //var previousDateTime = dateTimeFilter.AddMonths(-1);
                    //var lastMonthTotalValue = GetLastMonthTotal(previousDateTime.Month, previousDateTime.Year, weavingUnitId);
                    //var lastMonthAverageValue = new List<double>();

                    //foreach (var value in totalValue)
                    //{
                    //    var selectedLastMonthTotal = lastMonthTotalValue.FirstOrDefault(o => o.WarpName == value.WarpName);
                    //    if (selectedLastMonthTotal != null)
                    //    {
                    //        lastMonthAverageValue.Add(selectedLastMonthTotal.TotalValue);
                    //    }
                    //    else
                    //    {
                    //        lastMonthAverageValue.Add(0);
                    //    }
                    //}

                    //var footer = new WarpingBrokenThreadsReportFooterDto(totalValue, maxValue, minValue, lastMonthAverageValue);

                    //result.Month = monthName;
                    //result.Year = year.ToString();
                    //result.WeavingUnitName = weavingUnitName;
                    //result.HeaderSuppliers = headerSuppliers;
                    //result.HeaderWarps = headerWarps;
                    //result.BodyBrokens = bodyBrokenList;
                    //result.Footers = footer;
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
            double convertedResult = 0;

            if (uomId == 136)
            {
                result = BrokenWarpingConstants.BROKEN_WARPING_DEFAULT_CONST / totalWarpingBeamLength * amountOfCones * totalBroken;
            }
            else if (uomId == 195)
            {
                result = BrokenWarpingConstants.MTR_CONST * BrokenWarpingConstants.BROKEN_WARPING_DEFAULT_CONST / totalWarpingBeamLength * amountOfCones * totalBroken;
            }
            else
            {
                result = 0;
            }

            convertedResult = Math.Round(result, 4);
            return convertedResult;
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
                        .FirstOrDefault().ConstructionDocumentId.Value;

                //Get ConstructionDocument using ConstructionId from above, to get List of Warp Id (warpIds)
                var constructionDocument =
                    _fabricConstructionRepository
                        .Find(c => c.Identity.Equals(constructionId))
                        .FirstOrDefault();

                //Get WarpName using List of Warp Id (warpIds)
                var warpIds = constructionDocument.ConstructionWarpsDetail.Select(o => o.YarnId.Value);
                var warpNames = _yarnRepository.Find(o => warpIds.Contains(o.Identity)).Select(y => y.Name);

                var warpOriginId = _orderRepository.Find(o => o.Identity == document.OrderDocumentId.Value).FirstOrDefault().WarpOriginId;
                var supplierName = _supplierRepository.Find(o => o.Identity == warpOriginId.Value).FirstOrDefault().Name;

                var resultBodyBroken = new WarpingBrokenThreadsReportBodyBrokenDto();
                foreach (var warpName in warpNames)
                {
                    foreach (var beamProduct in groupedBeamProducts)
                    {
                        foreach (var broken in beamProduct.BrokenCauses)
                        {
                            var brokenName = _warpingBrokenCauseRepository.Find(o => o.Identity == broken.BrokenCauseId).FirstOrDefault().WarpingBrokenCauseName;

                            //Calculate BrokenTotal based on UomId(136 = YARD or 195 = MTR)
                            double brokenValue = CalculateWarpingBroken(beamProduct.WarpingTotalBeamLengthUomId.Value, beamProduct.WarpingTotalBeamLength, document.AmountOfCones, broken.TotalBroken);

                            //resultBodyBroken = new WarpingBrokenThreadsReportBodyBrokenDto(brokenName,
                            //                                                               //supplierName, 
                            //                                                               warpName,
                            //                                                               brokenValue);
                            //bodyBrokenList.Add(resultBodyBroken);
                        }
                    }
                }

                //var headerWarps =
                //    bodyBrokenList
                //        .GroupBy(g => new { supplierName, g.BrokenName, g.WarpName })
                //        .Select(s => new WarpingBrokenThreadsReportHeaderWarpDto(s.Key.supplierName,
                //                                                                 s.Key.BrokenName,
                //                                                                 s.Key.WarpName))
                //        .ToList();

                //totalValue =
                //    bodyBrokenList.GroupBy(o => new { o.WarpName }).Select(s => new WarpingBrokenThreadsReportFooterTotalDto(s.Key.WarpName, s.Sum(sum => sum.BrokenValue))).ToList();
            }
            return totalValue;
        }
    }
}
