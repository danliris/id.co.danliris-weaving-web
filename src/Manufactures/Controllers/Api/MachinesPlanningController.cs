using Barebone.Controllers;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachineTypes.Repositories;
using Manufactures.Domain.OperationalMachinesPlanning.Commands;
using Manufactures.Domain.OperationalMachinesPlanning.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.OperationalMachinesPlanning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                _machinesPlanningRepository.Find(query)
                               .Select(item => new MachinesPlanningListDto(item));

            if (!string.IsNullOrEmpty(keyword))
            {
                machinesPlanning =
                    machinesPlanning.Where(entity => entity.Area.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                    entity.Blok.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                    entity.BlokKaizen.Contains(keyword, StringComparison.OrdinalIgnoreCase));
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
                    machinesPlanning = machinesPlanning.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    machinesPlanning = machinesPlanning.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            machinesPlanning = machinesPlanning.Skip(page * size).Take(size);
            int totalRows = machinesPlanning.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(machinesPlanning, info: new
            {
                page,
                size,
                total = totalRows
            });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var Identity = Guid.Parse(Id);
            var machinePlanning =
                _machinesPlanningRepository.Find(item => item.Identity == Identity).FirstOrDefault();

            var manufacturingMachine = _machineRepository.Find(item => item.Identity == machinePlanning.MachineId.Value).FirstOrDefault();
            var manufacturingMachineType = _machineTypeRepository.Find(item => item.Identity == manufacturingMachine.MachineTypeId.Value).FirstOrDefault();

            var machine = new ManufactureMachine(manufacturingMachine, manufacturingMachineType);
            var resultDto = new MachinesPlanningDocumentDto(machinePlanning, machine);

            await Task.Yield();

            if (machinePlanning == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(machinePlanning);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddNewEnginePlanningCommand command)
        {
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
