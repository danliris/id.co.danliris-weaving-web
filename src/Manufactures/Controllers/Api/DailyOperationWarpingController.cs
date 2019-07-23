using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Warping.DTOs;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    /**
     * This is Warping daily operation controller
     * 
     * **/
    [Produces("application/json")]
    [Route("weaving/daily-operations-warping")]
    [ApiController]
    [Authorize]
    public class DailyOperationWarpingController : ControllerApiBase
    {
        private readonly IWarpingQuery<DailyOperationWarpingListDto> _queries;

        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationWarpingController(IServiceProvider serviceProvider,
                                               IWarpingQuery<DailyOperationWarpingListDto> warpingQuery)
            : base(serviceProvider)
        {
            _queries = warpingQuery ?? throw new ArgumentNullException(nameof(warpingQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            var dailyOperationWarpingDocuments = await _queries.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                dailyOperationWarpingDocuments =
                    dailyOperationWarpingDocuments
                        .Where(x => x.ConstructionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.DailyOperationNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.LatestBeamNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(DailyOperationWarpingListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    dailyOperationWarpingDocuments =
                        dailyOperationWarpingDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    dailyOperationWarpingDocuments =
                        dailyOperationWarpingDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            int totalRows = dailyOperationWarpingDocuments.Count();
            var result = dailyOperationWarpingDocuments.Skip((page - 1) * size).Take(size);
            var resultCount = result.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(result, info: new { page, size, totalRows, resultCount });
        }

        //Preparation Warping Daily Operation Request
        [HttpPost("entry-process")]
        public async Task<IActionResult> 
            Preparation([FromBody]PreparationWarpingOperationCommand command)
        {
            // Sending command to command handler
            var dailyOperationWarping = await Mediator.Send(command);

            //Return result from command handler as Identity(Id)
            return Ok(dailyOperationWarping.Identity);
        }

        //Start Warping Daily Operation Request
        [HttpPost("start-process")]
        public async Task<IActionResult>
            Start([FromBody]StartWarpingOperationCommand command)
        {
            // Sending command to command handler
            var dailyOperationWarping = await Mediator.Send(command);

            //Return result from command handler as Identity(Id)
            var resultProcess = 
                dailyOperationWarping
                    .DailyOperationWarpingBeamProducts
                    .Select(x => new DailyOperationWarpingStartDto(x));
            return Ok(resultProcess);
        }
    }
}
