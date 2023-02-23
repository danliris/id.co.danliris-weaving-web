using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.Operators.DataTransferObjects;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Domain.Operators.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.Operators.QueryHandlers
{
    public class OperatorQueryHandler : IOperatorQuery<OperatorListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IOperatorRepository
            _operatorRepository;

        public OperatorQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                   serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
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

        public async Task<IEnumerable<OperatorListDto>> GetAll()
        {
            var result = new List<OperatorListDto>();

            var operatorQuery =
                _operatorRepository
                    .Query
                    .OrderByDescending(o => o.CreatedDate);

            var operatorDocuments =
                _operatorRepository
                    .Find(operatorQuery);

            foreach (var operatorDocument in operatorDocuments)
            {
                //Get Weaving Unit
                await Task.Yield();
                SingleUnitResult unitData = GetUnit(operatorDocument.UnitId.Value);
                var weavingUnitName = unitData.data.Name;

                var operatorDto = new OperatorListDto(operatorDocument, weavingUnitName);

                result.Add(operatorDto);
            }

            return result;
        }

        public async Task<OperatorListDto> GetById(Guid id)
        {
            var operatorQuery =
                _operatorRepository
                    .Query
                    .OrderByDescending(o => o.CreatedDate);

            var operatorDocument =
                _operatorRepository
                    .Find(operatorQuery)
                    .Where(o => o.Identity == id)
                    .FirstOrDefault();

            //Get Weaving Unit
            await Task.Yield();
            SingleUnitResult unitData = GetUnit(operatorDocument.UnitId.Value);
            var weavingUnitName = unitData.data.Name;

            await Task.Yield();
            var result = new OperatorByIdDto(operatorDocument, weavingUnitName);

            return result;
        }
    }
}
