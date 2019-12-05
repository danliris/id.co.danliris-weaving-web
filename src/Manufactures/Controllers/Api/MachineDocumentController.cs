using Barebone.Controllers;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Domain.Machines.Commands;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachineTypes.Repositories;
using Manufactures.Dtos.Machine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/machines")]
    [ApiController]
    [Authorize]
    public class MachineDocumentController : ControllerApiBase
    {
        //protected readonly IHttpClientService _http;
        private readonly IMachineRepository _machineRepository;
        private readonly IMachineTypeRepository 
            _machineTypeRepository;

        public MachineDocumentController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            //_http =
            //    serviceProvider.GetService<IHttpClientService>();
            _machineRepository = 
                this.Storage.GetRepository<IMachineRepository>();
            _machineTypeRepository =
                this.Storage.GetRepository<IMachineTypeRepository>();
        }

        //protected SingleUomResult GetUom(int id)
        //{
        //    var masterUnitUri = MasterDataSettings.Endpoint + $"master/uoms/{id}";
        //    var unitResponse = _http.GetAsync(masterUnitUri).Result;

        //    if (unitResponse.IsSuccessStatusCode)
        //    {
        //        SingleUomResult uomResult = JsonConvert.DeserializeObject<SingleUomResult>(unitResponse.Content.ReadAsStringAsync().Result);
        //        return uomResult;
        //    }
        //    else
        //    {
        //        return new SingleUomResult();
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            page = page - 1;
            var query =
                _machineRepository.Query.OrderByDescending(item => item.CreatedDate);
            var allMachine =
                _machineRepository.Find(query);

            var machines = new List<MachineListDto>();

            foreach (var machine in allMachine)
            {
                var machineType =
                    _machineTypeRepository.Find(o => o.Identity.Equals(machine.MachineTypeId.Value)).FirstOrDefault().TypeName;
                var machineDto  = new MachineListDto(machine);
                machineDto.SetMachineType(machineType);

                machines.Add(machineDto);
            }


            if (!string.IsNullOrEmpty(keyword))
            {
                machines =
                    machines.Where(entity => entity.MachineNumber.Contains(keyword,
                                                                          StringComparison.OrdinalIgnoreCase) ||
                                            entity.Location.Contains(keyword, 
                                                                     StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(MachineListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    machines = machines.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    machines = machines.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            var ResultMachine = machines.Skip(page * size).Take(size);
            int totalRows = machines.Count();
            int resultCount = ResultMachine.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(ResultMachine, info: new
            {
                page,
                size,
                total = totalRows,
                count = resultCount
            });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var Identity = Guid.Parse(Id);
            var machine =
                _machineRepository.Find(item => item.Identity == Identity)
                                  .FirstOrDefault();

            await Task.Yield();
            if (machine == null)
            {
                return NotFound();
            }
            else
            {
                var result = new MachineDocumentDto(machine);
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddNewMachineCommand command)
        {
            var machine = await Mediator.Send(command);

            return Ok(machine.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateExistingMachineCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var machine = await Mediator.Send(command);

            return Ok(machine.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveExistingMachineCommand();
            command.SetId(Identity);

            var machine = await Mediator.Send(command);

            return Ok(machine.Identity);
        }
    }
}
