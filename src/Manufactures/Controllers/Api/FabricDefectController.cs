using Barebone.Controllers;
using Manufactures.Application.Defects.FabricDefect.DataTransferObjects;
using Manufactures.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Defects.FabricDefect;
using Manufactures.Domain.Defects.FabricDefect.Commands;
using Manufactures.Domain.Defects.FabricDefect.ReadModels;
using Manufactures.Domain.Defects.FabricDefect.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/fabric-defect")]
    [ApiController]
    [Authorize]
    public class FabricDefectController : ControllerApiBase
    {
        private readonly IFabricDefectRepository _fabricDefectRepository;
        public FabricDefectController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _fabricDefectRepository = this.Storage.GetRepository<IFabricDefectRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                            int size = 25,
                                            string order = "{}",
                                            string keyword = null,
                                            string filter = "{}")
        {
            //page = page - 1;
            //page = page < 1 ? 1 : page;
            var query = _fabricDefectRepository.Query;

            if (!filter.Contains("{}"))
            {
                Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                query = QueryHelper<FabricDefectReadModel>.Filter(query, filterDictionary);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(w => w.DefectCode.Contains(keyword, StringComparison.OrdinalIgnoreCase) || w.DefectCategory.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                query = QueryHelper<FabricDefectReadModel>.Order(query, orderDictionary);
            }

            var total = query.Count();
            var data = await query.Skip((page - 1) * size).Take(size).Select(s => new FabricDefectDocument(s)).Select(s => new FabricDefectDto(s)).ToListAsync();

            return Ok(data, info: new
            {
                page,
                size,
                total,
                count = data.Count
            });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var Identity = Guid.Parse(Id);
            var fabricDefect =
                _fabricDefectRepository
                    .Find(item => item.Identity == Identity)
                    .Select(item => new FabricDefectDto(item))
                    .FirstOrDefault();

            await Task.Yield();

            if (fabricDefect == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(fabricDefect);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddFabricDefectCommand command)
        {
            var beam = await Mediator.Send(command);

            return Ok(beam.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]UpdateFabricDefectCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var fabricDefect = await Mediator.Send(command);

            return Ok(fabricDefect.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveFabricDefectCommand();
            command.SetId(Identity);

            var fabricDefect = await Mediator.Send(command);

            return Ok(fabricDefect.Identity);
        }
    }
}
