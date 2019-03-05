using Barebone.Controllers;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.YarnNumbers.Repositories;
using Manufactures.Domain.Yarns.Commands;
using Manufactures.Domain.Yarns.Repositories;
using Manufactures.Dtos.Yarn;
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
    [Route("weaving/yarns")]
    [ApiController]
    [Authorize]
    public class YarnDocumentController : ControllerApiBase
    {
        public readonly IYarnDocumentRepository _yarnDocumentRepository;
        public readonly IMaterialTypeRepository _materialTypeRepository;
        public readonly IYarnNumberRepository _yarnNumberRepository;

        public YarnDocumentController(IServiceProvider serviceProvider,
                                      IWorkContext workContext) : base(serviceProvider)
        {
            _yarnDocumentRepository =
                this.Storage.GetRepository<IYarnDocumentRepository>();
            _materialTypeRepository =
                this.Storage.GetRepository<IMaterialTypeRepository>();
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
                _yarnDocumentRepository.Query.OrderByDescending(item => item.CreatedDate)
                                             .Take(size)
                                             .Skip(page * size);
            var yarns =
                _yarnDocumentRepository.Find(query)
                                       .Select(item => new YarnDocumentListDto(item));

            if (!string.IsNullOrEmpty(keyword))
            {
                yarns =
                    yarns.Where(entity => entity.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                          entity.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(YarnDocumentListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    yarns = yarns.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    yarns = yarns.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            yarns = yarns.ToArray();
            int totalRows = yarns.Count();

            await Task.Yield();

            return Ok(yarns, info: new
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
            var yarn =
                _yarnDocumentRepository.Find(item => item.Identity == Identity)
                                       .FirstOrDefault();
            var materialType =
                _materialTypeRepository.Find(item => item.Identity == yarn.MaterialTypeId.Value)
                                       .Select(x => new MaterialTypeValueObject(x.Identity, x.Code, x.Name))
                                       .FirstOrDefault();
            var yarnNumberDocument =
                _yarnNumberRepository.Find(item => item.Identity == yarn.YarnNumberId.Value)
                                     .Select(x => new YarnNumberValueObject(x.Identity, x.Code, x.Number))
                                     .FirstOrDefault();
            await Task.Yield();

            if (yarn == null || materialType == null || yarnNumberDocument == null)
            {
                return NotFound();
            }
            else
            {
                var result = new YarnDocumentDto(yarn, materialType, yarnNumberDocument);
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateNewYarnCommand command)
        {
            var yarnDocument = await Mediator.Send(command);

            return Ok(yarnDocument.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateExsistingYarnCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var yarnDocument = await Mediator.Send(command);

            return Ok(yarnDocument.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveExsistingYarnCommand();
            command.SetId(Identity);

            var yarnDocument = await Mediator.Send(command);

            return Ok(yarnDocument.Identity);
        }
    }
}
