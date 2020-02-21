using Barebone.Controllers;
using Manufactures.Domain.Operators.Commands;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Application.Operators.DataTransferObjects;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/operators")]
    [ApiController]
    [Authorize]
    public class OperatorController : ControllerApiBase
    {
        private readonly IOperatorRepository _operatorRepository;
        private readonly IOperatorQuery<OperatorListDto> _operatorQuery;

        public OperatorController(IServiceProvider serviceProvider,
                                  IWorkContext workContext,
                                  IOperatorQuery<OperatorListDto> operatorQuery) : base(serviceProvider)
        {
            _operatorQuery = operatorQuery ?? throw new ArgumentNullException(nameof(operatorQuery));

            _operatorRepository =
                this.Storage.GetRepository<IOperatorRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            VerifyUser();
            var operatorDocuments = await _operatorQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                operatorDocuments =
                    operatorDocuments
                        .Where(o => o.Username
                                        .Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    o.UnitName
                                        .Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    o.Group
                                        .Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    o.Type
                                        .Contains(keyword, StringComparison.CurrentCultureIgnoreCase))
                        .ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(OperatorListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    operatorDocuments = 
                        operatorDocuments.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    operatorDocuments = 
                        operatorDocuments.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            //int totalRows = dailyOperationWarpingDocuments.Count();
            var result = operatorDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("operator-by-name")]
        public async Task<IActionResult> GetOrderByNumber(int page = 1,
                                                          int size = 25,
                                                          string order = "{}",
                                                          string keyword = null,
                                                          string filter = "{}")
        {
            var operatorDocuments = await _operatorQuery.GetAll();


            if (!string.IsNullOrEmpty(keyword))
            {
                operatorDocuments =
                    operatorDocuments
                        .Where(o => o.Username.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        .ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(OperatorListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    operatorDocuments =
                        operatorDocuments.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    operatorDocuments =
                        operatorDocuments.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            var result = operatorDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            VerifyUser();
            var Identity = Guid.Parse(Id);
            var operatorDocument = await _operatorQuery.GetById(Identity);

            if (operatorDocument == null)
            {
                return NotFound(Identity);
            }

            return Ok(operatorDocument);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddOperatorCommand command)
        {
            var mongodbId = command.CoreAccount.MongoId;

            var existingOperator = 
                _operatorRepository
                    .Query
                    .Where(o => o.CoreAccount
                                 .Deserialize<CoreAccount>()
                                 .MongoId.Equals(mongodbId) && 
                                o.Group.Equals(command.Group))
                    .FirstOrDefault();

            if(existingOperator != null)
            {
                throw Validator
                    .ErrorValidation(("Id", 
                                      "Has existing account operator with same Group"));
            }

            var operatorDocument = await Mediator.Send(command);

            return Ok(operatorDocument.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateOperatorCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var operatorDocument = await Mediator.Send(command);

            return Ok(operatorDocument.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveOperatorCommand();
            command.SetId(Identity);

            var operatorDocument = await Mediator.Send(command);

            return Ok(operatorDocument.Identity);
        }
    }
}
