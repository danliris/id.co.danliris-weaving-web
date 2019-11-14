using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects.DailyOperationReachingReport;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Reaching.Queries.DailyOperationReachingReport;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.QueryHandlers.DailyOperationReachingReport
{
    public class DailyOperationReachingReportQueryHandler : IDailyOperationReachingReportQuery<DailyOperationReachingReportListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingRepository;
        private readonly IWeavingOrderDocumentRepository
            _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IShiftRepository
            _shiftRepository;
        private readonly IBeamRepository
            _beamRepository;

        public DailyOperationReachingReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _dailyOperationReachingRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IWeavingOrderDocumentRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
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

        public async Task<IEnumerable<DailyOperationReachingReportListDto>> GetReports(string machineId,
                                                                                       string orderId,
                                                                                       string constructionId,
                                                                                       string beamId,
                                                                                       string operationStatus,
                                                                                       int unitId,
                                                                                       DateTimeOffset? dateFrom,
                                                                                       DateTimeOffset? dateTo,
                                                                                       int page,
                                                                                       int size,
                                                                                       string order = "{}")
        {
            try
            {
                //Add Shell (result) for Daily Operation Reaching Report Dto
                var result = new List<DailyOperationReachingReportListDto>();

                //Query for Daily Operation Reaching
                var dailyOperationReachingQuery =
                    _dailyOperationReachingRepository
                        .Query
                        .Include(o => o.ReachingHistories)
                        .AsQueryable();

                //Check if Machine Id Null
                await Task.Yield();
                if (!string.IsNullOrEmpty(machineId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(machineId, out Guid machineGuid))
                    {
                        dailyOperationReachingQuery = dailyOperationReachingQuery.Where(o => o.MachineDocumentId.Value == machineGuid);
                    }
                    else
                    {
                        return result;
                    }
                }

                //Check if Order Id Null
                await Task.Yield();
                if (!string.IsNullOrEmpty(orderId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(orderId, out Guid orderGuid))
                    {
                        dailyOperationReachingQuery = dailyOperationReachingQuery.Where(x => x.OrderDocumentId == orderGuid);
                    }
                    else
                    {
                        return result;
                    }
                }

                //Check if Beam Id Null
                if (!string.IsNullOrEmpty(beamId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(beamId, out Guid beamGuid))
                    {
                        dailyOperationReachingQuery = dailyOperationReachingQuery.Where(x => x.SizingBeamId == beamGuid);
                    }
                    else
                    {
                        return result;
                    }
                }

                //Check if Operation Status Null
                await Task.Yield();
                if (!string.IsNullOrEmpty(operationStatus))
                {
                    dailyOperationReachingQuery = dailyOperationReachingQuery.Where(x => x.OperationStatus == operationStatus);
                }

                //Get Daily Operation Sizing Data from Daily Operation Sizing Repo
                var dailyOperationReachingDocuments =
                    _dailyOperationReachingRepository
                        .Find(dailyOperationReachingQuery.OrderByDescending(x => x.CreatedDate));

                foreach(var document in dailyOperationReachingDocuments)
                {
                    //Get Order Production Number
                    await Task.Yield();
                    var orderDocumentId = document.OrderDocumentId.Value;
                    var orderDocumentQuery =
                        _weavingOrderDocumentRepository
                            .Query
                            .OrderByDescending(o => o.CreatedDate);

                    var orderDocuments =
                        _weavingOrderDocumentRepository
                            .Find(orderDocumentQuery)
                            .Where(o => o.Identity.Equals(orderDocumentId));

                    //Instantiate New Value on orderDocuments if Unit Id not 0
                    if (unitId != 0)
                    {
                        orderDocuments = orderDocuments.Where(x => x.UnitId.Value == unitId);
                    }

                    //Instantiate New Value on orderDocuments if constructionId not Null
                    if (!string.IsNullOrEmpty(constructionId))
                    {
                        //Parse if Not Null
                        if (Guid.TryParse(constructionId, out Guid constructionGuid))
                        {
                            orderDocuments = orderDocuments.Where(x => x.ConstructionId.Value == constructionGuid);
                        }
                    }

                    //Get First Element from Order Documents to Get Order Number
                    var orderDocument = orderDocuments.FirstOrDefault();
                    if (orderDocument == null)
                    {
                        continue;
                    }
                    var orderNumber = orderDocument.OrderNumber;

                    //Get Construction Number
                    await Task.Yield();
                    var fabricConstructionId = orderDocument.ConstructionId.Value;
                    var fabricConstructionQuery =
                        _fabricConstructionRepository
                            .Query
                            .OrderBy(o => o.CreatedDate);
                    var fabricConstructionDocument =
                        _fabricConstructionRepository
                            .Find(fabricConstructionQuery)
                            .Where(o => o.Identity.Equals(fabricConstructionId))
                            .FirstOrDefault();
                    var constructionNumber = fabricConstructionDocument.ConstructionNumber;

                    //Get Weaving Unit
                    await Task.Yield();
                    var weavingUnitId = orderDocument.UnitId.Value;

                    SingleUnitResult unitData = GetUnit(weavingUnitId);
                    var weavingUnitName = unitData.data.Name;

                    //Get Beam Number
                    var beamQuery =
                        _beamRepository
                            .Query
                            .OrderByDescending(o => o.CreatedDate);

                    var beamDocuments =
                        _beamRepository
                            .Find(beamQuery)
                            .Where(o => o.Identity.Equals(document.SizingBeamId.Value));

                    //Get First Element from Machine Documents to Get Machine Number
                    var beamDocument = beamDocuments.FirstOrDefault();
                    if (beamDocument == null)
                    {
                        continue;
                    }
                    var beamNumber = beamDocument.Number;

                    //Get Machine Number
                    await Task.Yield();
                    var weavingMachineId = document.MachineDocumentId.Value;
                    var weavingMachineQuery =
                        _machineRepository
                            .Query
                            .OrderByDescending(o => o.CreatedDate);
                    var machineDocuments =
                        _machineRepository
                            .Find(weavingMachineQuery)
                            .Where(o => o.Identity.Equals(weavingMachineId));

                    //Get First Element from Machine Documents to Get Machine Number
                    var machineDocument = machineDocuments.FirstOrDefault();
                    if (orderDocument == null)
                    {
                        continue;
                    }
                    var machineNumber = machineDocument.MachineNumber;

                    //Get Histories
                    var reachingHistories = document.ReachingHistories.OrderByDescending(x => x.CreatedDate);

                    //Get First History, if Histories = null, skip This Document
                    var firstHistory = reachingHistories.LastOrDefault();     //Use This History to Get History at Preparation State
                    if (firstHistory == null)
                    {
                        continue;
                    }

                    if (dateFrom != null && dateTo != null)
                    {
                        if (!(dateFrom.Value.Date <= firstHistory.DateTimeMachine.Date && firstHistory.DateTimeMachine.Date <= dateTo.Value.Date))
                        {
                            continue;
                        }
                    }
                    else if (dateFrom != null && dateTo == null)
                    {
                        if (dateFrom.Value.Date > firstHistory.DateTimeMachine.Date)
                        {
                            continue;
                        }
                    }
                    else if (dateFrom == null && dateTo != null)
                    {
                        if (firstHistory.DateTimeMachine.Date > dateTo.Value.Date)
                        {
                            continue;
                        }
                    }

                    //Get Preparation Date (First History (PreparationState))
                    var preparationDate = firstHistory.DateTimeMachine;

                    //Get Latest History
                    var latestHistory = reachingHistories.FirstOrDefault();   //Use This History to Get Latest History

                    //Get Last Modified Time
                    var lastModifiedTime = latestHistory.DateTimeMachine.TimeOfDay;

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
                    var reachingOperatorGroup = operatorDocument.Group;

                    //Get Shift (Latest History)
                    var shiftId = latestHistory.ShiftDocumentId;
                    var shiftQuery =
                    _shiftRepository
                        .Query
                        .OrderByDescending(o => o.CreatedDate);
                    var shiftDocument =
                        _shiftRepository
                            .Find(shiftQuery)
                            .Where(o => o.Identity.Equals(shiftId))
                            .FirstOrDefault();
                    var shiftName = shiftDocument.Name;

                    //Instantiate Value to DailyOperationReachingReportListDto
                    var dailyOperationReachingReport = new DailyOperationReachingReportListDto(document,
                                                                                               machineNumber,
                                                                                               orderNumber,
                                                                                               constructionNumber,
                                                                                               weavingUnitName,
                                                                                               beamNumber,
                                                                                               operatorName,
                                                                                               reachingOperatorGroup,
                                                                                               preparationDate,
                                                                                               lastModifiedTime,
                                                                                               shiftName);

                    //Add DailyOperationReachingDto to List of DailyOperationReachingDto
                    result.Add(dailyOperationReachingReport);
                }

                if (!order.Contains("{}"))
                {
                    Dictionary<string, string> orderDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              orderDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(DailyOperationReachingReportListDto).GetProperty(key);

                    if (orderDictionary.Values.Contains("asc"))
                    {
                        result = result.OrderBy(x => prop.GetValue(x, null)).ToList();
                    }
                    else
                    {
                        result = result.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                    }
                }
                    return result.Skip((page - 1) * size).Take(size);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
