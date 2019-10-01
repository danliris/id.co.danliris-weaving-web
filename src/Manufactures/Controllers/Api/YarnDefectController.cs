using Barebone.Controllers;
using Manufactures.Application.Defects.YarnDefect.DataTransferObjects;
using Manufactures.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Defects.YarnDefect;
using Manufactures.Domain.Defects.YarnDefect.Commands;
using Manufactures.Domain.Defects.YarnDefect.ReadModels;
using Manufactures.Domain.Defects.YarnDefect.Repositories;
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
    [Route("weaving/yarn-defect")]
    [ApiController]
    [Authorize]
    public class YarnDefectController : ControllerApiBase
    {
        private readonly IYarnDefectRepository _yarnDefectRepository;
        public YarnDefectController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _yarnDefectRepository = this.Storage.GetRepository<IYarnDefectRepository>();
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
            var query = _yarnDefectRepository.Query;

            if (!filter.Contains("{}"))
            {
                Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                query = QueryHelper<YarnDefectReadModel>.Filter(query, filterDictionary);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(w => w.DefectCode.Contains(keyword, StringComparison.OrdinalIgnoreCase) || w.DefectCategory.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                query = QueryHelper<YarnDefectReadModel>.Order(query, orderDictionary);
            }

            var total = query.Count();
            var data = await query.Skip((page - 1) * size).Take(size).Select(s => new YarnDefectDocument(s)).Select(s => new YarnDefectDto(s)).ToListAsync();

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
            var yarnDefect =
                _yarnDefectRepository
                    .Find(item => item.Identity == Identity)
                    .Select(item => new YarnDefectDto(item))
                    .FirstOrDefault();

            await Task.Yield();

            if (yarnDefect == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(yarnDefect);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddYarnDefectCommand command)
        {
            var beam = await Mediator.Send(command);

            return Ok(beam.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]UpdateYarnDefectCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var yarnDefect = await Mediator.Send(command);

            return Ok(yarnDefect.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveYarnDefectCommand();
            command.SetId(Identity);

            var yarnDefect = await Mediator.Send(command);

            return Ok(yarnDefect.Identity);
        }
    }
}
