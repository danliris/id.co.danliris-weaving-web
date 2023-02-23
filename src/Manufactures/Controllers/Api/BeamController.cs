using Barebone.Controllers;
using Manufactures.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.Commands;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.DataTransferObjects.Beams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            //var query =
            //    _beamRepository.Query.OrderByDescending(o => o.CreatedDate);

            page = page < 1 ? 1 : page;
            var query = _beamRepository.Query;

            if (!filter.Contains("{}"))
            {
                Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                query = QueryHelper<BeamReadModel>.Filter(query, filterDictionary);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(w => w.Number.Contains(keyword, StringComparison.OrdinalIgnoreCase) || w.Type.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                query = QueryHelper<BeamReadModel>.Order(query, orderDictionary);
            }

            var total = query.Count();
            var data = await query.Skip((page - 1) * size).Take(size).Select(s => new BeamDocument(s)).Select(s => new BeamDto(s)).ToListAsync();

            //var beams = _beamRepository.Find(query).Select(o => new BeamDto(o));

            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    beams =
            //        beams.Where(entity =>
            //            entity.Number.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            //            entity.Type.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            //}

            //if (!order.Contains("{}"))
            //{
            //    Dictionary<string, string> orderDictionary =
            //        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
            //              orderDictionary.Keys.First().Substring(1);
            //    System.Reflection.PropertyInfo prop = typeof(BeamDto).GetProperty(key);

            //    if (orderDictionary.Values.Contains("asc"))
            //    {
            //        beams = beams.OrderBy(x => prop.GetValue(x, null));
            //    }
            //    else
            //    {
            //        beams = beams.OrderByDescending(x => prop.GetValue(x, null));
            //    }
            //}

            //if (!filter.Contains("{}"))
            //{
            //    Dictionary<string, string> filterDictionary =
            //        JsonConvert.DeserializeObject<Dictionary<string, string>>(filter);
            //    var key = filterDictionary.Keys.First().Substring(0, 1).ToUpper() +
            //              filterDictionary.Keys.First().Substring(1);
            //    System.Reflection.PropertyInfo prop = typeof(BeamDto).GetProperty(key);

            //    if (filterDictionary != null && filterDictionary.Count > 0)
            //    {
            //        foreach (var dct in filterDictionary)
            //        {
            //            var filterKey = dct.Key;
            //            var valueKey = dct.Value;

            //            var filterQuery = string.Concat(string.Empty, filterKey, " ==@0 ");

            //            var beamQuery = beams.AsQueryable().Where(filterQuery, valueKey);
            //        }
            //    }
            //}

            //var resultBeams = beams.Skip(page * size).Take(size);
            //int totalRows = beams.Count();
            //int resultCount = resultBeams.Count();
            //page = page + 1;

            //await Task.Yield();

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
