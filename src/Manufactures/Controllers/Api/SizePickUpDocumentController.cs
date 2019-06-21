using Barebone.Controllers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Dtos;
using Manufactures.Helpers.XlsTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
//    [Produces("application/json")]
//    [Route("weaving/size-pickup")]
//    [ApiController]
//    [Authorize]
//    public class SizePickUpDocumentController : ControllerApiBase
//    {
//        private readonly IDailyOperationSizingRepository
//               _dailyOperationSizingDocumentRepository;
//        private readonly IMachineRepository
//            _machineRepository;
//        private readonly IFabricConstructionRepository
//            _constructionDocumentRepository;
//        private readonly IShiftRepository
//            _shiftDocumentRepository;
//        private readonly IBeamRepository
//            _beamDocumentRepository;

//        public SizePickUpDocumentController(IServiceProvider serviceProvider,
//                                                 IWorkContext workContext)
//            : base(serviceProvider)
//        {
//            _dailyOperationSizingDocumentRepository =
//                this.Storage.GetRepository<IDailyOperationSizingRepository>();
//            _machineRepository =
//                this.Storage.GetRepository<IMachineRepository>();
//            _constructionDocumentRepository =
//                this.Storage.GetRepository<IFabricConstructionRepository>();
//            _shiftDocumentRepository =
//                this.Storage.GetRepository<IShiftRepository>();
//            _beamDocumentRepository =
//                this.Storage.GetRepository<IBeamRepository>();
//        }

//        [HttpGet("size-pickup/{periodType}/start-date/{startDate}/end-date/{endDate}/unit-id/{unitId}/shift/{shift}")]
//        public async Task<IActionResult> GetReportByDateRange(SizePickupPeriodStatus periodType, DateTimeOffset startDate, DateTimeOffset endDate, UnitId weavingUnitId, ShiftId shiftId)
//        {
//            try
//            {
//                int totalCount = 0;

//                var acceptRequest = Request.Headers.Values.ToList();
//                var index = acceptRequest.IndexOf("application/pdf") > 0;

//                var resultData = new List<SizePickupListDto>();
//                var query =
//                    _dailyOperationSizingDocumentRepository
//                        .Query.OrderByDescending(item => item.CreatedDate);
//                var sizePickupDtos =
//                    _dailyOperationSizingDocumentRepository
//                        .Find(query);

//                foreach (var sizePickupDto in sizePickupDtos)
//                {
//                    foreach(var detail in sizePickupDto.Details)
//                    {
//                        var dateDto = detail.DateTimeOperation;
                        
//                        if (dateDto>=startDate&&dateDto>=endDate)
//                        {
//                            var requestedDto = new SizePickupListDto(sizePickupDto, detail);
//                            resultData.Add(requestedDto);
//                        }
//                    }
//                }

//                //var results = SizePickupReportXlsTemplate.GetDataByDateRange(startDate, endDate, weavingUnitId, shiftId);
//                //data = results;
//                //totalCount = results.Count;

//                await Task.Yield();
//                return Ok(resultData, info: new
//                {
//                    count = totalCount
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }

//        [HttpGet("size-pickup/{periodType}/month/{month}/unit-id/{unitId}/shift/{shift}")]
//        public async Task<IActionResult> GetReportByMonth(DateTimeOffset month, UnitId weavingUnitId, ShiftId shiftId)
//        {
//            try
//            {
//                int totalCount = 0;

//                var acceptRequest = Request.Headers.Values.ToList();
//                var index = acceptRequest.IndexOf("application/pdf") > 0;

//                var resultData = new List<SizePickupListDto>();
//                var query =
//                    _dailyOperationSizingDocumentRepository
//                        .Query.OrderByDescending(item => item.CreatedDate);
//                var sizePickupDtos =
//                    _dailyOperationSizingDocumentRepository
//                        .Find(query);

//                foreach (var sizePickupDto in sizePickupDtos)
//                {
//                    foreach (var detail in sizePickupDto.Details)
//                    {
//                        var dateDto = detail.DateTimeOperation;

//                        if (dateDto == month)
//                        {
//                            var requestedDto = new SizePickupListDto(sizePickupDto, detail);
//                            resultData.Add(requestedDto);
//                        }
//                    }
//                }

//                //var results = SizePickupReportXlsTemplate.GetDataByMonth(periodType, month, weavingUnitId, shiftId);
//                //data = results;
//                //totalCount = results.Count;

//                await Task.Yield();
//                return Ok(resultData, info: new
//                {
//                    count = totalCount
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }
//    }
}
