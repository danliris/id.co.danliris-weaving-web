using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Spu.DataTransferObjects;
//using Manufactures.Application.DailyOperations.Spu.DataTransferObjects.DailyOperationSpuReport;
using Manufactures.Application.Helpers;
using Manufactures.Application.Operators.DataTransferObjects;
using Manufactures.Application.Shifts.DTOs;
using Manufactures.Domain.Beams.Queries;
using Manufactures.Domain.Beams.Repositories;
//using Manufactures.Domain.DailyOperations.Spu.Commands;
using Manufactures.Domain.DailyOperations.Spu.Queries;
//using Manufactures.Domain.DailyOperations.Spu.Queries.DailyOperationSpuReport;
//using Manufactures.Domain.DailyOperations.Spu.Repositories;
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
//using Manufactures.Domain.DailyOperations.Spu.Queries.SpuProductionReport;
//using Manufactures.Application.DailyOperations.Spu.DataTransferObjects.SpuProductionReport;
using Manufactures.Helpers.PdfTemplates;
using System.Globalization;
//using Manufactures.Domain.DailyOperations.Spu.Queries.SpuBrokenThreadsReport;
//using Manufactures.Application.DailyOperations.Spu.DataTransferObjects.SpuBrokenThreadsReport;
using Manufactures.Domain.DailyOperations.Spu.Queries.WeavingDailyOperationSpuMachines;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using CsvHelper;
using System.Net;

namespace Manufactures.Controllers.Api
{
    /**
     * This is Warping daily operation controller
     * 
     * **/
    [Produces("application/json")]
    [Route("weaving/daily-operations-spu")]
    [ApiController]
    [Authorize]
    public class DailyOperationSpuController : ControllerApiBase
    {
        //private readonly IDailyOperationSpuDocumentQuery<DailyOperationSpuListDto>
          //  _warpingDocumentQuery;
        private readonly IOperatorQuery<OperatorListDto>
            _operatorQuery;
        private readonly IShiftQuery<ShiftDto>
            _shiftQuery;
        private readonly IBeamQuery<BeamListDto>
            _beamQuery;
        //private readonly IDailyOperationSpuReportQuery<DailyOperationSpuReportListDto>
        //    _dailyOperationWarpingReportQuery;
       // private readonly ISpuProductionReportQuery<SpuProductionReportListDto>
       //     _warpingProductionReportQuery;
        //private readonly ISpuBrokenThreadsReportQuery<SpuBrokenThreadsReportListDto>
          //  _warpingBrokenReportQuery;
        private readonly IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto>
          _weavingDailyOperationSpuMachineQuery;



       // private readonly IDailyOperationSpuRepository
        //    _dailyOperationSpuRepository;
        private readonly IBeamRepository
            _beamRepository;
        //private readonly IDailyOperationSpuHistoryRepository
         //   _dailyOperationSpuHistoryRepository;
        //private readonly IDailyOperationSpuBeamProductRepository
         //   _dailyOperationSpuBeamProductRepository;
        //private readonly IDailyOperationSpuBrokenCauseRepository
         //   _dailyOperationSpuBrokenCauseRepository;


        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationSpuController(IServiceProvider serviceProvider,
                                              // IDailyOperationSpuDocumentQuery<DailyOperationSpuListDto> spuQuery,
                                               IOperatorQuery<OperatorListDto> operatorQuery,
                                               IShiftQuery<ShiftDto> shiftQuery,
                                               IBeamQuery<BeamListDto> beamQuery,
                                              // IDailyOperationSpuReportQuery<DailyOperationSpuReportListDto> dailyOperationSpuReportQuery,
                                              // ISpuProductionReportQuery<SpuProductionReportListDto> spuProductionReportQuery,
                                             //  ISpuBrokenThreadsReportQuery<SpuBrokenThreadsReportListDto> spuBrokenReportQuery,
                                               IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto> weavingDailyOperationSpuMachineQuery)
            : base(serviceProvider)
        {
            //_spuDocumentQuery = spuQuery ?? throw new ArgumentNullException(nameof(spuQuery));
            //_operatorQuery = operatorQuery ?? throw new ArgumentNullException(nameof(operatorQuery));
            //_shiftQuery = shiftQuery ?? throw new ArgumentNullException(nameof(shiftQuery));
            //_beamQuery = beamQuery ?? throw new ArgumentNullException(nameof(beamQuery));
            //_dailyOperationSpuReportQuery = dailyOperationSpuReportQuery ?? throw new ArgumentNullException(nameof(dailyOperationSpuReportQuery));
            //_spuProductionReportQuery = spuProductionReportQuery ?? throw new ArgumentNullException(nameof(spuProductionReportQuery));
            //_spuBrokenReportQuery = spuBrokenReportQuery ?? throw new ArgumentNullException(nameof(spuBrokenReportQuery));
            _weavingDailyOperationSpuMachineQuery = weavingDailyOperationSpuMachineQuery ?? throw new ArgumentNullException(nameof(weavingDailyOperationSpuMachineQuery));

           // _dailyOperationSpuRepository =
            //    this.Storage.GetRepository<IDailyOperationSpuRepository>();
            _beamRepository =
                this.Storage.GetRepository<IBeamRepository>();
           // _dailyOperationSpuHistoryRepository =
            //    this.Storage.GetRepository<IDailyOperationSpuHistoryRepository>();
            //_dailyOperationSpuBeamProductRepository =
             //   this.Storage.GetRepository<IDailyOperationSpuBeamProductRepository>();
            //_dailyOperationSpuBrokenCauseRepository =
              //  this.Storage.GetRepository<IDailyOperationSpuBrokenCauseRepository>();



        }





      



        [HttpGet("get-spu-daily-operation-report")]
        public async Task<IActionResult> GetSpuDailyOperationReport(DateTime fromDate, DateTime toDate, string shift, string machineSizing, string groupui, string name, string code)
        {
            VerifyUser();
            var acceptRequest = Request.Headers.Values.ToList();
            var productionSpuReport = _weavingDailyOperationSpuMachineQuery.GetDailyReports(fromDate, toDate, shift, machineSizing, groupui, name, code);

            return Ok(productionSpuReport);
        }

        //[HttpGet("get-spu-production-report/download")]
        //public async Task<IActionResult> GetWarpingProductionExcel(DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string threadNo, string code)
        //{

        //    try
        //    {
        //        VerifyUser();
        //        var acceptRequest = Request.Headers.Values.ToList();

        //        var productionWarpingReport = _weavingDailyOperationWarpingMachineQuery.GetReports(fromDate, toDate, shift, mcNo, sp, threadNo, code);


        //        byte[] xlsInBytes;


        //        var fileName = "Laporan Produksi Warping per Operator" + fromDate.ToShortDateString() + "_" + toDate.ToShortDateString();
        //        WeavingDailyOperationWarpingMachineReportXlsTemplate xlsTemplate = new WeavingDailyOperationWarpingMachineReportXlsTemplate();
        //        MemoryStream xls = xlsTemplate.GenerateXls(productionWarpingReport, fromDate, toDate, shift, mcNo, sp, threadNo, code);

        //        fileName += ".xlsx";

        //        xlsInBytes = xls.ToArray();
        //        var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //        return file;
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        //    }

        //}



    }
}
