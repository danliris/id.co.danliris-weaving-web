using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.DailyOperationWarpingReport;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Manufactures.Domain.DailyOperations.Warping.Queries.DailyOperationWarpingReport;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers.DailyOperationWarpingReport
{
    public class DailyOperationWarpingReportQueryHandler : IDailyOperationWarpingReportQuery<DailyOperationWarpingReportListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IWeavingOrderDocumentRepository
            _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IMaterialTypeRepository
            _materialTypeRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IShiftRepository
            _shiftRepository;

        public DailyOperationWarpingReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IWeavingOrderDocumentRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _materialTypeRepository =
                _storage.GetRepository<IMaterialTypeRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
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

        public async Task<(IEnumerable<DailyOperationWarpingReportListDto>, int)> GetReports(string orderId,
                                                                                      int unitId,
                                                                                      string materialId,
                                                                                      DateTimeOffset? startDate,
                                                                                      DateTimeOffset? endDate,
                                                                                      string operationStatus,
                                                                                      int page,
                                                                                      int size,
                                                                                      string order = "{}")
        {
            try
            {
                //Add Shell (result) for Daily Operation Warping Report Dto
                var result = new List<DailyOperationWarpingReportListDto>();

                //Query for Daily Operation Warping
                var dailyOperationWarpingQuery =
                    _dailyOperationWarpingRepository
                        .Query
                        .Include(o => o.WarpingBeamProducts)
                        .Include(o => o.WarpingHistories)
                        .AsQueryable();

                await Task.Yield();
                //Check if Order Id Null
                if (!string.IsNullOrEmpty(orderId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(orderId, out Guid orderGuid))
                    {
                        dailyOperationWarpingQuery = dailyOperationWarpingQuery.Where(x => x.OrderDocumentId == orderGuid);
                    }
                    else
                    {
                        return (result, result.Count);
                    }
                }

                //Check if Material Id Null
                if (!string.IsNullOrEmpty(materialId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(materialId, out Guid materialGuid))
                    {
                        dailyOperationWarpingQuery = dailyOperationWarpingQuery.Where(x => x.MaterialTypeId == materialGuid);
                    }
                    else
                    {
                        return (result, result.Count);
                    }
                }

                //Check if Operation Status Null
                if (!string.IsNullOrEmpty(operationStatus))
                {
                    dailyOperationWarpingQuery = dailyOperationWarpingQuery.Where(x => x.OperationStatus == operationStatus);
                }

                //Get Daily Operation Warping Data from Daily Operation Warping Repo
                var dailyOperationWarpingDocuments =
                    _dailyOperationWarpingRepository
                        .Find(dailyOperationWarpingQuery.OrderByDescending(x => x.CreatedDate));

                foreach (var document in dailyOperationWarpingDocuments)
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

                    //Get Material Type
                    await Task.Yield();
                    var materialTypeId = document.MaterialTypeId.Value;
                    var materialTypeQuery =
                        _materialTypeRepository
                            .Query
                            .OrderBy(o => o.CreatedDate);
                    var materialTypeDocument =
                        _materialTypeRepository
                            .Find(materialTypeQuery)
                            .Where(o => o.Identity.Equals(materialTypeId))
                            .FirstOrDefault();
                    var materialTypeName = materialTypeDocument.Name;

                    //Get Amount of Cones
                    var amountOfCones = document.AmountOfCones;

                    //Get Colour of Cones
                    var colourOfCones = document.ColourOfCone;

                    //Get Histories
                    var warpingHistories = document.WarpingHistories.OrderByDescending(x => x.CreatedDate);

                    //Get First History, if Histories = null, skip This Document
                    var firstHistory = warpingHistories.LastOrDefault();     //Use This History to Get History at Preparation State
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
                    var latestHistory = warpingHistories.FirstOrDefault();   //Use This History to Get Latest History

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
                    var warpingOperatorGroup = operatorDocument.Group;

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

                    //Instantiate Value to DailyOperationWarpingReportListDto
                    var dailyOperationWarpingReport = new DailyOperationWarpingReportListDto(document,
                                                                                             orderNumber,
                                                                                             constructionNumber,
                                                                                             weavingUnitName,
                                                                                             materialTypeName,
                                                                                             operatorName,
                                                                                             warpingOperatorGroup,
                                                                                             preparationDate,
                                                                                             lastModifiedTime,
                                                                                             shiftName);

                    //Add DailyOperationWarpingDto to List of DailyOperationWarpingDto
                    result.Add(dailyOperationWarpingReport);
                }

                if (!order.Contains("{}"))
                {
                    Dictionary<string, string> orderDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              orderDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(DailyOperationWarpingReportListDto).GetProperty(key);

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
