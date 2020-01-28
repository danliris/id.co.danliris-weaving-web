using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.Estimations.Productions.DataTransferObjects;
using Manufactures.Domain.Estimations.Productions.Queries;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.Estimations.Productions.QueryHandlers
{
    public class EstimatedProductionQueryHandler : IEstimatedProductionDocumentQuery<EstimatedProductionListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IEstimatedProductionDocumentRepository
            _estimatedProductionDocumentRepository;
        private readonly IEstimatedProductionDetailRepository
            _estimatedProductionDetailRepository;
        private readonly IOrderRepository
            _orderRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IConstructionYarnDetailRepository
            _constructionYarnDetailRepository;

        public EstimatedProductionQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage = storage;
            _estimatedProductionDocumentRepository =
                _storage.GetRepository<IEstimatedProductionDocumentRepository>();
            _estimatedProductionDetailRepository =
                _storage.GetRepository<IEstimatedProductionDetailRepository>();
            _orderRepository =
                _storage.GetRepository<IOrderRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _constructionYarnDetailRepository =
                _storage.GetRepository<IConstructionYarnDetailRepository>();
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

        public async Task<IEnumerable<EstimatedProductionListDto>> GetAll()
        {
            var resultListDto = new List<EstimatedProductionListDto>();

            var estimatedProductionQuery =
                _estimatedProductionDocumentRepository
                    .Query
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var estimatedProductionDocument =
                _estimatedProductionDocumentRepository
                    .Find(estimatedProductionQuery);

            foreach (var document in estimatedProductionDocument)
            {
                var resultDto = new EstimatedProductionListDto(document);

                resultListDto.Add(resultDto);
            }

            return resultListDto;
        }

        public async Task<EstimatedProductionListDto> GetById(Guid id)
        {
            var estimatedProductionDocument =
                _estimatedProductionDocumentRepository
                    .Find(o => o.Identity == id)
                    .FirstOrDefault();

            SingleUnitResult unitData = GetUnit(estimatedProductionDocument.UnitId.Value);
            var weavingUnitName = unitData.data.Name;

            await Task.Yield();
            var resultDto = new EstimatedProductionByIdDto(estimatedProductionDocument, weavingUnitName);

            var estimatedProductionDetails =
                _estimatedProductionDetailRepository
                    .Find(o => o.EstimatedProductionDocumentId == estimatedProductionDocument.Identity);

            foreach (var estimatedProductionDetail in estimatedProductionDetails)
            {
                var order =
                    _orderRepository
                        .Find(o => o.Identity == estimatedProductionDetail.OrderId.Value)
                        .FirstOrDefault();

                var construction =
                    _fabricConstructionRepository
                        .Find(o => o.Identity == estimatedProductionDetail.ConstructionId.Value)
                        .FirstOrDefault();

                var resultDetailDto = new EstimatedProductionDetailDto(order, construction, estimatedProductionDetail);

                resultDto.EstimatedDetails.Add(resultDetailDto);
            }

            return resultDto;
        }
    }
}
