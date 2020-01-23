using Barebone.Controllers;
using Manufactures.Domain.FabricConstructions.Commands;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.YarnNumbers.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manufactures.Application.FabricConstructions.DataTransferObjects;
using Manufactures.Domain.FabricConstructions.Queries;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/fabric-constructions")]
    [ApiController]
    [Authorize]
    public class FabricConstructionController : ControllerApiBase
    {
        private readonly IFabricConstructionRepository _constructionDocumentRepository;
        private readonly IMaterialTypeRepository _materialTypeRepository;
        private readonly IYarnDocumentRepository _yarnDocumentRepository;
        private readonly IYarnNumberRepository _yarnNumberRepository;

        private readonly IFabricConstructionQuery<FabricConstructionListDto> _constructionDocumentQuery;

        public FabricConstructionController(IServiceProvider serviceProvider,
                                            IFabricConstructionQuery<FabricConstructionListDto> constructionDocumentQuery,
                                            IWorkContext workContext) : base(serviceProvider)
        {
            _constructionDocumentRepository = 
                this.Storage.GetRepository<IFabricConstructionRepository>();
            _materialTypeRepository =
                this.Storage.GetRepository<IMaterialTypeRepository>();
            _yarnDocumentRepository =
                this.Storage.GetRepository<IYarnDocumentRepository>();
            _yarnNumberRepository =
                this.Storage.GetRepository<IYarnNumberRepository>();

            _constructionDocumentQuery = constructionDocumentQuery ?? throw new ArgumentNullException(nameof(constructionDocumentQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, 
                                             int size = 25, 
                                             string order = "{}", 
                                             string keyword = null, 
                                             string filter = "{}")
        {
            var constructionDocuments = await _constructionDocumentQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                constructionDocuments = 
                    constructionDocuments
                        .Where(o => o.ConstructionNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = 
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(FabricConstructionListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    constructionDocuments = 
                        constructionDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    constructionDocuments = 
                        constructionDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            var result = constructionDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var identity = Guid.Parse(Id);
            var constructionDocument = await _constructionDocumentQuery.GetById(identity);

            if (constructionDocument == null)
            {
                return NotFound(identity);
            }

            return Ok(constructionDocument);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddFabricConstructionCommand command)
        {
            var newConstructionDocument = await Mediator.Send(command);

            return Ok(newConstructionDocument.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id, 
                                             [FromBody]UpdateFabricConstructionCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }

            command.SetId(documentId);
            var updateConstructionDocument = await Mediator.Send(command);

            return Ok(updateConstructionDocument.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }

            var command = new RemoveFabricConstructionCommand();
            command.SetId(documentId);

            var deletedConstructionDocument = await Mediator.Send(command);

            return Ok(deletedConstructionDocument.Identity);
        }

        [HttpGet("construction-number/{id}")]
        public async Task<IActionResult> GetConstructionNumber(string id)
        {
            var constructionId = new Guid(id);

            var constructionDocument =
                _constructionDocumentRepository
                    .Find(e => e.Identity.Equals(constructionId))
                    .FirstOrDefault();
            var constructionNumber = constructionDocument.ConstructionNumber;

            if (constructionDocument != null)
            {
                await Task.Yield();
                return Ok(constructionNumber);
            }
            else
            {
                await Task.Yield();
                return NotFound();
                throw Validator.ErrorValidation(("ConstructionNumber", "Can't Find Construction Number"));
            }
        }
    }
}
