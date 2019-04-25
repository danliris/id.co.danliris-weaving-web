using Barebone.Controllers;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Dtos.DailyOperations.Loom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/daily-operations-loom")]
    [ApiController]
    [Authorize]
    public class DailyOperationLoomController : ControllerApiBase
    {
        private readonly IDailyOperationLoomRepository 
            _dailyOperationalDocumentRepository;
        private readonly IWeavingOrderDocumentRepository 
            _weavingOrderDocumentRepository;
        private readonly IConstructionDocumentRepository 
            _constructionDocumentRepository;
        private readonly IMachineRepository 
            _machineRepository;

        public DailyOperationLoomController(IServiceProvider serviceProvider, 
                                                 IWorkContext workContext) 
            : base(serviceProvider)
        {
            _dailyOperationalDocumentRepository = 
                this.Storage.GetRepository<IDailyOperationLoomRepository>();
            _weavingOrderDocumentRepository = 
                this.Storage.GetRepository<IWeavingOrderDocumentRepository>();
            _constructionDocumentRepository =
                this.Storage.GetRepository<IConstructionDocumentRepository>();
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
            var domQuery =
                _dailyOperationalDocumentRepository
                    .Query
                    .OrderByDescending(item => item.CreatedDate);
            var dailyOperationalMachineDocuments =
                _dailyOperationalDocumentRepository
                    .Find(domQuery.Include(d => d.DailyOperationLoomDetails));

            var resultDto = new List<DailyOperationLoomListDto>();

            foreach (var dailyOperation in dailyOperationalMachineDocuments)
            {
                var orderNumber = "";
                var machineDocument = 
                    await _machineRepository
                        .Query
                        .Where(d => d.Identity.Equals(dailyOperation.MachineId.Value))
                        .FirstOrDefaultAsync();

                foreach (var detail in dailyOperation.DailyOperationMachineDetails)
                {
                    var orderDocument = 
                        await _weavingOrderDocumentRepository
                            .Query
                            .Where(o => o.Identity.Equals(detail.OrderDocumentId.Value))
                            .FirstOrDefaultAsync();
                    
                    if(orderNumber == "")
                    {
                        orderNumber = orderDocument.OrderNumber;
                    }
                }

                var dto = 
                    new DailyOperationLoomListDto(dailyOperation, 
                                                       orderNumber,
                                                       machineDocument.MachineNumber);

                resultDto.Add(dto);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                resultDto = 
                    resultDto.Where(entity => entity
                                                .OrderNumber
                                                .Contains(keyword,
                                                          StringComparison
                                                            .OrdinalIgnoreCase) || 
                                              entity
                                                .MachineNumber
                                                .Contains(keyword, 
                                                          StringComparison
                                                            .OrdinalIgnoreCase))
                             .ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = 
                    orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                    orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = 
                    typeof(DailyOperationLoomListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    resultDto =
                        resultDto
                            .OrderBy(x => prop.GetValue(x, null))
                            .ToList();
                }
                else
                {
                    resultDto =
                        resultDto
                            .OrderByDescending(x => prop.GetValue(x, null))
                            .ToList();
                }
            }

            resultDto =
                resultDto.Skip(page * size).Take(size).ToList();
            int totalRows = resultDto.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(resultDto, info: new
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
            var query = _dailyOperationalDocumentRepository.Query;
            var dailyOperationalMachineDocument = 
                _dailyOperationalDocumentRepository
                    .Find(query.Include(p => p.DailyOperationLoomDetails))
                    .Where(o => o.Identity == Identity)
                    .FirstOrDefault();



            await Task.Yield();

            if (Identity == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(dailyOperationalMachineDocument);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddNewDailyOperationLoomCommand command)
        {
            var newDailyOperationalMachineDocument = await Mediator.Send(command);

            return Ok(newDailyOperationalMachineDocument.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateDailyOperationLoomCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }

            command.SetId(documentId);
            var updateDailyOperationalMachineDocument = await Mediator.Send(command);

            return Ok(updateDailyOperationalMachineDocument.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }

            var command = new RemoveDailyOperationLoomCommand();
            command.SetId(documentId);

            var deletedDailyOperationalMachineDocument = await Mediator.Send(command);

            return Ok(deletedDailyOperationalMachineDocument.Identity);
        }
    }
}
