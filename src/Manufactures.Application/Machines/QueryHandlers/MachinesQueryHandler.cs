using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.Machines.DataTransferObjects;
using Manufactures.Domain.Machines.Queries;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachineTypes.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.Machines.QueryHandlers
{
    public class MachinesQueryHandler : IMachineQuery<MachineListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IMachineTypeRepository
            _machineTypeRepository;

        public MachinesQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                   serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _machineTypeRepository =
                _storage.GetRepository<IMachineTypeRepository>();
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

        //protected SingleUomResult GetUom(int id)
        //{
        //    var masterUnitUri = MasterDataSettings.Endpoint + $"master/uoms/{id}";
        //    var unitResponse = _http.GetAsync(masterUnitUri).Result;

        //    if (unitResponse.IsSuccessStatusCode)
        //    {
        //        SingleUomResult unitResult = JsonConvert.DeserializeObject<SingleUomResult>(unitResponse.Content.ReadAsStringAsync().Result);
        //        return unitResult;
        //    }
        //    else
        //    {
        //        return new SingleUomResult();
        //    }
        //}

        public async Task<IEnumerable<MachineListDto>> GetAll()
        {
            var result = new List<MachineListDto>();

            var machineQuery =
                _machineRepository
                    .Query
                    .OrderByDescending(o => o.CreatedDate);

            await Task.Yield();
            var machineDocuments =
                _machineRepository
                    .Find(machineQuery);

            foreach (var machineDocument in machineDocuments)
            {
                var machineType =
                    _machineTypeRepository
                        .Find(o => o.Identity == machineDocument.MachineTypeId.Value)
                        .FirstOrDefault()?
                        .TypeName;

                var operatorDto = new MachineListDto(machineDocument);
                operatorDto.SetMachineType(machineType);

                result.Add(operatorDto);
            }

            return result;
        }

        public async Task<MachineListDto> GetById(Guid id)
        {
            var machineQuery =
                   _machineRepository
                       .Query
                       .OrderByDescending(o => o.CreatedDate);

            var machineDocument =
                _machineRepository
                    .Find(machineQuery)
                    .Where(o=>o.Identity == id)
                    .FirstOrDefault();

            var machineTypeDocument =
                _machineTypeRepository
                    .Find(o => o.Identity == machineDocument.MachineTypeId.Value)
                    .FirstOrDefault();

            //Get Weaving Unit
            await Task.Yield();
            SingleUnitResult unitData = GetUnit(machineDocument.WeavingUnitId.Value);
            var weavingUnitName = unitData.data.Name;

            //Get Unit of Measure
            //await Task.Yield();
            //SingleUomResult uomData = GetUom(machineDocument.CutmarkUom.Value);
            //var uomName = uomData.data.Unit;

            await Task.Yield();
            var result = new MachineByIdDto(machineDocument, weavingUnitName, machineTypeDocument);
            result.SetMachineType(machineTypeDocument.TypeName);

            return result;
        }
    }
}
