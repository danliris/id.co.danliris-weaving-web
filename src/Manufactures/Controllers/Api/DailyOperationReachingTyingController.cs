using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects;
using Manufactures.Application.Operators.DTOs;
using Manufactures.Application.Shifts.DTOs;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.DailyOperations.Reaching.Queries;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Domain.Shifts.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/daily-operations-reaching-tying")]
    [ApiController]
    [Authorize]
    public class DailyOperationReachingTyingController : ControllerApiBase
    {
        private readonly IReachingTyingQuery<DailyOperationReachingTyingListDto> _reachingTyingQuery;
        private readonly IOperatorQuery<OperatorListDto> _operatorQuery;
        private readonly IShiftQuery<ShiftDto> _shiftQuery;
        
        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationReachingTyingController(IServiceProvider serviceProvider,
                                                     IReachingTyingQuery<DailyOperationReachingTyingListDto> reachingTyingQuery,
                                                     IOperatorQuery<OperatorListDto> operatorQuery,
                                                     IShiftQuery<ShiftDto> shiftQuery)
            : base(serviceProvider)
        {
            _reachingTyingQuery = reachingTyingQuery ?? throw new ArgumentNullException(nameof(reachingTyingQuery));
            _operatorQuery = operatorQuery ?? throw new ArgumentNullException(nameof(operatorQuery));
            _shiftQuery = shiftQuery ?? throw new ArgumentNullException(nameof(shiftQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                            int size = 25,
                                            string order = "{}",
                                            string keyword = null,
                                            string filter = "{}")
        {
            var dailyOperationReachingDocuments = await _reachingTyingQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                dailyOperationReachingDocuments =
                    dailyOperationReachingDocuments
                        .Where(x => x.ConstructionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.MachineNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.SizingBeamNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(DailyOperationReachingTyingListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    await Task.Yield();
                    dailyOperationReachingDocuments =
                        dailyOperationReachingDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    await Task.Yield();
                    dailyOperationReachingDocuments =
                        dailyOperationReachingDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            int totalRows = dailyOperationReachingDocuments.Count();
            var result = dailyOperationReachingDocuments.Skip((page - 1) * size).Take(size);
            var resultCount = result.Count();

            return Ok(result, info: new { page, size, totalRows, resultCount });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]NewEntryDailyOperationReachingTyingCommand command)
        {
            // Sending Command to Command Handler
            var dailyOperationReaching = await Mediator.Send(command);

            //Return Result from Command Handler as Identity(Id)
            return Ok(dailyOperationReaching.Identity);
        }
    }
}
