using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Sizing.Calculations.SizePickupReport;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects.SizePickupReport;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Queries.SizePickupReport;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Materials.Repositories;
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
        private readonly IDailyOperationSizingHistoryRepository
            _dailyOperationSizingHistoryRepository;
        private readonly IDailyOperationSizingBeamProductRepository
            _dailyOperationSizingBeamProductRepository;
        private readonly IOrderRepository
            _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IMaterialTypeRepository
            _materialTypeRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IShiftRepository
            _shiftRepository;

        public SizePickupReportQueryHandler(IStorage storage)
        {
            //_http =
            //    serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _dailyOperationSizingRepository =
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationSizingHistoryRepository =
                _storage.GetRepository<IDailyOperationSizingHistoryRepository>();
            _dailyOperationSizingBeamProductRepository =
                _storage.GetRepository<IDailyOperationSizingBeamProductRepository>();
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
            _materialTypeRepository =
                _storage.GetRepository<IMaterialTypeRepository>();
        }

        public async Task<(IEnumerable<SizePickupReportListDto>, int)> GetReportsAsync(string shiftId,
                                                                            string spuStatus,
                                                                            int unitId,
                                                                            DateTimeOffset? date,
                                                                            DateTimeOffset? dateFrom,
                                                                            DateTimeOffset? dateTo, 
                                                                            int? month, int? year,
                                                                            int page, int size, string order = "{}")
        {
            try
            {
                //Add Shell (result) for Daily Operation Sizing Report Dto
                var result = new List<SizePickupReportListDto>();

                //Get Daily Operation Sizing Data from Daily Operation Sizing Repo
                var sizePickupQuery =
                    _dailyOperationSizingRepository
                        .Query
                        .AsQueryable();

                var sizingDocuments =
                    _dailyOperationSizingRepository
                        .Find(sizePickupQuery)
                        .OrderByDescending(x => x.AuditTrail.CreatedDate);


                foreach (var sizingDocument in sizingDocuments)
                {
                    //Get Beam Product
                    var sizingBeamProducts = 
                        _dailyOperationSizingBeamProductRepository
                            .Find(o=>o.DailyOperationSizingDocumentId == sizingDocument.Identity && o.BeamStatus == BeamStatus.ROLLEDUP)
                            .OrderByDescending(x => x.AuditTrail.CreatedDate);
                    foreach (var beamProduct in sizingBeamProducts)
                    {
                        //Get Order Production Number
                        await Task.Yield();
                        var orderDocumentQuery =
                            _weavingOrderDocumentRepository
                                .Query
                                .OrderByDescending(o => o.CreatedDate);

                        var orderDocumentFind =
                            _weavingOrderDocumentRepository
                                .Find(orderDocumentQuery);

                        var orderDocuments =
                            orderDocumentFind
                                .Where(o => o.Identity.Equals(sizingDocument.OrderDocumentId.Value));

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

                        //Get Order Number
                        var orderNumber = orderDocument.OrderNumber;

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

                        //Filter Standart Material Type
                        await Task.Yield();

                        var materialTypeQuery =
                            _materialTypeRepository
                                .Query
                                .OrderBy(o => o.CreatedDate);

                        var materialType = fabricConstructionDocument.MaterialType;

                        var materialTypeDocument =
                            _materialTypeRepository
                                .Find(materialTypeQuery)
                                .Where(m => m.Name.Equals(materialType))
                                .FirstOrDefault();

                        var materialCode = materialTypeDocument.Code;

                        var pc = new List<string>() { "A", "B", "C", "PV", "AA" };
                        var cotton = new List<string>() { "G", "M", "O", "R", "RN", "RR", "RS", "J", "Z", "Q", "F" };
                        var pe = new List<string>() { "L", "N", "S", "X", "H", "T" };
                        var rayon = new List<string>() { "Y", "YV" };

                        var materialPC = pc
                            .Where(m => m.Contains(materialCode))
                            .FirstOrDefault();

                        var materialCotton = cotton
                           .Where(m => m.Contains(materialCode))
                           .FirstOrDefault();

                        var materialPE = pe
                           .Where(m => m.Contains(materialCode))
                           .FirstOrDefault();

                        var materialRayon = rayon
                           .Where(m => m.Contains(materialCode))
                           .FirstOrDefault();

                        if (materialCotton != null)
                        {
                            filteredConstructionNumber = "COTTON";
                        }

                        if (materialPC != null)
                        {
                            filteredConstructionNumber = "PC";
                        }

                        if (materialPE != null)
                        {
                            filteredConstructionNumber = "PE";
                        }

                        if (materialRayon != null)
                        {
                            filteredConstructionNumber = "RAYON";
                        }

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

                        ////Get Histories
                        //var sizingHistories = 
                        //    _dailyOperationSizingHistoryRepository
                        //        .Find(o=>o.DailyOperationSizingDocumentId == sizingDocument.Identity && o.MachineStatus == MachineStatus.ONFINISH)
                        //        .OrderByDescending(x => x.AuditTrail.CreatedDate)
                        //        .AsQueryable();

                        //Get Histories
                        var sizingHistories = _dailyOperationSizingHistoryRepository
                                .Find(o => o.DailyOperationSizingDocumentId == sizingDocument.Identity)
                                .OrderByDescending(i => i.AuditTrail.CreatedDate)
                                .AsQueryable();


                        if (shiftIdHistory != null)
                        {
                            sizingHistories = sizingHistories.Where(o => o.ShiftDocumentId.Value == shiftIdHistory);
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

                        else if (month != 0 && year != 0)
                        {
                            if ((latestHistory.DateTimeMachine.Month != month) || (latestHistory.DateTimeMachine.Year != year))
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
                                .Where(o => o.Identity == operatorId.Value)
                                .FirstOrDefault();
                        var operatorName = operatorDocument.CoreAccount.Name;

                        //Get Sizing Operator Group (Latest History)
                        var sizingOperatorGroup = operatorDocument.Group;

                        //Get Beam Number
                        var beamQuery =
                            _beamRepository
                                .Query
                                .Where(o => o.Identity == beamProduct.SizingBeamId.Value)
                                .OrderByDescending(o => o.CreatedDate);

                        var beamNumber =
                            _beamRepository
                                .Find(beamQuery)
                                .FirstOrDefault()
                                .Number;

                        var sizePickupReport = new SizePickupReportListDto();
                        var spuBeamProduct = beamProduct.SPU;

                        if (shiftId != null)
                        {
                            if (!(shiftId == shiftIdHistory.ToString()))
                            {
                                continue;
                            }
                        }

                        var category = "";

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
                                                                                   beamProduct.CounterStart,
                                                                                   beamProduct.CounterFinish,
                                                                                   beamProduct.WeightNetto,
                                                                                   beamProduct.WeightBruto,
                                                                                   beamProduct.PISMeter,
                                                                                   spuBeamProduct,
                                                                                   beamNumber,
                                                                                   resultPC,
                                                                                   orderNumber);

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
                                                                                   beamProduct.CounterStart,
                                                                                   beamProduct.CounterFinish,
                                                                                   beamProduct.WeightNetto,
                                                                                   beamProduct.WeightBruto,
                                                                                   beamProduct.PISMeter,
                                                                                   spuBeamProduct,
                                                                                   beamNumber,
                                                                                   resultCVC,
                                                                                   orderNumber);

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
                                                                                   beamProduct.CounterStart,
                                                                                   beamProduct.CounterFinish,
                                                                                   beamProduct.WeightNetto,
                                                                                   beamProduct.WeightBruto,
                                                                                   beamProduct.PISMeter,
                                                                                   spuBeamProduct,
                                                                                   beamNumber,
                                                                                   resultCotton,
                                                                                   orderNumber);

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
                                                                                   beamProduct.CounterStart,
                                                                                   beamProduct.CounterFinish,
                                                                                   beamProduct.WeightNetto,
                                                                                   beamProduct.WeightBruto,
                                                                                   beamProduct.PISMeter,
                                                                                   spuBeamProduct,
                                                                                   beamNumber,
                                                                                   resultPE,
                                                                                   orderNumber);

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
                                                                                   beamProduct.CounterStart,
                                                                                   beamProduct.CounterFinish,
                                                                                   beamProduct.WeightNetto,
                                                                                   beamProduct.WeightBruto,
                                                                                   beamProduct.PISMeter,
                                                                                   spuBeamProduct,
                                                                                   beamNumber,
                                                                                   resultRayon,
                                                                                   orderNumber);

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
                                                                               beamProduct.CounterStart,
                                                                               beamProduct.CounterFinish,
                                                                               beamProduct.WeightNetto,
                                                                               beamProduct.WeightBruto,
                                                                               beamProduct.PISMeter,
                                                                               spuBeamProduct,
                                                                               beamNumber,
                                                                               category,
                                                                               orderNumber);

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

        public Task GetReports(string v1, string v2, int v3, object p1, object p2, object p3, object p4, int v4, int v5, int v6, int v7, string v8)
        {
            throw new NotImplementedException();
        }
    }
}
