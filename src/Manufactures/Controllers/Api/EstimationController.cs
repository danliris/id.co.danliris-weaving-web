using Barebone.Controllers;
using Manufactures.Domain.Estimations.Productions.Commands;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Dtos.EstimationsProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/estimation-productions")]
    [ApiController]
    [Authorize]
    public class EstimationController : ControllerApiBase
    {
        private readonly IEstimationProductRepository _estimationProductRepository;

        public EstimationController(IServiceProvider serviceProvider, 
                                    IWorkContext workContext) : base(serviceProvider)
        {
            _estimationProductRepository = 
                this.Storage.GetRepository<IEstimationProductRepository>();
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
                _estimationProductRepository.Query.OrderByDescending(item => item.CreatedDate)
                                                  .Take(size)
                                                  .Skip(page * size);
            var estimationDocument = 
                _estimationProductRepository.Find(query.Include(p => p.EstimationProducts))
                                            .Select(item => new ListEstimationDto(item));

            if (!string.IsNullOrEmpty(keyword))
            {
                estimationDocument = 
                    estimationDocument.Where(entity => entity.EstimationNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = 
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() + 
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(ListEstimationDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    estimationDocument = 
                        estimationDocument.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    estimationDocument = 
                        estimationDocument.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            var constructionDocumentsResult = estimationDocument.ToArray();
            int totalRows = constructionDocumentsResult.Count();

            await Task.Yield();

            return Ok(constructionDocumentsResult, info: new
            {
                page,
                size,
                total = totalRows
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Id = Guid.Parse(id);
            var query = _estimationProductRepository.Query;
            var estimationDocument = 
                _estimationProductRepository.Find(query.Include(p => p.EstimationProducts))
                                            .Where(o => o.Identity == Id)
                                            .Select(item => new EstimationByIdDto(item))
                                            .FirstOrDefault();
            await Task.Yield();

            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(estimationDocument);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddNewEstimationCommand command)
        {
            var newEstimationDocument = await Mediator.Send(command);

            return Ok(newEstimationDocument.Identity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, 
                                             [FromBody]UpdateEstimationProductCommand command)
        {
            if (!Guid.TryParse(id, out Guid documentId))
            {
                return NotFound();
            }

            command.SetId(documentId);
            var updateEstimationDocument = await Mediator.Send(command);

            return Ok(updateEstimationDocument.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid documentId))
            {
                return NotFound();
            }

            var command = new RemoveEstimationProductCommand();
            command.SetId(documentId);

            var deletedEstimationDocument = await Mediator.Send(command);

            return Ok(deletedEstimationDocument.Identity);
        }
    }
}
