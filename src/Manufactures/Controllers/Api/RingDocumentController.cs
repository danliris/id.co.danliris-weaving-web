using Barebone.Controllers;
using Manufactures.Domain.Rings.Commands;
using Manufactures.Domain.Rings.Repositories;
using Manufactures.Dtos;
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
    [Route("weaving/ring-numbers")]
    [ApiController]
    [Authorize]
    class RingDocumentController : ControllerApiBase
    {
        private readonly IRingRepository _ringRepository;

        public RingDocumentController(IServiceProvider serviceProvider, IWorkContext workContext) : base(serviceProvider)
        {
            _ringRepository = this.Storage.GetRepository<IRingRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 0, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            var query = _ringRepository.Query.OrderByDescending(item => item.CreatedDate).Take(size).Skip(page * size);
            var ringDocuments = _ringRepository.Find(query).Select(item => new RingDocumentDto(item));

            if (!string.IsNullOrEmpty(keyword))
            {
                ringDocuments = ringDocuments.Where(entity => entity.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                              entity.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                              entity.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() + orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(RingDocumentDto).GetProperty(key);

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
                count = totalRows
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Identity = Guid.Parse(id);
            var ringDocuments = _ringRepository.Find(item => item.Identity == Identity).Select(item => new RingDocumentDto(item)).FirstOrDefault();
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
        public async Task<IActionResult> Post([FromBody]CreateRingDocumentCommand command)
        {
            var ringDocument = await Mediator.Send(command);

            return Ok(ringDocument.Identity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]UpdateRingDocumentCommand command)
        {
            if (!Guid.TryParse(id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var ringDocument = await Mediator.Send(command);

            return Ok(ringDocument.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveRingDocumentCommand();
            command.SetId(Identity);

            var ringDocument = await Mediator.Send(command);

            return Ok(ringDocument.Identity);
        }
    }
}
