using Barebone.Controllers;
using Manufactures.Domain.Shifts;
using Manufactures.Domain.Shifts.Commands;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Dtos.Shift;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/shifts")]
    [ApiController]
    [Authorize]
    public class ShiftController : ControllerApiBase
    {
        private readonly IShiftRepository _shiftRepository;

        public ShiftController(IServiceProvider serviceProvider,
                               IWorkContext workContext) : base(serviceProvider)
        {
            _shiftRepository =
               this.Storage.GetRepository<IShiftRepository>();
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
                _shiftRepository.Query.OrderByDescending(item => item.CreatedDate);
            var shiftDocuments =
                _shiftRepository.Find(query).Select(x => new ShiftDto(x));

            if (!string.IsNullOrEmpty(keyword))
            {
                shiftDocuments =
                    shiftDocuments
                        .Where(entity => entity.Name
                                               .Contains(keyword,
                                                         StringComparison.OrdinalIgnoreCase))
                        .ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(ShiftDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    shiftDocuments =
                        shiftDocuments.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    shiftDocuments =
                        shiftDocuments.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            var ResultShiftDocuments = shiftDocuments.Skip(page * size).Take(size).ToList();
            int totalRows = shiftDocuments.Count();
            int resultCount = ResultShiftDocuments.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(ResultShiftDocuments, info: new
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
            var shiftDocument =
                _shiftRepository
                    .Find(item => item.Identity == Identity)
                    .FirstOrDefault();
            await Task.Yield();

            if (shiftDocument == null)
            {
                return NotFound();
            }
            else
            {
                var resultData = new ShiftDto(shiftDocument);

                return Ok(resultData);
            }
        }

        [HttpGet("check-shift/{time}")]
        public async Task<IActionResult> CheckShift(string time)
        {
            var checkTime = TimeSpan.Parse(time);
            var defaultTime = checkTime;
            var query = _shiftRepository.Query;
            var existingShift =
                _shiftRepository
                    .Find(query)
                    .Select(shift => new ShiftDto(shift));

            foreach (var shift in existingShift)
            {
                checkTime = defaultTime;
                var shiftEnd = new TimeSpan();
                var isMoreDays = false;

                if (shift.StartTime > shift.EndTime)
                {
                    if (checkTime < shift.StartTime)
                    {
                        // Add more days
                        checkTime = checkTime + TimeSpan.FromHours(24);
                    }

                    // Add more days
                    shiftEnd = shift.EndTime + TimeSpan.FromHours(24);
                    isMoreDays = true;
                }

                if (checkTime >= shift.StartTime)
                {
                    if (isMoreDays == false)
                    {
                        if (checkTime <= shift.EndTime)
                        {
                            await Task.Yield();
                            return Ok(shift);
                        }
                    }
                    else
                    {
                        if (checkTime <= shiftEnd)
                        {
                            await Task.Yield();
                            return Ok(shift);
                        }
                    }
                }
            }

            await Task.Yield();
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddShiftCommand command)
        {
            var existingOperator =
                _shiftRepository
                    .Query
                    .Where(o => o.Name.Equals(command.Name))
                    .FirstOrDefault();

            if (existingOperator != null)
            {
                throw Validator.ErrorValidation(("Name", "Has existing ShiftDocumentId Name"));
            }

            var shiftDocument = await Mediator.Send(command);

            return Ok(shiftDocument.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateShiftCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var shiftDocument = await Mediator.Send(command);

            return Ok(shiftDocument.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveShiftCommand();
            command.SetId(Identity);
            var shiftDocument = await Mediator.Send(command);

            return Ok(shiftDocument.Identity);
        }
    }
}
