using Barebone.Controllers;
using Manufactures.Domain.FabricConstructions.Repositories;
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
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom.ValueObjects;

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
        private readonly IFabricConstructionRepository
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
                this.Storage.GetRepository<IFabricConstructionRepository>();
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
                _dailyOperationalDocumentRepository
                    .Query
                    .Include(d => d.DailyOperationLoomDetails)
                    .Where(o => o.DailyOperationStatus != Constants.FINISH)
                    .OrderByDescending(item => item.CreatedDate);
            var dailyOperationalMachineDocuments =
                _dailyOperationalDocumentRepository
                    .Find(query);

            var resultDto = new List<DailyOperationLoomListDto>();

            foreach (var dailyOperation in dailyOperationalMachineDocuments)
            {
                var dateOperated = new DateTimeOffset();

                var machineDocument =
                    await _machineRepository
                        .Query
                        .Where(d => d.Identity.Equals(dailyOperation.MachineId.Value))
                        .FirstOrDefaultAsync();
                var orderDocument =
                       await _weavingOrderDocumentRepository
                           .Query
                           .Where(o => o.Identity.Equals(dailyOperation.OrderId))
                           .FirstOrDefaultAsync();

                foreach (var detail in dailyOperation.DailyOperationMachineDetails)
                {
                    if (detail.DailyOperationLoomHistory
                            .Deserialize<DailyOperationLoomHistory>()
                                .MachineStatus == DailyOperationMachineStatus.ONENTRY)
                    {
                        dateOperated = detail.DailyOperationLoomHistory
                            .Deserialize<DailyOperationLoomHistory>().MachineDate
                                .AddHours(detail.DailyOperationLoomHistory
                                    .Deserialize<DailyOperationLoomHistory>()
                                        .MachineTime.TotalHours);
                    }
                }

                var dto =
                    new DailyOperationLoomListDto(dailyOperation,
                                                  orderDocument.OrderNumber,
                                                  machineDocument.MachineNumber,
                                                  dateOperated);

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
            var query =
                _dailyOperationalDocumentRepository
                    .Query
                    .Include(p => p.DailyOperationLoomDetails)
                    .Where(o => o.Identity == Identity);
            var dailyOperationalLoom =
                _dailyOperationalDocumentRepository
                    .Find(query)
                    .FirstOrDefault();

            await Task.Yield();

            if (Identity == null || dailyOperationalLoom == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(dailyOperationalLoom);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddNewDailyOperationLoomCommand command)
        {
            var newDailyOperationalMachineDocument = await Mediator.Send(command);

            return Ok(newDailyOperationalMachineDocument.Identity);
        }
    }
}
