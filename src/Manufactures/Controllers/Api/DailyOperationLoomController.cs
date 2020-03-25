using Barebone.Controllers;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects;
using Manufactures.Domain.DailyOperations.Loom.Queries;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Manufactures.Application.Helpers;
using Moonlay;
using Manufactures.Domain.DailyOperations.Loom.Queries.DailyOperationLoomReport;
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects.DailyOperationLoomReport;
using Manufactures.Helpers.XlsTemplates;
using System.IO;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/daily-operations-loom")]
    [ApiController]
    [Authorize]
    public class DailyOperationLoomController : ControllerApiBase
    {
        private readonly IDailyOperationLoomQuery<DailyOperationLoomListDto>
            _loomDocumentQuery;
        private readonly IDailyOperationLoomBeamsUsedQuery<DailyOperationLoomBeamsUsedDto>
            _loomBeamsUsedQuery;
        private readonly IDailyOperationLoomReportQuery<DailyOperationLoomReportListDto> 
            _dailyOperationLoomReportQuery;
        //private readonly IDailyOperationLoomBeamProductQuery<DailyOperationLoomBeamProductDto>
        //    _loomBeamProductQuery;

        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IDailyOperationLoomHistoryRepository 
            _dailyOperationLoomHistoryRepository;
        private readonly IDailyOperationLoomBeamUsedRepository 
            _dailyOperationLoomUsedRepository;

        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationLoomController(IServiceProvider serviceProvider,
                                            IDailyOperationLoomQuery<DailyOperationLoomListDto> loomDocumentQuery,
                                            IDailyOperationLoomBeamsUsedQuery<DailyOperationLoomBeamsUsedDto> loomBeamsUsedQuery,
                                            IDailyOperationLoomReportQuery<DailyOperationLoomReportListDto> dailyOperationLoomReportQuery) : base(serviceProvider)
        {
            _loomDocumentQuery =
                loomDocumentQuery ?? throw new ArgumentNullException(nameof(loomDocumentQuery));
            _loomBeamsUsedQuery =
                loomBeamsUsedQuery ?? throw new ArgumentNullException(nameof(loomBeamsUsedQuery));
            _dailyOperationLoomReportQuery = 
                dailyOperationLoomReportQuery ?? throw new ArgumentNullException(nameof(dailyOperationLoomReportQuery));

            _dailyOperationLoomRepository =
                this.Storage.GetRepository<IDailyOperationLoomRepository>();
            _beamRepository =
                this.Storage.GetRepository<IBeamRepository>();
            _machineRepository =
                this.Storage.GetRepository<IMachineRepository>();
            _dailyOperationLoomHistoryRepository = 
                this.Storage.GetRepository<IDailyOperationLoomHistoryRepository>();
            _dailyOperationLoomUsedRepository =
                this.Storage.GetRepository<IDailyOperationLoomBeamUsedRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            VerifyUser();
            var dailyOperationLoomDocuments = await _loomDocumentQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                dailyOperationLoomDocuments =
                    dailyOperationLoomDocuments
                        .Where(x => x.FabricConstructionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.OrderProductionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(DailyOperationLoomListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    await Task.Yield();
                    dailyOperationLoomDocuments =
                        dailyOperationLoomDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    await Task.Yield();
                    dailyOperationLoomDocuments =
                        dailyOperationLoomDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            //int totalRows = dailyOperationLoomDocuments.Count();
            var result = dailyOperationLoomDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        //[HttpGet("get-loom-beam-products-by-order")]
        //public async Task<IActionResult> GetLoomBeamProducts(int page = 1,
        //                                                     int size = 25,
        //                                                     string order = "{}",
        //                                                     string keyword = null,
        //                                                     string filter = "{}")
        //{
        //    VerifyUser();
        //    var dailyOperationLoomBeamProducts = new List<DailyOperationLoomBeamProductDto>();

        //    //if (!string.IsNullOrEmpty(filter))
        //    //{
        //    //    var orderFilter = new { OrderId = "" };
        //    //    var serializedFilter = JsonConvert.DeserializeAnonymousType(filter, orderFilter);

        //    //    if (!Guid.TryParse(serializedFilter.OrderId, out Guid OrderGuid))
        //    //    {
        //    //        return NotFound();
        //    //    }

        //    //    dailyOperationLoomBeamProducts = await _loomBeamProductQuery.GetLoomBeamProductsByOrder(OrderGuid);
        //    //}

        //    if (!filter.Contains("{}"))
        //    {
        //        Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
        //        var LoomOperationId = filterDictionary["LoomOperationId"].ToString();

        //        if (!LoomOperationId.Equals(null))
        //        {
        //            var LoomOperationGuid = Guid.Parse(LoomOperationId);

        //            await Task.Yield();
        //            var existingDailyOperationLoomDocument =
        //                _dailyOperationLoomRepository
        //                    .Find(o => o.Identity == LoomOperationGuid)
        //                    .FirstOrDefault();

        //            var loomHistories = 
        //                _dailyOperationLoomHistoryRepository
        //                    .Find(o => o.DailyOperationLoomDocumentId == existingDailyOperationLoomDocument.Identity);
        //            var loomBeamProducts = 
        //                _dailyOperationLoomUsedRepository
        //                    .Find(s => s.DailyOperationLoomDocumentId == existingDailyOperationLoomDocument.Identity);

        //            await Task.Yield();
        //            foreach (var loomBeamProduct in loomBeamProducts)
        //            {
        //                await Task.Yield();
        //                var sizingBeamStatus = loomBeamProduct.BeamUsedStatus;
        //                if (sizingBeamStatus.Equals(BeamStatus.ONPROCESS))
        //                {
        //                    //Get Beam Number
        //                    await Task.Yield();
        //                    var beamQuery =
        //                        _beamRepository
        //                            .Query
        //                            .OrderByDescending(o => o.CreatedDate);
        //                    var beamNumber =
        //                        _beamRepository
        //                            .Find(beamQuery)
        //                            .Where(o => o.Identity.Equals(loomBeamProduct.BeamDocumentId))
        //                            .FirstOrDefault()
        //                            .Number;

        //                    //Get Machine Number
        //                    await Task.Yield();
        //                    var machineQuery =
        //                        _machineRepository
        //                            .Query
        //                            .OrderByDescending(o => o.CreatedDate);
        //                    var machineNumber =
        //                        _machineRepository
        //                            .Find(machineQuery)
        //                            .Where(o => o.Identity.Equals(loomBeamProduct.MachineDocumentId))
        //                            .FirstOrDefault()
        //                            .MachineNumber;

        //                    await Task.Yield();
        //                    var latestDateTimeBeamProduct = loomBeamProduct.LastDateTimeProcessed;
        //                    var loomProcess = loomBeamProduct.Process;
        //                    var beamProductStatus = loomBeamProduct.BeamUsedStatus;

        //                    var loomSizingBeam = new DailyOperationLoomBeamProductDto(loomBeamProduct.Identity,
        //                                                                              loomBeamProduct.BeamOrigin,
        //                                                                              beamNumber,
        //                                                                              loomBeamProduct.CombNumber,
        //                                                                              machineNumber, 
        //                                                                              latestDateTimeBeamProduct, 
        //                                                                              loomProcess, 
        //                                                                              beamProductStatus);
        //                    loomSizingBeam.SetBeamDocumentId(loomBeamProduct.BeamDocumentId.Value);

        //                    dailyOperationLoomBeamProducts.Add(loomSizingBeam);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            throw Validator.ErrorValidation(("Id", "Id Operasi Tidak Ditemukan"));
        //        }
        //    }
        //    else
        //    {
        //        throw Validator.ErrorValidation(("Id", "Id Operasi Tidak Ditemukan"));
        //    }

        //    var total = dailyOperationLoomBeamProducts.Count();
        //    var data = dailyOperationLoomBeamProducts.Skip((page - 1) * size).Take(size);

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
            try
            {
                VerifyUser();
                var identity = Guid.Parse(Id);
                var dailyOperationLoomDocument = await _loomDocumentQuery.GetById(identity);

                if (dailyOperationLoomDocument == null)
                {
                    return NotFound(identity);
                }

                return Ok(dailyOperationLoomDocument);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpGet("get-loom-beams-used")]
        public async Task<IActionResult> GetLoomBeamsUsed(int page = 1,
                                                          int size = 25,
                                                          string order = "{}",
                                                          string keyword = null,
                                                          string filter = "{}")
        {
            try
            {
                VerifyUser();
                var dailyOperationLoomBeamsUsed = await _loomBeamsUsedQuery.GetAllBeamsUsed();

                if (!filter.Contains("{}"))
                {
                    Dictionary<string, string> filterDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(filter);
                    var key = filterDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              filterDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(DailyOperationLoomBeamsUsedDto).GetProperty(key);

                    if (filterDictionary.Keys.Contains("LoomOperationId"))
                    {
                        if (!Guid.TryParse(filterDictionary["LoomOperationId"], out Guid operationId))
                        {
                            return NotFound();
                        }
                        dailyOperationLoomBeamsUsed =
                            dailyOperationLoomBeamsUsed
                                .Where(x => x.DailyOperationLoomDocumentId == operationId && x.BeamUsedStatus == BeamStatus.QUEUE);
                    }
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    await Task.Yield();
                    dailyOperationLoomBeamsUsed =
                        dailyOperationLoomBeamsUsed
                            .Where(x => x.BeamNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
                }

                if (!order.Contains("{}"))
                {
                    Dictionary<string, string> orderDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              orderDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(DailyOperationLoomBeamsUsedDto).GetProperty(key);

                    if (orderDictionary.Values.Contains("asc"))
                    {
                        await Task.Yield();
                        dailyOperationLoomBeamsUsed =
                            dailyOperationLoomBeamsUsed.OrderBy(x => prop.GetValue(x, null));
                    }
                    else
                    {
                        await Task.Yield();
                        dailyOperationLoomBeamsUsed =
                            dailyOperationLoomBeamsUsed.OrderByDescending(x => prop.GetValue(x, null));
                    }
                }

                var result = dailyOperationLoomBeamsUsed.Skip((page - 1) * size).Take(size);
                var total = result.Count();

                return Ok(result, info: new { page, size, total });
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpGet("get-loom-beams-used-processed")]
        public async Task<IActionResult> GetLoomBeamsUsedProcessed(int page = 1,
                                                                   int size = 25,
                                                                   string order = "{}",
                                                                   string keyword = null,
                                                                   string filter = "{}")
        {
            try
            {
                VerifyUser();
                var dailyOperationLoomBeamsUsed = await _loomBeamsUsedQuery.GetAllBeamsUsed();

                if (!filter.Contains("{}"))
                {
                    Dictionary<string, string> filterDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(filter);
                    var key = filterDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              filterDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(DailyOperationLoomBeamsUsedDto).GetProperty(key);

                    if (filterDictionary.Keys.Contains("LoomOperationId"))
                    {
                        if (!Guid.TryParse(filterDictionary["LoomOperationId"], out Guid operationId))
                        {
                            return NotFound();
                        }
                        dailyOperationLoomBeamsUsed =
                            dailyOperationLoomBeamsUsed
                                .Where(x => x.DailyOperationLoomDocumentId == operationId && (x.BeamUsedStatus == BeamStatus.ONPROCESS || x.BeamUsedStatus == BeamStatus.REPROCESS));
                    }
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    await Task.Yield();
                    dailyOperationLoomBeamsUsed =
                        dailyOperationLoomBeamsUsed
                            .Where(x => x.BeamNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
                }

                if (!order.Contains("{}"))
                {
                    Dictionary<string, string> orderDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              orderDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(DailyOperationLoomBeamsUsedDto).GetProperty(key);

                    if (orderDictionary.Values.Contains("asc"))
                    {
                        await Task.Yield();
                        dailyOperationLoomBeamsUsed =
                            dailyOperationLoomBeamsUsed.OrderBy(x => prop.GetValue(x, null));
                    }
                    else
                    {
                        await Task.Yield();
                        dailyOperationLoomBeamsUsed =
                            dailyOperationLoomBeamsUsed.OrderByDescending(x => prop.GetValue(x, null));
                    }
                }

                var result = dailyOperationLoomBeamsUsed.Skip((page - 1) * size).Take(size);
                var total = result.Count();

                return Ok(result, info: new { page, size, total });
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PreparationDailyOperationLoomCommand command)
        {
            VerifyUser();
            var preparationDailyOperationLoomDocument = await Mediator.Send(command);

            return Ok(preparationDailyOperationLoomDocument.Identity);
        }

        [HttpPut("{Id}/start")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]UpdateStartDailyOperationLoomCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var startDailyOperationLoomDocument = await Mediator.Send(command);

            return Ok(startDailyOperationLoomDocument.Identity);
        }

        [HttpPut("{Id}/reprocess")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]ReprocessDailyOperationLoomCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var reprocessDailyOperationLoomDocument = await Mediator.Send(command);

            return Ok(reprocessDailyOperationLoomDocument.Identity);
        }

        [HttpPut("{Id}/produce-greige")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]ProduceGreigeDailyOperationLoomCommand command)
        {
            VerifyUser();
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var produceGreigeDailyOperationLoomDocument = await Mediator.Send(command);

            return Ok(produceGreigeDailyOperationLoomDocument.Identity);
        }

        //[HttpPut("{Id}/pause")]
        //public async Task<IActionResult> Put(string Id,
        //                                     [FromBody]UpdatePauseDailyOperationLoomCommand command)
        //{
        //    VerifyUser();
        //    if (!Guid.TryParse(Id, out Guid documentId))
        //    {
        //        return NotFound();
        //    }
        //    command.SetId(documentId);
        //    var updatePauseDailyOperationLoomDocument = await Mediator.Send(command);

        //    return Ok(updatePauseDailyOperationLoomDocument.Identity);
        //}

        //[HttpPut("{Id}/resume")]
        //public async Task<IActionResult> Put(string Id,
        //                                     [FromBody]UpdateResumeDailyOperationLoomCommand command)
        //{
        //    VerifyUser();
        //    if (!Guid.TryParse(Id, out Guid documentId))
        //    {
        //        return NotFound();
        //    }
        //    command.SetId(documentId);
        //    var updateResumeDailyOperationLoomDocument = await Mediator.Send(command);

        //    return Ok(updateResumeDailyOperationLoomDocument.Identity);
        //}

        //[HttpPut("{Id}/finish")]
        //public async Task<IActionResult> Put(string Id,
        //                                     [FromBody]FinishDailyOperationLoomCommand command)
        //{
        //    VerifyUser();
        //    if (!Guid.TryParse(Id, out Guid documentId))
        //    {
        //        return NotFound();
        //    }
        //    command.SetId(documentId);
        //    var finishDailyOperationLoomDocument = await Mediator.Send(command);

        //    return Ok(finishDailyOperationLoomDocument.Identity);
        //}

        //Controller for Daily Operation Reaching Report
        [HttpGet("get-report")]
        public async Task<IActionResult> GetReport(string orderId,
                                                   string constructionId,
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

            var dailyOperationLoomReport = await _dailyOperationLoomReportQuery.GetReports(orderId,
                                                                                           constructionId,
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

                DailyOperationLoomReportXlsTemplate xlsTemplate = new DailyOperationLoomReportXlsTemplate();
                MemoryStream xls = xlsTemplate.GenerateDailyOperationLoomReportXls(dailyOperationLoomReport.Item1.ToList());
                xlsInBytes = xls.ToArray();
                var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Operasional Mesin Harian Reaching");
                return xlsFile;
            }
            else
            {
                return Ok(dailyOperationLoomReport.Item1, info: new
                {
                    count = dailyOperationLoomReport.Item2
                });
            }
        }
    }
}
