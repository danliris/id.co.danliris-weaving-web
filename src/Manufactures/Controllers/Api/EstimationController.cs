using Barebone.Controllers;
using Manufactures.Application.Estimations.Productions.DataTransferObjects;
using Manufactures.Domain.Estimations.Productions.Commands;
using Manufactures.Domain.Estimations.Productions.Queries;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/estimation-productions")]
    [ApiController]
    [Authorize]
    public class EstimationController : ControllerApiBase
    {
        private readonly IEstimatedProductionDocumentRepository _estimationProductRepository;

        private readonly IEstimatedProductionDocumentQuery<EstimatedProductionListDto> _estimatedProductionDocumentQuery;

        public EstimationController(IServiceProvider serviceProvider, 
                                    IWorkContext workContext,
                                    IEstimatedProductionDocumentQuery<EstimatedProductionListDto> estimatedProductionDocumentQuery) : base(serviceProvider)
        {
            _estimationProductRepository = 
                this.Storage.GetRepository<IEstimatedProductionDocumentRepository>();

            _estimatedProductionDocumentQuery = estimatedProductionDocumentQuery ?? throw new ArgumentNullException(nameof(estimatedProductionDocumentQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, 
                                             int size = 25, 
                                             string order = "{}", 
                                             string keyword = null, 
                                             string filter = "{}")
        {
            var estimatedProductionDocuments = await _estimatedProductionDocumentQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                estimatedProductionDocuments = 
                    estimatedProductionDocuments
                        .Where(o => o.EstimatedNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = 
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() + 
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(EstimatedProductionListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    estimatedProductionDocuments =
                        estimatedProductionDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    estimatedProductionDocuments =
                        estimatedProductionDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            var result = estimatedProductionDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var Identity = Guid.Parse(Id);
            var estimatedProductionDocument = await _estimatedProductionDocumentQuery.GetById(Identity);

            await Task.Yield();
            if (Identity == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(estimatedProductionDocument);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddNewEstimationCommand command)
        {
            var newEstimationDocument = await Mediator.Send(command);

            return Ok(newEstimationDocument.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id, 
                                             [FromBody]UpdateEstimationProductCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }

            command.SetId(documentId);
            var updateEstimationDocument = await Mediator.Send(command);

            return Ok(updateEstimationDocument.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
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
