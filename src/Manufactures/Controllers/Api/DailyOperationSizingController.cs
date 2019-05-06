using Barebone.Controllers;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Dtos.DailyOperations.Sizing;
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
    [Route("weaving/daily-operations-sizing")]
    [ApiController]
    [Authorize]
    public class DailyOperationSizingController : ControllerApiBase
    {
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IFabricConstructionRepository
            _constructionDocumentRepository;
        private readonly IShiftRepository
            _shiftDocumentRepository;

        public DailyOperationSizingController(IServiceProvider serviceProvider,
                                                 IWorkContext workContext)
            : base(serviceProvider)
        {
            _dailyOperationSizingDocumentRepository =
                this.Storage.GetRepository<IDailyOperationSizingRepository>();
            _machineRepository =
                this.Storage.GetRepository<IMachineRepository>();
            _constructionDocumentRepository =
                this.Storage.GetRepository<IFabricConstructionRepository>();
            _shiftDocumentRepository =
                this.Storage.GetRepository<IShiftRepository>();
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
                _dailyOperationSizingDocumentRepository
                    .Query
                    .OrderByDescending(item => item.CreatedDate);
            var dailyOperationSizingDocuments =
                _dailyOperationSizingDocumentRepository
                    .Find(domQuery.Include(d => d.DailyOperationSizingDetails));

            var resultDto = new List<DailyOperationSizingListDto>();

            foreach (var dailyOperation in dailyOperationSizingDocuments)
            {

                foreach (var detail in dailyOperation.DailyOperationSizingDetails)
                {
                    var dto = new DailyOperationSizingListDto(dailyOperation, detail);

                    resultDto.Add(dto);

                }
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() + orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop =
                    typeof(DailyOperationSizingListDto).GetProperty(key);

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
            var query = _dailyOperationSizingDocumentRepository.Query;
            var dailyOperationSizingDocument =
                _dailyOperationSizingDocumentRepository
                    .Find(query.Include(p => p.DailyOperationSizingDetails))
                    .Where(o => o.Identity == Identity)
                    .FirstOrDefault();

            await Task.Yield();

            if (Identity == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(dailyOperationSizingDocument);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddNewDailyOperationSizingCommand command)
        {
            var newDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(newDailyOperationSizingDocument.Identity);
        }

        [HttpPut("{Id}/pause")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdatePauseDailyOperationSizingCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updatePauseDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updatePauseDailyOperationSizingDocument.Identity);
        }

        [HttpPut("{Id}/resume")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateResumeDailyOperationSizingCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateResumeDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updateResumeDailyOperationSizingDocument.Identity);
        }

        [HttpPut("{Id}/doff")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateDoffDailyOperationSizingCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateDoffDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updateDoffDailyOperationSizingDocument.Identity);
        }

        //[HttpDelete("{Id}")]
        //public async Task<IActionResult> Delete(string Id)
        //{
        //    if (!Guid.TryParse(Id, out Guid documentId))
        //    {
        //        return NotFound();
        //    }

        //    var command = new RemoveDailyOperationSizingCommand();
        //    command.SetId(documentId);

        //    var deletedDailyOperationSizingDocument = await Mediator.Send(command);

        //    return Ok(deletedDailyOperationSizingDocument.Identity);
        //}
    }
}
