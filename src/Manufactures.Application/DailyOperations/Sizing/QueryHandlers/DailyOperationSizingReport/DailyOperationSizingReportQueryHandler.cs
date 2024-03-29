﻿using ExtCore.Data.Abstractions;
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
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.QueryHandlers.DailyOperationSizingReport
{
    public class DailyOperationSizingReportQueryHandler : IDailyOperationSizingReportQuery<DailyOperationSizingReportListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationWarpingBeamProductRepository
            _dailyOperationWarpingBeamProductRepository;
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingRepository;
        private readonly IDailyOperationSizingBeamsWarpingRepository
            _dailyOperationBeamsWarpingRepository;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IDailyOperationSizingHistoryRepository
            _dailyOperationSizingHistoryRepository;
        private readonly IOrderRepository
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
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationBeamsWarpingRepository =
                _storage.GetRepository<IDailyOperationSizingBeamsWarpingRepository>();
            _dailyOperationWarpingBeamProductRepository =
                _storage.GetRepository<IDailyOperationWarpingBeamProductRepository>();
            _dailyOperationSizingHistoryRepository =
               _storage.GetRepository<IDailyOperationSizingHistoryRepository>();
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IOrderRepository>();
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

        public async Task<(IEnumerable<DailyOperationSizingReportListDto>, int)> GetReports(string machineId, 
                                                                                            string orderId,
                                                                                            string operationStatus,
                                                                                            int unitId, 
                                                                                            DateTimeOffset? dateFrom, 
                                                                                            DateTimeOffset? dateTo,
                                                                                            int? month,
                                                                                            int? year,
                                                                                            int page, 
                                                                                            int size,
                                                                                            string order = "{}")
        {
            try
            {
                //Add Shell (result) for Daily Operation Sizing Report Dto
                var result = new List<DailyOperationSizingReportListDto>();

                //Query for Daily Operation Sizing
                var dailyOperationSizingQuery =
                    _dailyOperationSizingRepository
                    .Query
                    .AsQueryable();

                //Check if Machine Id Null
                await Task.Yield();
                if (!string.IsNullOrEmpty(machineId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(machineId, out Guid machineGuid))
                    {
                        dailyOperationSizingQuery = dailyOperationSizingQuery.Where(x => x.MachineDocumentId == machineGuid);
                    }
                    else
                    {
                        return (result, result.Count);
                    }
                }

                //Check if Order Id Null
                await Task.Yield();
                if (!string.IsNullOrEmpty(orderId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(orderId, out Guid orderGuid))
                    {
                        dailyOperationSizingQuery = dailyOperationSizingQuery.Where(x => x.OrderDocumentId == orderGuid);
                    }
                    else
                    {
                        return (result, result.Count);
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

                    //get Machine Speed
                    var machineSpeed = document.MachineSpeed;

                    //get TexSeq
                    var texSeq = document.TexSQ;

                    //get Visco
                    var visco = document.Visco;

                    //Get Histories
                    var sizingHistories = _dailyOperationSizingHistoryRepository
                            .Find(o => o.DailyOperationSizingDocumentId == document.Identity)
                            .OrderByDescending(i => i.AuditTrail.CreatedDate);

                    //Get First History, if Histories = null, skip This Document
                    var firstHistory = sizingHistories.LastOrDefault();     //Use This History to Get History at Preparation State
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
                            .Where(o => o.Identity == operatorId.Value)
                            .FirstOrDefault();
                    var operatorName = operatorDocument.CoreAccount.Name;

                    //Get Sizing Operator Group (Latest History)
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
                            .Where(o => o.Identity == shiftId.Value)
                            .FirstOrDefault();
                    var shiftName = shiftDocument.Name;

                    //Get Daily Operation Warping with Same Order Used in Daily Operation Sizing
                    //var dailyOperationWarpingQuery =
                    //    _dailyOperationWarpingRepository
                    //        .Query
                    //        .Include(o=>o.WarpingBeamProducts)
                    //        .Include(o=>o.WarpingHistories)
                    //        .Where(o => o.OrderDocumentId.Equals(document.OrderDocumentId.Value))
                    //        .OrderByDescending(o => o.CreatedDate);
                    var dailyOperationWarpingDocument =
                        _dailyOperationWarpingRepository
                            .Find(x => x.OrderDocumentId == document.OrderDocumentId.Value)
                            .OrderByDescending(x => x.AuditTrail.CreatedDate);


                    //Get ALL BEAM PRODUCT OF WARPING That Used Same Order With Current Sizing Operation And Add to Warping Beam Data Transfer Object
                    List<DailyOperationWarpingBeamDto> warpingListBeamProducts = new List<DailyOperationWarpingBeamDto>();
                    foreach (var warping in dailyOperationWarpingDocument)
                    {
                    var beamProducts =
                        _dailyOperationWarpingBeamProductRepository
                        .Find(x => x.DailyOperationWarpingDocumentId == warping.Identity);
                        foreach (var warpingBeamProduct in beamProducts)
                        {
                            await Task.Yield();
                            var warpingBeamStatus = warpingBeamProduct.BeamStatus;
                            if (warpingBeamStatus.Equals(BeamStatus.ROLLEDUP))
                            {
                                await Task.Yield();
                                var warpingBeamYarnStrands = warping.AmountOfCones;
                                var warpingBeam = new DailyOperationWarpingBeamDto(warpingBeamProduct.WarpingBeamId.Value, warpingBeamYarnStrands);
                                warpingListBeamProducts.Add(warpingBeam);
                            }
                        }
                    }

                    var yarnStrands = 0;
                    double emptyWeight = 0;


                    var sizingBeamsWarping =
                        _dailyOperationBeamsWarpingRepository
                            .Find(o => o.DailyOperationSizingDocumentId == document.Identity)
                            .OrderByDescending(x => x.AuditTrail.CreatedDate);


                    //Get ONLY BEAM PRODUCT OF WARPING Used in The Current Sizing Operation And Incremented YarnStrands and EmptyWeight Value using Short Hand Operators
                    foreach (var warpingBeamProduct in warpingListBeamProducts)
                    {
                        foreach (var beamWarpingId in sizingBeamsWarping)
                        {
                            await Task.Yield();
                            if (warpingBeamProduct.Id.Equals(beamWarpingId.Identity))
                            {
                                //Get Beam Document
                                await Task.Yield();
                                var beamDocument =
                                    _beamRepository
                                        .Find(o => o.Identity.Equals(beamWarpingId.Identity))
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

                if (!order.Contains("{}"))
                {
                    Dictionary<string, string> orderDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              orderDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(DailyOperationSizingReportListDto).GetProperty(key);

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
