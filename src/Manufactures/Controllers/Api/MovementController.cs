using Barebone.Controllers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Movements.Repositories;
using Manufactures.Dtos.Movements;
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

        [HttpGet("beam/{Id}/beam-number/{number}")]
        public async Task<IActionResult> GetBeamById(string Id, string number)
        {
            var Identity = Guid.Parse(Id);
            var queryMovement =
               _movementRepository
                   .Query
                   .OrderByDescending(item => item.CreatedDate);
            var movement =
                _movementRepository
                    .Find(queryMovement)
                    .Where(o => o.Identity.Equals(Identity))
                    .FirstOrDefault();

            if (movement.MovementType.Equals(MovementStatusConstant.SIZING))
            {
                var query =
                    _dailyOperationSizingRepository
                        .Query
                        .Include(o => o.Details);
                var dailyOperation =
                        _dailyOperationSizingRepository
                            .Find(query)
                            .Where(o => o.Identity.Equals(movement.DailyOperationId.Value))
                            .FirstOrDefault();

                foreach (var beamId in dailyOperation.WarpingBeamsId)
                {
                    var beam =
                       _beamRepository
                           .Find(o => o.Identity.Equals(beamId.Value))
                           .FirstOrDefault();

                    if (beam.Number.Equals(number))
                    {
                        var details = dailyOperation.Details;
                        details = details.OrderByDescending(o => o.DateTimeOperation).ToList();
                        var beamMovementDto =
                            new BeamMovementDto(movement.Identity,
                                                movement.MovementType,
                                                beam.Number);

                        foreach (var dailyOperationDetail in details)
                        {
                            var beamMovementDetailDto = new BeamMovementDetailDto(dailyOperationDetail);

                            beamMovementDto.BeamMovementDetails.Add(beamMovementDetailDto);
                        }

                        await Task.Yield();
                        return Ok(beamMovementDto);
                    }
                }
            }
            else if (movement.MovementType.Equals(MovementStatusConstant.LOOM))
            {
                var query =
                    _dailyOperationalLoomRepository
                        .Query
                        .Include(o => o.DailyOperationLoomDetails);
                var dailyOperation =
                        _dailyOperationalLoomRepository
                            .Find(query)
                            .Where(o => o.Identity.Equals(movement.DailyOperationId.Value))
                            .FirstOrDefault();
                var beam =
                       _beamRepository
                          .Find(o => o.Identity.Equals(dailyOperation.BeamId.Value) && o.Number.Equals(number))
                          .FirstOrDefault();

                var beamMovementDto =
                            new BeamMovementDto(movement.Identity,
                                                movement.MovementType,
                                                beam.Number);

                foreach(var detail in dailyOperation.DailyOperationMachineDetails)
                {
                    var beamMovementDetailDto = new BeamMovementDetailDto(detail);
                    beamMovementDto.BeamMovementDetails.Add(beamMovementDetailDto);
                }

                await Task.Yield();
                return Ok(beamMovementDto);
            }

            await Task.Yield();
            return Ok();
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
            var result = new List<BeamMovementListDto>();
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
                        var beam =
                       _beamRepository
                           .Find(o => o.Identity.Equals(beamId.Value))
                           .FirstOrDefault();
                        var beamListDto = new BeamMovementListDto(movement.Identity, movement.MovementType, beam);
                        result.Add(beamListDto);
                    }

                }
                else if (movement.MovementType.Equals(MovementStatusConstant.LOOM))
                {
                    var dailyOperation =
                        _dailyOperationalLoomRepository
                            .Find(o => o.Identity.Equals(movement.DailyOperationId.Value))
                            .FirstOrDefault();
                    var beam =
                        _beamRepository
                            .Find(o => o.Identity.Equals(dailyOperation.BeamId.Value))
                            .FirstOrDefault();
                    var beamListDto = new BeamMovementListDto(movement.Identity, movement.MovementType, beam);
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
                    typeof(BeamMovementListDto).GetProperty(key);

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

        [HttpGet("order")]
        public async Task<IActionResult> GetOrder(int page = 1,
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

            await Task.Yield();
            return Ok();
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
            await Task.Yield();
            return Ok();
        }
    }
}
