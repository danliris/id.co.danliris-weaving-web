using Barebone.Controllers;
using Manufactures.Domain.FabricConstructions.Commands;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.FabricConstructions.ValueObjects;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.YarnNumbers.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using Manufactures.Dtos.FabricConstructions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public FabricConstructionController(IServiceProvider serviceProvider, 
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
                _constructionDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
            var constructionDocuments = 
                _constructionDocumentRepository.Find(query)
                                               .Select(item => new FabricConstructionDocumentDto(item));

            if (!string.IsNullOrEmpty(keyword))
            {
                constructionDocuments = 
                    constructionDocuments
                        .Where(entity => entity.ConstructionNumber.Contains(keyword, 
                                                                            StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = 
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(FabricConstructionDocumentDto).GetProperty(key);

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

            var ResultConstructionDocuments =
                constructionDocuments.Skip(page * size).Take(size);
            int totalRows = constructionDocuments.Count();
            int resultCount = ResultConstructionDocuments.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(ResultConstructionDocuments, info: new
            {
                page,
                size,
                total = totalRows,
                count = resultCount
            });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var Identity = Guid.Parse(Id);
            var constructionDocument =
                _constructionDocumentRepository.Find(o => o.Identity == Identity)
                                               .FirstOrDefault();

            var result = new FabricConstructionByIdDto(constructionDocument);
            
            foreach(var detail in constructionDocument.ListOfWarp)
            {
                var yarn = 
                    _yarnDocumentRepository.Find(o => o.Identity == detail.YarnId.Value)
                                           .FirstOrDefault();
                var materialType = 
                    _materialTypeRepository.Find(o => o.Identity == yarn.MaterialTypeId.Value)
                                           .FirstOrDefault();
                var yarnNumber = 
                    _yarnNumberRepository.Find(o => o.Identity == yarn.YarnNumberId.Value)
                                         .FirstOrDefault();

                var yarnValueObject = 
                    new YarnValueObject(yarn.Identity, 
                                        yarn.Code, 
                                        yarn.Name, 
                                        materialType.Code, 
                                        yarnNumber.Code);

                var warp = 
                    new Warp(detail.Quantity, 
                             detail.Information, 
                             yarnValueObject);

                result.AddWarp(warp);
                await Task.Yield();
            }

            foreach(var detail in constructionDocument.ListOfWeft)
            {
                var yarn = _yarnDocumentRepository.Find(o => o.Identity == detail.YarnId.Value).FirstOrDefault();
                var materialType = _materialTypeRepository.Find(o => o.Identity == yarn.MaterialTypeId.Value).FirstOrDefault();
                var yarnNumber = _yarnNumberRepository.Find(o => o.Identity == yarn.YarnNumberId.Value).FirstOrDefault();

                var yarnValueObject = new YarnValueObject(yarn.Identity, yarn.Code, yarn.Name, materialType.Code, yarnNumber.Code);

                var weft = new Weft(detail.Quantity, detail.Information, yarnValueObject);

                result.AddWeft(weft);
                await Task.Yield();
            }

            await Task.Yield();

            if (constructionDocument == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
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
    }
}
