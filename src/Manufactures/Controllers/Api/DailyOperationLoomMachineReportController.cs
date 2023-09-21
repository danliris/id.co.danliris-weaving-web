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

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            VerifyUser();
            var dailyOperationLooms = await _reachingQuery.GetAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                dailyOperationLooms =
                   dailyOperationLooms
                       .Where(x => x.CreatedDate.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                   x.MonthPeriode.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                   x.YearPeriode.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)); //||

            }
            var total = dailyOperationLooms.Count();
            var result = dailyOperationLooms.Skip((page - 1) * size).Take(size);

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("monthYear")]
        public async Task<IActionResult> GetByMonthYear(int page = 1, int size = 100, int monthId=0, string year="")
        {
            var weavingDailyOperations = await _reachingQuery.GetByMonthYear(monthId, year);

            var total = weavingDailyOperations.Count();
            var result = weavingDailyOperations.Skip((page - 1) * size).Take(size);
            return Ok(result, info: new { page, size, total });
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

        //Loom - Listview mencontoh dr SPU
        [HttpGet("get-loom-daily-operation-report")]
        public async Task<IActionResult> GetLoomDailyOperationReport(DateTime fromDate, DateTime toDate, string jenisMesin, string namaBlok, string namaMtc, string operatornya, string shift, string sp, int page, int size)
        {

            VerifyUser();
            var acceptRequest = Request.Headers.Values.ToList();
            var weavingDailyOperations =  _reachingQuery.GetDailyReports(fromDate,toDate, jenisMesin,  namaBlok,  namaMtc, operatornya, shift, sp);

           // var total1 = weavingDailyOperations.Count<DailyOperationLoomListDto>(fromDate);
            var total = weavingDailyOperations.Count();
            var result = weavingDailyOperations.Skip((page - 1) * size).Take(size);
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
