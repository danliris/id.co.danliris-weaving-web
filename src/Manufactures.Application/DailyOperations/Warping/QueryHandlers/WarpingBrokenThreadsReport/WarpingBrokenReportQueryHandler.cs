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
                var filteredResult = reportQueries.Where(reportQuery => reportQuery.DateTimeProductBeam.HasValue && reportQuery.DateTimeProductBeam.Value.Month == month);

                // get broken causes
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
                        }

                        dictionary[element.BrokenCauseName] = element.BrokenEachYarn.ToString();
                        tempResult.Add(dictionary);
                    }
                    else
                    {
                        int.TryParse(existingDictionary[element.BrokenCauseName], out var totalBroken);
                        existingDictionary[element.BrokenCauseName] = (totalBroken + element.BrokenEachYarn).ToString();
                    }
                }

                ////Grouped FilteredReportQueries by SupplierName, BrokenCauseName & YarnName
                //var anonymousHeaderBrokens =
                //    filteredReportQueries
                //        .GroupBy(item => new { item.SupplierName, item.BrokenCauseName, item.YarnName })
                //        .Select(entity => new
                //        {
                //            entity.Key.SupplierName,
                //            entity.Key.YarnName,
                //            entity.Key.BrokenCauseName,
                //            TotalBrokenYarn = entity.Sum(sum => sum.BrokenEachYarn)
                //        })
                //        .ToList();

                ////Mapping to HeaderBrokenDto
                //var headerBrokensDto =
                //    anonymousHeaderBrokens
                //        .GroupBy(item => new { item.BrokenCauseName })
                //        .Select(o => new WarpingBrokenThreadsReportHeaderBrokenDto(o.Key.BrokenCauseName, lastMonthName))
                //        .ToList();

                ////Grouped FilteredReportQueries by SupplierName, & YarnName
                //var anonymousBodyBrokens = filteredReportQueries
                //    .GroupBy(o => new { o.SupplierName, o.YarnName }).ToList();

                //var bodyBrokensDto = new List<WarpingBrokenThreadsReportBodyBrokenDto>();
                //foreach (var bodyBroken in anonymousBodyBrokens)
                //{
                //    var bodyBrokenDto = new WarpingBrokenThreadsReportBodyBrokenDto();

                //    bodyBrokenDto.SupplierName =
                //        bodyBroken.Key.SupplierName;
                //    bodyBrokenDto.YarnName =
                //        bodyBroken.Key.YarnName;
                //    bodyBrokenDto.BrokenEachYarn =
                //        bodyBroken
                //            .Select(o => CalculateWarpingBroken(o.BeamPerOperatorUom,
                //                                                o.BeamLength,
                //                                                o.AmountOfCones,
                //                                                o.BrokenEachYarn)).ToList();
                //    bodyBrokenDto.TotalAllBroken =
                //        bodyBroken
                //            .Sum(s => GetBrokenTotalAndAverage(s.BeamPerOperatorUom,
                //                                               s.BeamLength,
                //                                               s.AmountOfCones,
                //                                               s.DateTimeProductBeam.GetValueOrDefault().Month,
                //                                               s.DateTimeProductBeam.GetValueOrDefault().Year,
                //                                               s.WeavingUnitId));
                //    bodyBrokenDto.MaxBroken =
                //        bodyBroken
                //            .Max(s => GetBrokenMax(s.BeamPerOperatorUom,
                //                                   s.BeamLength,
                //                                   s.AmountOfCones,
                //                                   s.DateTimeProductBeam.GetValueOrDefault().Year,
                //                                   s.WeavingUnitId));
                //    bodyBrokenDto.MinBroken =
                //        bodyBroken
                //            .Min(s => GetBrokenMin(s.BeamPerOperatorUom,
                //                                   s.BeamLength,
                //                                   s.AmountOfCones,
                //                                   s.DateTimeProductBeam.GetValueOrDefault().Year,
                //                                   s.WeavingUnitId));
                //    bodyBrokenDto.LastMonthAverageBroken =
                //        bodyBroken
                //        .Average(s => GetBrokenTotalAndAverage(s.BeamPerOperatorUom, s.BeamLength, s.AmountOfCones, s.DateTimeProductBeam.GetValueOrDefault().Year != s.DateTimeProductBeam.GetValueOrDefault().AddMonths(-1).Year ? s.DateTimeProductBeam.GetValueOrDefault().Month : s.DateTimeProductBeam.GetValueOrDefault().AddMonths(-1).Month,
                //            s.DateTimeProductBeam.GetValueOrDefault().Year != s.DateTimeProductBeam.GetValueOrDefault().AddMonths(-1).Year ? s.DateTimeProductBeam.GetValueOrDefault().Year : s.DateTimeProductBeam.GetValueOrDefault().AddMonths(-1).Year, s.WeavingUnitId));

                //    bodyBrokensDto.Add(bodyBrokenDto);
                //}

                //var mappedBodyBrokens =
                //    bodyBrokensDto
                //        .GroupBy(o => new { o.SupplierName })
                //        .Select(r => new WarpingBrokenThreadsReportMappedBodyBrokenDto()
                //        {
                //            SupplierName = r.Key.SupplierName,
                //            ListOfYarn = r.Select(e => new WarpingBrokenThreadsReportListOfYarnDto()
                //            {
                //                YarnName = e.YarnName,
                //                BrokenEachYarn = e.BrokenEachYarn,
                //                TotalAllBroken = e.TotalAllBroken,
                //                MaxBroken = e.MaxBroken,
                //                MinBroken = e.MinBroken,
                //                LastMonthAverageBroken = e.LastMonthAverageBroken
                //            }).ToList(),
                //        }).ToList();

                //var mappedBodyBrokensDto =
                //    mappedBodyBrokens
                //        .Select(r => new WarpingBrokenThreadsReportMappedBodyBrokenDto()
                //        {
                //            SupplierName = r.SupplierName,
                //            ListOfYarn = r.ListOfYarn,
                //            YarnNameRowSpan = r.ListOfYarn.Count()
                //        }).ToList();

                //Grouped FilteredReportQueries by SupplierName, & YarnName
                //var anonymousBodyBrokens = filteredReportQueries
                //    .GroupBy(o => new { o.SupplierName })
                //    .Select(r=>new WarpingBrokenThreadsReportBodyBrokenDto()
                //    {
                //        SupplierName = r.Key.SupplierName,
                //        ListOfYarn = r.Select(entity => new WarpingBrokenThreadsReportListOfYarnDto()
                //        {
                //            YarnName = entity.YarnName,
                //            BrokenEachYarn = r.Select(b=>new TotalBrokenEachYarnValueDto(CalculateWarpingBroken(b.BeamPerOperatorUom,
                //                                                                                                b.BeamLength,
                //                                                                                                b.AmountOfCones,
                //                                                                                                b.BrokenEachYarn))).ToList(),
                //            TotalAllBroken = r.Sum(s=>GetBrokenTotalAndAverage(s.BeamPerOperatorUom,
                //                                                               s.BeamLength,
                //                                                               s.AmountOfCones,
                //                                                               s.DateTimeProductBeam.GetValueOrDefault().Month,
                //                                                               s.DateTimeProductBeam.GetValueOrDefault().Year,
                //                                                               s.WeavingUnitId)),
                //            MaxBroken = r.Max(s => GetBrokenMax(s.BeamPerOperatorUom,
                //                                   s.BeamLength,
                //                                   s.AmountOfCones,
                //                                   s.DateTimeProductBeam.GetValueOrDefault().Year,
                //                                   s.WeavingUnitId)),
                //            MinBroken = r.Min(s => GetBrokenMin(s.BeamPerOperatorUom,
                //                                   s.BeamLength,
                //                                   s.AmountOfCones,
                //                                   s.DateTimeProductBeam.GetValueOrDefault().Year,
                //                                   s.WeavingUnitId))
                //        }).ToList(),
                //        YarnNameRowSpan = r.Count()
                //    })
                //    .ToList();

                result.Month = monthName;
                result.Year = year.ToString();
                result.WeavingUnitName = weavingUnitName;
                //result.HeaderBrokens = headerBrokensDto;
                //result.BodyBrokensValue = mappedBodyBrokensDto;

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
                result = CalculateWarpingBrokenYard(totalWarpingBeamLength, amountOfCones, totalBroken);
            }
            else if (uomId == 195)
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
            double constantPerLengthCones = BrokenWarpingConstants.BROKEN_WARPING_DEFAULT_CONST / lengthCones;
            result = constantPerLengthCones * totalBroken;

            return result;
        }

        private double CalculateWarpingBrokenMeter(double totalWarpingBeamLength, int amountOfCones, int totalBroken)
        {
            double result = 0;

            double lengthCones = totalWarpingBeamLength * amountOfCones;
            double constantMeter = 0.9144 * BrokenWarpingConstants.BROKEN_WARPING_DEFAULT_CONST;
            double constantMeterPerLengthCones = constantMeter / lengthCones;
            result = constantMeterPerLengthCones * totalBroken;

            return result;
        }

        private List<WarpingBrokenReportQueryDto> GetWarpingBrokenReportQueries(int year, int weavingUnitId)
        {
            var reportQueries = (from brokenCause in _warpingBrokenCauseRepository.Query

                                 join warpingBrokenCause in _dailyOperationWarpingBrokenCauseRepository.Query
                                                         on brokenCause.Identity equals warpingBrokenCause.BrokenCauseId
                                                         into joinWarpingBrokenCause
                                 from brokenCauseJoinWarpingBrokenCause in joinWarpingBrokenCause.DefaultIfEmpty()

                                 join warpingBeamProduct in _dailyOperationWarpingBeamProductRepository.Query.Where(o=>o.LatestDateTimeBeamProduct.Year == year)
                                                         on brokenCauseJoinWarpingBrokenCause.DailyOperationWarpingBeamProductId equals warpingBeamProduct.Identity
                                                         into joinWarpingBeamProduct
                                 from brokenCauseJoinBeamProduct in joinWarpingBeamProduct.DefaultIfEmpty()

                                 join warpingDocument in _dailyOperationWarpingRepository.Query
                                                      on brokenCauseJoinBeamProduct.DailyOperationWarpingDocumentId equals warpingDocument.Identity
                                                      into joinWarpingDocument
                                 from beamProductJoinWarpingDocument in joinWarpingDocument.DefaultIfEmpty()

                                 join orderDocument in _orderRepository.Query.Where(o=>o.UnitId == weavingUnitId)
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
                                     DateTimeProduceBeam = brokenCauseJoinBeamProduct != null ? brokenCauseJoinBeamProduct.LatestDateTimeBeamProduct : DateTimeOffset.Now,
                                     WeavingUnitId = warpingDocumentJoinOrder.UnitId,
                                     SupplierName = orderDocumentJoinSupplier.Name,
                                     BrokenCauseName = brokenCause.WarpingBrokenCauseName,
                                     YarnName = constructionYarnDetailJoinYarn.Name,

                                     YarnId = constructionYarnDetailJoinYarn.YarnNumberId.Value,
                                     BeamLength = brokenCauseJoinBeamProduct.WarpingTotalBeamLength,
                                     beamProductJoinWarpingDocument.AmountOfCones,
                                     BrokenEachYarn = brokenCauseJoinWarpingBrokenCause.TotalBroken,
                                     BeamPerOperatorUom = brokenCauseJoinBeamProduct.WarpingTotalBeamLengthUomId
                                 })
                                 .ToList(); // ubah

            var warpingBrokenReport = new List<WarpingBrokenReportQueryDto>();
            foreach (var report in reportQueries)
            {
                var brokenReportDto = new WarpingBrokenReportQueryDto(report.DateTimeProduceBeam,
                                                                      report.WeavingUnitId,
                                                                      report.BrokenCauseName,
                                                                      report.SupplierName,
                                                                      report.YarnName,
                                                                      report.YarnId,
                                                                      report.BeamLength,
                                                                      report.AmountOfCones,
                                                                      report.BrokenEachYarn,
                                                                      report.BeamPerOperatorUom);
                warpingBrokenReport.Add(brokenReportDto);
            }

            return warpingBrokenReport;
        }

        private double GetBrokenTotalAndAverage(int uomId, double totalWarpingBeamLength, int amountOfCones, int month, int year, int weavingId)
        {
            var reportQueries = GetWarpingBrokenReportQueries(year, weavingId);

            var filteredQueries =
                reportQueries
                    .Where(o => o.DateTimeProductBeam.GetValueOrDefault().Month == month && o.DateTimeProductBeam.GetValueOrDefault().Year == year && o.WeavingUnitId == weavingId)
                    .ToList();

            var totalBroken =
                filteredQueries
                    .GroupBy(o => new { o.YarnId })
                    .Sum(s => s.Sum(sum => sum.BrokenEachYarn));

            var calculateTotal = CalculateWarpingBroken(uomId, totalWarpingBeamLength, amountOfCones, totalBroken);

            return calculateTotal;
        }

        private double GetBrokenMax(int uomId, double totalWarpingBeamLength, int amountOfCones, int year, int weavingId)
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

            var calculateMax = CalculateWarpingBroken(uomId, totalWarpingBeamLength, amountOfCones, maxBroken);

            return calculateMax;
        }

        private double GetBrokenMin(int uomId, double totalWarpingBeamLength, int amountOfCones, int year, int weavingId)
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

            var calculateMin = CalculateWarpingBroken(uomId, totalWarpingBeamLength, amountOfCones, minBroken);

            return calculateMin;
        }
    }
}
