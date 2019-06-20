using Barebone.Controllers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Movements.Repositories;
using Manufactures.Dtos.Movements;
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
    [Route("weaving/movements")]
    [ApiController]
    [Authorize]
    public class MovementController : ControllerApiBase
    {
        public readonly IMovementRepository
            _movementRepository;
        private readonly IDailyOperationLoomRepository
            _dailyOperationalLoomRepository;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingRepository;
        private readonly IBeamRepository
            _beamRepository;

        public MovementController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _movementRepository =
                this.Storage.GetRepository<IMovementRepository>();
            _dailyOperationSizingRepository =
               this.Storage.GetRepository<IDailyOperationSizingRepository>();
            _dailyOperationalLoomRepository =
               this.Storage.GetRepository<IDailyOperationLoomRepository>();
            _beamRepository =
               this.Storage.GetRepository<IBeamRepository>();

        }

        [HttpGet("beam")]
        public async Task<IActionResult> GetBeamLoom(int page = 1,
                                           int size = 25,
                                           string order = "{}",
                                           string keyword = null,
                                           string filter = "{}")
        {
            page = page - 1;
            var queryMovement =
                _movementRepository
                    .Query
                    .OrderByDescending(item => item.CreatedDate);
            var movements =
                _movementRepository
                    .Find(queryMovement)
                    .Where(o => o.IsActive.Equals(true));
            var result = new List<BeamListDto>();
            //Extract value to dto
            foreach (var movement in movements)
            {
                if (movement.MovementType.Equals(MovementStatusConstant.SIZING))
                {
                    var dailyOperation =
                        _dailyOperationSizingRepository
                            .Find(o => o.Identity.Equals(movement.DailyOperationId.Value))
                            .FirstOrDefault();

                    foreach (var beamId in dailyOperation.WarpingBeamsId)
                    {
                        var beamNumber =
                       _beamRepository
                           .Find(o => o.Identity.Equals(beamId.Value))
                           .FirstOrDefault().Number;
                        var beamListDto = new BeamListDto(movement.Identity, movement.MovementType, beamNumber);
                        result.Add(beamListDto);
                    }

                }
                else if (movement.MovementType.Equals(MovementStatusConstant.LOOM))
                {
                    var dailyOperation =
                        _dailyOperationalLoomRepository
                            .Find(o => o.Identity.Equals(movement.DailyOperationId.Value))
                            .FirstOrDefault();
                    var beamNumber =
                        _beamRepository
                            .Find(o => o.Identity.Equals(dailyOperation.BeamId.Value))
                            .FirstOrDefault().Number;
                    var beamListDto = new BeamListDto(movement.Identity, movement.MovementType, beamNumber);
                    result.Add(beamListDto);
                }
            }

            //Search by keyword
            if (!string.IsNullOrEmpty(keyword))
            {
                result =
                    result.Where(entity => entity
                                                .BeamNumber
                                                .Contains(keyword,
                                                          StringComparison
                                                            .OrdinalIgnoreCase) ||
                                              entity
                                                .Status
                                                .Contains(keyword,
                                                          StringComparison
                                                            .OrdinalIgnoreCase))
                             .ToList();
            }

            //Order by
            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key =
                    orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                    orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop =
                    typeof(BeamListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    result =
                        result
                            .OrderBy(x => prop.GetValue(x, null))
                            .ToList();
                }
                else
                {
                    result =
                        result
                            .OrderByDescending(x => prop.GetValue(x, null))
                            .ToList();
                }
            }

            //Give to final result
            var resultDto = result.Skip(page * size).Take(size).ToList();
            var totalRows = result.Count();
            int resultCount = resultDto.Count();
            page = page + 1;

            await Task.Yield();
            return Ok(resultDto, info: new
            {
                page,
                size,
                total = totalRows,
                count = resultCount
            });
        }

        [HttpGet("machine-loom")]
        public async Task<IActionResult> GetMachineLoom(int page = 1,
                                           int size = 25,
                                           string order = "{}",
                                           string keyword = null,
                                           string filter = "{}")
        {
            page = page - 1;
            var query =
                _movementRepository
                    .Query
                    .OrderByDescending(item => item.CreatedDate);

            return Ok();
        }
    }
}
