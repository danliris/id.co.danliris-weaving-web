using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Sizing.Calculations;
using Manufactures.Application.DailyOperations.Sizing.SizePickup;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Calculation;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachineTypes.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Domain.Shifts.ValueObjects;
using Manufactures.Domain.StockCard.Events.Sizing;
using Manufactures.Domain.StockCard.Events.Warping;
using Manufactures.Dtos;
using Manufactures.Dtos.Beams;
using Manufactures.Dtos.DailyOperations.Sizing;
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
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/daily-operations-sizing")]
    [ApiController]
    [Authorize]
    public class DailyOperationSizingController : ControllerApiBase
    {
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IMachineTypeRepository
            _machineTypeRepository;
        private readonly IFabricConstructionRepository
            _constructionDocumentRepository;
        private readonly IShiftRepository
            _shiftDocumentRepository;
        private readonly IBeamRepository
            _beamDocumentRepository;
        private readonly IOperatorRepository
            _operatorDocumentRepository;

        public DailyOperationSizingController(IServiceProvider serviceProvider,
                                                 IWorkContext workContext)
            : base(serviceProvider)
        {
            _dailyOperationSizingDocumentRepository =
                this.Storage.GetRepository<IDailyOperationSizingRepository>();
            _machineRepository =
                this.Storage.GetRepository<IMachineRepository>();
            _machineTypeRepository =
                this.Storage.GetRepository<IMachineTypeRepository>();
            _constructionDocumentRepository =
                this.Storage.GetRepository<IFabricConstructionRepository>();
            _shiftDocumentRepository =
                this.Storage.GetRepository<IShiftRepository>();
            _beamDocumentRepository =
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
            page = page - 1;
            var domQuery =
                _dailyOperationSizingDocumentRepository
                    .Query
                    .OrderByDescending(item => item.CreatedDate);
            var dailyOperationSizingDocuments =
                _dailyOperationSizingDocumentRepository
                    .Find(domQuery.Include(d => d.SizingDetails));

            var dailyOperationSizings = new List<DailyOperationSizingListDto>();

            foreach (var dailyOperation in dailyOperationSizingDocuments)
            {
                var machineDocument =
                   _machineRepository
                       .Find(e => e.Identity.Equals(dailyOperation.MachineDocumentId.Value))
                       .FirstOrDefault();

                var constructionDocument =
                    _constructionDocumentRepository
                        .Find(e => e.Identity.Equals(dailyOperation.ConstructionDocumentId.Value))
                        .FirstOrDefault();

                var shiftOnDetail = new ShiftValueObject();
                var dailyOperationEntryDateTime = dailyOperation.SizingDetails.OrderBy(e => e.DateTimeMachine).FirstOrDefault().DateTimeMachine;
                var lastDailyOperationStatus = dailyOperation.OperationStatus;

                foreach (var detail in dailyOperation.SizingDetails)
                {
                    var shiftDocument =
                        _shiftDocumentRepository
                            .Find(e => e.Identity.Equals(detail.ShiftDocumentId)).LastOrDefault();

                    shiftOnDetail = new ShiftValueObject(shiftDocument.Name, shiftDocument.StartTime, shiftDocument.EndTime);
                }

                var dto = new DailyOperationSizingListDto(dailyOperation, machineDocument, constructionDocument, shiftOnDetail, lastDailyOperationStatus, dailyOperationEntryDateTime);

                dailyOperationSizings.Add(dto);
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() + orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop =
                    typeof(DailyOperationSizingListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    dailyOperationSizings =
                        dailyOperationSizings
                            .OrderBy(x => prop.GetValue(x, null))
                            .ToList();
                }
                else
                {
                    dailyOperationSizings =
                        dailyOperationSizings
                            .OrderByDescending(x => prop.GetValue(x, null))
                            .ToList();
                }
            }

            var ResultDailyOperationSizings =
                dailyOperationSizings.Skip(page * size).Take(size).ToList();
            int totalRows = dailyOperationSizings.Count();
            int resultCount = ResultDailyOperationSizings.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(ResultDailyOperationSizings, info: new
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
            try
            {
                var Identity = Guid.Parse(Id);
                var query = _dailyOperationSizingDocumentRepository.Query;
                var dailyOperationalSizing =
                    _dailyOperationSizingDocumentRepository.Find(query
                                                           .Include(detail => detail.SizingDetails).Where(detailId => detailId.Identity == Identity)
                                                           .Include(beamDocument => beamDocument.SizingBeamDocuments).Where(beamDocumentId => beamDocumentId.Identity == Identity))
                                                           .FirstOrDefault();

                var machineDocument =
                       _machineRepository
                           .Find(e => e.Identity.Equals(dailyOperationalSizing.MachineDocumentId.Value))
                           .FirstOrDefault();
                var machineNumber = machineDocument.MachineNumber;

                var machineTypeDocument = _machineTypeRepository
                    .Find(m => m.Identity.Equals(machineDocument.MachineTypeId.Value))
                    .FirstOrDefault();
                var machineType = machineTypeDocument.TypeName;

                var constructionDocument =
                        _constructionDocumentRepository
                            .Find(e => e.Identity.Equals(dailyOperationalSizing.ConstructionDocumentId.Value))
                            .FirstOrDefault();

                var constructionNumber = constructionDocument.ConstructionNumber;

                var warpingBeams = new List<BeamDto>();

                foreach (var beam in dailyOperationalSizing.BeamsWarping)
                {
                    var beamDocument = _beamDocumentRepository.Find(b => b.Identity.Equals(beam.Value)).FirstOrDefault();

                    var beamsDto = new BeamDto(beamDocument);

                    warpingBeams.Add(beamsDto);
                }

                var yarnStrands = dailyOperationalSizing.YarnStrands;
                var neReal = dailyOperationalSizing.NeReal;

                var dto = new DailyOperationSizingByIdDto(dailyOperationalSizing, machineNumber, machineType, constructionNumber, warpingBeams, yarnStrands, neReal);

                foreach (var detail in dailyOperationalSizing.SizingDetails)
                {
                    var shiftDocument =
                        _shiftDocumentRepository
                            .Find(e => e.Identity.Equals(detail.ShiftDocumentId))
                            .FirstOrDefault();

                    var shiftName = shiftDocument.Name;

                    var history = new DailyOperationSizingDetailsHistoryDto(detail.DateTimeMachine, detail.MachineStatus, detail.Information);

                    var detailCauses = detail.Causes.Deserialize<DailyOperationSizingDetailsCausesDto>();

                    var causes = new DailyOperationSizingDetailsCausesDto(detailCauses.BrokenBeam, detailCauses.MachineTroubled);

                    var sizingBeamNumberOnDetail = detail.SizingBeamNumber;

                    var detailsDto = new DailyOperationSizingDetailsDto(shiftName, history, causes, sizingBeamNumberOnDetail);

                    dto.SizingDetails.Add(detailsDto);
                }
                dto.SizingDetails = dto.SizingDetails.OrderByDescending(history => history.DateTimeMachineHistory).ToList();

                foreach (var beamDocument in dailyOperationalSizing.SizingBeamDocuments)
                {
                    var beamSizingDocument = _beamDocumentRepository
                                                .Find(e => e.Identity.Equals(beamDocument.SizingBeamId))
                                                .FirstOrDefault();

                    var dateTimeBeamDocument = beamDocument.DateTimeBeamDocument;

                    var beamDocumentCounter = beamDocument.Counter.Deserialize<DailyOperationSizingBeamDocumentsCounterDto>();
                    var startCounter = beamDocumentCounter.Start;
                    var finishCounter = beamDocumentCounter.Finish;

                    var beamDocumentWeight = beamDocument.Weight.Deserialize<DailyOperationSizingBeamDocumentsWeightDto>();
                    var nettoWeight = beamDocumentWeight.Netto;
                    var brutoWeight = beamDocumentWeight.Bruto;
                    var theoriticalWeight = beamDocumentWeight.Theoritical;

                    var pisMeter = beamDocument.PISMeter;
                    var spu = beamDocument.SPU;
                    var sizingBeamStatus = beamDocument.SizingBeamStatus;

                    if (beamSizingDocument != null)
                    {
                        if (beamSizingDocument.Number != null)
                        {
                            var sizingBeamNumberOnBeamDocument = beamSizingDocument.Number;
                            var beamDocumentsDto = new DailyOperationSizingBeamDocumentsDto(sizingBeamNumberOnBeamDocument, dateTimeBeamDocument, startCounter, finishCounter, nettoWeight, brutoWeight, pisMeter, spu, sizingBeamStatus);
                            dto.SizingBeamDocuments.Add(beamDocumentsDto);
                        }
                    }
                    else
                    {
                        var beamDocumentsDto = new DailyOperationSizingBeamDocumentsDto(dateTimeBeamDocument, startCounter, finishCounter, nettoWeight, brutoWeight, pisMeter, spu, sizingBeamStatus);
                        dto.SizingBeamDocuments.Add(beamDocumentsDto);
                    }
                }
                dto.SizingBeamDocuments = dto.SizingBeamDocuments.OrderByDescending(beamDocument => beamDocument.DateTimeBeamDocument).ToList();

                await Task.Yield();

                if (Identity == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(dto);
                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]NewEntryDailyOperationSizingCommand command)
        {
            var newDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(newDailyOperationSizingDocument.Identity);
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

            //Preparing Event
            var moveOutWarpingBeam = new MoveOutBeamStockWarpingEvent();

            //Initiate beam stock
            var moveOutWarpingBeamIds = 
                updateStartDailyOperationSizingDocument.BeamsWarping;

            foreach(var beamId in moveOutWarpingBeamIds)
            {
                //Manipulate datetime to be stocknumber

                //wait 1 seconds
                await Task.Delay(1000);
                var dateTimeNow = DateTimeOffset.UtcNow.AddHours(7);
                StringBuilder stockNumber = new StringBuilder();

                stockNumber.Append(dateTimeNow.ToString("HH"));
                stockNumber.Append("/");
                stockNumber.Append(dateTimeNow.ToString("mm"));
                stockNumber.Append("/");
                stockNumber.Append(StockCardStatus.SIZING_STOCK);
                stockNumber.Append("/");
                stockNumber.Append(dateTimeNow.ToString("dd'/'MM'/'yyyy"));

                moveOutWarpingBeam.BeamId = beamId;
                moveOutWarpingBeam.StockNumber = stockNumber.ToString();
                moveOutWarpingBeam.DailyOperationId = new DailyOperationId(updateStartDailyOperationSizingDocument.Identity);
                moveOutWarpingBeam.DateTimeOperation = dateTimeNow;

                //Update stock
                await Mediator.Publish(moveOutWarpingBeam);
            }

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

        [HttpPut("{Id}/doff")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateDoffFinishDailyOperationSizingCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateDoffDailyOperationSizingDocument = await Mediator.Send(command);

            return Ok(updateDoffDailyOperationSizingDocument.Identity);
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

            //Preparing Event
            var addStockEvent = new MoveInBeamStockSizingEvent();

            //Manipulate datetime to be stocknumber

            //wait 1 seconds
            await Task.Delay(1000);
            var dateTimeNow = DateTimeOffset.UtcNow.AddHours(7);
            StringBuilder stockNumber = new StringBuilder();
            stockNumber.Append(dateTimeNow.ToString("HH"));
            stockNumber.Append("/");
            stockNumber.Append(dateTimeNow.ToString("mm"));
            stockNumber.Append("/");
            stockNumber.Append(StockCardStatus.SIZING_STOCK);
            stockNumber.Append("/");
            stockNumber.Append(dateTimeNow.ToString("dd'/'MM'/'yyyy"));

            //Initiate events existingDailyOperation.SizingBeamDocuments.OrderByDescending(b => b.DateTimeBeamDocument);
            addStockEvent.BeamId = 
                new BeamId(reuseBeamsDailyOperationSizingDocument
                    .SizingBeamDocuments
                    .OrderByDescending(b => b.DateTimeBeamDocument)
                    .FirstOrDefault()
                    .SizingBeamId);
            addStockEvent.StockNumber = stockNumber.ToString();
            addStockEvent.DailyOperationId = new DailyOperationId(reuseBeamsDailyOperationSizingDocument.Identity);
            addStockEvent.DateTimeOperation = dateTimeNow;

            //Update stock
            await Mediator.Publish(addStockEvent);

            return Ok(reuseBeamsDailyOperationSizingDocument.Identity);
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

        [HttpGet("calculate/pis-in-pieces/start/{counterStart}/finish/{counterFinish}")]
        public async Task<IActionResult> CalculatePISInPieces(double counterStart, double counterFinish)
        {
            double pisInMeter;

            if (counterStart >= 0 && counterFinish > 0)
            {
                PIS calculate = new PIS();
                pisInMeter = calculate.CalculateInPieces(counterStart, counterFinish);

                await Task.Yield();
                return Ok(pisInMeter);
            }
            else
            {
                await Task.Yield();
                return NotFound();
                throw Validator.ErrorValidation(("ProduceBeamsFinishCounter", "PIS (m) cannot less than Start Counter"));
            }
        }

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

                var resultData = new List<SizePickupListDto>();

                var query =
                    _dailyOperationSizingDocumentRepository.Query
                                                           .Include(d => d.SizingDetails).OrderByDescending(item => item.CreatedDate)
                                                           .Include(b => b.SizingBeamDocuments).OrderByDescending(item => item.CreatedDate);
                var sizePickupDtos =
                    _dailyOperationSizingDocumentRepository.Find(query)
                                                           .Where(sizePickup => sizePickup.WeavingUnitId.Value.Equals(weavingUnitId) &&
                                                                                sizePickup.OperationStatus.Equals(OperationStatus.ONFINISH)).ToList();

                foreach (var document in sizePickupDtos)
                {
                    var results = new SizePickupListDto();

                    var recipeCode = document.RecipeCode;
                    var machineSpeed = document.MachineSpeed;
                    var texSQ = document.TexSQ;
                    var visco = document.Visco;

                    var constructionDocument =
                        _constructionDocumentRepository
                            .Find(e => e.Identity.Equals(document.ConstructionDocumentId.Value))
                            .FirstOrDefault();
                    var constructionNumber = constructionDocument.ConstructionNumber;
                    string[] splittedConstructionNumber = constructionNumber.Split(" ");
                    var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var filteredConstructionNumber = splittedConstructionNumber[0].TrimEnd(digits);

                    var filteredSizingBeamDocuments = document.SizingBeamDocuments.Where(b => b.DateTimeBeamDocument.Month.Equals(month) && b.SizingBeamStatus.Equals(BeamStatus.ROLLEDUP));

                    if (filteredSizingBeamDocuments != null)
                    {
                        foreach (var sizingBeam in filteredSizingBeamDocuments)
                        {
                            //Get Beam Number from BeamRepository, relate by BeamId
                            var sizingBeamQuery = _beamDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var beamDetail = _beamDocumentRepository.Find(sizingBeamQuery).Where(b => b.Identity.Equals(sizingBeam.SizingBeamId)).FirstOrDefault();
                            var beamNumber = beamDetail.Number;

                            //Filter Entry Details
                            //var filteredDetailsEntry= document.SizingDetails.Where(d => d.MachineStatus.Equals(MachineStatus.ONCOMPLETE)).FirstOrDefault();
                            //var entryDate = filteredDetailsEntry.DateTimeMachine;

                            //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                            var allDetailIndex = 0;
                            var filteredDetails = document.SizingDetails.Where(d => d.DateTimeMachine.Month.Equals(month) && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                            var filteredDetail = filteredDetails.ToList()[allDetailIndex++];
                            if (shiftId != "All")
                            {
                                var detailIndex = 0;
                                //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                                filteredDetails = document.SizingDetails.Where(d => d.ShiftDocumentId.ToString().Equals(shiftId) && d.DateTimeMachine.Month.Equals(month) && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                                filteredDetail = filteredDetails.ToList()[detailIndex++];
                            }

                            //Get Operator Document from OperatorRepository, relate by filteredDetail.OperatorDocumentId
                            var operatorQuery = _operatorDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var operatorDocument = _operatorDocumentRepository.Find(operatorQuery).Where(o => o.Identity.Equals(filteredDetail.OperatorDocumentId)).FirstOrDefault();

                            var dateTime = sizingBeam.DateTimeBeamDocument;
                            var counter = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsCounterDto>(sizingBeam.Counter);
                            var weight = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsWeightDto>(sizingBeam.Weight);
                            double pisMeter = sizingBeam.PISMeter;
                            double spu = sizingBeam.SPU;

                            switch (filteredConstructionNumber)
                            {
                                case SizePickupSPUConstants.PCCONSTRUCTION:
                                    Filtering filteringPC = new Filtering();
                                    var resultPC = filteringPC.ComparingPCCVC(spu);
                                    if (resultPC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.CVCCONSTRUCTION:
                                    Filtering filteringCVC = new Filtering();
                                    var resultCVC = filteringCVC.ComparingPCCVC(spu);
                                    if (resultCVC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.COTTONCONSTRUCTION:
                                    Filtering filteringCotton = new Filtering();
                                    var resultCotton = filteringCotton.ComparingPCCVC(spu);
                                    if (resultCotton == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.PECONSTRUCTION:
                                    Filtering filteringPE = new Filtering();
                                    var resultPE = filteringPE.ComparingPCCVC(spu);
                                    if (resultPE == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.RAYONCONSTRUCTION:
                                    Filtering filteringRayon = new Filtering();
                                    var resultRayon = filteringRayon.ComparingPCCVC(spu);
                                    if (resultRayon == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                default:
                                    //Placing Value as Parameter for SizePickupListDto and add to resultData
                                    results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
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


                //if (resultData.Count != 0)
                //{
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
                //}
                //else
                //{
                //    return NotFound();
                //}
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

                var resultData = new List<SizePickupListDto>();

                var query =
                    _dailyOperationSizingDocumentRepository.Query
                                                           .Include(d => d.SizingDetails).OrderByDescending(item => item.CreatedDate)
                                                           .Include(b => b.SizingBeamDocuments).OrderByDescending(item => item.CreatedDate);
                var sizePickupDtos =
                    _dailyOperationSizingDocumentRepository.Find(query)
                                                           .Where(sizePickup => sizePickup.WeavingUnitId.Value.Equals(weavingUnitId) &&
                                                                                sizePickup.OperationStatus.Equals(OperationStatus.ONFINISH)).ToList();

                foreach (var document in sizePickupDtos)
                {
                    var results = new SizePickupListDto();
                    var convertedDate = DateTimeOffset.Parse(date).Date;

                    var recipeCode = document.RecipeCode;
                    var machineSpeed = document.MachineSpeed;
                    var texSQ = document.TexSQ;
                    var visco = document.Visco;

                    var constructionDocument =
                        _constructionDocumentRepository
                            .Find(e => e.Identity.Equals(document.ConstructionDocumentId.Value))
                            .FirstOrDefault();
                    var constructionNumber = constructionDocument.ConstructionNumber;
                    string[] splittedConstructionNumber = constructionNumber.Split(" ");
                    var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var filteredConstructionNumber = splittedConstructionNumber[0].TrimEnd(digits);

                    var filteredSizingBeamDocuments = document.SizingBeamDocuments.Where(b => b.DateTimeBeamDocument.Date.Equals(convertedDate) && b.SizingBeamStatus.Equals(BeamStatus.ROLLEDUP));

                    if (filteredSizingBeamDocuments != null)
                    {
                        foreach (var sizingBeam in filteredSizingBeamDocuments)
                        {
                            //Get Beam Number from BeamRepository, relate by BeamId
                            var sizingBeamQuery = _beamDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var beamDetail = _beamDocumentRepository.Find(sizingBeamQuery).Where(b => b.Identity.Equals(sizingBeam.SizingBeamId)).FirstOrDefault();
                            var beamNumber = beamDetail.Number;

                            //Filter Entry Details
                            //var filteredDetailsEntry= document.SizingDetails.Where(d => d.MachineStatus.Equals(MachineStatus.ONCOMPLETE)).FirstOrDefault();
                            //var entryDate = filteredDetailsEntry.DateTimeMachine;

                            //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                            var allDetailIndex = 0;
                            var filteredDetails = document.SizingDetails.Where(d => d.DateTimeMachine.Date.Equals(convertedDate) && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                            var filteredDetail = filteredDetails.ToList()[allDetailIndex++];
                            if (shiftId != "All")
                            {
                                var detailIndex = 0;
                                //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                                filteredDetails = document.SizingDetails.Where(d => d.ShiftDocumentId.ToString().Equals(shiftId) && d.DateTimeMachine.Date.Equals(convertedDate) && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                                filteredDetail = filteredDetails.ToList()[detailIndex++];
                            }

                            //Get Operator Document from OperatorRepository, relate by filteredDetail.OperatorDocumentId
                            var operatorQuery = _operatorDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var operatorDocument = _operatorDocumentRepository.Find(operatorQuery).Where(o => o.Identity.Equals(filteredDetail.OperatorDocumentId)).FirstOrDefault();

                            var dateTime = sizingBeam.DateTimeBeamDocument;
                            var counter = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsCounterDto>(sizingBeam.Counter);
                            var weight = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsWeightDto>(sizingBeam.Weight);
                            double pisMeter = sizingBeam.PISMeter;
                            double spu = sizingBeam.SPU;

                            switch (filteredConstructionNumber)
                            {
                                case SizePickupSPUConstants.PCCONSTRUCTION:
                                    Filtering filteringPC = new Filtering();
                                    var resultPC = filteringPC.ComparingPCCVC(spu);
                                    if (resultPC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.CVCCONSTRUCTION:
                                    Filtering filteringCVC = new Filtering();
                                    var resultCVC = filteringCVC.ComparingPCCVC(spu);
                                    if (resultCVC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.COTTONCONSTRUCTION:
                                    Filtering filteringCotton = new Filtering();
                                    var resultCotton = filteringCotton.ComparingPCCVC(spu);
                                    if (resultCotton == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.PECONSTRUCTION:
                                    Filtering filteringPE = new Filtering();
                                    var resultPE = filteringPE.ComparingPCCVC(spu);
                                    if (resultPE == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.RAYONCONSTRUCTION:
                                    Filtering filteringRayon = new Filtering();
                                    var resultRayon = filteringRayon.ComparingPCCVC(spu);
                                    if (resultRayon == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                default:
                                    //Placing Value as Parameter for SizePickupListDto and add to resultData
                                    results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
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

                //if (resultData.Count != 0)
                //{
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
                //}
                //else
                //{
                //    return NotFound();
                //}
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

                var resultData = new List<SizePickupListDto>();

                var query =
                    _dailyOperationSizingDocumentRepository.Query
                                                           .Include(d => d.SizingDetails).OrderByDescending(item => item.CreatedDate)
                                                           .Include(b => b.SizingBeamDocuments).OrderByDescending(item => item.CreatedDate);
                var sizePickupDtos =
                    _dailyOperationSizingDocumentRepository.Find(query)
                                                           .Where(sizePickup => sizePickup.WeavingUnitId.Value.Equals(weavingUnitId) &&
                                                                                sizePickup.OperationStatus.Equals(OperationStatus.ONFINISH)).ToList();

                foreach (var document in sizePickupDtos)
                {
                    var results = new SizePickupListDto();
                    var convertedStartDate = DateTimeOffset.Parse(startDate).Date;
                    var convertedEndDate = DateTimeOffset.Parse(endDate).Date;

                    var recipeCode = document.RecipeCode;
                    var machineSpeed = document.MachineSpeed;
                    var texSQ = document.TexSQ;
                    var visco = document.Visco;

                    var constructionDocument =
                        _constructionDocumentRepository
                            .Find(e => e.Identity.Equals(document.ConstructionDocumentId.Value))
                            .FirstOrDefault();
                    var constructionNumber = constructionDocument.ConstructionNumber;
                    string[] splittedConstructionNumber = constructionNumber.Split(" ");
                    var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var filteredConstructionNumber = splittedConstructionNumber[0].TrimEnd(digits);

                    var filteredSizingBeamDocuments = document.SizingBeamDocuments.Where(b => b.DateTimeBeamDocument.DateTime >= convertedStartDate && b.DateTimeBeamDocument.DateTime <= convertedEndDate && b.SizingBeamStatus.Equals(BeamStatus.ROLLEDUP));

                    if (filteredSizingBeamDocuments != null)
                    {
                        foreach (var sizingBeam in filteredSizingBeamDocuments)
                        {
                            //Get Beam Number from BeamRepository, relate by BeamId
                            var sizingBeamQuery = _beamDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var beamDetail = _beamDocumentRepository.Find(sizingBeamQuery).Where(b => b.Identity.Equals(sizingBeam.SizingBeamId)).FirstOrDefault();
                            var beamNumber = beamDetail.Number;

                            //Filter Entry Details
                            //var filteredDetailsEntry= document.SizingDetails.Where(d => d.MachineStatus.Equals(MachineStatus.ONCOMPLETE)).FirstOrDefault();
                            //var entryDate = filteredDetailsEntry.DateTimeMachine;

                            //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                            var allDetailIndex = 0;
                            var filteredDetails = document.SizingDetails.Where(d => d.DateTimeMachine.DateTime >= convertedStartDate && d.DateTimeMachine.DateTime <= convertedEndDate && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                            var filteredDetail = filteredDetails.ToList()[allDetailIndex++];
                            if (shiftId != "All")
                            {
                                var detailIndex = 0;
                                //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                                filteredDetails = document.SizingDetails.Where(d => d.ShiftDocumentId.ToString().Equals(shiftId) && d.DateTimeMachine.DateTime >= convertedStartDate && d.DateTimeMachine.DateTime <= convertedEndDate && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                                filteredDetail = filteredDetails.ToList()[detailIndex++];
                            }

                            //Get Operator Document from OperatorRepository, relate by filteredDetail.OperatorDocumentId
                            var operatorQuery = _operatorDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var operatorDocument = _operatorDocumentRepository.Find(operatorQuery).Where(o => o.Identity.Equals(filteredDetail.OperatorDocumentId)).FirstOrDefault();

                            var dateTime = sizingBeam.DateTimeBeamDocument;
                            var counter = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsCounterDto>(sizingBeam.Counter);
                            var weight = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsWeightDto>(sizingBeam.Weight);
                            double pisMeter = sizingBeam.PISMeter;
                            double spu = sizingBeam.SPU;

                            switch (filteredConstructionNumber)
                            {
                                case SizePickupSPUConstants.PCCONSTRUCTION:
                                    Filtering filteringPC = new Filtering();
                                    var resultPC = filteringPC.ComparingPCCVC(spu);
                                    if (resultPC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.CVCCONSTRUCTION:
                                    Filtering filteringCVC = new Filtering();
                                    var resultCVC = filteringCVC.ComparingPCCVC(spu);
                                    if (resultCVC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.COTTONCONSTRUCTION:
                                    Filtering filteringCotton = new Filtering();
                                    var resultCotton = filteringCotton.ComparingPCCVC(spu);
                                    if (resultCotton == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.PECONSTRUCTION:
                                    Filtering filteringPE = new Filtering();
                                    var resultPE = filteringPE.ComparingPCCVC(spu);
                                    if (resultPE == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                case SizePickupSPUConstants.RAYONCONSTRUCTION:
                                    Filtering filteringRayon = new Filtering();
                                    var resultRayon = filteringRayon.ComparingPCCVC(spu);
                                    if (resultRayon == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                default:
                                    //Placing Value as Parameter for SizePickupListDto and add to resultData
                                    results = new SizePickupListDto(document, operatorDocument.CoreAccount.Name, operatorDocument.Group, dateTime, counter, weight, pisMeter, spu, beamNumber);
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

                //if (resultData.Count != 0)
                //{
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
                //}
                //else
                //{
                //    return NotFound();
                //}
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
