using Barebone.Controllers;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;
using System.Threading.Tasks;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects;
using Manufactures.Domain.DailyOperations.Loom.Queries;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Manufactures.Application.Helpers;
using Moonlay;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/daily-operations-loom")]
    [ApiController]
    [Authorize]
    public class DailyOperationLoomController : ControllerApiBase
    {
        private readonly IDailyOperationLoomQuery<DailyOperationLoomListDto> _loomQuery;

        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IMachineRepository
            _machineRepository;

        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationLoomController(IServiceProvider serviceProvider,
                                            IDailyOperationLoomQuery<DailyOperationLoomListDto> loomQuery) : base(serviceProvider)
        {
            _loomQuery = loomQuery ?? throw new ArgumentNullException(nameof(loomQuery));

            _dailyOperationLoomRepository =
                this.Storage.GetRepository<IDailyOperationLoomRepository>();
            _beamRepository =
                this.Storage.GetRepository<IBeamRepository>();
            _machineRepository =
                this.Storage.GetRepository<IMachineRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            var dailyOperationLoomDocuments = await _loomQuery.GetAll();

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

        [HttpGet("get-loom-beam-products")]
        public async Task<IActionResult> GetLoomBeamProducts(string keyword, string filter = "{}", int page = 1, int size = 25)
        {
            page = page - 1;
            List<DailyOperationLoomBeamProductDto> loomListBeamProducts = new List<DailyOperationLoomBeamProductDto>();
            if (!filter.Contains("{}"))
            {
                Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                var LoomOperationId = filterDictionary["LoomOperationId"].ToString();
                if (!LoomOperationId.Equals(null))
                {
                    var LoomOperationGuid = Guid.Parse(LoomOperationId);

                    await Task.Yield();
                    var loomQuery =
                         _dailyOperationLoomRepository
                             .Query
                             .Include(o => o.LoomBeamHistories)
                             .Include(o => o.LoomBeamProducts)
                             .OrderByDescending(o => o.CreatedDate);

                    await Task.Yield();
                    var existingDailyOperationLoomDocument =
                        _dailyOperationLoomRepository
                            .Find(loomQuery)
                            .Where(o => o.Identity.Equals(LoomOperationGuid))
                            .FirstOrDefault();

                    await Task.Yield();
                    foreach (var loomBeamProduct in existingDailyOperationLoomDocument.LoomBeamProducts)
                    {
                        await Task.Yield();
                        var sizingBeamStatus = loomBeamProduct.BeamProductStatus;
                        if (sizingBeamStatus.Equals(BeamStatus.ONPROCESS))
                        {
                            //Get Beam Number
                            await Task.Yield();
                            var beamQuery =
                                _beamRepository
                                    .Query
                                    .OrderByDescending(o => o.CreatedDate);
                            var beamNumber =
                                _beamRepository
                                    .Find(beamQuery)
                                    .Where(o => o.Identity.Equals(loomBeamProduct.BeamDocumentId))
                                    .FirstOrDefault()
                                    .Number;

                            //Get Machine Number
                            await Task.Yield();
                            var machineQuery =
                                _machineRepository
                                    .Query
                                    .OrderByDescending(o => o.CreatedDate);
                            var machineNumber =
                                _machineRepository
                                    .Find(machineQuery)
                                    .Where(o => o.Identity.Equals(loomBeamProduct.MachineDocumentId))
                                    .FirstOrDefault()
                                    .MachineNumber;

                            await Task.Yield();
                            var latestDateTimeBeamProduct = loomBeamProduct.LatestDateTimeBeamProduct;
                            var loomProcess = loomBeamProduct.LoomProcess;
                            var beamProductStatus = loomBeamProduct.BeamProductStatus;

                            var loomSizingBeam = new DailyOperationLoomBeamProductDto(beamNumber, machineNumber, latestDateTimeBeamProduct, loomProcess, beamProductStatus);
                            loomListBeamProducts.Add(loomSizingBeam);
                        }
                    }
                }
                else
                {
                    throw Validator.ErrorValidation(("Id", "Id Operasi Tidak Ditemukan"));
                }
            }
            else
            {
                throw Validator.ErrorValidation(("Id", "Id Operasi Tidak Ditemukan"));
            }

            var total = loomListBeamProducts.Count();
            var data = loomListBeamProducts.Skip((page - 1) * size).Take(size);

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
            var identity = Guid.Parse(Id);
            var dailyOperationLoomDocument = await _loomQuery.GetById(identity);

            if (dailyOperationLoomDocument == null)
            {
                return NotFound(identity);
            }

            return Ok(dailyOperationLoomDocument);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PreparationDailyOperationLoomCommand command)
        {
            var preparationDailyOperationLoomDocument = await Mediator.Send(command);

            return Ok(preparationDailyOperationLoomDocument.Identity);
        }

        [HttpPut("{Id}/start")]
        public async Task<IActionResult> Put(string Id,
                                            [FromBody]UpdateStartDailyOperationLoomCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateStartDailyOperationLoomDocument = await Mediator.Send(command);

            return Ok(updateStartDailyOperationLoomDocument.Identity);
        }

        [HttpPut("{Id}/pause")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdatePauseDailyOperationLoomCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updatePauseDailyOperationLoomDocument = await Mediator.Send(command);

            return Ok(updatePauseDailyOperationLoomDocument.Identity);
        }

        [HttpPut("{Id}/resume")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateResumeDailyOperationLoomCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateResumeDailyOperationLoomDocument = await Mediator.Send(command);

            return Ok(updateResumeDailyOperationLoomDocument.Identity);
        }

        [HttpPut("{Id}/finish")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]FinishDailyOperationLoomCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }
            command.SetId(documentId);
            var updateDoffDailyOperationLoomDocument = await Mediator.Send(command);

            return Ok(updateDoffDailyOperationLoomDocument.Identity);
        }
    }
}
