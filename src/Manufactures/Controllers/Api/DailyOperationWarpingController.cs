using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Warping;
using Manufactures.Application.DailyOperations.Warping.DTOs;
using Manufactures.Application.Operators.DTOs;
using Manufactures.Application.Shifts.DTOs;
using Manufactures.Domain.Beams.Queries;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts.Queries;
using Manufactures.Domain.StockCard.Events.Warping;
using Manufactures.Dtos.Beams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    /**
     * This is Warping daily operation controller
     * 
     * **/
    [Produces("application/json")]
    [Route("weaving/daily-operations-warping")]
    [ApiController]
    [Authorize]
    public class DailyOperationWarpingController : ControllerApiBase
    {
        private readonly IWarpingQuery<DailyOperationWarpingListDto> _warpingQuery;
        private readonly IOperatorQuery<OperatorListDto> _operatorQuery;
        private readonly IShiftQuery<ShiftDto> _shiftQuery;
        private readonly IBeamQuery<BeamListDto> _beamQuery;

        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationWarpingController(IServiceProvider serviceProvider,
                                               IWarpingQuery<DailyOperationWarpingListDto> warpingQuery,
                                               IOperatorQuery<OperatorListDto> operatorQuery,
                                               IShiftQuery<ShiftDto> shiftQuery,
                                               IBeamQuery<BeamListDto> beamQuery)
            : base(serviceProvider)
        {
            _warpingQuery = warpingQuery ?? throw new ArgumentNullException(nameof(warpingQuery));
            _operatorQuery = operatorQuery ?? throw new ArgumentNullException(nameof(operatorQuery));
            _shiftQuery = shiftQuery ?? throw new ArgumentNullException(nameof(shiftQuery));
            _beamQuery = beamQuery ?? throw new ArgumentNullException(nameof(beamQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            var dailyOperationWarpingDocuments = await _warpingQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                dailyOperationWarpingDocuments =
                    dailyOperationWarpingDocuments
                        .Where(x => x.ConstructionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.OrderProductionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(DailyOperationWarpingListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    await Task.Yield();
                    dailyOperationWarpingDocuments =
                        dailyOperationWarpingDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    await Task.Yield();
                    dailyOperationWarpingDocuments =
                        dailyOperationWarpingDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            int totalRows = dailyOperationWarpingDocuments.Count();
            var result = dailyOperationWarpingDocuments.Skip((page - 1) * size).Take(size);
            var resultCount = result.Count();

            return Ok(result, info: new { page, size, totalRows, resultCount });
        }

        [HttpGet("get-beams")]
        public async Task<IActionResult> GetBeam(string keyword = null)
        {
            var beams = await _beamQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                beams = beams.Where(x => x.Type.Equals("Warping")&& x.Number.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            }

            var result = beams;
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var identity = Guid.Parse(Id);
            var dailyOperationWarpingDocument = await _warpingQuery.GetById(identity);

            if (dailyOperationWarpingDocument == null)
            {
                return NotFound(identity);
            }

            return Ok(dailyOperationWarpingDocument);
        }

        //Preparation Warping Daily Operation Request
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PreparationDailyOperationWarpingCommand command)
        {
            // Sending command to command handler
            var dailyOperationWarping = await Mediator.Send(command);

            //Return result from command handler as Identity(Id)
            return Ok(dailyOperationWarping.Identity);
        }

        //Start Warping Daily Operation Request
        [HttpPut("{Id}/start-process")]
        public async Task<IActionResult> Start(string Id, [FromBody]UpdateStartDailyOperationWarpingCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateStartDailyOperationWarping = await Mediator.Send(command);

            return Ok(updateStartDailyOperationWarping.Identity);
            //// Sending command to command handler
            //var dailyOperationWarping = await Mediator.Send(command);

            ////Extract warping beam from command handler as Identity(Id)
            //await Task.Yield();
            //var warpingBeams = 
            //    dailyOperationWarping
            //        .WarpingBeamProducts
            //        .Select(x => new DailyOperationWarpingBeamProductDto(x)).ToList();

            ////Extract history
            //var warpingHistory =
            //    dailyOperationWarping
            //        .WarpingHistories;
            //var historys = new List<DailyOperationHistory>();

            //foreach (var history in warpingHistory)
            //{
            //    await Task.Yield();
            //    var operatorById = await _operatorQuery.GetById(history.OperatorDocumentId);

            //    await Task.Yield();
            //    var shiftById = await _shiftQuery.GetById(history.ShiftDocumentId);

            //    await Task.Yield();
            //    var operationHistory =
            //        new DailyOperationHistory(history.Identity,
            //                                  history.WarpingBeamNumber,
            //                                  operatorById.Username,
            //                                  operatorById.Group,
            //                                  history.DateTimeMachine,
            //                                  history.MachineStatus,
            //                                  shiftById.Name);

            //    historys.Add(operationHistory);
            //}

            //await Task.Yield();
            //var result = new StartProcessDto(warpingBeams, historys);

            //return Ok(result);
        }

        //Pause Warping Daily Operation Request
        [HttpPut("{Id}/pause-process")]
        public async Task<IActionResult> Pause(string Id, [FromBody]UpdatePauseDailyOperationWarpingCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updatePauseDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updatePauseDailyOperationSizingDocument.Identity);

            //// Sending command to command handler
            //var dailyOperationWarping = await Mediator.Send(command);

            ////Return result from command handler as Identity(Id)
            //var warpingHistory =
            //    dailyOperationWarping
            //        .WarpingHistories;

            //var result = new List<DailyOperationHistory>();

            //foreach(var history in warpingHistory)
            //{
            //    await Task.Yield();
            //    var operatorById = await _operatorQuery.GetById(history.OperatorDocumentId);

            //    await Task.Yield();
            //    var shiftById = await _shiftQuery.GetById(history.ShiftDocumentId);

            //    var operationHistory =
            //        new DailyOperationHistory(history.Identity, 
            //                                  history.WarpingBeamNumber, 
            //                                  operatorById.Username, 
            //                                  operatorById.Group, 
            //                                  history.DateTimeMachine, 
            //                                  history.MachineStatus, 
            //                                  shiftById.Name);

            //    result.Add(operationHistory);
            //}

            //return Ok(result);
        }

        //Resume Warping Daily Operation Request
        [HttpPut("{Id}/resume-process")]
        public async Task<IActionResult> Resume(string Id, [FromBody]UpdateResumeDailyOperationWarpingCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateResumeDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updateResumeDailyOperationSizingDocument.Identity);
            //// Sending command to command handler
            //var dailyOperationWarping = await Mediator.Send(command);

            ////Extract history
            //await Task.Yield();
            //var warpingHistory =
            //    dailyOperationWarping
            //        .WarpingHistories;

            //var result = new List<DailyOperationHistory>();

            //foreach (var history in warpingHistory)
            //{
            //    await Task.Yield();
            //    var operatorById = await _operatorQuery.GetById(history.OperatorDocumentId);

            //    await Task.Yield();
            //    var shiftById = await _shiftQuery.GetById(history.ShiftDocumentId);

            //    var operationHistory =
            //        new DailyOperationHistory(history.Identity,
            //                                  history.WarpingBeamNumber,
            //                                  operatorById.Username,
            //                                  operatorById.Group,
            //                                  history.DateTimeMachine,
            //                                  history.MachineStatus,
            //                                  shiftById.Name);

            //    result.Add(operationHistory);
            //}

            //return Ok(result);
        }

        //Produce Beams Warping Daily Operation Request
        [HttpPut("{Id}/produce-beams-process")]
        public async Task<IActionResult> ProduceBeams(string Id, [FromBody]ProduceBeamsDailyOperationWarpingCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var produceBeamsDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(produceBeamsDailyOperationSizingDocument.Identity);

            //// Sending command to command handler
            //var dailyOperationWarping = await Mediator.Send(command);

            ////Extract warping beam from command handler as Identity(Id)
            //await Task.Yield();
            //var warpingBeams =
            //    dailyOperationWarping
            //        .WarpingBeamProducts
            //        .Select(x => new DailyOperationBeamProductDto(x)).ToList();

            ////Get Latest product
            //await Task.Yield();
            //var latestBeamProduct =
            //    dailyOperationWarping
            //        .WarpingBeamProducts
            //        .OrderByDescending(x => x.CreatedDate)
            //        .FirstOrDefault();

            ////Preparing Event
            //var addStockEvent = new MoveInBeamStockWarpingEvent();

            ////Manipulate datetime to be stocknumber
            //var dateTimeNow = DateTimeOffset.UtcNow.AddHours(7);
            //StringBuilder stockNumber = new StringBuilder();
            //stockNumber.Append(dateTimeNow.ToString("HH"));
            //stockNumber.Append("/");
            //stockNumber.Append(dateTimeNow.ToString("mm"));
            //stockNumber.Append("/");
            //stockNumber.Append("stock-weaving");
            //stockNumber.Append("/");
            //stockNumber.Append(dateTimeNow.ToString("dd'/'MM'/'yyyy"));

            ////Initiate events
            //addStockEvent.BeamId = new BeamId(latestBeamProduct.WarpingBeamId);
            //addStockEvent.StockNumber = stockNumber.ToString();
            //addStockEvent.DailyOperationId = new DailyOperationId(dailyOperationWarping.Identity);
            //addStockEvent.DateTimeOperation = dateTimeNow;

            ////Update stock
            //await Mediator.Publish(addStockEvent);

            ////Extract history
            //var warpingHistory =
            //    dailyOperationWarping
            //        .WarpingHistories;
            //var historys = new List<DailyOperationHistory>();

            //foreach (var history in warpingHistory)
            //{
            //    await Task.Yield();
            //    var operatorById = await _operatorQuery.GetById(history.OperatorDocumentId);

            //    await Task.Yield();
            //    var shiftById = await _shiftQuery.GetById(history.ShiftDocumentId);

            //    await Task.Yield();
            //    var operationHistory =
            //        new DailyOperationHistory(history.Identity,
            //                                  history.WarpingBeamNumber,
            //                                  operatorById.Username,
            //                                  operatorById.Group,
            //                                  history.DateTimeMachine,
            //                                  history.MachineStatus,
            //                                  shiftById.Name);

            //    historys.Add(operationHistory);
            //}

            //await Task.Yield();
            //var result = new StartProcessDto(warpingBeams, historys);

            //return Ok(result);
        }

        //Finish Warping Daily Operation Request
        [HttpPut("{Id}/finish-process")]
        public async Task<IActionResult> Finish(string Id, [FromBody]FinishDailyOperationWarpingCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var finishDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(finishDailyOperationSizingDocument.Identity);
        }
    }
}
