using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Sizing.Calculations.SizePickupReport;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects.SizePickupReport;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Queries.SizePickupReport;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.QueryHandlers.SizePickupReport
{
    public class SizePickupReportQueryHandler : ISizePickupReportQuery<SizePickupReportListDto>
    {
        //protected readonly IHttpClientService
        //    _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingRepository;
        private readonly IOrderRepository
            _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IShiftRepository
            _shiftRepository;

        public SizePickupReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            //_http =
            //    serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _dailyOperationSizingRepository =
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IOrderRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
        }

        public async Task<(IEnumerable<SizePickupReportListDto>, int)> GetReports(string shiftId,
                                                                                  string spuStatus,
                                                                                  int unitId,
                                                                                  DateTimeOffset? date,
                                                                                  DateTimeOffset? dateFrom,
                                                                                  DateTimeOffset? dateTo,
                                                                                  int? month,
                                                                                  int page,
                                                                                  int size,
                                                                                  string order = "{}")
        {
            try
            {
                //Add Shell (result) for Daily Operation Sizing Report Dto
                var result = new List<SizePickupReportListDto>();

                //Query for Daily Operation Sizing
                var dailyOperationSizingQuery =
                    _dailyOperationSizingRepository
                        .Query
                        .Include(o => o.SizingBeamProducts)
                        .Include(o => o.SizingHistories)
                        .Where(o => o.OperationStatus.Equals(OperationStatus.ONFINISH))
                        .AsQueryable();

                //Get Daily Operation Sizing Data from Daily Operation Sizing Repo
                var dailyOperationSizingDocuments =
                    _dailyOperationSizingRepository
                        .Find(dailyOperationSizingQuery.OrderByDescending(x => x.CreatedDate));

                foreach (var sizingDocument in dailyOperationSizingDocuments)
                {
                    //Get Beam Product
                    var sizingBeamProducts = sizingDocument.SizingBeamProducts.Where(o => o.BeamStatus.Equals(BeamStatus.ROLLEDUP)).OrderByDescending(x => x.CreatedDate);
                    foreach (var beamProduct in sizingBeamProducts)
                    {
                        //Get Order Production Number
                        await Task.Yield();
                        var orderDocumentId = sizingDocument.OrderDocumentId.Value;
                        var orderDocumentQuery =
                            _weavingOrderDocumentRepository
                                .Query
                                .OrderByDescending(o => o.CreatedDate);

                        var orderDocuments =
                            _weavingOrderDocumentRepository
                                .Find(orderDocumentQuery)
                                .Where(o => o.Identity.Equals(orderDocumentId));

                        //Instantiate New Value if Unit Id not 0
                        if (unitId != 0)
                        {
                            orderDocuments = orderDocuments.Where(x => x.UnitId.Value == unitId);
                        }

                        //Get First Element from Order Documents
                        var orderDocument = orderDocuments.FirstOrDefault();
                        if (orderDocument == null)
                        {
                            continue;
                        }

                        //Get Construction Number
                        await Task.Yield();
                        var fabricConstructionId = orderDocument.ConstructionDocumentId.Value;
                        var fabricConstructionQuery =
                            _fabricConstructionRepository
                                .Query
                                .OrderBy(o => o.CreatedDate);
                        var fabricConstructionDocument =
                            _fabricConstructionRepository
                                .Find(fabricConstructionQuery)
                                .Where(O => O.Identity.Equals(fabricConstructionId))
                                .FirstOrDefault();
                        var constructionNumber = fabricConstructionDocument.ConstructionNumber;

                        //Filter Construction Number to Calculate SPU Constants
                        string[] splittedConstructionNumber = constructionNumber.Split(" ");
                        var charToTrim = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.', '/' };
                        var filteredConstructionNumber = splittedConstructionNumber[0].TrimEnd(charToTrim);

                        //Get Shift (Latest History)
                        var shiftQuery =
                        _shiftRepository
                            .Query
                            .AsQueryable();

                        if (!string.IsNullOrEmpty(shiftId))
                        {
                            if (Guid.TryParse(shiftId, out Guid shiftGuid))
                            {
                                shiftQuery = shiftQuery.Where(o => o.Identity.Equals(shiftGuid));
                            }
                            else
                            {
                                return (result, result.Count);
                            }
                        }
                        var shiftIdHistory =
                            _shiftRepository
                                .Find(shiftQuery)
                                .FirstOrDefault()
                                .Identity;

                        //Get Histories
                        var sizingHistories = sizingDocument.SizingHistories.Where(o => o.MachineStatus.Equals(MachineStatus.ONFINISH)).OrderByDescending(x => x.CreatedDate).AsQueryable();

                        if (shiftIdHistory != null)
                        {
                            sizingHistories = sizingHistories.Where(o => o.ShiftDocumentId.Equals(shiftIdHistory));
                        }

                        //Get Latest History, if Histories = null, skip This Document
                        var latestHistory = sizingHistories.FirstOrDefault();     //Use This History to Get Latest History
                        if (latestHistory == null)
                        {
                            continue;
                        }

                        if (date != null)
                        {
                            if (!(date.Value.Date == latestHistory.DateTimeMachine.Date))
                            {
                                continue;
                            }
                        }

                        else if (dateFrom != null && dateTo != null)
                        {
                            if (!(dateFrom.Value.Date <= latestHistory.DateTimeMachine.Date && latestHistory.DateTimeMachine.Date <= dateTo.Value.Date))
                            {
                                continue;
                            }
                        }
                        else if (dateFrom != null && dateTo == null)
                        {
                            if (dateFrom.Value.Date > latestHistory.DateTimeMachine.Date)
                            {
                                continue;
                            }
                        }
                        else if (dateFrom == null && dateTo != null)
                        {
                            if (latestHistory.DateTimeMachine.Date > dateTo.Value.Date)
                            {
                                continue;
                            }
                        }

                        else if (month != 0)
                        {
                            if (!(latestHistory.DateTimeMachine.Month == month))
                            {
                                continue;
                            }
                        }

                        //Get Preparation Date (First History (PreparationState))
                        var latestDateTimeHistory = latestHistory.DateTimeMachine;

                        //Get Operator Name (Latest History)
                        var operatorId = latestHistory.OperatorDocumentId;
                        var operatorQuery =
                            _operatorRepository
                                .Query
                                .OrderByDescending(o => o.CreatedDate);
                        var operatorDocument =
                            _operatorRepository
                                .Find(operatorQuery)
                                .Where(o => o.Identity.Equals(operatorId))
                                .FirstOrDefault();
                        var operatorName = operatorDocument.CoreAccount.Name;

                        //Get Sizing Operator Group (Latest History)
                        var sizingOperatorGroup = operatorDocument.Group;

                        //Get Beam Number
                        var beamQuery =
                            _beamRepository
                                .Query
                                .Where(o => o.Identity.Equals(beamProduct.SizingBeamId))
                                .OrderByDescending(o => o.CreatedDate);
                        var beamNumber =
                            _beamRepository
                                .Find(beamQuery)
                                .FirstOrDefault()
                                .Number;

                        var sizePickupReport = new SizePickupReportListDto();
                        var spuBeamProduct = beamProduct.SPU ?? 0;

                        switch (filteredConstructionNumber)
                        {
                            case SizePickupSPUConstants.PCCONSTRUCTION:
                                Filtering filteringPC = new Filtering();
                                var resultPC = filteringPC.ComparingPCCVC(spuBeamProduct);
                                if (resultPC == spuStatus || spuStatus.Equals("All"))
                                {
                                    //Instantiate Value to DailyOperationSizingReportListDto
                                    sizePickupReport = new SizePickupReportListDto(sizingDocument,
                                                                                   operatorName,
                                                                                   sizingOperatorGroup,
                                                                                   latestDateTimeHistory,
                                                                                   beamProduct.CounterStart ?? 0,
                                                                                   beamProduct.CounterFinish ?? 0,
                                                                                   beamProduct.WeightNetto ?? 0,
                                                                                   beamProduct.WeightBruto ?? 0,
                                                                                   beamProduct.PISMeter ?? 0,
                                                                                   spuBeamProduct,
                                                                                   beamNumber);

                                    //Add SizePickupReportListDto to List of SizePickupReportListDto
                                    result.Add(sizePickupReport);
                                }
                                break;
                            case SizePickupSPUConstants.CVCCONSTRUCTION:
                                Filtering filteringCVC = new Filtering();
                                var resultCVC = filteringCVC.ComparingPCCVC(spuBeamProduct);
                                if (resultCVC == spuStatus || spuStatus.Equals("All"))
                                {
                                    //Instantiate Value to DailyOperationSizingReportListDto
                                    sizePickupReport = new SizePickupReportListDto(sizingDocument,
                                                                                   operatorName,
                                                                                   sizingOperatorGroup,
                                                                                   latestDateTimeHistory,
                                                                                   beamProduct.CounterStart ?? 0,
                                                                                   beamProduct.CounterFinish ?? 0,
                                                                                   beamProduct.WeightNetto ?? 0,
                                                                                   beamProduct.WeightBruto ?? 0,
                                                                                   beamProduct.PISMeter ?? 0,
                                                                                   spuBeamProduct,
                                                                                   beamNumber);

                                    //Add SizePickupReportListDto to List of SizePickupReportListDto
                                    result.Add(sizePickupReport);
                                }
                                break;
                            case SizePickupSPUConstants.COTTONCONSTRUCTION:
                                Filtering filteringCotton = new Filtering();
                                var resultCotton = filteringCotton.ComparingPCCVC(spuBeamProduct);
                                if (resultCotton == spuStatus || spuStatus.Equals("All"))
                                {
                                    //Instantiate Value to DailyOperationSizingReportListDto
                                    sizePickupReport = new SizePickupReportListDto(sizingDocument,
                                                                                   operatorName,
                                                                                   sizingOperatorGroup,
                                                                                   latestDateTimeHistory,
                                                                                   beamProduct.CounterStart ?? 0,
                                                                                   beamProduct.CounterFinish ?? 0,
                                                                                   beamProduct.WeightNetto ?? 0,
                                                                                   beamProduct.WeightBruto ?? 0,
                                                                                   beamProduct.PISMeter ?? 0,
                                                                                   spuBeamProduct,
                                                                                   beamNumber);

                                    //Add SizePickupReportListDto to List of SizePickupReportListDto
                                    result.Add(sizePickupReport);
                                }
                                break;
                            case SizePickupSPUConstants.PECONSTRUCTION:
                                Filtering filteringPE = new Filtering();
                                var resultPE = filteringPE.ComparingPCCVC(spuBeamProduct);
                                if (resultPE == spuStatus || spuStatus.Equals("All"))
                                {
                                    //Instantiate Value to DailyOperationSizingReportListDto
                                    sizePickupReport = new SizePickupReportListDto(sizingDocument,
                                                                                   operatorName,
                                                                                   sizingOperatorGroup,
                                                                                   latestDateTimeHistory,
                                                                                   beamProduct.CounterStart ?? 0,
                                                                                   beamProduct.CounterFinish ?? 0,
                                                                                   beamProduct.WeightNetto ?? 0,
                                                                                   beamProduct.WeightBruto ?? 0,
                                                                                   beamProduct.PISMeter ?? 0,
                                                                                   spuBeamProduct,
                                                                                   beamNumber);

                                    //Add SizePickupReportListDto to List of SizePickupReportListDto
                                    result.Add(sizePickupReport);
                                }
                                break;
                            case SizePickupSPUConstants.RAYONCONSTRUCTION:
                                Filtering filteringRayon = new Filtering();
                                var resultRayon = filteringRayon.ComparingPCCVC(spuBeamProduct);
                                if (resultRayon == spuStatus || spuStatus.Equals("All"))
                                {
                                    //Instantiate Value to DailyOperationSizingReportListDto
                                    sizePickupReport = new SizePickupReportListDto(sizingDocument,
                                                                                   operatorName,
                                                                                   sizingOperatorGroup,
                                                                                   latestDateTimeHistory,
                                                                                   beamProduct.CounterStart ?? 0,
                                                                                   beamProduct.CounterFinish ?? 0,
                                                                                   beamProduct.WeightNetto ?? 0,
                                                                                   beamProduct.WeightBruto ?? 0,
                                                                                   beamProduct.PISMeter ?? 0,
                                                                                   spuBeamProduct,
                                                                                   beamNumber);

                                    //Add SizePickupReportListDto to List of SizePickupReportListDto
                                    result.Add(sizePickupReport);
                                }
                                break;
                            default:
                                //Instantiate Value to DailyOperationSizingReportListDto
                                sizePickupReport = new SizePickupReportListDto(sizingDocument,
                                                                               operatorName,
                                                                               sizingOperatorGroup,
                                                                               latestDateTimeHistory,
                                                                               beamProduct.CounterStart ?? 0,
                                                                               beamProduct.CounterFinish ?? 0,
                                                                               beamProduct.WeightNetto ?? 0,
                                                                               beamProduct.WeightBruto ?? 0,
                                                                               beamProduct.PISMeter ?? 0,
                                                                               spuBeamProduct,
                                                                               beamNumber);

                                //Add SizePickupReportListDto to List of SizePickupReportListDto
                                result.Add(sizePickupReport);
                                break;
                        }
                    }
                }

                if (!order.Contains("{}"))
                {
                    Dictionary<string, string> orderDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              orderDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(SizePickupReportListDto).GetProperty(key);

                    if (orderDictionary.Values.Contains("asc"))
                    {
                        result = result.OrderBy(x => prop.GetValue(x, null)).ToList();
                    }
                    else
                    {
                        result = result.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                    }
                }

                var pagedResult = result.Skip((page - 1) * size).Take(size);

                return (pagedResult, result.Count);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
