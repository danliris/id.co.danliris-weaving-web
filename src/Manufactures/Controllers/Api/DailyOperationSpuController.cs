using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Spu.DataTransferObjects;


using Manufactures.Helpers.XlsTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;

using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Manufactures.Domain.DailyOperations.Spu.Queries.WeavingDailyOperationSpuMachines;

using System.Net;

namespace Manufactures.Controllers.Api
{
    
    [Produces("application/json")]
    [Route("weaving/daily-operations-spu")]
    [ApiController]
    [Authorize]
    public class DailyOperationSpuController : ControllerApiBase
    {
 
       
   
        private readonly IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto>
          _weavingDailyOperationSpuMachineQuery;

    
        
        public DailyOperationSpuController(IServiceProvider serviceProvider,
                                       
                                               IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto> weavingDailyOperationSpuMachineQuery)
            : base(serviceProvider)
        {

            _weavingDailyOperationSpuMachineQuery = weavingDailyOperationSpuMachineQuery ?? throw new ArgumentNullException(nameof(weavingDailyOperationSpuMachineQuery));

        }





      


        //SPU - Listview
        [HttpGet("get-spu-daily-operation-report")]
        public async Task<IActionResult> GetSpuDailyOperationReport(DateTime fromDate, DateTime toDate, string shift, string machineSizing, string groupui, string name, string code,int page, int size)
        {
            VerifyUser();
            var acceptRequest = Request.Headers.Values.ToList();
            var productionSpuReport = _weavingDailyOperationSpuMachineQuery.GetDailyReports(fromDate, toDate, shift, machineSizing, groupui, name, code);

            //ini utk paging
            var total = productionSpuReport.Count();
            var result = productionSpuReport.Skip((page - 1) * size).Take(size);
            return Ok(result, info: new { page, size, total });
            //----------------------

            //ini awalnya tnp paging
            //return Ok(productionSpuReport);
        }

        //SPU - Download
        [HttpGet("get-spu-daily-operation-report/download")]
        public async Task<IActionResult> GetWarpingDailyOperationReportExcel(DateTime fromDate, DateTime toDate, string shift, string machineSizing, string groupui, string name, string code)
        {

            try
            {
                VerifyUser();
                var acceptRequest = Request.Headers.Values.ToList();

                var productionWarpingReport = _weavingDailyOperationSpuMachineQuery.GetDailyReports(fromDate, toDate, shift, machineSizing, groupui, name, code);


                byte[] xlsInBytes;


                var fileName = "Laporan SPU" + fromDate.ToShortDateString() + "_" + toDate.ToShortDateString();
                WeavingDailyOperationSpuReportXlsTemplate xlsTemplate = new WeavingDailyOperationSpuReportXlsTemplate();
                MemoryStream xls = xlsTemplate.GenerateDailyOperationSpuReportXls(productionWarpingReport, fromDate, toDate, shift, machineSizing, groupui, name, code);

                fileName += ".xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }


        //Laporan Harian Sizing - Listview
        [HttpGet("get-sizing-daily-operation-report")]
        public async Task<IActionResult> GetSizingDailyOperationReport(DateTime fromDate, DateTime toDate, string shift, string machineSizing, string groupui, string name, string code,string sp, int page, int size)
        {
            VerifyUser();
            var acceptRequest = Request.Headers.Values.ToList();
            var productionSpuReport = _weavingDailyOperationSpuMachineQuery.GetDailySizingReports(fromDate, toDate, shift, machineSizing, groupui, name, code,sp);

            //ini utk paging
            var total = productionSpuReport.Count();
            var result = productionSpuReport.Skip((page - 1) * size).Take(size);
            return Ok(result, info: new { page, size, total });
            //----------------------


            //ini awalnya tnp paging
            //return Ok(productionSpuReport);
        }


        //Laporan Harian Sizing - Download
        [HttpGet("get-sizing-daily-operation-report/download")]
        public async Task<IActionResult> GetSizingDailyOperationReportExcel(DateTime fromDate, DateTime toDate, string shift, string machineSizing, string groupui, string name, string code, string sp)
        {

            try
            {
                VerifyUser();
                var acceptRequest = Request.Headers.Values.ToList();

                var productionWarpingReport = _weavingDailyOperationSpuMachineQuery.GetDailySizingReports(fromDate, toDate, shift, machineSizing, groupui, name, code, sp);


                byte[] xlsInBytes;


                var fileName = "LAPORAN OPERASIONAL HARIAN SIZING" + fromDate.ToShortDateString() + "_" + toDate.ToShortDateString();
                WeavingDailyOperationSizingReportXlsTemplate xlsTemplate = new WeavingDailyOperationSizingReportXlsTemplate();
                MemoryStream xls = xlsTemplate.GenerateDailyOperationSizingReportXls(productionWarpingReport, fromDate, toDate, shift, machineSizing, groupui, sp ,code);

                fileName += ".xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

    }
}
