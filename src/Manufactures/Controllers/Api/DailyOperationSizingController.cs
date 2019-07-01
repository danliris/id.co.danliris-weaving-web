using Barebone.Controllers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Domain.Shifts.ValueObjects;
using Manufactures.Dtos;
using Manufactures.Dtos.Beams;
using Manufactures.Dtos.DailyOperations.Sizing;
using Manufactures.Helpers.XlsTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IMachineRepository
            _machineRepository;
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
                    .Find(domQuery.Include(d => d.Details));

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
                var dailyOperationEntryDateTime = dailyOperation.Details.OrderBy(e => e.DateTimeOperation).FirstOrDefault().DateTimeOperation;
                var lastDailyOperationStatus = dailyOperation.OperationStatus;

                foreach (var detail in dailyOperation.Details)
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
                    _dailyOperationSizingDocumentRepository.Find(query.Include(p => p.Details))
                                                .Where(o => o.Identity == Identity)
                                                .FirstOrDefault();

                var machineDocument =
                       _machineRepository
                           .Find(e => e.Identity.Equals(dailyOperationalSizing.MachineDocumentId.Value))
                           .FirstOrDefault();

                var machineNumber = machineDocument.MachineNumber;

                var constructionDocument =
                        _constructionDocumentRepository
                            .Find(e => e.Identity.Equals(dailyOperationalSizing.ConstructionDocumentId.Value))
                            .FirstOrDefault();

                var constructionNumber = constructionDocument.ConstructionNumber;

                var warpingBeams = new List<BeamDto>();

                foreach (var beam in dailyOperationalSizing.WarpingBeamsId)
                {
                    var beamDocument = _beamDocumentRepository.Find(b => b.Identity.Equals(beam.Value)).FirstOrDefault();

                    var beamsDto = new BeamDto(beamDocument);

                    warpingBeams.Add(beamsDto);
                }

                var dto = new DailyOperationSizingByIdDto(dailyOperationalSizing, machineNumber, constructionNumber, warpingBeams);

                foreach (var detail in dailyOperationalSizing.Details)
                {
                    var shiftDocument =
                        _shiftDocumentRepository
                            .Find(e => e.Identity.Equals(detail.ShiftDocumentId))
                            .FirstOrDefault();

                    var shiftName = shiftDocument.Name;

                    var history = new DailyOperationSizingHistoryDto(detail.DateTimeOperation, detail.MachineStatus, detail.Information);

                    var detailCauses = detail.Causes.Deserialize<DailyOperationSizingCausesDto>();

                    var causes = new DailyOperationSizingCausesDto(detailCauses.BrokenBeam, detailCauses.MachineTroubled);

                    var detailsDto = new DailyOperationSizingDetailsDto(shiftName, history, causes);

                    dto.Details.Add(detailsDto);
                }
                dto.Details = dto.Details.OrderBy(history => history.DateTimeOperationHistory).ToList();

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

        [HttpGet("size-pickup/date/{date}/unit-id/{weavingUnitId}/shift/{shiftId}")]
        public async Task<IActionResult> GetReportByDate(string date, int weavingUnitId, string shiftId)
        {
            try
            {
                int count = 0;

                var acceptRequest = Request.Headers.Values.ToList();
                var index = acceptRequest.IndexOf("application/xls") > 0;

                var resultData = new List<SizePickupListDto>();
                var query =
                    _dailyOperationSizingDocumentRepository
                        .Query.Include(o => o.Details).OrderByDescending(item => item.CreatedDate);
                var filteredSizePickupDtos =
                    _dailyOperationSizingDocumentRepository
                        .Find(query).Where(sizePickup => sizePickup.WeavingUnitId.Value.Equals(weavingUnitId) && sizePickup.OperationStatus.Equals(DailyOperationMachineStatus.ONFINISH)).ToList();

                foreach (var data in filteredSizePickupDtos)
                {
                    var convertedDate = DateTime.Parse(date);

                    var filteredDetails = data.Details.Where(x => x.DateTimeOperation.DateTime.Equals(convertedDate) &&
                                                                  x.ShiftDocumentId.ToString().Equals(shiftId) &&
                                                                  x.MachineStatus.Equals(DailyOperationMachineStatus.ONCOMPLETE)).FirstOrDefault();

                    if (filteredDetails != null)
                    {
                        var machineStatus = filteredDetails.MachineStatus;
                        var machineDateTime = filteredDetails.DateTimeOperation;

                        var beamQuery = _beamDocumentRepository.Query.OrderByDescending(b => b.CreatedDate);
                        var filteredBeam = _beamDocumentRepository.Find(beamQuery).Where(beam => beam.Identity.Equals(data.SizingBeamDocumentId.Value));
                        var filteredBeamNumber = " ";

                        foreach (var beam in filteredBeam)
                        {
                            filteredBeamNumber = beam.Number;
                        }

                        var operatorQuery = _operatorDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                        var operatorDetail = _operatorDocumentRepository.Find(operatorQuery).Where(detail => detail.Identity.Equals(filteredDetails.OperatorDocumentId));
                        var operatorName = " ";
                        var operatorGroup = " ";
                        foreach (var item in operatorDetail)
                        {
                            operatorName = item.CoreAccount.Name;
                            operatorGroup = item.Group;
                        }

                        var results = new SizePickupListDto(data, machineDateTime, operatorName, operatorGroup, filteredBeamNumber);
                        resultData.Add(results);
                        resultData = resultData.OrderBy(o => o.DateTimeMachineHistory).ToList();
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

        [HttpGet("size-pickup/daterange/start-date/{startDate}/end-date/{endDate}/unit-id/{weavingUnitId}/shift/{shiftId}")]
        public async Task<IActionResult> GetReportByDateRange(string startDate, string endDate, int weavingUnitId, string shiftId)
        {
            try
            {
                int count = 0;

                var acceptRequest = Request.Headers.Values.ToList();
                var index = acceptRequest.IndexOf("application/xls") > 0;

                var resultData = new List<SizePickupListDto>();
                var query =
                    _dailyOperationSizingDocumentRepository
                        .Query.Include(o => o.Details).OrderByDescending(item => item.CreatedDate);
                var filteredSizePickupDtos =
                    _dailyOperationSizingDocumentRepository
                        .Find(query).Where(sizePickup => sizePickup.WeavingUnitId.Value.Equals(weavingUnitId) && sizePickup.OperationStatus.Equals(DailyOperationMachineStatus.ONFINISH)).ToList();

                foreach (var data in filteredSizePickupDtos)
                {
                    var convertedStartDate = DateTime.Parse(startDate);
                    var convertedEndDate = DateTime.Parse(endDate);

                    var filteredDetails = data.Details.Where(x => (x.DateTimeOperation.DateTime >= convertedStartDate &&
                                                                   x.DateTimeOperation.DateTime <= convertedEndDate) &&
                                                                   x.ShiftDocumentId.ToString().Equals(shiftId) &&
                                                                   x.MachineStatus.Equals(DailyOperationMachineStatus.ONCOMPLETE)).FirstOrDefault();

                    if (filteredDetails != null)
                    {
                        var machineStatus = filteredDetails.MachineStatus;
                        var machineDateTime = filteredDetails.DateTimeOperation;

                        var beamQuery = _beamDocumentRepository.Query.OrderByDescending(b => b.CreatedDate);
                        var filteredBeam = _beamDocumentRepository.Find(beamQuery).Where(beam => beam.Identity.Equals(data.SizingBeamDocumentId.Value));
                        var filteredBeamNumber = " ";

                        foreach (var beam in filteredBeam)
                        {
                            filteredBeamNumber = beam.Number;
                        }

                        var operatorQuery = _operatorDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                        var operatorDetail = _operatorDocumentRepository.Find(operatorQuery).Where(detail => detail.Identity.Equals(filteredDetails.OperatorDocumentId));
                        var operatorName = " ";
                        var operatorGroup = " ";
                        foreach (var item in operatorDetail)
                        {
                            operatorName = item.CoreAccount.Name;
                            operatorGroup = item.Group;
                        }

                        var results = new SizePickupListDto(data, machineDateTime, operatorName, operatorGroup, filteredBeamNumber);
                        resultData.Add(results);
                        resultData = resultData.OrderBy(o => o.DateTimeMachineHistory).ToList();
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

        [HttpGet("size-pickup/month/{month}/unit-id/{weavingUnitId}/shift/{shiftId}")]
        public async Task<IActionResult> GetReportByMonth(int month, int weavingUnitId, string shiftId)

        {
            try
            {
                int count = 0;

                var acceptRequest = Request.Headers.Values.ToList();
                var index = acceptRequest.IndexOf("application/xls") > 0;

                var resultData = new List<SizePickupListDto>();
                var query =
                    _dailyOperationSizingDocumentRepository
                        .Query.Include(o => o.Details).OrderByDescending(item => item.CreatedDate);
                var filteredSizePickupDtos =
                    _dailyOperationSizingDocumentRepository
                        .Find(query).Where(sizePickup => sizePickup.WeavingUnitId.Value.Equals(weavingUnitId) && sizePickup.OperationStatus.Equals(DailyOperationMachineStatus.ONFINISH)).ToList();

                foreach (var data in filteredSizePickupDtos)
                {
                    var filteredDetails = data.Details.FirstOrDefault(x => x.ShiftDocumentId.ToString().Equals(shiftId) && x.DateTimeOperation.Month.Equals(month) && x.MachineStatus.Equals(DailyOperationMachineStatus.ONCOMPLETE));
                    var machineStatus = filteredDetails.MachineStatus;
                    var machineDateTime = filteredDetails.DateTimeOperation;

                    var beamQuery = _beamDocumentRepository.Query.OrderByDescending(b => b.CreatedDate);
                    var filteredBeam = _beamDocumentRepository.Find(beamQuery).Where(beam => beam.Identity.Equals(data.SizingBeamDocumentId.Value));
                    var filteredBeamNumber = " ";

                    foreach (var beam in filteredBeam)
                    {
                        filteredBeamNumber = beam.Number;
                    }

                    var operatorQuery = _operatorDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
                    var operatorDetail = _operatorDocumentRepository.Find(operatorQuery).Where(detail => detail.Identity.Equals(filteredDetails.OperatorDocumentId));
                    var operatorName = " ";
                    var operatorGroup = " ";
                    foreach (var item in operatorDetail)
                    {
                        operatorName = item.CoreAccount.Name;
                        operatorGroup = item.Group;
                    }

                    if (filteredDetails != null)
                    {
                        var results = new SizePickupListDto(data, machineDateTime, operatorName, operatorGroup, filteredBeamNumber);
                        resultData.Add(results);
                        resultData = resultData.OrderBy(o => o.DateTimeMachineHistory).ToList();
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
    }
}
