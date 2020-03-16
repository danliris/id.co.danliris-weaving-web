using Barebone.Controllers;
using Manufactures.Domain.MachineTypes.Commands;
using Manufactures.Domain.MachineTypes.Repositories;
using Manufactures.DataTransferObjects.MachineType;
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
    [Route("weaving/machine-types")]
    [ApiController]
    [Authorize]
    public class MachineTypeDocumentController : ControllerApiBase
    {
        private readonly IMachineTypeRepository _machineTypeRepository;

        public MachineTypeDocumentController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
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
            var machineTypeQuery = 
                _machineTypeRepository.Query;
            machineTypeQuery = 
                machineTypeQuery
                    .OrderByDescending(o => o.CreatedDate);

            //if (!filter.Contains("{}"))
            //{
            //    Dictionary<string, string> filterDictionary =
            //       JsonConvert.DeserializeObject<Dictionary<string, string>>(filter);

            //    var filterUnit = filterDictionary["MachineUnit"];
            //    machineTypeQuery = machineTypeQuery.Where(x => x.MachineUnit == filterUnit);
            //}

            var machineTypeDtos =
                        _machineTypeRepository.Find(machineTypeQuery)
                                .Select(item => new MachineTypeDocumentDto(item));

            if (!string.IsNullOrEmpty(keyword))
            {
                machineTypeDtos =
                    machineTypeDtos
                        .Where(o => o.TypeName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> machineTypeDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = machineTypeDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          machineTypeDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(MachineTypeDocumentDto).GetProperty(key);

                if (machineTypeDictionary.Values.Contains("asc"))
                {
                    await Task.Yield();
                    machineTypeDtos =
                        machineTypeDtos.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    await Task.Yield();
                    machineTypeDtos =
                        machineTypeDtos.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            //if (!order.Contains("{}"))
            //{
            //    Dictionary<string, string> orderDictionary =
            //        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
            //              orderDictionary.Keys.First().Substring(1);
            //    System.Reflection.PropertyInfo prop = typeof(MachineTypeDocumentDto).GetProperty(key);

            //    if (orderDictionary.Values.Contains("asc"))
            //    {
            //        machineTypeDtos = machineTypeDtos.OrderBy(x => prop.GetValue(x, null));
            //    }
            //    else
            //    {
            //        machineTypeDtos = machineTypeDtos.OrderByDescending(x => prop.GetValue(x, null));
            //    }
            //}

            var result = machineTypeDtos.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });

            //var ResultMachineTypeDtos = machineTypeDtos.Skip(page * size).Take(size);
            //int totalRows = machineTypeDtos.Count();
            //int resultCount = ResultMachineTypeDtos.Count();
            //page = page + 1;

            //await Task.Yield();

            //return Ok(ResultMachineTypeDtos, info: new
            //{
            //    page,
            //    size,
            //    total = totalRows,
            //    count = resultCount
            //});
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var Identity = Guid.Parse(Id);
            var MachineType =
                _machineTypeRepository.Find(item => item.Identity == Identity)
                                  .FirstOrDefault();

            await Task.Yield();

            if (MachineType == null)
            {
                return NotFound();
            }
            else
            {
                var result = new MachineTypeDocumentDto(MachineType);
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddNewMachineTypeCommand command)
        {
            var machineType = await Mediator.Send(command);

            return Ok(machineType.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateExistingMachineTypeCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var machineType = await Mediator.Send(command);

            return Ok(machineType.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveExistingMachineTypeCommand();
            command.SetId(Identity);

            var MachineType = await Mediator.Send(command);

            return Ok(MachineType.Identity);
        }
    }
}
