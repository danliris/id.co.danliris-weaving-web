using Barebone.Controllers;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects;
using Manufactures.Domain.DailyOperations.Reaching.Queries;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
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
    [Route("weaving/daily-operations-reaching-machine")]
    [ApiController]
    [Authorize]
    public class DailyOperationMachineReachingController : ControllerApiBase
    {
        private readonly IDailyOperationReachingMachineQuery<DailyOperationMachineReachingDto> _reachingQuery;
        //Dependency Injection activated from constructor need IServiceProvider
        public DailyOperationMachineReachingController(IServiceProvider serviceProvider, IDailyOperationReachingMachineQuery<DailyOperationMachineReachingDto> reachingQuery) : base(serviceProvider)
        {
            _reachingQuery = reachingQuery ?? throw new ArgumentNullException(nameof(reachingQuery));
            //_reachingQuery = this.Storage.GetRepository<IDailyOperationReachingMachineRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            VerifyUser();
            var dailyOperationReachings = await _reachingQuery.GetAll();

            var total = dailyOperationReachings.Count();
            var result = dailyOperationReachings.Skip((page - 1) * size).Take(size);

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("monthYear")]
        public async Task<IActionResult> GetByMonthYear(int monthId, string year)
        {
            var weavingDailyOperations = await _reachingQuery.GetByMonthYear(monthId, year);

            return Ok(weavingDailyOperations);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(string month, int year, int monthId)
        {
            VerifyUser();

            if (Request.Form.Files.Count > 0)
            {
                IFormFile UploadedFile = Request.Form.Files[0];
                if (System.IO.Path.GetExtension(UploadedFile.FileName) == ".xlsx")
                {

                    using (var excelPack = new ExcelPackage())
                    {
                        using (var stream = UploadedFile.OpenReadStream())
                        {
                            excelPack.Load(stream);
                        }
                        var sheet = excelPack.Workbook.Worksheets;


                        var weavingMachine = await _reachingQuery.Upload(sheet, month, year, monthId);
                        return Ok(weavingMachine);
                    }
                }
                else
                {
                    throw new Exception($"Ekstensi file harus bertipe .xlsx");
                }
            }
            else
            {
                throw new Exception($"Gagal menyimpan data");
            }
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetWarpingDailyOperationReport(DateTime fromDate, DateTime toDate, string shift, string mcNo)
        {
            VerifyUser();
            var acceptRequest = Request.Headers.Values.ToList();
            var productionWarpingReport = _reachingQuery.GetDailyReports(fromDate, toDate, shift, mcNo);

            return Ok(productionWarpingReport);
        }

        [HttpGet("report-download")]
        public async Task<IActionResult> GeReportExcel(DateTime fromDate, DateTime toDate, string shift, string mcNo)
        {

            try
            {
                VerifyUser();
                var acceptRequest = Request.Headers.Values.ToList();

                var reachingReport = _reachingQuery.GetDailyReports(fromDate, toDate, shift, mcNo);


                byte[] xlsInBytes;


                var fileName = "Laporan Operasional Harian Reaching" + fromDate.ToShortDateString() + "_" + toDate.ToShortDateString();
                ReachingDailyOperationReportXlsTemplate xlsTemplate = new ReachingDailyOperationReportXlsTemplate();
                MemoryStream xls = xlsTemplate.GenerateXls(reachingReport, fromDate, toDate, shift, mcNo);

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
