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

                var reportQueries = GetWarpingBrokenReportQueries();

                var filteredReportQueries =
                    reportQueries
                        .Where(o => o.DateTimeProductBeam.GetValueOrDefault().Month == month && o.DateTimeProductBeam.GetValueOrDefault().Year == year && o.WeavingUnitId == weavingUnitId)
                        .ToList();

                var groupedReportHeaderQueries =
                    filteredReportQueries
                        .GroupBy(item => new { item.SupplierName, item.BrokenCauseName, item.YarnName })
                        .Select(entity => new
                        {
                            entity.Key.BrokenCauseName,
                            entity.Key.SupplierName,
                            entity.Key.YarnName,
                            Total = entity.Sum(sum => sum.BrokenEachCause)
                        })
                        .ToList();

                var headerSupplier =
                    groupedReportHeaderQueries
                        .GroupBy(item => new { item.SupplierName })
                        .Select(o => new WarpingBrokenThreadsReportHeaderSupplierDto(o.Key.SupplierName, o.Count()))
                        .ToList();

                var headerYarn =
                    groupedReportHeaderQueries
                        .GroupBy(item => new { item.SupplierName, item.BrokenCauseName, item.YarnName })
                        .Select(o => new WarpingBrokenThreadsReportHeaderYarnDto(o.Key.SupplierName, o.Key.BrokenCauseName, o.Key.YarnName))
                        .ToList();

                var bodyBrokens =
                   filteredReportQueries
                        .GroupBy(item => new
                        {
                            item.BrokenCauseName,
                            item.SupplierName,
                            item.YarnName,
                            item.BeamPerOperatorUom,
                            item.TotalBeamLength,
                            item.AmountOfCones,
                            item.BrokenEachCause
                        })
                        .Select(o => new WarpingBrokenThreadsReportBodyBrokenDto()
                        {
                            BrokenName = o.Key.BrokenCauseName,
                            ListBroken = o.Select(entity => new WarpingListBrokenDto(entity.SupplierName,
                                                                                     entity.YarnName,
                                                                                     CalculateWarpingBroken(entity.BeamPerOperatorUom,
                                                                                                            entity.TotalBeamLength,
                                                                                                            entity.AmountOfCones,
                                                                                                            entity.BrokenEachCause)))
                                          .ToList()
                        })
                        .ToList();

                var footerTotal = GetFooter(month, year, weavingUnitId);

                var monthName = new DateTime(year, month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("id-ID"));

                SingleUnitResult unitData = GetUnit(weavingUnitId);
                var weavingUnitName = unitData.data.Name;

                result.Month = monthName;
                result.Year = year.ToString();
                result.WeavingUnitName = weavingUnitName;
                result.HeaderSuppliers = headerSupplier;
                result.HeaderYarns = headerYarn;
                result.BodyBrokensValue = bodyBrokens;
                result.FooterTotals = footerTotal;

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private double CalculateWarpingBroken(int uomId, double totalWarpingBeamLength, int amountOfCones, int totalBroken)
        {
            double result = 0;
            double convertedResult = 0;

            if (uomId == 136)
            {
                result = (BrokenWarpingConstants.BROKEN_WARPING_DEFAULT_CONST / (totalWarpingBeamLength * amountOfCones)) * totalBroken;
            }
            else if (uomId == 195)
            {
                result = ((BrokenWarpingConstants.MTR_CONST * BrokenWarpingConstants.BROKEN_WARPING_DEFAULT_CONST) / (totalWarpingBeamLength * amountOfCones)) * totalBroken;
            }
            else
            {
                result = 0;
            }

            convertedResult = Math.Round(result, 4);
            return convertedResult;
        }

        private List<WarpingBrokenReportQueryDto> GetWarpingBrokenReportQueries()
        {
            var reportQueries = (from brokenCause in _warpingBrokenCauseRepository.Query

                                 join warpingBrokenCause in _dailyOperationWarpingBrokenCauseRepository.Query
                                                         on brokenCause.Identity equals warpingBrokenCause.BrokenCauseId
                                                         into joinWarpingBrokenCause
                                 from brokenCauseJoinWarpingBrokenCause in joinWarpingBrokenCause.DefaultIfEmpty()

                                 join warpingBeamProduct in _dailyOperationWarpingBeamProductRepository.Query
                                                         on brokenCauseJoinWarpingBrokenCause.DailyOperationWarpingBeamProductId equals warpingBeamProduct.Identity
                                                         into joinWarpingBeamProduct
                                 from brokenCauseJoinBeamProduct in joinWarpingBeamProduct.DefaultIfEmpty()

                                 join warpingDocument in _dailyOperationWarpingRepository.Query
                                                      on brokenCauseJoinBeamProduct.DailyOperationWarpingDocumentId equals warpingDocument.Identity
                                                      into joinWarpingDocument
                                 from beamProductJoinWarpingDocument in joinWarpingDocument.DefaultIfEmpty()

                                 join orderDocument in _orderRepository.Query
                                                    on beamProductJoinWarpingDocument.OrderDocumentId equals orderDocument.Identity
                                                    into joinOrderDocument
                                 from warpingDocumentJoinOrder in joinOrderDocument.DefaultIfEmpty()

                                 join supplierDocument in _supplierRepository.Query
                                                       on warpingDocumentJoinOrder.WarpOriginId equals supplierDocument.Identity
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
                                     DateTimeProduceBeam = brokenCauseJoinBeamProduct != null ? brokenCauseJoinBeamProduct.LatestDateTimeBeamProduct : DateTimeOffset.Now,
                                     WeavingUnitId = warpingDocumentJoinOrder != null ? warpingDocumentJoinOrder.UnitId : 0,
                                     BrokenCauseName = brokenCause != null ? brokenCause.WarpingBrokenCauseName : "",
                                     SupplierName = orderDocumentJoinSupplier != null ? orderDocumentJoinSupplier.Name : "",
                                     YarnName = constructionYarnDetailJoinYarn != null ? constructionYarnDetailJoinYarn.Name : "",
                                     BrokenEachCause = brokenCauseJoinWarpingBrokenCause != null ? brokenCauseJoinWarpingBrokenCause.TotalBroken : 0,
                                     BeamPerOperatorUom = brokenCauseJoinBeamProduct != null ? brokenCauseJoinBeamProduct.WarpingTotalBeamLengthUomId : 0,
                                     TotalBeamLength = brokenCauseJoinBeamProduct != null ? brokenCauseJoinBeamProduct.WarpingTotalBeamLength : 0,
                                     AmountOfCones = beamProductJoinWarpingDocument != null ? beamProductJoinWarpingDocument.AmountOfCones : 0
                                 })
                                 .ToList();

            var warpingBrokenReport = new List<WarpingBrokenReportQueryDto>();
            foreach(var report in reportQueries)
            {
                var brokenReportDto = new WarpingBrokenReportQueryDto(report.DateTimeProduceBeam,
                                                                      report.WeavingUnitId,
                                                                      report.BrokenCauseName,
                                                                      report.SupplierName,
                                                                      report.YarnName,
                                                                      report.BrokenEachCause,
                                                                      report.BeamPerOperatorUom,
                                                                      report.TotalBeamLength,
                                                                      report.AmountOfCones);
                warpingBrokenReport.Add(brokenReportDto);
            }

            return warpingBrokenReport;
        }

        private List<WarpingListBrokenDto> GetWarpingListBroken(List<WarpingBrokenReportQueryDto> warpingBrokenReportQueries)
        {
            var groupedReportQueries =
                warpingBrokenReportQueries
                    .GroupBy(item => new
                    {
                        item.SupplierName,
                        item.YarnName,
                        item.BeamPerOperatorUom,
                        item.TotalBeamLength,
                        item.AmountOfCones,
                        item.BrokenEachCause
                    })
                    .Select(o => new WarpingBrokenGroupedReportQueryDto(o.Key.SupplierName,
                                                                        o.Key.YarnName,
                                                                        o.Key.BeamPerOperatorUom,
                                                                        o.Key.TotalBeamLength,
                                                                        o.Key.AmountOfCones,
                                                                        o.Sum(sum => sum.BrokenEachCause)))
                    .ToList();

            var listBroken =
                groupedReportQueries
                    .Select(b => new WarpingListBrokenDto(b.SupplierName, b.YarnName, CalculateWarpingBroken(b.BeamPerOperatorUom, b.TotalBeamLength, b.AmountOfCones, b.TotalBrokenCause)))
                    .ToList();

            return listBroken;
        }

        private WarpingBrokenThreadsReportFooterDto GetFooter(int month, int year, int weavingId)
        {
            var reportQueries = GetWarpingBrokenReportQueries();

            var filteredTotalQueries =
                reportQueries
                    .Where(o => o.DateTimeProductBeam.GetValueOrDefault().Month == month && o.DateTimeProductBeam.GetValueOrDefault().Year == year && o.WeavingUnitId == weavingId)
                    .ToList();

            var listBroken = GetWarpingListBroken(filteredTotalQueries);

            var listOfTotal = GetBrokenTotal(month, year, weavingId);
            var listOfMax = GetBrokenMax(year, weavingId);
            var listOfMin = GetBrokenMin(year, weavingId);

            var lastMonthDateTime = new DateTime(year, month, 1, 1, 1, 1).AddMonths(-1);
            var listOfAverage = GetBrokenTotal(lastMonthDateTime.Month, lastMonthDateTime.Year, weavingId);
            var lastMonth = new DateTime(lastMonthDateTime.Year, lastMonthDateTime.Month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("id-ID"));

            var result = new WarpingBrokenThreadsReportFooterDto(lastMonth, listOfTotal, listOfMax, listOfMin, listOfAverage);

            return result;
        }

        private List<WarpingBrokenThreadsReportFooterTotalDto> GetBrokenTotal(int month, int year, int weavingId)
        {
            var reportQueries = GetWarpingBrokenReportQueries();

            var filteredQueries =
                reportQueries
                    .Where(o => o.DateTimeProductBeam.GetValueOrDefault().Month == month && o.DateTimeProductBeam.GetValueOrDefault().Year == year && o.WeavingUnitId == weavingId)
                    .ToList();

            var listBroken = GetWarpingListBroken(filteredQueries);

            var totalResult =
                listBroken
                    .GroupBy(o => new { o.YarnName })
                    .Select(s => new WarpingBrokenThreadsReportFooterTotalDto(s.Key.YarnName, s.Sum(sum => sum.BrokenEachYarn)))
                    .ToList();

            return totalResult;
        }

        private List<double> GetBrokenMax(int year, int weavingId)
        {
            var reportQueries = GetWarpingBrokenReportQueries();

            var filteredQueries =
                reportQueries
                    .Where(o => o.DateTimeProductBeam.GetValueOrDefault().Year == year && o.WeavingUnitId == weavingId)
                    .ToList();

            var listBroken = GetWarpingListBroken(filteredQueries);

            var maxResult = 
                listBroken
                    .GroupBy(o => new { o.YarnName })
                    .Select(s => s.Max(max => max.BrokenEachYarn))
                    .ToList();

            return maxResult;
        }

        private List<double> GetBrokenMin(int year, int weavingId)
        {
            var reportQueries = GetWarpingBrokenReportQueries();

            var filteredQueries =
                reportQueries
                    .Where(o => o.DateTimeProductBeam.GetValueOrDefault().Year == year && o.WeavingUnitId == weavingId)
                    .ToList();

            var listBroken = GetWarpingListBroken(filteredQueries);

            var minResult =
                listBroken
                    .GroupBy(o => new { o.YarnName })
                    .Select(s => s.Min(min => min.BrokenEachYarn))
                    .ToList();

            return minResult;
        }
    }
}
