using Barebone.Controllers;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IWarpingQuery<DailyOperationWarpingDocument> _queries;

        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationWarpingController(IServiceProvider serviceProvider,
                                               IWarpingQuery<DailyOperationWarpingDocument> warpingQuery)
            : base(serviceProvider)
        {
            _queries = warpingQuery;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            var result = await _queries.Get(page, size, order, keyword, filter);

            return Ok();
        }

            //Preparation Warping Daily Operation Request
            public async Task<IActionResult> 
            Preparation([FromBody]PreparationWarpingOperationCommand command)
        {
            // Sending command to command handler
            var dailyOperationWarping = await Mediator.Send(command);

            //Return result from command handler as Identity(Id)
            return Ok(dailyOperationWarping.Identity);
        }
    }
}
