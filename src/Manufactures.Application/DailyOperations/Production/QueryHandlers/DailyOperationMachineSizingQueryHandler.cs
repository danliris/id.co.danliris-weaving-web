using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Production.DataTransferObjects;
using Manufactures.Domain.DailyOperations.Productions.Queries;
using Manufactures.Domain.DailyOperations.Productions.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Production.QueryHandlers
{
    public class DailyOperationMachineSizingQueryHandler : IDailyOperationMachineSizingDocumentQuery<DailyOperationMachineSizingListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationMachineSizingDocumentRepository
            _dailyOperationMachineSizingDocumentRepository;
        private readonly IDailyOperationMachineSizingDetailRepository
            _dailyOperationMachineSizingDetailRepository;
        private readonly IOrderRepository
            _orderRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IConstructionYarnDetailRepository
            _constructionYarnDetailRepository;

        public DailyOperationMachineSizingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage = storage;
            _dailyOperationMachineSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationMachineSizingDocumentRepository>();
            _dailyOperationMachineSizingDetailRepository =
                _storage.GetRepository<IDailyOperationMachineSizingDetailRepository>();
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

        public async Task<IEnumerable<DailyOperationMachineSizingListDto>> GetAll()
        {
            var resultListDto = new List<DailyOperationMachineSizingListDto>();

            var estimatedProductionQuery =
                _dailyOperationMachineSizingDocumentRepository
                    .Query
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var estimatedProductionDocument =
                _dailyOperationMachineSizingDocumentRepository
                    .Find(estimatedProductionQuery);

            foreach (var document in estimatedProductionDocument)
            {
                var resultDto = new DailyOperationMachineSizingListDto(document);

                resultListDto.Add(resultDto);
            }

            return resultListDto;
        }

        public async Task<DailyOperationMachineSizingListDto> GetById(Guid id)
        {
            var estimatedProductionDocument =
                _dailyOperationMachineSizingDocumentRepository
                    .Find(o => o.Identity == id)
                    .FirstOrDefault();

            SingleUnitResult unitData = GetUnit(estimatedProductionDocument.UnitId.Value);
            var weavingUnitName = unitData.data.Name;

            await Task.Yield();
            var resultDto = new ViewDailyOperationMachineSizingByIdDto(estimatedProductionDocument, weavingUnitName);

            var estimatedProductionDetails =
                _dailyOperationMachineSizingDetailRepository
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

                var resultDetailDto = new ViewDailyOperationMachineSizingDetailDto(order, construction, estimatedProductionDetail);

                resultDto.EstimatedDetails.Add(resultDetailDto);
            }

            return resultDto;
        }

        public async Task<DailyOperationMachineSizingListDto> GetByIdUpdate(Guid id)
        {
            var estimatedProductionDocument =
                   _dailyOperationMachineSizingDocumentRepository
                       .Find(o => o.Identity == id)
                       .FirstOrDefault();

            SingleUnitResult unitData = GetUnit(estimatedProductionDocument.UnitId.Value);
            var weavingUnitName = unitData.data.Name;

            await Task.Yield();
            var resultDto = new UpdateDailyOperationMachineSizingByIdDto(estimatedProductionDocument, weavingUnitName);

            var estimatedProductionDetails =
                _dailyOperationMachineSizingDetailRepository
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

                var resultDetailDto = new UpdateDailyOperationMachineSizingDetailDto(order, construction, estimatedProductionDetail);

                resultDto.EstimatedDetails.Add(resultDetailDto);
            }

            return resultDto;
        }
    }
}
