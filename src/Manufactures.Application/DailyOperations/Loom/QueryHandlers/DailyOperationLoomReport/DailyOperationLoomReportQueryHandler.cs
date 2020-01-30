using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects.DailyOperationLoomReport;
using Manufactures.Domain.DailyOperations.Loom.Queries.DailyOperationLoomReport;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Suppliers.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.QueryHandlers.DailyOperationLoomReport
{
    public class DailyOperationLoomReportQueryHandler : IDailyOperationLoomReportQuery<DailyOperationLoomReportListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomRepository;
        private readonly IOrderRepository
            _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IWeavingSupplierRepository
            _weavingSupplierRepository;

        public DailyOperationLoomReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _dailyOperationLoomRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IOrderRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _weavingSupplierRepository =
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

        public async Task<(IEnumerable<DailyOperationLoomReportListDto>, int)> GetReports(string orderId, 
                                                                                          string constructionId,
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
                //Add Shell (result) for Daily Operation Loom Report Dto
                var result = new List<DailyOperationLoomReportListDto>();

                //Query for Daily Operation Loom
                var dailyOperationLoomQuery =
                    _dailyOperationLoomRepository
                        .Query
                        .Include(o => o.LoomBeamProducts)
                        .Include(o => o.LoomBeamHistories)
                        .AsQueryable();

                //Check if Order Id Null
                await Task.Yield();
                if (!string.IsNullOrEmpty(orderId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(orderId, out Guid orderGuid))
                    {
                        dailyOperationLoomQuery = dailyOperationLoomQuery.Where(x => x.OrderDocumentId == orderGuid);
                    }
                    else
                    {
                        return (result, result.Count);
                    }
                }

                //Check if Operation Status Null
                await Task.Yield();
                if (!string.IsNullOrEmpty(operationStatus))
                {
                    dailyOperationLoomQuery = dailyOperationLoomQuery.Where(x => x.OperationStatus == operationStatus);
                }

                //Get Daily Operation Loom Data from Daily Operation Loom Repo
                var dailyOperationLoomDocuments =
                    _dailyOperationLoomRepository
                        .Find(dailyOperationLoomQuery.OrderByDescending(x => x.CreatedDate));

                foreach(var loomDocument in dailyOperationLoomDocuments)
                {
                    //Get Order Production Number
                    await Task.Yield();
                    var orderDocumentId = loomDocument.OrderDocumentId.Value;
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
                            orderDocuments = orderDocuments.Where(x => x.ConstructionDocumentId.Value == constructionGuid);
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
                    var fabricConstructionId = orderDocument.ConstructionDocumentId.Value;
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

                    //Get Warp Origin Code and Weft Origin Code
                    await Task.Yield();
                    var warpId = orderDocument.WarpOriginId;
                    var weftId = orderDocument.WeftOriginId;

                    await Task.Yield();
                    var warpCode =
                        _weavingSupplierRepository
                            .Find(o => o.Identity.ToString().Equals(warpId))
                            .FirstOrDefault()
                            .Code;

                    await Task.Yield();
                    var weftCode =
                        _weavingSupplierRepository
                            .Find(o => o.Identity.ToString().Equals(weftId))
                            .FirstOrDefault()
                            .Code;

                    //Get Histories
                    var loomHistories = loomDocument.LoomBeamHistories.OrderByDescending(x => x.CreatedDate);

                    //Get First History, if Histories = null, skip This Document
                    var firstHistory = loomHistories.LastOrDefault();     //Use This History to Get History at Preparation State
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
                    var latestHistory = loomHistories.FirstOrDefault();   //Use This History to Get Latest History

                    //Get Last Modified Time
                    var lastModifiedTime = latestHistory.DateTimeMachine.TimeOfDay;

                    //Instantiate Value to DailyOperationLoomReportListDto
                    var dailyOperationLoomReport = new DailyOperationLoomReportListDto(loomDocument,
                                                                                       orderNumber,
                                                                                       constructionNumber,
                                                                                       weavingUnitName,
                                                                                       warpCode,
                                                                                       weftCode,
                                                                                       preparationDate,
                                                                                       lastModifiedTime);

                    //Add DailyOperationLoomDto to List of DailyOperationLoomDto
                    result.Add(dailyOperationLoomReport);
                }

                if (!order.Contains("{}"))
                {
                    Dictionary<string, string> orderDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              orderDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(DailyOperationLoomReportListDto).GetProperty(key);

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
