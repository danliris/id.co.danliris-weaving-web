using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.DailyOperationWarpingReport;
using Manufactures.Application.Helpers;
using Manufactures.Application.Operators.DataTransferObjects;
using Manufactures.Application.Shifts.DTOs;
using Manufactures.Domain.Beams.Queries;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Manufactures.Domain.DailyOperations.Warping.Queries.DailyOperationWarpingReport;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Domain.Shifts.Queries;
using Manufactures.DataTransferObjects.Beams;
using Manufactures.Helpers.XlsTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Manufactures.Domain.DailyOperations.Warping.Queries.WarpingProductionReport;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
using Manufactures.Helpers.PdfTemplates;
using System.Globalization;
using Manufactures.Domain.DailyOperations.Warping.Queries.WarpingBrokenThreadsReport;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport;

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
        private readonly IDailyOperationWarpingDocumentQuery<DailyOperationWarpingListDto> _warpingQuery;
        private readonly IOperatorQuery<OperatorListDto> _operatorQuery;
        private readonly IShiftQuery<ShiftDto> _shiftQuery;
        private readonly IBeamQuery<BeamListDto> _beamQuery;
        private readonly IDailyOperationWarpingReportQuery<DailyOperationWarpingReportListDto> _dailyOperationWarpingReportQuery;
        private readonly IWarpingProductionReportQuery<WarpingProductionReportListDto> _warpingProductionReportQuery;
        private readonly IWarpingBrokenThreadsReportQuery<WarpingBrokenThreadsReportListDto> _warpingBrokenReportQuery;

        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IDailyOperationWarpingHistoryRepository
            _dailyOperationWarpingHistoryRepository;
        private readonly IDailyOperationWarpingBeamProductRepository
            _dailyOperationWarpingBeamProductRepository;
        private readonly IDailyOperationWarpingBrokenCauseRepository
            _dailyOperationWarpingBrokenCauseRepository;

        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationWarpingController(IServiceProvider serviceProvider,
                                               IDailyOperationWarpingDocumentQuery<DailyOperationWarpingListDto> warpingQuery,
                                               IOperatorQuery<OperatorListDto> operatorQuery,
                                               IShiftQuery<ShiftDto> shiftQuery,
                                               IBeamQuery<BeamListDto> beamQuery,
                                               IDailyOperationWarpingReportQuery<DailyOperationWarpingReportListDto> dailyOperationWarpingReportQuery,
                                               IWarpingProductionReportQuery<WarpingProductionReportListDto> warpingProductionReportQuery,
                                               IWarpingBrokenThreadsReportQuery<WarpingBrokenThreadsReportListDto> warpingBrokenReportQuery)
            : base(serviceProvider)
        {
            _warpingQuery = warpingQuery ?? throw new ArgumentNullException(nameof(warpingQuery));
            _operatorQuery = operatorQuery ?? throw new ArgumentNullException(nameof(operatorQuery));
            _shiftQuery = shiftQuery ?? throw new ArgumentNullException(nameof(shiftQuery));
            _beamQuery = beamQuery ?? throw new ArgumentNullException(nameof(beamQuery));
            _dailyOperationWarpingReportQuery = dailyOperationWarpingReportQuery ?? throw new ArgumentNullException(nameof(dailyOperationWarpingReportQuery));
            _warpingProductionReportQuery = warpingProductionReportQuery ?? throw new ArgumentNullException(nameof(warpingProductionReportQuery));
            _warpingBrokenReportQuery = warpingBrokenReportQuery ?? throw new ArgumentNullException(nameof(warpingBrokenReportQuery));

            _dailyOperationWarpingRepository = this.Storage.GetRepository<IDailyOperationWarpingRepository>();
            _beamRepository = this.Storage.GetRepository<IBeamRepository>();
            _dailyOperationWarpingHistoryRepository =
                Storage.GetRepository<IDailyOperationWarpingHistoryRepository>();
            _dailyOperationWarpingBeamProductRepository =
                Storage.GetRepository<IDailyOperationWarpingBeamProductRepository>();
            _dailyOperationWarpingBrokenCauseRepository =
                Storage.GetRepository<IDailyOperationWarpingBrokenCauseRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            VerifyUser();
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

            //int totalRows = dailyOperationWarpingDocuments.Count();
            var result = dailyOperationWarpingDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("get-warping-beams")]
        public async Task<IActionResult> GetWarpingBeamIds(string keyword, string filter = "{}", int page = 1, int size = 25)
        {
            VerifyUser();
            page = page - 1;
            List<DailyOperationWarpingBeamDto> warpingListBeamProducts = new List<DailyOperationWarpingBeamDto>();
            List<BeamDto> warpingBeams = new List<BeamDto>();
            if (!filter.Contains("{}"))
            {
                Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                var OrderDocumentId = filterDictionary["OrderId"].ToString();
                if (!OrderDocumentId.Equals(null))
                {
                    var OrderIdentity = Guid.Parse(OrderDocumentId);

                    await Task.Yield();

                    var existingDailyOperationWarpingDocument =
                        _dailyOperationWarpingRepository.Find(x => x.OrderDocumentId == OrderIdentity);

                    foreach (var document in existingDailyOperationWarpingDocument)
                    {
                        var beamProducts = _dailyOperationWarpingBeamProductRepository.Find(x => x.DailyOperationWarpingDocumentId == document.Identity);
                        foreach (var product in beamProducts)
                        {
                            await Task.Yield();
                            var warpingBeamStatus = product.BeamStatus;
                            if (warpingBeamStatus.Equals(BeamStatus.ROLLEDUP))
                            {
                                await Task.Yield();
                                var warpingBeamYarnStrands = document.AmountOfCones;
                                var warpingBeam = new DailyOperationWarpingBeamDto(product.WarpingBeamId.Value, warpingBeamYarnStrands);
                                warpingListBeamProducts.Add(warpingBeam);
                            }
                        }
                        document.WarpingBeamProducts = beamProducts;

                        
                    }
                    await Task.Yield();
                    
                    
                    foreach (var warpingBeam in warpingListBeamProducts)
                    {
                        await Task.Yield();
                        var warpingBeamQuery = _beamRepository.Query.Where(beam => beam.Identity.Equals(warpingBeam.Id) && beam.Number.Contains(keyword, StringComparison.OrdinalIgnoreCase));
                        var warpingBeamDocument = _beamRepository.Find(warpingBeamQuery).FirstOrDefault();

                        if (warpingBeamDocument == null)
                        {
                            continue;
                        }

                        await Task.Yield();
                        var warpingBeamDto = new BeamDto(warpingBeam, warpingBeamDocument);
                        warpingBeams.Add(warpingBeamDto);
                    }
                }
                else
                {
                    throw Validator.ErrorValidation(("OrderDocument", "No. Order Produksi Harus Diisi"));
                }
            }
            else
            {
                throw Validator.ErrorValidation(("OrderDocument", "No. Order Produksi Harus Diisi"));
            }

            var total = warpingBeams.Count();
            var data = warpingBeams.Skip((page - 1) * size).Take(size);

            return Ok(data, info: new
            {
                page,
                size,
                total
            });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            VerifyUser();
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
            VerifyUser();
            // Sending command to command handler
            var dailyOperationWarping = await Mediator.Send(command);

            //Return result from command handler as Identity(Id)
            return Ok(dailyOperationWarping.Identity);
        }

        //Start Warping Daily Operation Request
        [HttpPut("{Id}/start-process")]
        public async Task<IActionResult> Start(string Id, [FromBody]UpdateStartDailyOperationWarpingCommand command)
        {
            VerifyUser();
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

        ////Pause Warping Daily Operation Request
        //[HttpPut("{Id}/pause-process")]
        //public async Task<IActionResult> Pause(string Id, [FromBody]UpdatePauseDailyOperationWarpingCommand command)
        //{
        //    if (!Guid.TryParse(Id, out Guid documentId))
        //    {
        //        return NotFound();
        //    }
        //    command.SetId(documentId);
        //    var updatePauseDailyOperationSizingDocument = await Mediator.Send(command);

        //    return Ok(updatePauseDailyOperationSizingDocument.Identity);
        //}

        ////Resume Warping Daily Operation Request
        //[HttpPut("{Id}/resume-process")]
        //public async Task<IActionResult> Resume(string Id, [FromBody]UpdateResumeDailyOperationWarpingCommand command)
        //{
        //    if (!Guid.TryParse(Id, out Guid documentId))
        //    {
        //        return NotFound();
        //    }
        //    command.SetId(documentId);
        //    var updateResumeDailyOperationSizingDocument = await Mediator.Send(command);

        //    return Ok(updateResumeDailyOperationSizingDocument.Identity);
        //}

        //Produce Beams Warping Daily Operation Request
        [HttpPut("{Id}/produce-beams-process")]
        public async Task<IActionResult> ProduceBeams(string Id, [FromBody]ProduceBeamsDailyOperationWarpingCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var produceBeamsDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(produceBeamsDailyOperationSizingDocument.Identity);

            //Lucky's Work
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
        [HttpPut("{Id}/completed-process")]
        public async Task<IActionResult> Finish(string Id, [FromBody]FinishDailyOperationWarpingCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var completedDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(completedDailyOperationSizingDocument.Identity);
        }

        //Controller for Daily Operation Warping Report
        [HttpGet("get-report")]
        public async Task<IActionResult> GetReport(string orderId,
                                                   string materialId,
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

            var dailyOperationWarpingReport = await _dailyOperationWarpingReportQuery.GetReports(orderId,
                                                                                                 materialId,
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

                DailyOperationWarpingReportXlsTemplate xlsTemplate = new DailyOperationWarpingReportXlsTemplate();
                MemoryStream xls = xlsTemplate.GenerateDailyOperationWarpingReportXls(dailyOperationWarpingReport.Item1.ToList());
                xlsInBytes = xls.ToArray();
                var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Operasional Mesin Harian Warping");
                return xlsFile;
            }
            else
            {
                return Ok(dailyOperationWarpingReport.Item1, info: new
                {
                    count = dailyOperationWarpingReport.Item2
                });
            }
        }

        //Controller for Daily Operation Warping Production Report
        [HttpGet("get-warping-production-report")]
        public async Task<IActionResult> GetWarpingProductionReport(int month = 0,
                                                                    int year = 0)
        {
            VerifyUser();
            var acceptRequest = Request.Headers.Values.ToList();
            var index = acceptRequest.IndexOf("application/pdf") > 0;

            var productionWarpingReport = _warpingProductionReportQuery.GetReports(month,
                                                                                         year);

            await Task.Yield();
            if (index.Equals(true))
            {
                var dateTime =
                    new DateTimeOffset(year, month, 1, 0, 0, 0, new TimeSpan(+7, 0, 0));

                var monthName = dateTime.ToString("MMMM", CultureInfo.CreateSpecificCulture("id-ID"));

                var fileName = "Laporan Produksi Warping Per Operator_" + monthName + "_" + year;

                WarpingProductionReportPdfTemplate pdfTemplate = new WarpingProductionReportPdfTemplate(productionWarpingReport);
                MemoryStream productionResultPdf = pdfTemplate.GeneratePdfTemplate();
                return new FileStreamResult(productionResultPdf, "application/pdf")
                {
                    FileDownloadName = string.Format(fileName)
                };
            }
            else
            {
                return Ok(productionWarpingReport);
            }
        }

        //Controller for Daily Operation Warping Broken Report
        [HttpGet("get-warping-broken-report")]
        public async Task<IActionResult> GetWarpingBrokenReport(int month = 0,
                                                                int year = 0,
                                                                int weavingUnitId = 0)
        {
            VerifyUser();
            var acceptRequest = Request.Headers.Values.ToList();
            var index = acceptRequest.IndexOf("application/pdf") > 0;

            var productionWarpingReport = _warpingBrokenReportQuery.GetReports(month, year, weavingUnitId);

            await Task.Yield();
            //if (index.Equals(true))
            //{
            //    var dateTime =
            //        new DateTimeOffset(year, month, 1, 0, 0, 0, new TimeSpan(+7, 0, 0));

            //    var monthName = dateTime.ToString("MMMM", CultureInfo.CreateSpecificCulture("id-ID"));

            //    var fileName = "Laporan Produksi Warping Per Operator_" + monthName + "_" + year;

            //    WarpingProductionReportPdfTemplate pdfTemplate = new WarpingProductionReportPdfTemplate(productionWarpingReport);
            //    MemoryStream productionResultPdf = pdfTemplate.GeneratePdfTemplate();
            //    return new FileStreamResult(productionResultPdf, "application/pdf")
            //    {
            //        FileDownloadName = string.Format(fileName)
            //    };
            //}
            //else
            //{
                return Ok(productionWarpingReport);
            //}
        }
    }
}
