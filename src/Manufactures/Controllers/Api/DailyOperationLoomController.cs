using Barebone.Controllers;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;
using System.Threading.Tasks;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects;
using Manufactures.Domain.DailyOperations.Loom.Queries;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/daily-operations-loom")]
    [ApiController]
    [Authorize]
    public class DailyOperationLoomController : ControllerApiBase
    {
        private readonly IDailyOperationLoomQuery<DailyOperationLoomListDto> _loomQuery;

        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationLoomController(IServiceProvider serviceProvider,
                                               IDailyOperationLoomQuery<DailyOperationLoomListDto> loomQuery)
            : base(serviceProvider)
        {
            _loomQuery = loomQuery ?? throw new ArgumentNullException(nameof(loomQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            var dailyOperationLoomDocuments = await _loomQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                dailyOperationLoomDocuments =
                    dailyOperationLoomDocuments
                        .Where(x => x.FabricConstructionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.OrderProductionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(DailyOperationLoomListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    await Task.Yield();
                    dailyOperationLoomDocuments =
                        dailyOperationLoomDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    await Task.Yield();
                    dailyOperationLoomDocuments =
                        dailyOperationLoomDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            //int totalRows = dailyOperationLoomDocuments.Count();
            var result = dailyOperationLoomDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var identity = Guid.Parse(Id);
            var dailyOperationLoomDocument = await _loomQuery.GetById(identity);

            if (dailyOperationLoomDocument == null)
            {
                return NotFound(identity);
            }

            return Ok(dailyOperationLoomDocument);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PreparationDailyOperationLoomCommand command)
        {
            var preparationDailyOperationLoomDocument = await Mediator.Send(command);

            return Ok(preparationDailyOperationLoomDocument.Identity);
        }
    }
}
