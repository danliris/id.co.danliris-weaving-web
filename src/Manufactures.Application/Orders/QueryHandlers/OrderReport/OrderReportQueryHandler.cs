using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.Orders.DataTransferObjects.OrderReport;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Orders.Queries.OrderReport;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moonlay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.Orders.QueryHandlers.OrderReport
{
    public class OrderReportQueryHandler : IOrderReportQuery<OrderReportListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IOrderRepository 
            _orderDocumentRepository;
        private readonly IFabricConstructionRepository
            _constructionDocumentRepository;
        private readonly IConstructionYarnDetailRepository
            _constructionYarnDetailRepository;
        private readonly IEstimatedProductionDocumentRepository
            _estimatedProductionDocumentRepository;
        private readonly IEstimatedProductionDetailRepository
            _estimatedProductionDetailRepository;
        private readonly IYarnDocumentRepository
            _yarnDocumentRepository;

        public OrderReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _orderDocumentRepository =
                _storage.GetRepository<IOrderRepository>();
            _constructionDocumentRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _constructionYarnDetailRepository =
                _storage.GetRepository<IConstructionYarnDetailRepository>();
            _estimatedProductionDocumentRepository =
                _storage.GetRepository<IEstimatedProductionDocumentRepository>();
            _estimatedProductionDetailRepository =
                _storage.GetRepository<IEstimatedProductionDetailRepository>();
            _yarnDocumentRepository =
                _storage.GetRepository<IYarnDocumentRepository>();
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

        public async Task<(IEnumerable<OrderReportListDto>, int)> GetReports(int unitId, 
                                                                             DateTimeOffset? dateFrom, 
                                                                             DateTimeOffset? dateTo, 
                                                                             int page, 
                                                                             int size, 
                                                                             string order="{}")
        {
            try
            {
                var result = new List<OrderReportListDto>();

                var orderQuery =
                    _orderDocumentRepository
                        .Query
                        .AsQueryable();
                var estimationQuery =
                    _estimatedProductionDocumentRepository
                        .Query
                        .AsQueryable();

                //Instantiate New Value if Unit Id not 0
                if (unitId != 0)
                {
                    orderQuery = orderQuery.Where(x => x.UnitId == unitId);
                    estimationQuery = estimationQuery.Where(o => o.UnitId == unitId);
                }
                else
                {
                    throw Validator.ErrorValidation(("WeavingUnit", "Unit Weaving Tidak Boleh Kosong"));
                }

                if (dateFrom != null && dateTo != null)
                {
                    orderQuery = orderQuery.Where(x => x.Period.Date >= dateFrom.Value.Date && x.Period.Date <= dateTo.Value.Date);
                    estimationQuery = estimationQuery.Where(x => x.Period.Date >= dateFrom.Value.Date && x.Period.Date <= dateTo.Value.Date);
                }
                else if (dateFrom != null && dateTo == null)
                {
                    orderQuery = orderQuery.Where(x => x.Period.Date >= dateFrom.Value.Date);
                    estimationQuery = estimationQuery.Where(x => x.Period.Date >= dateFrom.Value.Date);
                }
                else if (dateFrom == null && dateTo != null)
                {
                    orderQuery = orderQuery.Where(x => x.Period.Date <= dateTo.Value.Date);
                    estimationQuery = estimationQuery.Where(x => x.Period.Date <= dateTo.Value.Date);
                }

                var orderDocuments =
                    _orderDocumentRepository
                        .Find(orderQuery)
                        .OrderByDescending(o => o.AuditTrail.CreatedDate);
                if (orderDocuments == null)
                {
                    return (result, result.Count);
                }
                else
                {
                    foreach (var orderDocument in orderDocuments)
                    {
                        var constructionDocument =
                            _constructionDocumentRepository
                                .Find(e => e.Identity == orderDocument.ConstructionDocumentId.Value)
                                .FirstOrDefault();

                        //Get Weaving Unit
                        await Task.Yield();

                        SingleUnitResult unitData = GetUnit(orderDocument.UnitId.Value);
                        var weavingUnitName = unitData.data.Name;

                        var estimationDocuments =
                            _estimatedProductionDocumentRepository
                                .Find(estimationQuery)
                                .OrderByDescending(o => o.AuditTrail.CreatedDate);
                        var estimationDetails =
                            _estimatedProductionDetailRepository
                                .Find(o => estimationDocuments.Any(e => e.Identity == o.EstimatedProductionDocumentId) && o.OrderId == orderDocument.Identity);
                        
                        foreach(var estimationDetail in estimationDetails)
                        {
                            var newOrderReport = new OrderReportListDto(orderDocument, constructionDocument, estimationDetail, weavingUnitName);

                            result.Add(newOrderReport);
                        }
                    }

                    if (!order.Contains("{}"))
                    {
                        Dictionary<string, string> orderDictionary =
                            JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                        var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                                  orderDictionary.Keys.First().Substring(1);
                        System.Reflection.PropertyInfo prop = typeof(OrderReportListDto).GetProperty(key);

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
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
