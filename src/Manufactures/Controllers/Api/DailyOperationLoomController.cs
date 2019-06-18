using Barebone.Controllers;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Dtos.DailyOperations.Loom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.Shifts.Repositories;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/daily-operations-loom")]
    [ApiController]
    [Authorize]
    public class DailyOperationLoomController : ControllerApiBase
    {
        private readonly IDailyOperationLoomRepository
            _dailyOperationalDocumentRepository;
        private readonly IWeavingOrderDocumentRepository
            _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository
            _constructionDocumentRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IShiftRepository
            _shiftRepository;

        public DailyOperationLoomController(IServiceProvider serviceProvider,
                                                 IWorkContext workContext)
            : base(serviceProvider)
        {
            _dailyOperationalDocumentRepository =
                this.Storage.GetRepository<IDailyOperationLoomRepository>();
            _weavingOrderDocumentRepository =
                this.Storage.GetRepository<IWeavingOrderDocumentRepository>();
            _constructionDocumentRepository =
                this.Storage.GetRepository<IFabricConstructionRepository>();
            _machineRepository =
                this.Storage.GetRepository<IMachineRepository>();
            _operatorRepository =
                this.Storage.GetRepository<IOperatorRepository>();
            _beamRepository =
                this.Storage.GetRepository<IBeamRepository>();
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
            //Set page
            page = page - 1;
            //Add query
            var query =
                _dailyOperationalDocumentRepository
                    .Query
                    .Include(d => d.DailyOperationLoomDetails)
                    .OrderByDescending(item => item.CreatedDate);
            //Execute existing daily operation
            var dailyOperationalMachineDocuments =
                _dailyOperationalDocumentRepository
                    .Find(query);
            //Initiate new List 
            var dailyOperationLooms = new List<DailyOperationLoomListDto>();
            //Extract information from latest existing daily operation
            foreach (var dailyOperation in dailyOperationalMachineDocuments)
            {
                var dateOperated = new DateTimeOffset();

                var machineDocument =
                    await _machineRepository
                        .Query
                        .Where(d => d.Identity.Equals(dailyOperation.MachineId.Value))
                        .FirstOrDefaultAsync();
                var orderDocument =
                       await _weavingOrderDocumentRepository
                           .Query
                           .Where(o => o.Identity.Equals(dailyOperation.OrderId.Value))
                           .FirstOrDefaultAsync();

                foreach (var detail in dailyOperation.DailyOperationMachineDetails)
                {

                    if (detail.OperationStatus == DailyOperationMachineStatus.ONENTRY)
                    {
                        dateOperated = detail.DateTimeOperation;
                    }
                }

                var dto =
                    new DailyOperationLoomListDto(dailyOperation,
                                                  orderDocument.OrderNumber,
                                                  machineDocument.MachineNumber,
                                                  dateOperated);

                dailyOperationLooms.Add(dto);
            }
            //Search by keyword
            if (!string.IsNullOrEmpty(keyword))
            {
                dailyOperationLooms =
                    dailyOperationLooms.Where(entity => entity
                                                .OrderNumber
                                                .Contains(keyword,
                                                          StringComparison
                                                            .OrdinalIgnoreCase) ||
                                              entity
                                                .MachineNumber
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
                    typeof(DailyOperationLoomListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    dailyOperationLooms =
                        dailyOperationLooms
                            .OrderBy(x => prop.GetValue(x, null))
                            .ToList();
                }
                else
                {
                    dailyOperationLooms =
                        dailyOperationLooms
                            .OrderByDescending(x => prop.GetValue(x, null))
                            .ToList();
                }
            }
            //Give to final result
            var ResultDailyOperationLooms =
                dailyOperationLooms.Skip(page * size).Take(size).ToList();
            int totalRows = dailyOperationLooms.Count();
            int resultCount = ResultDailyOperationLooms.Count();
            page = page + 1;

            await Task.Yield();
            //Return to end point
            return Ok(ResultDailyOperationLooms, info: new
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
            var query =
                _dailyOperationalDocumentRepository
                    .Query
                    .Include(p => p.DailyOperationLoomDetails)
                    .Where(o => o.Identity == Identity);
            var dailyOperationalLoom =
                _dailyOperationalDocumentRepository
                    .Find(query)
                    .FirstOrDefault();
            var operationDate = new DateTimeOffset();
            var machineNumber =
                _machineRepository
                    .Find(o => o.Identity.Equals(dailyOperationalLoom.MachineId.Value))
                    .FirstOrDefault()
                    .MachineNumber;
            var order =
                _weavingOrderDocumentRepository
                    .Find(o => o.Identity.Equals(dailyOperationalLoom.OrderId.Value))
                    .FirstOrDefault();
            var orderNumber = order.OrderNumber;
            var fabricConstructionNumber =
                _constructionDocumentRepository
                    .Find(o => o.Identity.Equals(order.ConstructionId.Value))
                    .FirstOrDefault()
                    .ConstructionNumber;
            var beamNumber =
                _beamRepository
                    .Find(o => o.Identity.Equals(dailyOperationalLoom.BeamId.Value))
                    .FirstOrDefault()
                    .Number;
            var historys = new List<DailyOperationLoomHistoryDto>();

            foreach (var detail in dailyOperationalLoom.DailyOperationMachineDetails)
            {
                var beamOperator =
                    _operatorRepository
                        .Find(o => o.Identity.Equals(detail.BeamOperatorId))
                        .FirstOrDefault();
                var shiftName =
                    _shiftRepository
                        .Find(o => o.Identity.Equals(detail.ShiftId))
                        .FirstOrDefault()
                        .Name;

                if (detail.OperationStatus == DailyOperationMachineStatus.ONENTRY)
                {
                    operationDate = detail.DateTimeOperation;
                }

                var history =
                    new DailyOperationLoomHistoryDto(detail.Identity,
                                                     beamOperator.CoreAccount.Name,
                                                     beamOperator.Group,
                                                     detail.DateTimeOperation,
                                                     detail.OperationStatus,
                                                     shiftName);

                historys.Add(history);
            }

            var result =
                new DailyOperationLoomByIdDto(dailyOperationalLoom.Identity,
                                              operationDate,
                                              dailyOperationalLoom.UnitId.Value,
                                              machineNumber,
                                              beamNumber,
                                              orderNumber,
                                              fabricConstructionNumber);

            if (historys.Count > 0)
            {
                result.LoomHistory = historys;
            }

            await Task.Yield();

            if (Identity == null || result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost("entry-process")]
        public async Task<IActionResult> EntryPost([FromBody]AddNewDailyOperationLoomCommand command)
        {
            var dailyOperationLoom = await Mediator.Send(command);

            var dailyOperationLoomDetailAsHistory = dailyOperationLoom.DailyOperationMachineDetails;

            return Ok(dailyOperationLoom.Identity);
        }

        [HttpPut("start-process")]
        public async Task<IActionResult> StartPost([FromBody]StartDailyOperationLoomCommand command)
        {
            var dailyOperationLoom = await Mediator.Send(command);
            var historys = new List<DailyOperationLoomHistoryDto>();

            foreach (var detail in dailyOperationLoom.DailyOperationMachineDetails)
            {
                var beamOperator =
                    _operatorRepository.Find(e => e.Identity.Equals(detail.BeamOperatorId))
                                       .FirstOrDefault();
                var shiftName =
                    _shiftRepository
                        .Find(o => o.Identity.Equals(detail.ShiftId))
                        .FirstOrDefault()
                        .Name;

                var result =
                    new DailyOperationLoomHistoryDto(detail.Identity,
                                                     beamOperator.CoreAccount.Name,
                                                     beamOperator.Group,
                                                     detail.DateTimeOperation,
                                                     detail.OperationStatus,
                                                     shiftName);
                historys.Add(result);
            }

            historys =
                historys
                    .OrderByDescending(field => field.DateTimeOperation)
                    .ToList();

            return Ok(historys);
        }

        [HttpPut("stop-process")]
        public async Task<IActionResult> StopPost([FromBody]StopDailyOperationLoomCommand command)
        {
            var dailyOperationLoom = await Mediator.Send(command);
            var historys = new List<DailyOperationLoomHistoryDto>();

            foreach (var detail in dailyOperationLoom.DailyOperationMachineDetails)
            {
                var beamOperator =
                    _operatorRepository.Find(e => e.Identity.Equals(detail.BeamOperatorId))
                                       .FirstOrDefault();
                var shiftName =
                    _shiftRepository
                        .Find(o => o.Identity.Equals(detail.ShiftId))
                        .FirstOrDefault()
                        .Name;
                var result =
                    new DailyOperationLoomHistoryDto(detail.Identity,
                                                     beamOperator.CoreAccount.Name,
                                                     beamOperator.Group,
                                                     detail.DateTimeOperation,
                                                     detail.OperationStatus,
                                                     shiftName);
                historys.Add(result);
            }

            historys =
               historys
                   .OrderByDescending(field => field.DateTimeOperation)
                   .ToList();

            return Ok(historys);
        }

        [HttpPut("resume-process")]
        public async Task<IActionResult> ResumePost([FromBody]ResumeDailyOperationLoomCommand command)
        {
            var dailyOperationLoom = await Mediator.Send(command);
            var historys = new List<DailyOperationLoomHistoryDto>();

            foreach (var detail in dailyOperationLoom.DailyOperationMachineDetails)
            {
                var beamOperator =
                    _operatorRepository.Find(e => e.Identity.Equals(detail.BeamOperatorId))
                                       .FirstOrDefault();
                var shiftName =
                   _shiftRepository
                       .Find(o => o.Identity.Equals(detail.ShiftId))
                       .FirstOrDefault()
                       .Name;
                var result =
                    new DailyOperationLoomHistoryDto(detail.Identity,
                                                     beamOperator.CoreAccount.Name,
                                                     beamOperator.Group,
                                                     detail.DateTimeOperation,
                                                     detail.OperationStatus,
                                                     shiftName);
                historys.Add(result);
            }

            historys =
               historys
                   .OrderByDescending(field => field.DateTimeOperation)
                   .ToList();

            return Ok(historys);
        }

        [HttpPut("finish-process")]
        public async Task<IActionResult> FinishPost([FromBody]FinishDailyOperationLoomCommand command)
        {
            var dailyOperationLoom = await Mediator.Send(command);
            var historys = new List<DailyOperationLoomHistoryDto>();

            foreach (var detail in dailyOperationLoom.DailyOperationMachineDetails)
            {
                var beamOperator =
                    _operatorRepository.Find(e => e.Identity.Equals(detail.BeamOperatorId))
                                       .FirstOrDefault();
                var shiftName =
                   _shiftRepository
                       .Find(o => o.Identity.Equals(detail.ShiftId))
                       .FirstOrDefault()
                       .Name;
                var result =
                    new DailyOperationLoomHistoryDto(detail.Identity,
                                                     beamOperator.CoreAccount.Name,
                                                     beamOperator.Group,
                                                     detail.DateTimeOperation,
                                                     detail.OperationStatus,
                                                     shiftName);
                historys.Add(result);
            }

            historys =
               historys
                   .OrderByDescending(field => field.DateTimeOperation)
                   .ToList();

            return Ok(historys);
        }

        [HttpPut("update-loom-shift")]
        public async Task<IActionResult> UpdateShift([FromBody]UpdateShiftDailyOperationLoomCommand command)
        {
            var dailyOperationLoom = await Mediator.Send(command);
            var historys = new List<DailyOperationLoomHistoryDto>();

            foreach (var detail in dailyOperationLoom.DailyOperationMachineDetails)
            {
                var beamOperator =
                    _operatorRepository.Find(e => e.Identity.Equals(detail.BeamOperatorId))
                                       .FirstOrDefault();
                var shiftName =
                   _shiftRepository
                       .Find(o => o.Identity.Equals(detail.ShiftId))
                       .FirstOrDefault()
                       .Name;
                var result =
                    new DailyOperationLoomHistoryDto(detail.Identity,
                                                     beamOperator.CoreAccount.Name,
                                                     beamOperator.Group,
                                                     detail.DateTimeOperation,
                                                     detail.OperationStatus,
                                                     shiftName);
                historys.Add(result);
            }

            historys =
               historys
                   .OrderByDescending(field => field.DateTimeOperation)
                   .ToList();
            await Task.Yield();

            return Ok(historys);
        }
    }
}
