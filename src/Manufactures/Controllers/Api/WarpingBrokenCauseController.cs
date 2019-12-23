using Barebone.Controllers;
using Manufactures.Application.BrokenCauses.Warping.DataTransferObjects;
using Manufactures.Domain.BrokenCauses.Warping.Commands;
using Manufactures.Domain.BrokenCauses.Warping.Queries;
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
    [Route("weaving/broken-cause-warping")]
    [ApiController]
    [Authorize]
    public class WarpingBrokenCauseController : ControllerApiBase
    {
        private readonly IWarpingBrokenCauseQuery<WarpingBrokenCauseDto> _warpingBrokenCauseQuery;

        //Dependency Injection Activated from Constructor Need IServiceProvider
        public WarpingBrokenCauseController(IServiceProvider serviceProvider,
                                            IWarpingBrokenCauseQuery<WarpingBrokenCauseDto> warpingBrokenCauseQuery) : base(serviceProvider)
        {
            _warpingBrokenCauseQuery = warpingBrokenCauseQuery ?? throw new ArgumentNullException(nameof(warpingBrokenCauseQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            var warpingBrokenCauses = await _warpingBrokenCauseQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                warpingBrokenCauses =
                    warpingBrokenCauses
                        .Where(x => x.WarpingBrokenCauseName.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(WarpingBrokenCauseDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    await Task.Yield();
                    warpingBrokenCauses =
                        warpingBrokenCauses.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    await Task.Yield();
                    warpingBrokenCauses =
                        warpingBrokenCauses.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            var result = warpingBrokenCauses.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var identity = Guid.Parse(Id);
            var warpingBrokenCause = await _warpingBrokenCauseQuery.GetById(identity);

            if (warpingBrokenCause == null)
            {
                return NotFound(identity);
            }

            return Ok(warpingBrokenCause);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddWarpingBrokenCauseCommand command)
        {
            // Sending command to command handler
            var warpingBrokenCause = await Mediator.Send(command);

            //Return result from command handler as Identity(Id)
            return Ok(warpingBrokenCause.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Start(string Id, [FromBody]UpdateWarpingBrokenCauseCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateWarpingBrokenCause = await Mediator.Send(command);

            return Ok(updateWarpingBrokenCause.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveWarpingBrokenCauseCommand();
            command.SetId(Identity);

            var warpingBrokenCause = await Mediator.Send(command);

            return Ok(warpingBrokenCause.Identity);
        }
    }
}
