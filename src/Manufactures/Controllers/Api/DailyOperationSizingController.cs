using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Sizing.Calculations;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects;
using Manufactures.Application.DailyOperations.Sizing.SizePickup;
using Manufactures.Application.Helpers;
using Manufactures.Application.Operators.DTOs;
using Manufactures.Application.Shifts.DTOs;
using Manufactures.Domain.Beams.Queries;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Calculation;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Queries;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachineTypes.Repositories;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shifts.Queries;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Dtos;
using Manufactures.Dtos.Beams;
using Manufactures.Helpers.XlsTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moonlay;
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
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IMachineTypeRepository
            _machineTypeRepository;
        private readonly IWeavingOrderDocumentRepository
            _orderDocumentRepository;
        private readonly IFabricConstructionRepository
            _constructionDocumentRepository;
        private readonly IShiftRepository
            _shiftDocumentRepository;
        private readonly IBeamRepository
            _beamDocumentRepository;
        private readonly IOperatorRepository
            _operatorDocumentRepository;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingDocumentRepository;

        private readonly ISizingQuery<DailyOperationSizingListDto> _sizingQuery;
        private readonly IOperatorQuery<OperatorListDto> _operatorQuery;
        private readonly IShiftQuery<ShiftDto> _shiftQuery;
        private readonly IBeamQuery<BeamListDto> _beamQuery;

        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingRepository;
        private readonly IBeamRepository
            _beamRepository;

        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationSizingController(IServiceProvider serviceProvider,
                                               ISizingQuery<DailyOperationSizingListDto> sizingQuery,
                                               IOperatorQuery<OperatorListDto> operatorQuery,
                                               IShiftQuery<ShiftDto> shiftQuery,
                                               IBeamQuery<BeamListDto> beamQuery)
            : base(serviceProvider)
        {
            _sizingQuery = sizingQuery ?? throw new ArgumentNullException(nameof(sizingQuery));
            _operatorQuery = operatorQuery ?? throw new ArgumentNullException(nameof(operatorQuery));
            _shiftQuery = shiftQuery ?? throw new ArgumentNullException(nameof(shiftQuery));
            _beamQuery = beamQuery ?? throw new ArgumentNullException(nameof(beamQuery));

            _dailyOperationSizingRepository = this.Storage.GetRepository<IDailyOperationSizingRepository>();
            _beamRepository = this.Storage.GetRepository<IBeamRepository>();
        }

        //public DailyOperationSizingController(IServiceProvider serviceProvider,
        //                                         IWorkContext workContext)
        //    : base(serviceProvider)
        //{
        //    _dailyOperationSizingDocumentRepository =
        //        this.Storage.GetRepository<IDailyOperationSizingRepository>();
        //    _machineRepository =
        //        this.Storage.GetRepository<IMachineRepository>();
        //    _machineTypeRepository =
        //        this.Storage.GetRepository<IMachineTypeRepository>();
        //    _orderDocumentRepository =
        //        this.Storage.GetRepository<IWeavingOrderDocumentRepository>();
        //    _constructionDocumentRepository =
        //        this.Storage.GetRepository<IFabricConstructionRepository>();
        //    _shiftDocumentRepository =
        //        this.Storage.GetRepository<IShiftRepository>();
        //    _beamDocumentRepository =
        //        this.Storage.GetRepository<IBeamRepository>();
        //    _operatorDocumentRepository =
        //        this.Storage.GetRepository<IOperatorRepository>();
        //    _dailyOperationWarpingDocumentRepository =
        //        this.Storage.GetRepository<IDailyOperationWarpingRepository>();
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

                var resultData = new List<SizePickupListDto>();

                var query =
                    _dailyOperationSizingRepository.Query
                                                           .Include(d => d.SizingHistories).OrderByDescending(item => item.CreatedDate)
                                                           .Include(b => b.SizingBeamProducts).OrderByDescending(item => item.CreatedDate);
                var orderDocument =
                    _orderDocumentRepository
                        .Find(o => o.UnitId.Value.Equals(weavingUnitId))
                        .ToList();

                var listOfSizingDoc = new List<DailyOperationSizingDocument>();

                foreach (var order in orderDocument)
                {
                    var sizePickupDtos =
                    _dailyOperationSizingRepository
                        .Find(query)
                        .Where(sizePickup => sizePickup.OrderDocumentId.Value.Equals(order.Identity) &&
                                             sizePickup.OperationStatus.Equals(OperationStatus.ONFINISH))
                                             .ToList();

                    if (sizePickupDtos.Count > 0)
                    {
                        foreach (var sizingDoc in sizePickupDtos)
                        {
                            listOfSizingDoc.Add(sizingDoc);
                        }
                    }
                }

                foreach (var document in listOfSizingDoc)
                {
                    var results = new SizePickupListDto();

                    var recipeCode = document.RecipeCode;
                    var machineSpeed = document.MachineSpeed;
                    var texSQ = document.TexSQ;
                    var visco = document.Visco;

                    var constructionDocument =
                        _constructionDocumentRepository
                            .Find(e => e.Identity.Equals(document.OrderDocumentId.Value))
                            .FirstOrDefault();
                    var constructionNumber = constructionDocument.ConstructionNumber;
                    string[] splittedConstructionNumber = constructionNumber.Split(" ");
                    var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var filteredConstructionNumber = splittedConstructionNumber[0].TrimEnd(digits);

                    var filteredSizingBeamDocuments = document.SizingBeamProducts.Where(b => b.LatestDateTimeBeamProduct.Month.Equals(month) && b.BeamStatus.Equals(BeamStatus.ROLLEDUP));

                    if (filteredSizingBeamDocuments != null)
                    {
                        foreach (var sizingBeam in filteredSizingBeamDocuments)
                        {
                            //Get Beam Number from BeamRepository, relate by BeamId
                            var sizingBeamQuery = _beamRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var beamDetail = _beamRepository.Find(sizingBeamQuery).Where(b => b.Identity.Equals(sizingBeam.SizingBeamId)).FirstOrDefault();
                            var beamNumber = beamDetail.Number;

                            //Filter Entry Details
                            //var filteredDetailsEntry= document.SizingDetails.Where(d => d.MachineStatus.Equals(MachineStatus.ONCOMPLETE)).FirstOrDefault();
                            //var entryDate = filteredDetailsEntry.DateTimeMachine;

                            //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                            var allDetailIndex = 0;
                            var filteredDetails = document.SizingHistories.Where(d => d.DateTimeMachine.Month.Equals(month) && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                            var filteredDetail = filteredDetails.ToList()[allDetailIndex++];
                            if (shiftId != "All")
                            {
                                var detailIndex = 0;
                                //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                                filteredDetails = document.SizingHistories.Where(d => d.ShiftDocumentId.ToString().Equals(shiftId) && d.DateTimeMachine.Month.Equals(month) && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                                filteredDetail = filteredDetails.ToList()[detailIndex++];
                            }

                            //Get Operator Document from OperatorRepository, relate by filteredDetail.OperatorDocumentId
                            var operatorQuery = _operatorDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var operatorDocument = _operatorDocumentRepository.Find(operatorQuery).Where(o => o.Identity.Equals(filteredDetail.OperatorDocumentId)).FirstOrDefault();

                            var dateTime = sizingBeam.LatestDateTimeBeamProduct;
                            //var counter = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsCounterDto>(sizingBeam.Counter);
                            //var weight = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsWeightDto>(sizingBeam.Weight);
                            double pisMeter = sizingBeam.PISMeter ?? 0;
                            double spu = sizingBeam.SPU ?? 0;

                            switch (filteredConstructionNumber)
                            {
                                case SizePickupSPUConstants.PCCONSTRUCTION:
                                    Filtering filteringPC = new Filtering();
                                    var resultPC = filteringPC.ComparingPCCVC(spu);
                                    if (resultPC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, 
                                                                        operatorDocument.CoreAccount.Name, 
                                                                        operatorDocument.Group, 
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                                        results = new SizePickupListDto(document, 
                                                                        operatorDocument.CoreAccount.Name, 
                                                                        operatorDocument.Group, 
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0, 
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
                                        results = new SizePickupListDto(document, 
                                                                        operatorDocument.CoreAccount.Name, 
                                                                        operatorDocument.Group, 
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                                        results = new SizePickupListDto(document, 
                                                                        operatorDocument.CoreAccount.Name, 
                                                                        operatorDocument.Group, 
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                                        results = new SizePickupListDto(document, 
                                                                        operatorDocument.CoreAccount.Name, 
                                                                        operatorDocument.Group, 
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
                                                                        pisMeter, 
                                                                        spu, 
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                default:
                                    //Placing Value as Parameter for SizePickupListDto and add to resultData
                                    results = new SizePickupListDto(document, 
                                                                    operatorDocument.CoreAccount.Name, 
                                                                    operatorDocument.Group, 
                                                                    dateTime,
                                                                    sizingBeam.CounterStart ?? 0,
                                                                    sizingBeam.CounterFinish ?? 0,
                                                                    sizingBeam.WeightNetto ?? 0,
                                                                    sizingBeam.WeightBruto ?? 0,
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
                    _dailyOperationSizingRepository.Query
                                                           .Include(d => d.SizingHistories).OrderByDescending(item => item.CreatedDate)
                                                           .Include(b => b.SizingBeamProducts).OrderByDescending(item => item.CreatedDate);
                var orderDocument =
                    _orderDocumentRepository
                        .Find(o => o.UnitId.Value.Equals(weavingUnitId))
                        .ToList();

                var listOfSizingDoc = new List<DailyOperationSizingDocument>();

                foreach (var order in orderDocument)
                {
                    var sizePickupDtos =
                    _dailyOperationSizingRepository
                        .Find(query)
                        .Where(sizePickup => sizePickup.OrderDocumentId.Value.Equals(order.Identity) &&
                                             sizePickup.OperationStatus.Equals(OperationStatus.ONFINISH))
                                             .ToList();

                    if (sizePickupDtos.Count > 0)
                    {
                        foreach (var sizingDoc in sizePickupDtos)
                        {
                            listOfSizingDoc.Add(sizingDoc);
                        }
                    }
                }

                foreach (var document in listOfSizingDoc)
                {
                    var results = new SizePickupListDto();
                    var convertedDate = DateTimeOffset.Parse(date).Date;

                    var recipeCode = document.RecipeCode;
                    var machineSpeed = document.MachineSpeed;
                    var texSQ = document.TexSQ;
                    var visco = document.Visco;

                    var constructionDocument =
                        _constructionDocumentRepository
                            .Find(e => e.Identity.Equals(document.OrderDocumentId.Value))
                            .FirstOrDefault();
                    var constructionNumber = constructionDocument.ConstructionNumber;
                    string[] splittedConstructionNumber = constructionNumber.Split(" ");
                    var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var filteredConstructionNumber = splittedConstructionNumber[0].TrimEnd(digits);

                    var filteredSizingBeamDocuments = document.SizingBeamProducts.Where(b => b.LatestDateTimeBeamProduct.Date.Equals(convertedDate) && b.BeamStatus.Equals(BeamStatus.ROLLEDUP));

                    if (filteredSizingBeamDocuments != null)
                    {
                        foreach (var sizingBeam in filteredSizingBeamDocuments)
                        {
                            //Get Beam Number from BeamRepository, relate by BeamId
                            var sizingBeamQuery = _beamRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var beamDetail = _beamRepository.Find(sizingBeamQuery).Where(b => b.Identity.Equals(sizingBeam.SizingBeamId)).FirstOrDefault();
                            var beamNumber = beamDetail.Number;

                            //Filter Entry Details
                            //var filteredDetailsEntry= document.SizingDetails.Where(d => d.MachineStatus.Equals(MachineStatus.ONCOMPLETE)).FirstOrDefault();
                            //var entryDate = filteredDetailsEntry.DateTimeMachine;

                            //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                            var allDetailIndex = 0;
                            var filteredDetails = document.SizingHistories.Where(d => d.DateTimeMachine.Date.Equals(convertedDate) && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                            var filteredDetail = filteredDetails.ToList()[allDetailIndex++];
                            if (shiftId != "All")
                            {
                                var detailIndex = 0;
                                //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                                filteredDetails = document.SizingHistories.Where(d => d.ShiftDocumentId.ToString().Equals(shiftId) && d.DateTimeMachine.Date.Equals(convertedDate) && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                                filteredDetail = filteredDetails.ToList()[detailIndex++];
                            }

                            //Get Operator Document from OperatorRepository, relate by filteredDetail.OperatorDocumentId
                            var operatorQuery = _operatorDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var operatorDocument = _operatorDocumentRepository.Find(operatorQuery).Where(o => o.Identity.Equals(filteredDetail.OperatorDocumentId)).FirstOrDefault();

                            var dateTime = sizingBeam.LatestDateTimeBeamProduct;
                            //var counter = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsCounterDto>(sizingBeam.Counter);
                            //var weight = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsWeightDto>(sizingBeam.Weight);
                            double pisMeter = sizingBeam.PISMeter ??0 ;
                            double spu = sizingBeam.SPU ?? 0;

                            switch (filteredConstructionNumber)
                            {
                                case SizePickupSPUConstants.PCCONSTRUCTION:
                                    Filtering filteringPC = new Filtering();
                                    var resultPC = filteringPC.ComparingPCCVC(spu);
                                    if (resultPC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document, 
                                                                        operatorDocument.CoreAccount.Name, 
                                                                        operatorDocument.Group, 
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                                        results = new SizePickupListDto(document,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                                        results = new SizePickupListDto(document,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                                        results = new SizePickupListDto(document,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                                        results = new SizePickupListDto(document,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                default:
                                    //Placing Value as Parameter for SizePickupListDto and add to resultData
                                    results = new SizePickupListDto(document,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                    _dailyOperationSizingRepository.Query
                                                           .Include(d => d.SizingHistories).OrderByDescending(item => item.CreatedDate)
                                                           .Include(b => b.SizingBeamProducts).OrderByDescending(item => item.CreatedDate);
                var orderDocument =
                    _orderDocumentRepository
                        .Find(o => o.UnitId.Value.Equals(weavingUnitId))
                        .ToList();

                var listOfSizingDoc = new List<DailyOperationSizingDocument>();

                foreach (var order in orderDocument)
                {
                    var sizePickupDtos =
                    _dailyOperationSizingRepository
                        .Find(query)
                        .Where(sizePickup => sizePickup.OrderDocumentId.Value.Equals(order.Identity) &&
                                             sizePickup.OperationStatus.Equals(OperationStatus.ONFINISH))
                                             .ToList();

                    if (sizePickupDtos.Count > 0)
                    {
                        foreach (var sizingDoc in sizePickupDtos)
                        {
                            listOfSizingDoc.Add(sizingDoc);
                        }
                    }
                }

                foreach (var document in listOfSizingDoc)
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
                            .Find(e => e.Identity.Equals(document.OrderDocumentId.Value))
                            .FirstOrDefault();
                    var constructionNumber = constructionDocument.ConstructionNumber;
                    string[] splittedConstructionNumber = constructionNumber.Split(" ");
                    var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var filteredConstructionNumber = splittedConstructionNumber[0].TrimEnd(digits);

                    var filteredSizingBeamDocuments = document.SizingBeamProducts.Where(b => b.LatestDateTimeBeamProduct.DateTime >= convertedStartDate && b.LatestDateTimeBeamProduct.DateTime <= convertedEndDate && b.BeamStatus.Equals(BeamStatus.ROLLEDUP));

                    if (filteredSizingBeamDocuments != null)
                    {
                        foreach (var sizingBeam in filteredSizingBeamDocuments)
                        {
                            //Get Beam Number from BeamRepository, relate by BeamId
                            var sizingBeamQuery = _beamRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var beamDetail = _beamRepository.Find(sizingBeamQuery).Where(b => b.Identity.Equals(sizingBeam.SizingBeamId)).FirstOrDefault();
                            var beamNumber = beamDetail.Number;

                            //Filter Entry Details
                            //var filteredDetailsEntry= document.SizingDetails.Where(d => d.MachineStatus.Equals(MachineStatus.ONCOMPLETE)).FirstOrDefault();
                            //var entryDate = filteredDetailsEntry.DateTimeMachine;

                            //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                            var allDetailIndex = 0;
                            var filteredDetails = document.SizingHistories.Where(d => d.DateTimeMachine.DateTime >= convertedStartDate && d.DateTimeMachine.DateTime <= convertedEndDate && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                            var filteredDetail = filteredDetails.ToList()[allDetailIndex++];
                            if (shiftId != "All")
                            {
                                var detailIndex = 0;
                                //Filter Completed Details by ShiftId and Month from UI, and BeamNumber from BeamRepository
                                filteredDetails = document.SizingHistories.Where(d => d.ShiftDocumentId.ToString().Equals(shiftId) && d.DateTimeMachine.DateTime >= convertedStartDate && d.DateTimeMachine.DateTime <= convertedEndDate && d.SizingBeamNumber.Equals(beamNumber) && d.MachineStatus.Equals(MachineStatus.ONCOMPLETE));
                                filteredDetail = filteredDetails.ToList()[detailIndex++];
                            }

                            //Get Operator Document from OperatorRepository, relate by filteredDetail.OperatorDocumentId
                            var operatorQuery = _operatorDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                            var operatorDocument = _operatorDocumentRepository.Find(operatorQuery).Where(o => o.Identity.Equals(filteredDetail.OperatorDocumentId)).FirstOrDefault();

                            var dateTime = sizingBeam.LatestDateTimeBeamProduct;
                            //var counter = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsCounterDto>(sizingBeam.Counter);
                            //var weight = JsonConvert.DeserializeObject<DailyOperationSizingBeamDocumentsWeightDto>(sizingBeam.Weight);
                            double pisMeter = sizingBeam.PISMeter ?? 0;
                            double spu = sizingBeam.SPU ?? 0;

                            switch (filteredConstructionNumber)
                            {
                                case SizePickupSPUConstants.PCCONSTRUCTION:
                                    Filtering filteringPC = new Filtering();
                                    var resultPC = filteringPC.ComparingPCCVC(spu);
                                    if (resultPC == spuStatus || spuStatus.Equals("All"))
                                    {
                                        //Placing Value as Parameter for SizePickupListDto and add to resultData
                                        results = new SizePickupListDto(document,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                                        results = new SizePickupListDto(document,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                                        results = new SizePickupListDto(document,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                                        results = new SizePickupListDto(document,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
                                        results = new SizePickupListDto(document,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
                                                                        pisMeter,
                                                                        spu,
                                                                        beamNumber);
                                        resultData.Add(results);
                                    }
                                    break;
                                default:
                                    //Placing Value as Parameter for SizePickupListDto and add to resultData
                                    results = new SizePickupListDto(document,
                                                                        operatorDocument.CoreAccount.Name,
                                                                        operatorDocument.Group,
                                                                        dateTime,
                                                                        sizingBeam.CounterStart ?? 0,
                                                                        sizingBeam.CounterFinish ?? 0,
                                                                        sizingBeam.WeightNetto ?? 0,
                                                                        sizingBeam.WeightBruto ?? 0,
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
