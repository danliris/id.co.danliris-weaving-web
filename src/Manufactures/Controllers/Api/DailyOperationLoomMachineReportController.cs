using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects;
using Manufactures.Domain.DailyOperations.Loom.Queries;
using Manufactures.Helpers.XlsTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/daily-operations-loom-machine-report")]
    [ApiController]
    [Authorize]
    public class DailyOperationLoomMachineReportController: ControllerApiBase
    {
        private readonly IDailyOperationLoomMachineQuery<DailyOperationLoomMachineDto> _reachingQuery;
        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationLoomMachineReportController(IServiceProvider serviceProvider, IDailyOperationLoomMachineQuery<DailyOperationLoomMachineDto> reachingQuery) : base(serviceProvider)
        {
            _reachingQuery = reachingQuery ?? throw new ArgumentNullException(nameof(reachingQuery));
            //_reachingQuery = this.Storage.GetRepository<IDailyOperationLoomMachineRepository>();
        }

       
        //Loom - Listview mencontoh dr SPU
        [HttpGet("get-loom-daily-operation-report")]
        public async Task<IActionResult> GetLoomDailyOperationReport(DateTime fromDate, DateTime toDate, string jenisMesin, string namaBlok, string namaMtc, string operatornya, string shift, string sp, int page, int size)
        {

            VerifyUser();
            var acceptRequest = Request.Headers.Values.ToList();
            var weavingDailyOperations =  _reachingQuery.GetDailyReports(fromDate,toDate, jenisMesin,  namaBlok,  namaMtc, operatornya, shift, sp);

           // var total1 = weavingDailyOperations.Count<DailyOperationLoomListDto>(fromDate);
            var total = weavingDailyOperations == null? 0 : weavingDailyOperations.Count();

            var result = weavingDailyOperations == null ? weavingDailyOperations.AsEnumerable() : weavingDailyOperations.Skip((page - 1) * size).Take(size);

            return Ok(result, info: new { page, size, total });

        }

        //Loom - download mencontoh dr SPU
        [HttpGet("get-loom-daily-operation-report/download")]
        public async Task<IActionResult> GetWarpingDailyOperationReportExcel(DateTime fromDate, DateTime toDate, string jenisMesin, string namaBlok, string namaMtc, string operatornya, string shift, string sp, int page, int size)
        {

            try
            {
                VerifyUser();
                var acceptRequest = Request.Headers.Values.ToList();

                var weavingDailyOperations = _reachingQuery.GetDailyReports(fromDate, toDate, jenisMesin, namaBlok, namaMtc, operatornya, shift, sp);


                byte[] xlsInBytes;


                var fileName = "Laporan Loom" + fromDate.ToShortDateString() + " SD " + toDate.ToShortDateString();
                WeavingDailyOperationLoomReportXlsTemplate xlsTemplate = new WeavingDailyOperationLoomReportXlsTemplate();
                MemoryStream xls = xlsTemplate.GenerateDailyOperationLoomReportXls(weavingDailyOperations, fromDate, toDate, jenisMesin, namaBlok, namaMtc, operatornya, shift, sp);

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
