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
        private readonly IWeavingOrderDocumentRepository 
            _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository
            _constructionDocumentRepository;
        private readonly IEstimationProductRepository
            _estimationProductRepository;
        private readonly IYarnDocumentRepository
            _yarnDocumentRepository;

        public OrderReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IWeavingOrderDocumentRepository>();
            _constructionDocumentRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _estimationProductRepository =
                _storage.GetRepository<IEstimationProductRepository>();
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

        public async Task<(IEnumerable<OrderReportListDto>, int)> GetReports(int weavingUnitId, 
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
                    _weavingOrderDocumentRepository
                        .Query
                        .AsQueryable();

                //Instantiate New Value if Unit Id not 0
                if (weavingUnitId != 0)
                {
                    orderQuery = orderQuery.Where(x => x.UnitId.Value == weavingUnitId);
                }

                if (dateFrom != null && dateTo != null)
                {
                    orderQuery = orderQuery.Where(x => x.DateOrdered.Date >= dateFrom.Value.Date && x.DateOrdered.Date <= dateTo.Value.Date);
                }
                else if (dateFrom != null && dateTo == null)
                {
                    orderQuery = orderQuery.Where(x => x.DateOrdered.Date >= dateFrom.Value.Date);
                }
                else if (dateFrom == null && dateTo != null)
                {
                    orderQuery = orderQuery.Where(x => x.DateOrdered.Date <= dateTo.Value.Date);
                }

                var orderDocuments =
                    _weavingOrderDocumentRepository
                        .Find(orderQuery)
                        .OrderByDescending(o => o.DateOrdered);
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
                                .Find(e => e.Identity.Equals(orderDocument.ConstructionId.Value))
                                .FirstOrDefault();

                        //Get Weaving Unit
                        await Task.Yield();
                        var orderWeavingUnitId = orderDocument.UnitId.Value;

                        SingleUnitResult unitData = GetUnit(orderWeavingUnitId);
                        var weavingUnitName = unitData.data.Name;

                        var estimationsQuery =
                            _estimationProductRepository
                                .Query
                                .OrderByDescending(item => item.CreatedDate);

                        var estimationDocuments =
                            _estimationProductRepository
                                .Find(estimationsQuery.Include(o => o.EstimationProducts))
                                .Where(o => o.Period.Month.Equals(orderDocument.Period.Month) && 
                                            o.Period.Year.Equals(orderDocument.Period.Year) && 
                                            o.UnitId.Value.Equals(orderDocument.UnitId.Value))
                                .ToList();

                        var warpMaterials = new List<string>();
                        foreach (var item in constructionDocument.ListOfWarp)
                        {
                            var material = 
                                _yarnDocumentRepository
                                    .Find(o => o.Identity == item.YarnId.Value)
                                    .FirstOrDefault();
                            if (material != null)
                            {
                                if (!warpMaterials.Contains(material.Name))
                                {
                                    warpMaterials.Add(material.Name);
                                }
                            }
                        }

                        var weftMaterials = new List<string>();
                        foreach (var item in constructionDocument.ListOfWarp)
                        {
                            var material = _yarnDocumentRepository.Find(o => o.Identity == item.YarnId.Value).FirstOrDefault();
                            if (material != null)
                            {
                                if (!weftMaterials.Contains(material.Name))
                                {
                                    weftMaterials.Add(material.Name);
                                }
                            }
                        }

                        var warpType = "";
                        foreach (var item in warpMaterials)
                        {
                            if (warpType == "")
                            {
                                warpType = item;
                            }
                            else
                            {
                                warpType = warpType + item;
                            }
                        }

                        var weftType = "";
                        foreach (var item in weftMaterials)
                        {
                            if (weftType == "")
                            {
                                weftType = item;
                            }
                            else
                            {
                                weftType = item + item;
                            }
                        }

                        var yarnNumber = warpType + "X" + weftType;

                        if (constructionDocument == null)
                        {
                            throw Validator.ErrorValidation(("Construction Document",
                                                             "Invalid Construction Document with Order Identity " +
                                                             orderDocument.Identity +
                                                             " Not Found"));
                        }

                        var newOrder = new OrderReportListDto(orderDocument, constructionDocument, estimationDocuments, yarnNumber, weavingUnitName);

                        result.Add(newOrder);
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
