using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.Orders.DataTransferObjects;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Orders.Queries;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Suppliers.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.Orders.QueryHandlers
{
    public class OrderQueryHandler : IOrderQuery<OrderListDto>, IOrderByUnitAndPeriodQuery<OrderByUnitAndPeriodDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
               _storage;
        private readonly IOrderRepository
            _orderRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IWeavingSupplierRepository
            _supplierRepository;

        public OrderQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage = storage;
            _orderRepository =
                _storage.GetRepository<IOrderRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _supplierRepository =
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

        public async Task<IEnumerable<OrderListDto>> GetAll()
        {
            var resultListDto = new List<OrderListDto>();

            var orderQuery =
                _orderRepository
                    .Query
                    .OrderByDescending(o => o.CreatedDate);

            var orders =
                _orderRepository
                    .Find(orderQuery);

            foreach (var order in orders)
            {
                //Get Weaving Unit
                await Task.Yield();
                var unitName = GetUnit(order.UnitId.Value).data.Name;

                var constructionNumber =
                    _fabricConstructionRepository
                        .Query
                        .Where(o => o.Identity == order.ConstructionDocumentId.Value)
                        .FirstOrDefault()
                        .ConstructionNumber;

                var resultDto = new OrderListDto(order);

                resultDto.SetUnit(unitName);
                resultDto.SetConstructionNumber(constructionNumber);

                resultListDto.Add(resultDto);
            }

            return resultListDto;
        }

        public async Task<OrderListDto> GetById(Guid id)
        {
            var orderDocument =
                _orderRepository
                    .Find(o=>o.Identity == id)
                    .FirstOrDefault();

            var warpOrigin =
                _supplierRepository
                    .Find(o => o.Identity == orderDocument.WarpOriginId.Value)
                    .FirstOrDefault()
                    .Name;

            var weftOrigin =
                _supplierRepository
                    .Find(o => o.Identity == orderDocument.WeftOriginId.Value)
                    .FirstOrDefault()
                    .Name;

            var constructionNumber =
                _fabricConstructionRepository
                    .Find(o => o.Identity == orderDocument.ConstructionDocumentId.Value)
                    .FirstOrDefault()
                    .ConstructionNumber;

            var unitName = GetUnit(orderDocument.UnitId.Value).data.Name;

            await Task.Yield();
            var resultByIdDto = new OrderByIdDto(orderDocument);
            resultByIdDto.SetConstructionNumber(constructionNumber);
            resultByIdDto.SetUnit(unitName);

            return resultByIdDto;
        }

        public async Task<IEnumerable<OrderByUnitAndPeriodDto>> GetByUnitAndPeriod(int month, int year, int unitId)
        {
            var resultListDto = new List<OrderByUnitAndPeriodDto>();

            var orders =
                _orderRepository
                    .Find(o=>o.UnitId == unitId && o.Period.Year == year && o.Period.Month == month);

            foreach (var order in orders)
            {
                //Get Weaving Unit
                await Task.Yield();
                var constructionDocument =
                    _fabricConstructionRepository
                        .Find(o => o.Identity == order.ConstructionDocumentId.Value)
                        .FirstOrDefault();

                var resultDto = new OrderByUnitAndPeriodDto(order, constructionDocument);

                resultListDto.Add(resultDto);
            }

            return resultListDto;
        }
    }
}
