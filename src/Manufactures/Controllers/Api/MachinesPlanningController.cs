using Barebone.Controllers;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachineTypes.Repositories;
using Manufactures.Domain.MachinesPlanning.Commands;
using Manufactures.Domain.MachinesPlanning.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.MachinesPlanning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/machines-planning")]
    [ApiController]
    [Authorize]
    public class MachinesPlanningController : ControllerApiBase
    {
        private readonly IMachinesPlanningRepository _machinesPlanningRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IMachineTypeRepository _machineTypeRepository;

        public MachinesPlanningController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _machinesPlanningRepository =
                this.Storage.GetRepository<IMachinesPlanningRepository>();
            _machineRepository =
                this.Storage.GetRepository<IMachineRepository>();
            _machineTypeRepository =
                 this.Storage.GetRepository<IMachineTypeRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                            int size = 25,
                                            string order = "{}",
                                            string keyword = null,
                                            string filter = "{}")
        {
            page = page - 1;
            var query =
                _machinesPlanningRepository.Query.OrderByDescending(item => item.CreatedDate);
            var machinesPlanning =
                _machinesPlanningRepository.Find(query);

            var machinePlanningDtos = new List<MachinesPlanningListDto>();

            foreach (var machinePlanning in machinesPlanning)
            {
                var manufacturingMachine = 
                    _machineRepository.Find(item => item.Identity == machinePlanning.MachineId.Value)
                                      .FirstOrDefault();
                var manufacturingMachineType = 
                    _machineTypeRepository.Find(item => item.Identity == manufacturingMachine.MachineTypeId.Value)
                                          .FirstOrDefault();

                var machine = new ManufactureMachine(manufacturingMachine, manufacturingMachineType);
                var machineDto = new MachinesPlanningListDto(machinePlanning, machine);

                machinePlanningDtos.Add(machineDto);
            }


            if (!string.IsNullOrEmpty(keyword))
            {
                machinePlanningDtos =
                    machinePlanningDtos.Where(entity => entity.Area.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                entity.Blok.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                entity.BlokKaizen.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                               .ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(MachinesPlanningListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    machinePlanningDtos = machinePlanningDtos.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    machinePlanningDtos = machinePlanningDtos.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            var ResultMachinePlanningDtos = machinePlanningDtos.Skip(page * size).Take(size).ToList();
            int totalRows = machinePlanningDtos.Count();
            int resultCount = ResultMachinePlanningDtos.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(ResultMachinePlanningDtos, info: new
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
            var machinePlanning =
                _machinesPlanningRepository.Find(item => item.Identity == Identity).FirstOrDefault();

            var manufacturingMachine = 
                _machineRepository.Find(item => item.Identity == machinePlanning.MachineId.Value)
                                  .FirstOrDefault();
            var manufacturingMachineType = 
                _machineTypeRepository.Find(item => item.Identity == manufacturingMachine.MachineTypeId.Value)
                                      .FirstOrDefault();

            var machine = new ManufactureMachine(manufacturingMachine, manufacturingMachineType);
            var resultDto = new MachinesPlanningDocumentDto(machinePlanning, machine);

            await Task.Yield();

            if (resultDto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(resultDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddNewEnginePlanningCommand command)
        {
            var existingMachinePlanning = 
                _machinesPlanningRepository.Find(o => o.MachineId.Equals(command.MachineId) && o.Blok.Equals(command.Blok))
                                           .FirstOrDefault();

            if (existingMachinePlanning != null)
            {
                throw Validator.ErrorValidation(("MachineId", "Has available machine planning on same blok"));
            }

            var machinePlanning = await Mediator.Send(command);

            return Ok(machinePlanning.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateEnginePlanningCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var existingMachinePlanning = 
                _machinesPlanningRepository.Find(o => o.MachineId == command.MachineId && o.Blok == command.Blok)
                                           .FirstOrDefault();

            if (existingMachinePlanning != null)
            {
                throw Validator.ErrorValidation(("MachineId", "Has available machine planning on same blok"));
            }

            command.SetId(Identity);
            var machinePlanning = await Mediator.Send(command);

            return Ok(machinePlanning.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveEnginePlanningCommand();
            command.SetId(Identity);

            var machinePlanning = await Mediator.Send(command);

            return Ok(machinePlanning.Identity);
        }
    }
}
