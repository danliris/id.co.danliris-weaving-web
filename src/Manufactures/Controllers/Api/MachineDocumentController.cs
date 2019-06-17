using Barebone.Controllers;
using Manufactures.Domain.Machines.Commands;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Dtos.Machine;
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
    [Route("weaving/machines")]
    [ApiController]
    [Authorize]
    public class MachineDocumentController : ControllerApiBase
    {
        private readonly IMachineRepository _machineRepository;

        public MachineDocumentController(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            _machineRepository = 
                this.Storage.GetRepository<IMachineRepository>();
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
                _machineRepository.Query.OrderByDescending(item => item.CreatedDate);
            var machine =
                _machineRepository.Find(query)
                                       .Select(item => new MachineListDto(item));

            if (!string.IsNullOrEmpty(keyword))
            {
                machine =
                    machine.Where(entity => entity.MachineNumber.Contains(keyword,
                                                                          StringComparison.OrdinalIgnoreCase) ||
                                            entity.Location.Contains(keyword, 
                                                                     StringComparison.OrdinalIgnoreCase));
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
                    machine = machine.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    machine = machine.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            var ResultMachine = machine.Skip(page * size).Take(size);
            int totalRows = machine.Count();
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
