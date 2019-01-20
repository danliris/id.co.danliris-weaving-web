using Barebone.Controllers;
using Manufactures.Domain.Construction.Commands;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/fabric-construction")]
    [ApiController]
    [Authorize]
    public class ConstructionController : ControllerApiBase
    {
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;

        public ConstructionController(IServiceProvider serviceProvider, IWorkContext workContext) : base(serviceProvider)
        {
            _constructionDocumentRepository = this.Storage.GetRepository<IConstructionDocumentRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 0, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            int totalRows = _constructionDocumentRepository.Query.Count();
            var query = _constructionDocumentRepository.Query
                                                       .OrderByDescending(item => item.CreatedDate)
                                                       .Take(size)
                                                       .Skip(page * size);
            var constructionDocuments = _constructionDocumentRepository.Find(query)
                                                                       .Select(item => new ConstructionDocumentDto(item))
                                                                       .ToArray();

            await Task.Yield();

            return Ok(constructionDocuments, info: new
            {
                page,
                size,
                count = totalRows
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Id = Guid.Parse(id);
            var constructionDocument = _constructionDocumentRepository.Find(item => item.Identity == Id)
                                                                      .Select(item => new ConstructionDocumentDto(item))
                                                                      .FirstOrDefault();
            await Task.Yield();

            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(constructionDocument);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceConstructionCommand command)
        {
            var newConstructionDocument = await Mediator.Send(command);

            return Ok(newConstructionDocument.Identity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]UpdateConstructionCommand command)
        {
            if (!Guid.TryParse(id, out Guid documentId))
            {
                return NotFound();
            }

            command.SetId(documentId);
            var updateConstructionDocument = await Mediator.Send(command);

            return Ok(updateConstructionDocument.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid documentId))
            {
                return NotFound();
            }

            var command = new RemoveConstructionCommand();
            command.SetId(documentId);

            var deletedConstructionDocument = await Mediator.Send(command);

            return Ok(deletedConstructionDocument.Identity);
        }
    }
}
