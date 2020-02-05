using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects.DailyOperationReachingReport;
using Manufactures.Application.Operators.DTOs;
using Manufactures.Application.Shifts.DTOs;
using Manufactures.Domain.BeamStockMonitoring.Repositories;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.DailyOperations.Reaching.Queries;
using Manufactures.Domain.DailyOperations.Reaching.Queries.DailyOperationReachingReport;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Domain.Shifts.Queries;
using Manufactures.Helpers.XlsTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/daily-operations-reaching")]
    [ApiController]
    [Authorize]
    public class DailyOperationReachingController : ControllerApiBase
    {
        private readonly IDailyOperationReachingQuery<DailyOperationReachingListDto> _reachingQuery;
        private readonly IDailyOperationReachingBeamQuery<DailyOperationReachingBeamDto> _reachingBeamQuery;
        private readonly IOperatorQuery<OperatorListDto> _operatorQuery;
        private readonly IShiftQuery<ShiftDto> _shiftQuery;
        private readonly IDailyOperationReachingReportQuery<DailyOperationReachingReportListDto> _dailyOperationReachingReportQuery;

        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingRepository;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingRepository;
        private readonly IBeamStockMonitoringRepository
            _beamStockMonitoringRepository;

        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationReachingController(IServiceProvider serviceProvider,
                                                IWorkContext workContext,
                                                IDailyOperationReachingQuery<DailyOperationReachingListDto> reachingQuery,
                                                IDailyOperationReachingBeamQuery<DailyOperationReachingBeamDto> reachingBeamQuery,
                                                IOperatorQuery<OperatorListDto> operatorQuery,
                                                IShiftQuery<ShiftDto> shiftQuery,
                                                IDailyOperationReachingReportQuery<DailyOperationReachingReportListDto> dailyOperationReachingReportQuery)
            : base(serviceProvider)
        {
            _reachingQuery = reachingQuery ?? throw new ArgumentNullException(nameof(reachingQuery));
            _reachingBeamQuery = reachingBeamQuery ?? throw new ArgumentNullException(nameof(reachingBeamQuery));
            _operatorQuery = operatorQuery ?? throw new ArgumentNullException(nameof(operatorQuery));
            _shiftQuery = shiftQuery ?? throw new ArgumentNullException(nameof(shiftQuery));
            _dailyOperationReachingReportQuery = dailyOperationReachingReportQuery ?? throw new ArgumentNullException(nameof(dailyOperationReachingReportQuery));

            _dailyOperationReachingRepository =
                this.Storage.GetRepository<IDailyOperationReachingRepository>();
            _dailyOperationSizingRepository =
                this.Storage.GetRepository<IDailyOperationSizingRepository>();
            _beamStockMonitoringRepository =
                this.Storage.GetRepository<IBeamStockMonitoringRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                            int size = 25,
                                            string order = "{}",
                                            string keyword = null,
                                            string filter = "{}")
        {
            VerifyUser();
            var dailyOperationReachingDocuments = await _reachingQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                dailyOperationReachingDocuments =
                    dailyOperationReachingDocuments
                        .Where(x => x.ConstructionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.MachineNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.SizingBeamNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(DailyOperationReachingListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    await Task.Yield();
                    dailyOperationReachingDocuments =
                        dailyOperationReachingDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    await Task.Yield();
                    dailyOperationReachingDocuments =
                        dailyOperationReachingDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            //int totalRows = dailyOperationReachingDocuments.Count();
            var result = dailyOperationReachingDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            VerifyUser();
            var identity = Guid.Parse(Id);
            var dailyOperationReachingDocument = await _reachingQuery.GetById(identity);

            if (dailyOperationReachingDocument == null)
            {
                return NotFound(identity);
            }

            return Ok(dailyOperationReachingDocument);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PreparationDailyOperationReachingCommand command)
        {
            VerifyUser();
            // Sending Command to Command Handler
            var dailyOperationReaching = await Mediator.Send(command);

            //Reformat DateTime
            var year = command.PreparationDate.Year;
            var month = command.PreparationDate.Month;
            var day = command.PreparationDate.Day;
            var hour = command.PreparationTime.Hours;
            var minutes = command.PreparationTime.Minutes;
            var seconds = command.PreparationTime.Seconds;
            var dateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Get Last Beam Stock Monitoring Which Used Same Beam Id
            var beamStockMonitoring =
                _beamStockMonitoringRepository
                    .Query
                    .Where(o=>o.BeamDocumentId.Equals(command.SizingBeamId.Value) &&
                           o.OrderDocumentId.Equals(command.OrderDocumentId.Value))
                    .OrderByDescending(o => o.CreatedDate);

            var sameBeamIdBeamStockMonitoring =
                _beamStockMonitoringRepository
                    .Find(beamStockMonitoring)
                    .FirstOrDefault();

            sameBeamIdBeamStockMonitoring.SetReachingEntryDate(dateTime);
            sameBeamIdBeamStockMonitoring.SetReachingLengthStock(sameBeamIdBeamStockMonitoring.SizingLengthStock);
            sameBeamIdBeamStockMonitoring.SetPosition(2);

            await _beamStockMonitoringRepository.Update(sameBeamIdBeamStockMonitoring);
            Storage.Save();

            //Return Result from Command Handler as Identity(Id)
            return Ok(dailyOperationReaching.Identity);
        }

        [HttpPut("{Id}/reaching-in-start")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]UpdateReachingInStartDailyOperationReachingCommand command)
        {
            VerifyUser();
            //Parse Id
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);

            var updateReachingInStartDailyOperationReachingDocument = await Mediator.Send(command);

            return Ok(updateReachingInStartDailyOperationReachingDocument.Identity);
        }

        [HttpPut("{Id}/reaching-in-change-operator")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]ChangeOperatorReachingInDailyOperationReachingCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var changeOperatorReachingInDailyOperationReachingDocument = await Mediator.Send(command);

            return Ok(changeOperatorReachingInDailyOperationReachingDocument.Identity);
        }

        [HttpPut("{Id}/reaching-in-finish")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]UpdateReachingInFinishDailyOperationReachingCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateReachingInFinishDailyOperationReachingDocument = await Mediator.Send(command);

            return Ok(updateReachingInFinishDailyOperationReachingDocument.Identity);
        }

        [HttpPut("{Id}/comb-start")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]UpdateCombStartDailyOperationReachingCommand command)
        {
            VerifyUser();
            //Parse Id
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);

            var updateCombStartDailyOperationReachingDocument = await Mediator.Send(command);

            return Ok(updateCombStartDailyOperationReachingDocument.Identity);
        }

        [HttpPut("{Id}/comb-change-operator")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]ChangeOperatorCombDailyOperationReachingCommand command)
        {
            VerifyUser();
            //Parse Id
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);

            var changeOperatorCombDailyOperationReachingDocument = await Mediator.Send(command);

            return Ok(changeOperatorCombDailyOperationReachingDocument.Identity);
        }

        [HttpPut("{Id}/comb-finish")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]UpdateCombFinishDailyOperationReachingCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateCombFinishDailyOperationReachingDocument = await Mediator.Send(command);

            return Ok(updateCombFinishDailyOperationReachingDocument.Identity);
        }

        //Controller for Daily Operation Reaching Report
        [HttpGet("get-report")]
        public async Task<IActionResult> GetReport(string machineId, 
                                                   string orderId, 
                                                   string constructionId, 
                                                   string beamId, 
                                                   string operationStatus, 
                                                   int unitId = 0, 
                                                   DateTimeOffset? dateFrom = null, 
                                                   DateTimeOffset? dateTo = null, 
                                                   int page = 1, 
                                                   int size = 25,
                                                   string order = "{}")
        {
            VerifyUser();
            var acceptRequest = Request.Headers.Values.ToList();
            var index = acceptRequest.IndexOf("application/xls") > 0;

            var dailyOperationReachingReport = await _dailyOperationReachingReportQuery.GetReports(machineId, 
                                                                                                   orderId, 
                                                                                                   constructionId, 
                                                                                                   beamId, 
                                                                                                   operationStatus, 
                                                                                                   unitId, 
                                                                                                   dateFrom, 
                                                                                                   dateTo, 
                                                                                                   page, 
                                                                                                   size,
                                                                                                   order);

            await Task.Yield();
            if (index.Equals(true))
            {
                byte[] xlsInBytes;

                DailyOperationReachingReportXlsTemplate xlsTemplate = new DailyOperationReachingReportXlsTemplate();
                MemoryStream xls = xlsTemplate.GenerateDailyOperationReachingReportXls(dailyOperationReachingReport.Item1.ToList());
                xlsInBytes = xls.ToArray();
                var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Operasional Mesin Harian Reaching");
                return xlsFile;
            }
            else
            {
                return Ok(dailyOperationReachingReport.Item1, info: new
                {
                    count = dailyOperationReachingReport.Item2
                });
            }
        }

        [HttpGet("get-reaching-beam-products")]
        public async Task<IActionResult> GetReachingBeamProductsByOrder(string orderId,
                                                                        string keyword = null,
                                                                        string filter = "{}",
                                                                        int page = 1,
                                                                        int size = 25,
                                                                        string order = "{}")
        {
            VerifyUser();
            var dailyOperationReachingBeamDocuments = await _reachingBeamQuery.GetReachingBeamProductsByOrder(orderId,
                                                                                                              keyword,
                                                                                                              filter, 
                                                                                                              page, 
                                                                                                              size, 
                                                                                                              order);
            return Ok(dailyOperationReachingBeamDocuments.Item1, info: new
            {
                count = dailyOperationReachingBeamDocuments.Item2
            });
        }
    }
}
