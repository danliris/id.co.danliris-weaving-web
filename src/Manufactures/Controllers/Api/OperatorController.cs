using Barebone.Controllers;
using Manufactures.Domain.Operators;
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

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/operators")]
    [ApiController]
    [Authorize]
    public class OperatorController : ControllerApiBase
    {
        private readonly IOperatorRepository _OperatorRepository;

        public OperatorController(IServiceProvider serviceProvider,
                                  IWorkContext workContext) : base(serviceProvider)
        {
            _OperatorRepository =
                this.Storage.GetRepository<IOperatorRepository>();
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
                _OperatorRepository.Query.OrderByDescending(item => item.CreatedDate);
            var operatorDocuments =
                _OperatorRepository.Find(query);

            if (!string.IsNullOrEmpty(keyword))
            {
                operatorDocuments =
                    operatorDocuments
                        .Where(entity => entity.CoreAccount
                                               .Name
                                               .Contains(keyword, 
                                                         StringComparison.OrdinalIgnoreCase))
                        .ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(OperatorDocument).GetProperty(key);

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

            operatorDocuments = operatorDocuments.Skip(page * size).Take(size).ToList();
            int totalRows = operatorDocuments.Count();
            page = page + 1;

            var resultDocuments = operatorDocuments;

            await Task.Yield();

            return Ok(resultDocuments, info: new
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
            var operatorDocument =
                _OperatorRepository.Find(item => item.Identity == Identity).FirstOrDefault();
            await Task.Yield();

            if (operatorDocument == null)
            {
                return NotFound();
            }
            else
            {
                var resultData = operatorDocument;

                return Ok(resultData);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddOperatorCommand command)
        {
            var mongodbId = command.CoreAccount.MongoId;

            var existingOperator = 
                _OperatorRepository
                    .Query
                    .Where(o => o.CoreAccount.Deserialize<CoreAccount>()
                    .MongoId
                    .Equals(mongodbId))
                    .FirstOrDefault();

            if(existingOperator != null)
            {
                throw Validator.ErrorValidation(("Id", "Has existing account operator"));
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
