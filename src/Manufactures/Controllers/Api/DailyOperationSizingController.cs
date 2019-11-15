using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Sizing.Calculations;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects.DailyOperationSizingReport;
using Manufactures.Application.DailyOperations.Sizing.SizePickup;
using Manufactures.Application.Helpers;
using Manufactures.Application.Operators.DTOs;
using Manufactures.Application.Shifts.DTOs;
using Manufactures.Domain.Beams.Queries;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.BeamStockMonitoring.Commands;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Calculation;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Queries;
using Manufactures.Domain.DailyOperations.Sizing.Queries.DailyOperationSizingReport;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts.Queries;
using Manufactures.Dtos;
using Manufactures.Dtos.Beams;
using Manufactures.Helpers.XlsTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/daily-operations-sizing")]
    [ApiController]
    [Authorize]
    public class DailyOperationSizingController : ControllerApiBase
    {
        //private readonly IDailyOperationSizingRepository
        //    _dailyOperationSizingDocumentRepository;
        //private readonly IMachineRepository
        //    _machineRepository;
        //private readonly IMachineTypeRepository
        //    _machineTypeRepository;
        private readonly IWeavingOrderDocumentRepository
            _orderDocumentRepository;
        private readonly IFabricConstructionRepository
            _constructionDocumentRepository;
        //private readonly IShiftRepository
        //    _shiftDocumentRepository;
        //private readonly IBeamRepository
        //    _beamDocumentRepository;
        private readonly IOperatorRepository
            _operatorDocumentRepository;
        //private readonly IDailyOperationWarpingRepository
        //    _dailyOperationWarpingDocumentRepository;

        private readonly IDailyOperationSizingQuery<DailyOperationSizingListDto> _sizingQuery;
        private readonly IOperatorQuery<OperatorListDto> _operatorQuery;
        private readonly IShiftQuery<ShiftDto> _shiftQuery;
        private readonly IBeamQuery<BeamListDto> _beamQuery;
        private readonly IDailyOperationSizingReportQuery<DailyOperationSizingReportListDto> _dailyOperationSizingReportQuery;

        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingRepository;
        private readonly IBeamRepository
            _beamRepository;

        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationSizingController(IServiceProvider serviceProvider,
                                              IWorkContext workContext,
                                              IDailyOperationSizingQuery<DailyOperationSizingListDto> sizingQuery,
                                              IOperatorQuery<OperatorListDto> operatorQuery,
                                              IShiftQuery<ShiftDto> shiftQuery,
                                              IBeamQuery<BeamListDto> beamQuery,
                                              IDailyOperationSizingReportQuery<DailyOperationSizingReportListDto> dailyOperationSizingReportQuery)
            : base(serviceProvider)
        {
            _sizingQuery = sizingQuery ?? throw new ArgumentNullException(nameof(sizingQuery));
            _operatorQuery = operatorQuery ?? throw new ArgumentNullException(nameof(operatorQuery));
            _shiftQuery = shiftQuery ?? throw new ArgumentNullException(nameof(shiftQuery));
            _beamQuery = beamQuery ?? throw new ArgumentNullException(nameof(beamQuery));
            _dailyOperationSizingReportQuery = dailyOperationSizingReportQuery ?? throw new ArgumentNullException(nameof(dailyOperationSizingReportQuery));

            _dailyOperationSizingRepository =
                this.Storage.GetRepository<IDailyOperationSizingRepository>();
            _orderDocumentRepository =
                this.Storage.GetRepository<IWeavingOrderDocumentRepository>();
            _constructionDocumentRepository =
                this.Storage.GetRepository<IFabricConstructionRepository>();
            _beamRepository =
                this.Storage.GetRepository<IBeamRepository>();
            _operatorDocumentRepository =
                this.Storage.GetRepository<IOperatorRepository>();
        }

        //public DailyOperationSizingController(IServiceProvider serviceProvider,
        //                                         IWorkContext workContext)
        //    : base(serviceProvider)
        //{
        //_dailyOperationSizingDocumentRepository =
        //    this.Storage.GetRepository<IDailyOperationSizingRepository>();
        //_machineRepository =
        //    this.Storage.GetRepository<IMachineRepository>();
        //_machineTypeRepository =
        //    this.Storage.GetRepository<IMachineTypeRepository>();
        //_shiftDocumentRepository =
        //    this.Storage.GetRepository<IShiftRepository>();
        //_beamDocumentRepository =
        //    this.Storage.GetRepository<IBeamRepository>();
        //_dailyOperationWarpingDocumentRepository =
        //    this.Storage.GetRepository<IDailyOperationWarpingRepository>();
        //}

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            var dailyOperationSizingDocuments = await _sizingQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                dailyOperationSizingDocuments =
                    dailyOperationSizingDocuments
                        .Where(x => x.FabricConstructionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.OrderProductionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
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

            int totalRows = dailyOperationSizingDocuments.Count();
            var result = dailyOperationSizingDocuments.Skip((page - 1) * size).Take(size);
            var resultCount = result.Count();

            return Ok(result, info: new { page, size, totalRows, resultCount });
        }

        [HttpGet("get-sizing-beams")]
        public async Task<IActionResult> GetSizingBeamIds(string keyword, string filter = "{}", int page = 1, int size = 25)
        {
            page = page - 1;
            List<DailyOperationSizingBeamDto> sizingListBeamProducts = new List<DailyOperationSizingBeamDto>();
            List<BeamDto> sizingBeams = new List<BeamDto>();
            if (!filter.Contains("{}"))
            {
                Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                var OrderDocumentId = filterDictionary["OrderId"].ToString();
                if (!OrderDocumentId.Equals(null))
                {
                    var OrderIdentity = Guid.Parse(OrderDocumentId);

                    await Task.Yield();
                    var sizingQuery =
                         _dailyOperationSizingRepository
                             .Query
                             .Include(x => x.SizingHistories)
                             .Include(x => x.SizingBeamProducts)
                             .OrderByDescending(o => o.DateTimeOperation)
                    .Where(doc => doc.OrderDocumentId.Equals(OrderIdentity));

                    await Task.Yield();
                    var existingDailyOperationSizingDocument =
                        _dailyOperationSizingRepository
                            .Find(sizingQuery);

                    await Task.Yield();
                    foreach (var sizingDocument in existingDailyOperationSizingDocument)
                    {
                        foreach (var sizingBeamProduct in sizingDocument.SizingBeamProducts)
                        {
                            await Task.Yield();
                            var sizingBeamStatus = sizingBeamProduct.BeamStatus;
                            if (sizingBeamStatus.Equals(BeamStatus.ROLLEDUP))
                            {
                                await Task.Yield();
                                double counterStart = sizingBeamProduct.CounterStart ?? 0;
                                double counterFinish = sizingBeamProduct.CounterFinish ?? 0;
                                double sizingLengthCounter = counterFinish - counterStart;
                                var warpingBeam = new DailyOperationSizingBeamDto(sizingBeamProduct.SizingBeamId, sizingLengthCounter);
                                sizingListBeamProducts.Add(warpingBeam);
                            }
                        }
                    }

                    await Task.Yield();
                    foreach (var sizingBeam in sizingListBeamProducts)
                    {
                        await Task.Yield();
                        var sizingBeamQuery =
                            _beamRepository
                                .Query
                                .Where(beam => beam.Identity.Equals(sizingBeam.Id) &&
                                               beam.Number.Contains(keyword, StringComparison.OrdinalIgnoreCase));
                        var sizingBeamDocument =
                            _beamRepository
                                .Find(sizingBeamQuery)
                                .FirstOrDefault();

                        await Task.Yield();
                        var sizingBeamDto = new BeamDto(sizingBeam, sizingBeamDocument);
                        sizingBeams.Add(sizingBeamDto);
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

            var total = sizingBeams.Count();
            var data = sizingBeams.Skip((page - 1) * size).Take(size);

            return Ok(data, info: new
            {
                page,
                size,
                total
            });
        }

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
            var preparationDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(preparationDailyOperationSizingDocument.Identity);
        }

        [HttpPut("{Id}/start")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]UpdateStartDailyOperationSizingCommand command)
        {
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
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var reuseBeamsDailyOperationSizingDocument = await Mediator.Send(command);

            //Get Daily Operation Document Sizing
            var sizingQuery =
                _dailyOperationSizingRepository
                    .Query
                    .Include(x => x.SizingHistories)
                    .Include(x => x.SizingBeamProducts)
                    .Where(doc => doc.Identity.Equals(command.Id));
            var existingDailyOperationSizingDocument =
                _dailyOperationSizingRepository
                    .Find(sizingQuery)
                    .FirstOrDefault();

            //Get Daily Operation Beam Product
            var existingDailyOperationSizingBeamProduct =
                existingDailyOperationSizingDocument
                    .SizingBeamProducts
                    .OrderByDescending(beamProduct => beamProduct.LatestDateTimeBeamProduct);
            var lastSizingBeamProduct =
                existingDailyOperationSizingBeamProduct
                    .FirstOrDefault();

            var counterStart = lastSizingBeamProduct.CounterStart;
            var counterFinish = lastSizingBeamProduct.CounterFinish;
            var sizingLengthStock = counterFinish - counterStart;

            //Instantiate Beam Stock Command for Sizing
            var sizingStock = new BeamStockMonitoringCommand
            {
                BeamDocumentId = new BeamId(lastSizingBeamProduct.SizingBeamId),
                EntryDate = command.ProduceBeamDate,
                EntryTime = command.ProduceBeamTime,
                OrderDocumentId = existingDailyOperationSizingDocument.OrderDocumentId,
                LengthStock = sizingLengthStock ?? 0
            };
            var updateSizingOnMonitoringStockBeam = await Mediator.Send(sizingStock);

            return Ok(reuseBeamsDailyOperationSizingDocument.Identity);
        }

        [HttpPut("{Id}/finish-doff")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]FinishDoffDailyOperationSizingCommand command)
        {
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

        [HttpGet("size-pickup/month/{month}/unit-id/{weavingUnitId}/shift/{shiftId}/spu/{spuStatus}")]
        public async Task<IActionResult> GetReportByMonth(int month, int weavingUnitId, string shiftId, string spuStatus)
        {
            try
            {
                var acceptRequest = Request.Headers.Values.ToList();
                var index = acceptRequest.IndexOf("application/xls") > 0;

                //Preparing Container for List of Size Pickup
                var resultData = new List<SizePickupListDto>();

                //Get All Order Document Which Using Weaving Unit Id from Request
                var orderDocumentByWeavingUnit =
                    _orderDocumentRepository
                        .Find(o => o.UnitId.Value.Equals(weavingUnitId))
                        .ToList();

                //Query for Daily Operation Sizing
                var sizingQuery =
                    _dailyOperationSizingRepository
                        .Query
                        .Include(d => d.SizingHistories)
                            .OrderByDescending(item => item.CreatedDate)
                        .Include(b => b.SizingBeamProducts)
                            .OrderByDescending(item => item.CreatedDate);

                //Preparing Container for List of Daily Operation Sizing Document
                var listOfFilteredSizingDocument = new List<DailyOperationSizingDocument>();

                //Filter Sizing Document Which Using Order Document Id Above
                foreach (var order in orderDocumentByWeavingUnit)
                {
                    var filteredSizingDocument =
                        _dailyOperationSizingRepository
                            .Find(sizingQuery)
                            .Where(sizePickup =>
                                   sizePickup.OrderDocumentId.Value.Equals(order.Identity) &&
                                   sizePickup.OperationStatus.Equals(OperationStatus.ONFINISH))
                            .ToList();

                    //Add Filtered Sizing Doc to List of Filtered Sizing Document Container that Already Prepared Above
                    if (filteredSizingDocument.Count > 0)
                    {
                        foreach (var sizingDocument in filteredSizingDocument)
                        {
                            listOfFilteredSizingDocument.Add(sizingDocument);
                        }
                    }
                }

                //Filter Document in List of Filtered Sizing Document
                foreach (var filteredSizingDocument in listOfFilteredSizingDocument)
                {
                    //Preparing Container for Size Pick Up
                    var results = new SizePickupListDto();

                    var recipeCode = filteredSizingDocument.RecipeCode;
                    var machineSpeed = filteredSizingDocument.MachineSpeed;
                    var texSQ = filteredSizingDocument.TexSQ;
                    var visco = filteredSizingDocument.Visco;

                    //Get Order Document Used in Current Sizing Document
                    var sizingOrderDocument =
                        _orderDocumentRepository
                            .Find(o => o.Identity.Equals(filteredSizingDocument.OrderDocumentId.Value))
                            .FirstOrDefault();

                    //Get Fabric Construction Used in Current Sizing Document to Get Fabric Construction Number
                    var sizingFabricConstructionDocument =
                        _constructionDocumentRepository
                            .Find(o => o.Identity.Equals(sizingOrderDocument.ConstructionId.Value))
                            .FirstOrDefault();

                    //Get Fabric Construction Number
                    var constructionNumber = sizingFabricConstructionDocument.ConstructionNumber;
                    string[] splittedConstructionNumber = constructionNumber.Split(" ");
                    var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var filteredConstructionNumber = splittedConstructionNumber[0].TrimEnd(digits);

                    var filteredSizingBeamProducts = filteredSizingDocument.SizingBeamProducts
                                                                .Where(b => b.LatestDateTimeBeamProduct.Month.Equals(month) &&
                                                                            b.BeamStatus.Equals(BeamStatus.ROLLEDUP));

                    if (filteredSizingBeamProducts != null)
                    {
                        foreach (var sizingBeamProduct in filteredSizingBeamProducts)
                        {
                            //Get Beam Number from BeamRepository (Master Beam), by Beam Id Used in Beam Product (SizingBeamProduct) of Sizing Document
                            var beamQuery =
                                _beamRepository
                                    .Query
                                    .OrderByDescending(item => item.CreatedDate);
                            var beamDocument =
                                _beamRepository
                                    .Find(beamQuery)
                                    .Where(b => b.Identity.Equals(sizingBeamProduct.SizingBeamId))
                                    .FirstOrDefault();
                            var beamNumber = beamDocument.Number;

                            //Filter Details by Machine Status (COMPLETED), ShiftId (All Shift) and Month from UI (Request), and BeamNumber from BeamRepository Above
                            var historyByAllShiftIndex = 0;
                            var filteredSizingHistories =
                                filteredSizingDocument.SizingHistories
                                    .Where(d => d.DateTimeMachine.Month.Equals(month) &&
                                                d.SizingBeamNumber.Equals(beamNumber) &&
                                                d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                            var filteredSizingHistory = filteredSizingHistories.ToList()[historyByAllShiftIndex++];

                            if (shiftId != "All")
                            {
                                var historyBySpecifiedShiftIndex = 0;
                                //Filter Details by Machine Status (COMPLETED), ShiftId (Specified Shift) and Month from UI (Request), and BeamNumber from BeamRepository Above
                                filteredSizingHistories =
                                    filteredSizingDocument.SizingHistories
                                        .Where(d => d.ShiftDocumentId.ToString().Equals(shiftId) &&
                                                    d.DateTimeMachine.Month.Equals(month) &&
                                                    d.SizingBeamNumber.Equals(beamNumber) &&
                                                    d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                                filteredSizingHistory = filteredSizingHistories.ToList()[historyBySpecifiedShiftIndex++];
                            }

                            //Get Operator Document from OperatorRepository, relate by filteredDetail.OperatorDocumentId
                            var operatorQuery =
                                _operatorDocumentRepository
                                    .Query.OrderByDescending(item => item.CreatedDate);
                            var operatorDocument =
                                _operatorDocumentRepository
                                    .Find(operatorQuery)
                                    .Where(o => o.Identity.Equals(filteredSizingHistory.OperatorDocumentId))
                                    .FirstOrDefault();

                            var dateTimeSizingBeamProduct = sizingBeamProduct.LatestDateTimeBeamProduct;
                            double pisMeter = sizingBeamProduct.PISMeter ?? 0;
                            double spu = sizingBeamProduct.SPU ?? 0;

                            switch (filteredConstructionNumber)
                            {
                                case SizePickupSPUConstants.PCCONSTRUCTION:
                                    Filtering filteringPC = new Filtering();
                                    var resultPC = filteringPC.ComparingPCCVC(spu);
                                    if (resultPC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.CVCCONSTRUCTION:
                                    Filtering filteringCVC = new Filtering();
                                    var resultCVC = filteringCVC.ComparingPCCVC(spu);
                                    if (resultCVC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.COTTONCONSTRUCTION:
                                    Filtering filteringCotton = new Filtering();
                                    var resultCotton = filteringCotton.ComparingPCCVC(spu);
                                    if (resultCotton == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.PECONSTRUCTION:
                                    Filtering filteringPE = new Filtering();
                                    var resultPE = filteringPE.ComparingPCCVC(spu);
                                    if (resultPE == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.RAYONCONSTRUCTION:
                                    Filtering filteringRayon = new Filtering();
                                    var resultRayon = filteringRayon.ComparingPCCVC(spu);
                                    if (resultRayon == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                default:
                                    //Placing Value as Parameter for SizePickupListDto and add to resultData
                                    results = new SizePickupListDto(filteredSizingDocument,
                                                                    operatorDocument.CoreAccount.Name,
                                                                    operatorDocument.Group,
                                                                    dateTimeSizingBeamProduct,
                                                                    sizingBeamProduct.CounterStart ?? 0,
                                                                    sizingBeamProduct.CounterFinish ?? 0,
                                                                    sizingBeamProduct.WeightNetto ?? 0,
                                                                    sizingBeamProduct.WeightBruto ?? 0,
                                                                    pisMeter,
                                                                    spu,
                                                                    beamNumber);
                                    resultData.Add(results);
                                    break;
                            }
                        }
                    }

                    if (results != null)
                    {
                        try
                        {
                            resultData = resultData.OrderBy(o => o.DateTimeMachineHistory).ToList();
                        }
                        catch (Exception ex)
                        {
                            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                        }
                    }
                }

                await Task.Yield();

                if (index.Equals(true))
                {
                    byte[] xlsInBytes;
                    string monthOfReport = new CultureInfo("id-ID").DateTimeFormat.GetMonthName(month);
                    string yearOfReport = DateTime.Now.Year.ToString();
                    string fileName = "Laporan Size Pickup " + monthOfReport + "_" + yearOfReport;

                    SizePickupReportXlsTemplate xlsTemplate = new SizePickupReportXlsTemplate();
                    MemoryStream xls = xlsTemplate.GenerateSizePickupReportXls(resultData);
                    xlsInBytes = xls.ToArray();
                    var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    return xlsFile;
                }
                else
                {
                    return Ok(resultData, info: new
                    {
                        count = resultData.Count
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("size-pickup/date/{date}/unit-id/{weavingUnitId}/shift/{shiftId}/spu/{spuStatus}")]
        public async Task<IActionResult> GetReportByDate(string date, int weavingUnitId, string shiftId, string spuStatus)
        {
            try
            {
                var acceptRequest = Request.Headers.Values.ToList();
                var index = acceptRequest.IndexOf("application/xls") > 0;

                //Preparing Container for List of Size Pickup
                var resultData = new List<SizePickupListDto>();

                //Get All Order Document Which Using Weaving Unit Id from Request
                var orderDocumentByWeavingUnit =
                    _orderDocumentRepository
                        .Find(o => o.UnitId.Value.Equals(weavingUnitId))
                        .ToList();

                //Query for Daily Operation Sizing
                var sizingQuery =
                    _dailyOperationSizingRepository
                        .Query
                        .Include(d => d.SizingHistories)
                            .OrderByDescending(item => item.CreatedDate)
                        .Include(b => b.SizingBeamProducts)
                            .OrderByDescending(item => item.CreatedDate);

                //Preparing Container for List of Daily Operation Sizing Document
                var listOfFilteredSizingDocument = new List<DailyOperationSizingDocument>();

                //Filter Sizing Document Which Using Order Document Id Above
                foreach (var order in orderDocumentByWeavingUnit)
                {
                    var filteredSizingDocument =
                        _dailyOperationSizingRepository
                            .Find(sizingQuery)
                            .Where(sizePickup => sizePickup.OrderDocumentId.Value.Equals(order.Identity) &&
                                                 sizePickup.OperationStatus.Equals(OperationStatus.ONFINISH))
                                                 .ToList();

                    //Add Filtered Sizing Doc to List of Filtered Sizing Document Container that Already Prepared Above
                    if (filteredSizingDocument.Count > 0)
                    {
                        foreach (var sizingDocument in filteredSizingDocument)
                        {
                            listOfFilteredSizingDocument.Add(sizingDocument);
                        }
                    }
                }

                //Filter Document in List of Filtered Sizing Document
                foreach (var filteredSizingDocument in listOfFilteredSizingDocument)
                {
                    //Preparing Container for Size Pick Up
                    var results = new SizePickupListDto();

                    //Convert Date from Request Date (UI)
                    var convertedDate = DateTimeOffset.Parse(date).Date;

                    var recipeCode = filteredSizingDocument.RecipeCode;
                    var machineSpeed = filteredSizingDocument.MachineSpeed;
                    var texSQ = filteredSizingDocument.TexSQ;
                    var visco = filteredSizingDocument.Visco;

                    //Get Order Document Used in Current Sizing Document
                    var sizingOrderDocument =
                        _orderDocumentRepository
                            .Find(o => o.Identity.Equals(filteredSizingDocument.OrderDocumentId.Value))
                            .FirstOrDefault();

                    //Get Fabric Construction Used in Current Sizing Document to Get Fabric Construction Number
                    var sizingFabricConstructionDocument =
                        _constructionDocumentRepository
                            .Find(o => o.Identity.Equals(sizingOrderDocument.ConstructionId.Value))
                            .FirstOrDefault();

                    //Get Fabric Construction Number
                    var constructionNumber = sizingFabricConstructionDocument.ConstructionNumber;
                    string[] splittedConstructionNumber = constructionNumber.Split(" ");
                    var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var filteredConstructionNumber = splittedConstructionNumber[0].TrimEnd(digits);

                    var filteredSizingBeamProducts = filteredSizingDocument.SizingBeamProducts
                                                        .Where(b => b.LatestDateTimeBeamProduct.Date.Equals(convertedDate) &&
                                                                    b.BeamStatus.Equals(BeamStatus.ROLLEDUP));

                    if (filteredSizingBeamProducts != null)
                    {
                        foreach (var sizingBeamProduct in filteredSizingBeamProducts)
                        {
                            //Get Beam Number from BeamRepository (Master Beam), by Beam Id Used in Beam Product (SizingBeamProduct) of Sizing Document
                            var beamQuery =
                                _beamRepository
                                    .Query.OrderByDescending(item => item.CreatedDate);
                            var beamDocument =
                                _beamRepository
                                    .Find(beamQuery)
                                    .Where(b => b.Identity.Equals(sizingBeamProduct.SizingBeamId))
                                    .FirstOrDefault();
                            var beamNumber = beamDocument.Number;

                            //Filter Details by Machine Status (COMPLETED), ShiftId (All Shift) and Date from UI (Request), and BeamNumber from BeamRepository Above
                            var historyByAllShiftIndex = 0;
                            var filteredSizingHistories =
                                filteredSizingDocument.SizingHistories
                                    .Where(d => d.DateTimeMachine.Date.Equals(convertedDate) &&
                                                d.SizingBeamNumber.Equals(beamNumber) &&
                                                d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                            var filteredSizingHistory = filteredSizingHistories.ToList()[historyByAllShiftIndex++];

                            if (shiftId != "All")
                            {
                                var historyBySpecifiedShiftlIndex = 0;
                                //Filter Details by Machine Status (COMPLETED), ShiftId (Specified Shift) and Date from UI (Request), and BeamNumber from BeamRepository Above
                                filteredSizingHistories =
                                    filteredSizingDocument.SizingHistories
                                        .Where(d => d.ShiftDocumentId.ToString().Equals(shiftId) &&
                                                    d.DateTimeMachine.Date.Equals(convertedDate) &&
                                                    d.SizingBeamNumber.Equals(beamNumber) &&
                                                    d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                                filteredSizingHistory = filteredSizingHistories.ToList()[historyBySpecifiedShiftlIndex++];
                            }

                            //Get Operator Document from OperatorRepository, relate by filteredDetail.OperatorDocumentId
                            var operatorQuery =
                                _operatorDocumentRepository
                                    .Query.OrderByDescending(item => item.CreatedDate);
                            var operatorDocument =
                                _operatorDocumentRepository
                                    .Find(operatorQuery)
                                    .Where(o => o.Identity.Equals(filteredSizingHistory.OperatorDocumentId))
                                    .FirstOrDefault();

                            var dateTimeSizingBeamProduct = sizingBeamProduct.LatestDateTimeBeamProduct;
                            double pisMeter = sizingBeamProduct.PISMeter ?? 0;
                            double spu = sizingBeamProduct.SPU ?? 0;

                            switch (filteredConstructionNumber)
                            {
                                case SizePickupSPUConstants.PCCONSTRUCTION:
                                    Filtering filteringPC = new Filtering();
                                    var resultPC = filteringPC.ComparingPCCVC(spu);
                                    if (resultPC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.CVCCONSTRUCTION:
                                    Filtering filteringCVC = new Filtering();
                                    var resultCVC = filteringCVC.ComparingPCCVC(spu);
                                    if (resultCVC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.COTTONCONSTRUCTION:
                                    Filtering filteringCotton = new Filtering();
                                    var resultCotton = filteringCotton.ComparingPCCVC(spu);
                                    if (resultCotton == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.PECONSTRUCTION:
                                    Filtering filteringPE = new Filtering();
                                    var resultPE = filteringPE.ComparingPCCVC(spu);
                                    if (resultPE == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.RAYONCONSTRUCTION:
                                    Filtering filteringRayon = new Filtering();
                                    var resultRayon = filteringRayon.ComparingPCCVC(spu);
                                    if (resultRayon == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                default:
                                    //Placing Value as Parameter for SizePickupListDto and add to resultData
                                    results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                    resultData.Add(results);
                                    break;
                            }
                        }
                    }

                    if (results != null)
                    {
                        try
                        {
                            resultData = resultData.OrderBy(o => o.DateTimeMachineHistory).ToList();
                        }
                        catch (Exception ex)
                        {
                            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                        }
                    }
                }

                await Task.Yield();

                if (index.Equals(true))
                {
                    byte[] xlsInBytes;
                    string day = DateTime.Parse(date).Day.ToString();
                    int month = DateTime.Parse(date).Month;
                    string indonesianMonth = new CultureInfo("id-ID").DateTimeFormat.GetMonthName(month).ToString();
                    string year = DateTime.Parse(date).Year.ToString();
                    string dateOfReport = day + "-" + indonesianMonth + "-" + year;

                    string fileName = "Laporan Size Pickup " + dateOfReport;

                    SizePickupReportXlsTemplate xlsTemplate = new SizePickupReportXlsTemplate();
                    MemoryStream xls = xlsTemplate.GenerateSizePickupReportXls(resultData);
                    xlsInBytes = xls.ToArray();
                    var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    return xlsFile;
                }
                else
                {
                    return Ok(resultData, info: new
                    {
                        count = resultData.Count
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("size-pickup/daterange/start-date/{startDate}/end-date/{endDate}/unit-id/{weavingUnitId}/shift/{shiftId}/spu/{spuStatus}")]
        public async Task<IActionResult> GetReportByDateRange(string startDate, string endDate, int weavingUnitId, string shiftId, string spuStatus)
        {
            try
            {
                var acceptRequest = Request.Headers.Values.ToList();
                var index = acceptRequest.IndexOf("application/xls") > 0;

                //Preparing Container for List of Size Pickup
                var resultData = new List<SizePickupListDto>();

                //Get All Order Document Which Using Weaving Unit Id from Request
                var orderDocumentByWeavingUnit =
                    _orderDocumentRepository
                        .Find(o => o.UnitId.Value.Equals(weavingUnitId))
                        .ToList();

                //Query for Daily Operation Sizing
                var sizingQuery =
                    _dailyOperationSizingRepository
                        .Query
                        .Include(d => d.SizingHistories)
                            .OrderByDescending(item => item.CreatedDate)
                        .Include(b => b.SizingBeamProducts)
                            .OrderByDescending(item => item.CreatedDate);

                //Preparing Container for List of Daily Operation Sizing Document
                var listOfFilteredSizingDocument = new List<DailyOperationSizingDocument>();

                //Filter Sizing Document Which Using Order Document Id Above
                foreach (var order in orderDocumentByWeavingUnit)
                {
                    var filteredSizingDocument =
                    _dailyOperationSizingRepository
                        .Find(sizingQuery)
                        .Where(sizePickup => sizePickup.OrderDocumentId.Value.Equals(order.Identity) &&
                                             sizePickup.OperationStatus.Equals(OperationStatus.ONFINISH))
                                             .ToList();

                    if (filteredSizingDocument.Count > 0)
                    {
                        foreach (var sizingDocument in filteredSizingDocument)
                        {
                            listOfFilteredSizingDocument.Add(sizingDocument);
                        }
                    }
                }

                //Filter Document in List of Filtered Sizing Document
                foreach (var filteredSizingDocument in listOfFilteredSizingDocument)
                {
                    //Preparing Container for Size Pick Up
                    var results = new SizePickupListDto();

                    //Convert Date Range from Request Date (UI)
                    var convertedStartDate = DateTimeOffset.Parse(startDate).Date;
                    var convertedEndDate = DateTimeOffset.Parse(endDate).Date;

                    var recipeCode = filteredSizingDocument.RecipeCode;
                    var machineSpeed = filteredSizingDocument.MachineSpeed;
                    var texSQ = filteredSizingDocument.TexSQ;
                    var visco = filteredSizingDocument.Visco;

                    //Get Order Document Used in Current Sizing Document
                    var sizingOrderDocument =
                        _orderDocumentRepository
                            .Find(o => o.Identity.Equals(filteredSizingDocument.OrderDocumentId.Value))
                            .FirstOrDefault();

                    //Get Fabric Construction Used in Current Sizing Document to Get Fabric Construction Number
                    var sizingFabricConstructionDocument =
                        _constructionDocumentRepository
                            .Find(o => o.Identity.Equals(sizingOrderDocument.ConstructionId.Value))
                            .FirstOrDefault();

                    //Get Fabric Construction Number
                    var constructionNumber = sizingFabricConstructionDocument.ConstructionNumber;
                    string[] splittedConstructionNumber = constructionNumber.Split(" ");
                    var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var filteredConstructionNumber = splittedConstructionNumber[0].TrimEnd(digits);

                    var filteredSizingBeamProducts = filteredSizingDocument.SizingBeamProducts
                                                        .Where(b => b.LatestDateTimeBeamProduct.DateTime >= convertedStartDate &&
                                                                    b.LatestDateTimeBeamProduct.DateTime <= convertedEndDate &&
                                                                    b.BeamStatus.Equals(BeamStatus.ROLLEDUP));

                    if (filteredSizingBeamProducts != null)
                    {
                        foreach (var sizingBeamProduct in filteredSizingBeamProducts)
                        {
                            //Get Beam Number from BeamRepository, relate by BeamId
                            var beamQuery =
                                _beamRepository
                                    .Query.OrderByDescending(item => item.CreatedDate);
                            var beamDocument =
                                _beamRepository
                                    .Find(beamQuery)
                                    .Where(b => b.Identity.Equals(sizingBeamProduct.SizingBeamId))
                                    .FirstOrDefault();
                            var beamNumber = beamDocument.Number;

                            //Filter Details by Machine Status (COMPLETED), ShiftId (All Shift) and Date Range from UI (Request), and BeamNumber from BeamRepository Above
                            var historyByAllShiftIndex = 0;
                            var filteredSizingHistories =
                                filteredSizingDocument.SizingHistories
                                    .Where(d => d.DateTimeMachine.DateTime >= convertedStartDate &&
                                                d.DateTimeMachine.DateTime <= convertedEndDate &&
                                                d.SizingBeamNumber.Equals(beamNumber) &&
                                                d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                            var filteredDetail = filteredSizingHistories.ToList()[historyByAllShiftIndex++];
                            if (shiftId != "All")
                            {
                                var detailIndex = 0;
                                //Filter Details by Machine Status (COMPLETED), ShiftId (Specified Shift) and Date Range from UI (Request), and BeamNumber from BeamRepository Above
                                filteredSizingHistories =
                                    filteredSizingDocument.SizingHistories
                                        .Where(d => d.ShiftDocumentId.ToString().Equals(shiftId) &&
                                                    d.DateTimeMachine.DateTime >= convertedStartDate &&
                                                    d.DateTimeMachine.DateTime <= convertedEndDate &&
                                                    d.SizingBeamNumber.Equals(beamNumber) &&
                                                    d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                                filteredDetail = filteredSizingHistories.ToList()[detailIndex++];
                            }

                            //Get Operator Document from OperatorRepository, relate by filteredDetail.OperatorDocumentId
                            var operatorQuery =
                                _operatorDocumentRepository
                                    .Query.OrderByDescending(item => item.CreatedDate);
                            var operatorDocument =
                                _operatorDocumentRepository
                                    .Find(operatorQuery)
                                    .Where(o => o.Identity.Equals(filteredDetail.OperatorDocumentId))
                                    .FirstOrDefault();

                            var dateTimeSizingBeamProduct = sizingBeamProduct.LatestDateTimeBeamProduct;
                            double pisMeter = sizingBeamProduct.PISMeter ?? 0;
                            double spu = sizingBeamProduct.SPU ?? 0;

                            switch (filteredConstructionNumber)
                            {
                                case SizePickupSPUConstants.PCCONSTRUCTION:
                                    Filtering filteringPC = new Filtering();
                                    var resultPC = filteringPC.ComparingPCCVC(spu);
                                    if (resultPC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.CVCCONSTRUCTION:
                                    Filtering filteringCVC = new Filtering();
                                    var resultCVC = filteringCVC.ComparingPCCVC(spu);
                                    if (resultCVC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.COTTONCONSTRUCTION:
                                    Filtering filteringCotton = new Filtering();
                                    var resultCotton = filteringCotton.ComparingPCCVC(spu);
                                    if (resultCotton == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.PECONSTRUCTION:
                                    Filtering filteringPE = new Filtering();
                                    var resultPE = filteringPE.ComparingPCCVC(spu);
                                    if (resultPE == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.RAYONCONSTRUCTION:
                                    Filtering filteringRayon = new Filtering();
                                    var resultRayon = filteringRayon.ComparingPCCVC(spu);
                                    if (resultRayon == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                default:
                                    //Placing Value as Parameter for SizePickupListDto and add to resultData
                                    results = new SizePickupListDto(filteredSizingDocument,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTimeSizingBeamProduct,
                                                                        sizingBeamProduct.CounterStart ?? 0,
                                                                        sizingBeamProduct.CounterFinish ?? 0,
                                                                        sizingBeamProduct.WeightNetto ?? 0,
                                                                        sizingBeamProduct.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                    resultData.Add(results);
                                    break;
                            }
                        }
                    }

                    if (results != null)
                    {
                        try
                        {
                            resultData = resultData.OrderBy(o => o.DateTimeMachineHistory).ToList();
                        }
                        catch (Exception ex)
                        {
                            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                        }
                    }
                }

                await Task.Yield();

                if (index.Equals(true))
                {
                    byte[] xlsInBytes;
                    string startDay = DateTime.Parse(startDate).Day.ToString();
                    int startMonth = DateTime.Parse(startDate).Month;
                    string indonesianStartMonth = new CultureInfo("id-ID").DateTimeFormat.GetMonthName(startMonth).ToString();
                    string startYear = DateTime.Parse(startDate).Year.ToString();
                    string startDateOfReport = startDay + "-" + indonesianStartMonth + "-" + startYear;

                    string endDay = DateTime.Parse(endDate).Day.ToString();
                    int endMonth = DateTime.Parse(endDate).Month;
                    string indonesianEndMonth = new CultureInfo("id-ID").DateTimeFormat.GetMonthName(endMonth).ToString();
                    string endYear = DateTime.Parse(endDate).Year.ToString();
                    string endDateOfReport = endDay + "-" + indonesianEndMonth + "-" + endYear;

                    string fileName = "Laporan Size Pickup " + startDateOfReport + "_" + endDateOfReport;

                    SizePickupReportXlsTemplate xlsTemplate = new SizePickupReportXlsTemplate();
                    MemoryStream xls = xlsTemplate.GenerateSizePickupReportXls(resultData);
                    xlsInBytes = xls.ToArray();
                    var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    return xlsFile;
                }
                else
                {
                    return Ok(resultData, info: new
                    {
                        count = resultData.Count
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
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
