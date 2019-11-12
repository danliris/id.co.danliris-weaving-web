using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects.DailyOperationSizingReport;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Queries.DailyOperationSizingReport;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.QueryHandlers.DailyOperationSizingReport
{
    public class DailyOperationSizingReportQueryHandler : IDailyOperationSizingReportQuery<DailyOperationSizingReportListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingRepository;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
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

        public DailyOperationSizingReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _dailyOperationSizingRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
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

        public async Task<IEnumerable<DailyOperationSizingReportListDto>> GetReports(string machineId, 
                                                                                     string orderId, 
                                                                                     int unitId, 
                                                                                     DateTimeOffset? startDate, 
                                                                                     DateTimeOffset? endDate, 
                                                                                     string operationStatus, 
                                                                                     int page, 
                                                                                     int size)
        {
            try
            {
                //Add Shell (result) for Daily Operation Warping Report Dto
                var result = new List<DailyOperationSizingReportListDto>();

                //Query for Daily Operation Warping
                var dailyOperationSizingQuery =
                    _dailyOperationSizingRepository
                        .Query
                        .Include(o => o.SizingBeamProducts)
                        .Include(o => o.SizingHistories)
                        .AsQueryable();

                await Task.Yield();
                //Check if Order Id Null
                if (!string.IsNullOrEmpty(orderId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(orderId, out Guid orderGuid))
                    {
                        dailyOperationSizingQuery = dailyOperationSizingQuery.Where(x => x.OrderDocumentId == orderGuid);
                    }
                    else
                    {
                        return result;
                    }
                }

                //Check if Machine Id Null
                if (!string.IsNullOrEmpty(machineId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(machineId, out Guid machineGuid))
                    {
                        dailyOperationSizingQuery = dailyOperationSizingQuery.Where(x => x.MachineDocumentId == machineGuid);
                    }
                    else
                    {
                        return result;
                    }
                }

                //Check if Operation Status Null
                if (!string.IsNullOrEmpty(operationStatus))
                {
                    dailyOperationSizingQuery = dailyOperationSizingQuery.Where(x => x.OperationStatus == operationStatus);
                }

                //Get Daily Operation Sizing Data from Daily Operation Sizing Repo
                var dailyOperationSizingDocuments =
                    _dailyOperationSizingRepository
                        .Find(dailyOperationSizingQuery.OrderByDescending(x => x.CreatedDate));

                foreach(var document in dailyOperationSizingDocuments)
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

                    //Instantiate New Value if Unit Id not 0
                    if (unitId != 0)
                    {
                        orderDocuments = orderDocuments.Where(x => x.UnitId.Value == unitId);
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
                            .Where(O => O.Identity.Equals(fabricConstructionId))
                            .FirstOrDefault();
                    var constructionNumber = fabricConstructionDocument.ConstructionNumber;

                    //Get Weaving Unit
                    await Task.Yield();
                    var weavingUnitId = orderDocument.UnitId.Value;

                    SingleUnitResult unitData = GetUnit(weavingUnitId);
                    var weavingUnitName = unitData.data.Name;

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

                    //Get Recipe Code
                    var recipeCode = document.RecipeCode;

                    //Get Ne Real
                    var neReal = document.NeReal;

                    //Get Histories
                    var sizingHistories = document.SizingHistories.OrderByDescending(x => x.CreatedDate);

                    //Get First History, if Histories = null, skip This Document
                    var firstHistory = sizingHistories.LastOrDefault();     //Use This History to Get History at Preparation State
                    if (firstHistory == null)
                    {
                        continue;
                    }

                    if (startDate != null && endDate != null)
                    {
                        if (!(startDate.Value.Date <= firstHistory.DateTimeMachine.Date && firstHistory.DateTimeMachine.Date <= endDate.Value.Date))
                        {
                            continue;
                        }
                    }
                    else if (startDate != null && endDate == null)
                    {
                        if (startDate.Value.Date > firstHistory.DateTimeMachine.Date)
                        {
                            continue;
                        }
                    }
                    else if (startDate == null && endDate != null)
                    {
                        if (firstHistory.DateTimeMachine.Date > endDate.Value.Date)
                        {
                            continue;
                        }
                    }

                    //Get Preparation Date (First History (PreparationState))
                    var preparationDate = firstHistory.DateTimeMachine;

                    //Get Latest History
                    var latestHistory = sizingHistories.FirstOrDefault();   //Use This History to Get Latest History

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

                    //Get Warping Operator Group (Latest History)
                    var sizingOperatorGroup = operatorDocument.Group;

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

                    //Get Daily Operation Warping with Same Order Used in Daily Operation Sizing
                    var dailyOperationWarpingQuery =
                        _dailyOperationWarpingRepository
                            .Query
                            .Include(o=>o.WarpingBeamProducts)
                            .Include(o=>o.WarpingHistories)
                            .Where(o => o.OrderDocumentId.Equals(document.OrderDocumentId.Value))
                            .OrderByDescending(o => o.CreatedDate);
                    var dailyOperationWarpingDocument =
                        _dailyOperationWarpingRepository
                            .Find(dailyOperationWarpingQuery);

                    //Get ALL BEAM PRODUCT OF WARPING That Used Same Order With Current Sizing Operation And Add to Warping Beam Data Transfer Object
                    List<DailyOperationWarpingBeamDto> warpingListBeamProducts = new List<DailyOperationWarpingBeamDto>();
                    foreach (var warping in dailyOperationWarpingDocument)
                    {
                        foreach (var warpingBeamProduct in warping.WarpingBeamProducts)
                        {
                            await Task.Yield();
                            var warpingBeamStatus = warpingBeamProduct.BeamStatus;
                            if (warpingBeamStatus.Equals(BeamStatus.ROLLEDUP))
                            {
                                await Task.Yield();
                                var warpingBeamYarnStrands = warping.AmountOfCones;
                                var warpingBeam = new DailyOperationWarpingBeamDto(warpingBeamProduct.WarpingBeamId, warpingBeamYarnStrands);
                                warpingListBeamProducts.Add(warpingBeam);
                            }
                        }
                    }

                    var yarnStrands = 0;
                    double emptyWeight = 0;

                    //Get ONLY BEAM PRODUCT OF WARPING Used in The Current Sizing Operation And Incremented YarnStrands and EmptyWeight Value using Short Hand Operators
                    foreach (var warpingBeamProduct in warpingListBeamProducts)
                    {
                        foreach (var beamWarpingId in document.BeamsWarping)
                        {
                            await Task.Yield();
                            if (warpingBeamProduct.Id.Equals(beamWarpingId.Value))
                            {
                                //Get Beam Document
                                await Task.Yield();
                                var beamDocument =
                                    _beamRepository
                                        .Find(o => o.Identity.Equals(beamWarpingId.Value))
                                        .FirstOrDefault();

                                await Task.Yield();
                                yarnStrands += warpingBeamProduct.WarpingBeamConeAmount;
                                emptyWeight += beamDocument.EmptyWeight;
                            }
                        }
                    }

                    //Instantiate Value to DailyOperationSizingReportListDto
                    var dailyOperationSizingReport = new DailyOperationSizingReportListDto(document,
                                                                                           machineNumber,
                                                                                           orderNumber,
                                                                                           constructionNumber,
                                                                                           weavingUnitName,
                                                                                           operatorName,
                                                                                           sizingOperatorGroup,
                                                                                           preparationDate,
                                                                                           lastModifiedTime,
                                                                                           shiftName,
                                                                                           yarnStrands,
                                                                                           emptyWeight);

                    //Add DailyOperationSizingDto to List of DailyOperationSizingDto
                    result.Add(dailyOperationSizingReport);
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
