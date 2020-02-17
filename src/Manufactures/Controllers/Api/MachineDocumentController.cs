using Barebone.Controllers;
using Manufactures.Application.Machines.DataTransferObjects;
using Manufactures.Domain.Machines.Commands;
using Manufactures.Domain.Machines.Queries;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachineTypes.Repositories;
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
        private readonly IMachineRepository 
            _machineRepository;
        private readonly IMachineQuery<MachineListDto> 
            _machineQuery;

        public MachineDocumentController(IServiceProvider serviceProvider,
                                         IMachineQuery<MachineListDto> machineQuery) : base(serviceProvider)
        {
            _machineQuery = machineQuery ?? throw new ArgumentNullException(nameof(machineQuery));

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
            VerifyUser();
            var machineDocuments = await _machineQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                machineDocuments =
                    machineDocuments.Where(o => o.MachineNumber.Contains(keyword,
                                                                            StringComparison.OrdinalIgnoreCase) ||
                                                o.Location.Contains(keyword, 
                                                                            StringComparison.OrdinalIgnoreCase))
                                    .ToList();
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
                    machineDocuments = machineDocuments.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    machineDocuments = machineDocuments.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            //int totalRows = dailyOperationWarpingDocuments.Count();
            var result = machineDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            VerifyUser();
            var Identity = Guid.Parse(Id);
            var machineDocument = await _machineQuery.GetById(Identity);

            if (machineDocument == null)
            {
                return NotFound(Identity);
            }

            return Ok(machineDocument);
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
