using Barebone.Controllers;
using Manufactures.Domain.YarnNumbers.Commands;
using Manufactures.Domain.YarnNumbers.Repositories;
using Manufactures.Dtos.YarnNumber;
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
    [Route("weaving/yarn-numbers")]
    [ApiController]
    [Authorize]
    public class YarnNumberDocumentController : ControllerApiBase
    {
        private readonly IYarnNumberRepository _ringRepository;

        public YarnNumberDocumentController(IServiceProvider serviceProvider, 
                                      IWorkContext workContext) : base(serviceProvider)
        {
            _ringRepository = 
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
                _ringRepository.Query.OrderByDescending(item => item.CreatedDate)
                                     .Take(size)
                                     .Skip(page * size);
            var ringDocuments = 
                _ringRepository.Find(query)
                               .Select(item => new YarnNumberListDto(item));

            if (!string.IsNullOrEmpty(keyword))
            {
                ringDocuments = 
                    ringDocuments.Where(entity => entity.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = 
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() + 
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(YarnNumberListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    ringDocuments = ringDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    ringDocuments = ringDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            ringDocuments = ringDocuments.ToArray();
            int totalRows = ringDocuments.Count();

            await Task.Yield();

            return Ok(ringDocuments, info: new
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
            var ringDocuments = 
                _ringRepository.Find(item => item.Identity == Identity)
                               .Select(item => new YarnNumberDocumentDto(item)).FirstOrDefault();
            await Task.Yield();

            if (ringDocuments == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(ringDocuments);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddNewYarnNumberCommand command)
        {
            var ringDocument = await Mediator.Send(command);

            return Ok(ringDocument.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id, 
                                             [FromBody]UpdateYarnNumberCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var ringDocument = await Mediator.Send(command);

            return Ok(ringDocument.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveYarnNumberCommand();
            command.SetId(Identity);

            var ringDocument = await Mediator.Send(command);

            return Ok(ringDocument.Identity);
        }
    }
}
