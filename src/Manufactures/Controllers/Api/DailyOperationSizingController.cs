using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Sizing.Calculations;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects.DailyOperationSizingReport;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects.SizePickupReport;
using Manufactures.Application.Operators.DTOs;
using Manufactures.Application.Shifts.DTOs;
using Manufactures.Domain.Beams.Queries;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.BeamStockMonitoring.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Calculation;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Queries;
using Manufactures.Domain.DailyOperations.Sizing.Queries.DailyOperationSizingReport;
using Manufactures.Domain.DailyOperations.Sizing.Queries.SizePickupReport;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts.Queries;
using Manufactures.DataTransferObjects.Beams;
using Manufactures.Helpers.XlsTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay;
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
    [Route("weaving/daily-operations-sizing")]
    [ApiController]
    [Authorize]
    public class DailyOperationSizingController : ControllerApiBase
    {
        private readonly IOrderRepository
            _orderDocumentRepository;
        private readonly IFabricConstructionRepository
            _constructionDocumentRepository;
        private readonly IOperatorRepository
            _operatorDocumentRepository;

        private readonly IDailyOperationSizingDocumentQuery<DailyOperationSizingListDto> _sizingQuery;
        private readonly IOperatorQuery<OperatorListDto> _operatorQuery;
        private readonly IShiftQuery<ShiftDto> _shiftQuery;
        private readonly IBeamQuery<BeamListDto> _beamQuery;
        private readonly ISizePickupReportQuery<SizePickupReportListDto> _sizePickupReportQuery;
        private readonly IDailyOperationSizingReportQuery<DailyOperationSizingReportListDto> _dailyOperationSizingReportQuery;

        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingRepository;
        private readonly IDailyOperationSizingBeamProductRepository
            _dailyOperationSizingBeamProductRepository;
        private readonly IBeamRepository
            _beamRepository;

        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationSizingController(IServiceProvider serviceProvider,
                                              IWorkContext workContext,
                                              IDailyOperationSizingDocumentQuery<DailyOperationSizingListDto> sizingQuery,
                                              IOperatorQuery<OperatorListDto> operatorQuery,
                                              IShiftQuery<ShiftDto> shiftQuery,
                                              IBeamQuery<BeamListDto> beamQuery,
                                              ISizePickupReportQuery<SizePickupReportListDto> sizePickupReportQuery,
                                              IDailyOperationSizingReportQuery<DailyOperationSizingReportListDto> dailyOperationSizingReportQuery)
            : base(serviceProvider)
        {
            _sizingQuery = sizingQuery ?? throw new ArgumentNullException(nameof(sizingQuery));
            _operatorQuery = operatorQuery ?? throw new ArgumentNullException(nameof(operatorQuery));
            _shiftQuery = shiftQuery ?? throw new ArgumentNullException(nameof(shiftQuery));
            _beamQuery = beamQuery ?? throw new ArgumentNullException(nameof(beamQuery));
            _sizePickupReportQuery = sizePickupReportQuery ?? throw new ArgumentNullException(nameof(sizePickupReportQuery));
            _dailyOperationSizingReportQuery = dailyOperationSizingReportQuery ?? throw new ArgumentNullException(nameof(dailyOperationSizingReportQuery));

            _dailyOperationSizingRepository =
                this.Storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationSizingBeamProductRepository =
                this.Storage.GetRepository<IDailyOperationSizingBeamProductRepository>();
            _orderDocumentRepository =
                this.Storage.GetRepository<IOrderRepository>();
            _constructionDocumentRepository =
                this.Storage.GetRepository<IFabricConstructionRepository>();
            _beamRepository =
                this.Storage.GetRepository<IBeamRepository>();
            _operatorDocumentRepository =
                this.Storage.GetRepository<IOperatorRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            VerifyUser();
            var dailyOperationSizingDocuments = await _sizingQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                dailyOperationSizingDocuments =
                    dailyOperationSizingDocuments
                        .Where(x => x.DateTimeOperation.ToString("DD MMMM YYYY").Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.OrderProductionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.FabricConstructionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.WeavingUnit.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.OperationStatus.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(DailyOperationSizingListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    await Task.Yield();
                    dailyOperationSizingDocuments =
                        dailyOperationSizingDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    await Task.Yield();
                    dailyOperationSizingDocuments =
                        dailyOperationSizingDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }
            
            var result = dailyOperationSizingDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        //[HttpGet("get-sizing-beams")]
        //public async Task<IActionResult> GetSizingBeamIds(string keyword, string filter = "{}", int page = 1, int size = 25)
        //{
        //    VerifyUser();
        //    page = page - 1;
        //    List<DailyOperationSizingBeamDto> sizingListBeamProducts = new List<DailyOperationSizingBeamDto>();
        //    List<BeamDto> sizingBeams = new List<BeamDto>();
        //    if (!filter.Contains("{}"))
        //    {
        //        Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
        //        var OrderDocumentId = filterDictionary["OrderId"].ToString();
        //        if (!OrderDocumentId.Equals(null))
        //        {
        //            var OrderIdentity = Guid.Parse(OrderDocumentId);

        //            await Task.Yield();
        //            var sizingQuery =
        //                 _dailyOperationSizingRepository
        //                     .Query
        //                     .Include(x => x.SizingHistories)
        //                     .Include(x => x.SizingBeamProducts)
        //                     .OrderByDescending(o => o.DateTimeOperation)
        //            .Where(doc => doc.OrderDocumentId.Equals(OrderIdentity));

        //            await Task.Yield();
        //            var existingDailyOperationSizingDocument =
        //                _dailyOperationSizingRepository
        //                    .Find(sizingQuery);

        //            await Task.Yield();
        //            foreach (var sizingDocument in existingDailyOperationSizingDocument)
        //            {
        //                foreach (var sizingBeamProduct in sizingDocument.SizingBeamProducts)
        //                {
        //                    await Task.Yield();
        //                    var sizingBeamStatus = sizingBeamProduct.BeamStatus;
        //                    if (sizingBeamStatus.Equals(BeamStatus.ROLLEDUP))
        //                    {
        //                        await Task.Yield();
        //                        double counterStart = sizingBeamProduct.CounterStart ?? 0;
        //                        double counterFinish = sizingBeamProduct.CounterFinish ?? 0;
        //                        double sizingLengthCounter = counterFinish - counterStart;
        //                        var warpingBeam = new DailyOperationSizingBeamDto(sizingBeamProduct.SizingBeamId, sizingLengthCounter);
        //                        sizingListBeamProducts.Add(warpingBeam);
        //                    }
        //                }
        //            }

        //            await Task.Yield();
        //            foreach (var sizingBeam in sizingListBeamProducts)
        //            {
        //                await Task.Yield();
        //                var sizingBeamQuery =
        //                    _beamRepository
        //                        .Query
        //                        .Where(beam => beam.Identity.Equals(sizingBeam.Id) &&
        //                                       beam.Number.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        //                var sizingBeamDocument =
        //                    _beamRepository
        //                        .Find(sizingBeamQuery)
        //                        .FirstOrDefault();

        //                if (sizingBeamDocument == null)
        //                {
        //                    continue;
        //                }

        //                await Task.Yield();
        //                var sizingBeamDto = new BeamDto(sizingBeam, sizingBeamDocument);
        //                sizingBeams.Add(sizingBeamDto);
        //            }
        //        }
        //        else
        //        {
        //            throw Validator.ErrorValidation(("OrderDocument", "No. Order Produksi Harus Diisi"));
        //        }
        //    }
        //    else
        //    {
        //        throw Validator.ErrorValidation(("OrderDocument", "No. Order Produksi Harus Diisi"));
        //    }

        //    var total = sizingBeams.Count();
        //    var data = sizingBeams.Skip((page - 1) * size).Take(size);

        //    return Ok(data, info: new
        //    {
        //        page,
        //        size,
        //        total
        //    });
        //}

        //[HttpGet("get-sizing-beam-products")]
        //public async Task<IActionResult> GetSizingBeamProducts(string keyword, string filter = "{}", int page = 1, int size = 25)
        //{
        //    page = page - 1;
        //    List<BeamDto> sizingListBeamProducts = new List<BeamDto>();
        //    List<BeamId> listOfSizingBeamProductId = new List<BeamId>();

        //    await Task.Yield();
        //    var sizingQuery =
        //         _dailyOperationSizingRepository
        //             .Query
        //             .Include(x => x.SizingHistories)
        //             .Include(x => x.SizingBeamProducts)
        //             .OrderByDescending(o => o.DateTimeOperation);

        //    await Task.Yield();
        //    var existingDailyOperationSizingDocument =
        //        _dailyOperationSizingRepository
        //            .Find(sizingQuery);

        //    await Task.Yield();
        //    foreach (var sizingDocument in existingDailyOperationSizingDocument)
        //    {
        //        foreach (var sizingBeamProduct in sizingDocument.SizingBeamProducts)
        //        {
        //            await Task.Yield();
        //            var sizingBeamStatus = sizingBeamProduct.BeamStatus;
        //            if (sizingBeamStatus.Equals(BeamStatus.ROLLEDUP))
        //            {
        //                listOfSizingBeamProductId.Add(new BeamId(sizingBeamProduct.SizingBeamId));

        //                //await Task.Yield();
        //                //var beamQuery =
        //                //    _beamRepository
        //                //        .Query
        //                //        .OrderByDescending(o => o.CreatedDate)
        //                //        .AsQueryable();

        //                //if (!string.IsNullOrEmpty(keyword))
        //                //{
        //                //    beamQuery = beamQuery.Where(o => o.Number.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        //                //}

        //                //await Task.Yield();
        //                //var beamDocument =
        //                //    _beamRepository
        //                //        .Find(beamQuery)
        //                //        .Where(o => o.Identity.Equals(sizingBeamProduct.SizingBeamId))
        //                //        .FirstOrDefault();

        //                //await Task.Yield();
        //                //var sizingBeam = new DailyOperationSizingBeamProductDto(sizingBeamProduct, beamDocument);
        //                //sizingListBeamProducts.Add(sizingBeam);
        //            }
        //        }
        //    }

        //    foreach (var beam in listOfSizingBeamProductId)
        //    {
        //        await Task.Yield();
        //        var beamQuery =
        //            _beamRepository
        //                .Query
        //                .OrderByDescending(o => o.CreatedDate)
        //                .AsQueryable();

        //        if (!string.IsNullOrEmpty(keyword))
        //        {
        //            beamQuery = beamQuery.Where(o => o.Number.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        //        }

        //        await Task.Yield();
        //        var beamDocument =
        //            _beamRepository
        //                .Find(beamQuery)
        //                .Where(o => o.Identity.Equals(beam.Value))
        //                .FirstOrDefault();

        //        await Task.Yield();

        //        if(beamDocument == null)
        //        {
        //            continue;
        //        }
        //        var sizingBeam = new BeamDto(beamDocument);
        //        sizingListBeamProducts.Add(sizingBeam);
        //    }
        //    sizingListBeamProducts = sizingListBeamProducts.GroupBy(x => new { x.Id, x.Number })
        //        .Select(x => x.First()).ToList();
        //    var total = sizingListBeamProducts.Count();
        //    var data = sizingListBeamProducts.Skip((page - 1) * size).Take(size);

        //    return Ok(data, info: new
        //    {
        //        page,
        //        size,
        //        total
        //    });
        //}

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            VerifyUser();
            var identity = Guid.Parse(Id);
            var dailyOperationSizingDocument = await _sizingQuery.GetById(identity);

            if (dailyOperationSizingDocument == null)
            {
                return NotFound(identity);
            }

            return Ok(dailyOperationSizingDocument);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PreparationDailyOperationSizingCommand command)
        {
            VerifyUser();
            var preparationDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(preparationDailyOperationSizingDocument.Identity);
        }

        [HttpPut("{Id}/start")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]UpdateStartDailyOperationSizingCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateStartDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updateStartDailyOperationSizingDocument.Identity);
        }

        [HttpPut("{Id}/pause")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdatePauseDailyOperationSizingCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updatePauseDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updatePauseDailyOperationSizingDocument.Identity);
        }

        [HttpPut("{Id}/resume")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateResumeDailyOperationSizingCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateResumeDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updateResumeDailyOperationSizingDocument.Identity);
        }

        [HttpPut("{Id}/produce-beams")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]ProduceBeamDailyOperationSizingCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var reuseBeamsDailyOperationSizingDocument = await Mediator.Send(command);

            //Get Daily Operation Document Sizing
            var existingSizingDocument =
                _dailyOperationSizingRepository
                    .Find(o=>o.Identity == command.Id)
                    .FirstOrDefault();

            //Get Daily Operation Beam Product
            var lastSizingBeamProduct =
                _dailyOperationSizingBeamProductRepository
                    .Find(o => o.DailyOperationSizingDocumentId == existingSizingDocument.Identity)
                    .OrderByDescending(x => x.LatestDateTimeBeamProduct)
                    .FirstOrDefault();

            var counterStart = lastSizingBeamProduct.CounterStart;
            var counterFinish = lastSizingBeamProduct.CounterFinish;
            var sizingLengthStock = counterFinish - counterStart;

            //Instantiate Beam Stock Command for Sizing
            var sizingStock = new BeamStockMonitoringCommand
            {
                BeamDocumentId = new BeamId(lastSizingBeamProduct.SizingBeamId.Value),
                EntryDate = command.ProduceBeamDate,
                EntryTime = command.ProduceBeamTime,
                OrderDocumentId = existingSizingDocument.OrderDocumentId,
                LengthStock = sizingLengthStock
            };
            var updateSizingOnMonitoringStockBeam = await Mediator.Send(sizingStock);

            return Ok(reuseBeamsDailyOperationSizingDocument.Identity);
        }

        [HttpPut("{Id}/finish-doff")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]FinishDoffDailyOperationSizingCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateDoffDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updateDoffDailyOperationSizingDocument.Identity);
        }

        [HttpDelete("{operationId}/{historyId}")]
        public async Task<IActionResult> Delete(string operationId,
                                                string historyId)
        {
            VerifyUser();
            if (!Guid.TryParse(operationId, out Guid Identity))
            {
                return NotFound();
            }

            if (!Guid.TryParse(historyId, out Guid HistoryId))
            {
                return NotFound();
            }

            var command = new HistoryRemovePreparationDailyOperationSizingCommand();
            command.SetId(Identity);
            command.SetHistoryId(HistoryId);

            var dailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(dailyOperationSizingDocument.Identity);
        }

        [HttpPut("{operationId}/{historyId}/{status}")]
        public async Task<IActionResult> Put(string operationId,
                                             string historyId,
                                             string status,
                                             [FromBody]HistoryRemovePauseOrResumeOrFinishDailyOperationSizingCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(operationId, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateRemovePauseOrResumeOrFinishDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updateRemovePauseOrResumeOrFinishDailyOperationSizingDocument.Identity);
        }

        [HttpPut("{operationId}/{historyId}/{beamProductId}/{status}")]
        public async Task<IActionResult> Put(string operationId,
                                             string historyId,
                                             string beamProductId,
                                             string status,
                                             [FromBody]HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(operationId, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateRemoveStartOrProduceBeamDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updateRemoveStartOrProduceBeamDailyOperationSizingDocument.Identity);
        }

        [HttpGet("calculate/netto/empty-weight/{emptyWeight}/bruto/{bruto}")]
        public async Task<IActionResult> CalculateNetto(double emptyWeight, double bruto)
        {
            VerifyUser();
            double nettoCalculationResult;

            if (emptyWeight != 0 && bruto != 0)
            {
                Netto calculate = new Netto();
                nettoCalculationResult = calculate.CalculateNetto(emptyWeight, bruto);

                await Task.Yield();
                return Ok(nettoCalculationResult);
            }
            else
            {
                await Task.Yield();
                return NotFound();
                throw Validator.ErrorValidation(("ProduceBeamsBruto", "Bruto cannot less than Empty Weight"));
            }
        }

        [HttpGet("calculate/pis-in-meter/start/{counterStart}/finish/{counterFinish}")]
        public async Task<IActionResult> CalculatePISInMeter(double counterStart, double counterFinish)
        {
            VerifyUser();
            double pisInPieces;

            if (counterStart >= 0 && counterFinish > 0)
            {
                PIS calculate = new PIS();
                pisInPieces = calculate.CalculateInMeter(counterStart, counterFinish);

                await Task.Yield();
                return Ok(pisInPieces);
            }
            else
            {
                await Task.Yield();
                return NotFound();
                throw Validator.ErrorValidation(("ProduceBeamsFinishCounter", "PIS (m) cannot less than Start Counter"));
            }
        }

        //[HttpGet("calculate/pis-in-pieces/start/{counterStart}/finish/{counterFinish}")]
        //public async Task<IActionResult> CalculatePISInPieces(double counterStart, double counterFinish)
        //{
        //    double pisInMeter;

        //    if (counterStart >= 0 && counterFinish > 0)
        //    {
        //        PIS calculate = new PIS();
        //        pisInMeter = calculate.CalculateInPieces(counterStart, counterFinish);

        //        await Task.Yield();
        //        return Ok(pisInMeter);
        //    }
        //    else
        //    {
        //        await Task.Yield();
        //        return NotFound();
        //        throw Validator.ErrorValidation(("ProduceBeamsFinishCounter", "PIS (m) cannot less than Start Counter"));
        //    }
        //}

        [HttpGet("calculate/theoritical-kawamoto/pis/{pisMeter}/yarn-strands/{yarnStrands}/ne-real/{neReal}")]
        public async Task<IActionResult> CalculateTheoriticalKawamoto(double pisMeter, double yarnStrands, double neReal)
        {
            VerifyUser();
            double kawamotoCalculationResult;

            if (pisMeter > 0 && yarnStrands > 0 && neReal > 0)
            {
                Theoritical calculate = new Theoritical();
                kawamotoCalculationResult = calculate.CalculateKawamoto(pisMeter, yarnStrands, neReal);

                await Task.Yield();
                return Ok(kawamotoCalculationResult);
            }
            else
            {
                await Task.Yield();
                return NotFound();
                throw Validator.ErrorValidation(("ProduceBeamsFinishCounter", "PIS (m) cannot less than Start Counter"));
            }
        }

        [HttpGet("calculate/theoritical-sucker-muller/pis/{pisMeter}/yarn-strands/{yarnStrands}/ne-real/{neReal}")]
        public async Task<IActionResult> CalculateTheoriticalSuckerMuller(double pisMeter, double yarnStrands, double neReal)
        {
            VerifyUser();
            double suckerMullerCalculationResult;

            if (pisMeter > 0 && yarnStrands > 0 && neReal > 0)
            {
                Theoritical calculate = new Theoritical();
                suckerMullerCalculationResult = calculate.CalculateSuckerMuller(pisMeter, yarnStrands, neReal);

                await Task.Yield();
                return Ok(suckerMullerCalculationResult);
            }
            else
            {
                await Task.Yield();
                return NotFound();
                throw Validator.ErrorValidation(("ProduceBeamsFinishCounter", "PIS (m) cannot less than Start Counter"));
            }
        }

        [HttpGet("calculate/spu/netto/{netto}/theoritical/{theoritical}")]
        public async Task<IActionResult> CalculateSPU(double netto, double theoritical)
        {
            VerifyUser();
            double spuCalculationResult;

            if (netto != 0 && theoritical != 0)
            {
                SPU calculate = new SPU();
                spuCalculationResult = calculate.Calculate(netto, theoritical);

                await Task.Yield();
                return Ok(spuCalculationResult);
            }
            else
            {
                await Task.Yield();
                return NotFound();
                throw Validator.ErrorValidation(("ProduceBeamsFinishCounter", "PIS (m) cannot less than Start Counter"));
            }
        }

        //Controller for Size Pickup Report
        [HttpGet("get-size-pickup-report")]
        public async Task<IActionResult> GetSizePickupReport(string shiftId,
                                                             string spuStatus,
                                                             int unitId = 0,
                                                             DateTimeOffset? date = null,
                                                             DateTimeOffset? dateFrom = null,
                                                             DateTimeOffset? dateTo = null,
                                                             int? month = 0,
                                                             int page = 1,
                                                             int size = 25,
                                                             string order = "{}")
        {
            VerifyUser();
            var acceptRequest = Request.Headers.Values.ToList();
            var index = acceptRequest.IndexOf("application/xls") > 0;

            var sizePickupReport = await _sizePickupReportQuery.GetReports(shiftId,
                                                                           spuStatus,
                                                                           unitId,
                                                                           date,
                                                                           dateFrom,
                                                                           dateTo,
                                                                           month,
                                                                           page,
                                                                           size,
                                                                           order);

            await Task.Yield();
            if (index.Equals(true))
            {
                byte[] xlsInBytes;

                SizePickupReportXlsTemplate xlsTemplate = new SizePickupReportXlsTemplate();
                MemoryStream xls = xlsTemplate.GenerateSizePickupReportXls(sizePickupReport.Item1.ToList());
                xlsInBytes = xls.ToArray();
                var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Size Pickup");
                return xlsFile;
            }
            else
            {
                return Ok(sizePickupReport.Item1, info: new
                {
                    count = sizePickupReport.Item2
                });
            }
        }

        //Controller for Daily Operation Sizing Report
        [HttpGet("get-report")]
        public async Task<IActionResult> GetReport(string machineId,
                                                   string orderId,
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

            var dailyOperationSizingReport = await _dailyOperationSizingReportQuery.GetReports(machineId,
                                                                                               orderId,
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

                DailyOperationSizingReportXlsTemplate xlsTemplate = new DailyOperationSizingReportXlsTemplate();
                MemoryStream xls = xlsTemplate.GenerateDailyOperationSizingReportXls(dailyOperationSizingReport.Item1.ToList());
                xlsInBytes = xls.ToArray();
                var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Operasional Mesin Harian Sizing");
                return xlsFile;
            }
            else
            {
                return Ok(dailyOperationSizingReport.Item1, info: new
                {
                    count = dailyOperationSizingReport.Item2
                });
            }
        }
    }
}
