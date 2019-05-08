using Barebone.Controllers;
using Manufactures.Domain.Beams.Commands;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Dtos.Beams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/beams")]
    [ApiController]
    [Authorize]
    public class BeamController : ControllerApiBase
    {
        private readonly IBeamRepository _beamRepository;

        public BeamController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _beamRepository = this.Storage.GetRepository<IBeamRepository>();
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
                _beamRepository.Query.OrderByDescending(o => o.CreatedDate);
            var beams = _beamRepository.Find(query).Select(o => new BeamDto(o));

            if (!string.IsNullOrEmpty(keyword))
            {
                beams =
                    beams.Where(entity =>
                        entity.Number.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        entity.Type.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(BeamDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    beams = beams.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    beams = beams.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            beams = beams.Skip(page * size).Take(size);
            int totalRows = beams.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(beams, info: new
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
            var beam =
                _beamRepository
                    .Find(item => item.Identity == Identity)
                    .Select(item => new BeamDto(item))
                    .FirstOrDefault();

            await Task.Yield();

            if (beam == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(beam);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddBeamCommand command)
        {
            var beam = await Mediator.Send(command);

            return Ok(beam.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]UpdateBeamCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }
            
            command.SetId(Identity);
            var beam = await Mediator.Send(command);

            return Ok(beam.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveBeamCommand();
            command.SetId(Identity);

            var beam = await Mediator.Send(command);

            return Ok(beam.Identity);
        }
    }
}
