using Barebone.Controllers;
using Manufactures.Application.TroubleMachineMonitoring.DTOs;
using Manufactures.Application.TroubleMachineMonitoring.Queries;
using Manufactures.Domain.TroubleMachineMonitoring.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/trouble-machine-monitoring")]
    [ApiController]
    [Authorize]
    public class TroubleMachineMonitoringController : ControllerApiBase
    {
        private readonly ITroubleMachineMonitoringQuery _troubleMachineMonitoring;

        public TroubleMachineMonitoringController(IServiceProvider serviceProvider,
                                             IWorkContext workContext,
                                             ITroubleMachineMonitoringQuery TroubleMachineMonitoringQuery) : base(serviceProvider)
        {
            _troubleMachineMonitoring = TroubleMachineMonitoringQuery ?? throw new ArgumentNullException(nameof(TroubleMachineMonitoringQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            VerifyUser();
            var troubleMachineMonitoringDocuments = await _troubleMachineMonitoring.GetAll();


            if (!string.IsNullOrEmpty(keyword))
            {
                troubleMachineMonitoringDocuments =
                    troubleMachineMonitoringDocuments
                        .Where(t => t.OrderNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    t.ConstructionNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    t.Operator.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    t.Trouble.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        .ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(TroubleMachineMonitoringListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    troubleMachineMonitoringDocuments =
                        troubleMachineMonitoringDocuments.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    troubleMachineMonitoringDocuments =
                        troubleMachineMonitoringDocuments.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            var result = troubleMachineMonitoringDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var Identity = Guid.Parse(Id);

            var troubleMachineMonitoringDocument = await _troubleMachineMonitoring.GetById(Identity);

            await Task.Yield();

            if (troubleMachineMonitoringDocument == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(troubleMachineMonitoringDocument);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddTroubleMachineMonitoringCommand command)
        {
            var troubleMachine = await Mediator.Send(command);

            return Ok(troubleMachine.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateExistingTroubleMachineMonitoringCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var troubleMachine = await Mediator.Send(command);

            return Ok(troubleMachine.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveExistingTroubleMachineMonitoringCommand();
            command.SetId(Identity);

            var troubleMachine = await Mediator.Send(command);

            return Ok(troubleMachine.Identity);
        }
    }
}
