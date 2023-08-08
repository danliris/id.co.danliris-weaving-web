using Barebone.Controllers;
using Manufactures.Application.BeamStockUpload.DataTransferObjects;
using Manufactures.Domain.BeamStockUpload.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/beam-stocks")]
    [ApiController]
    [Authorize]
    public class BeamStockUploadController : ControllerApiBase
    {
        private readonly IBeamStockQuery<BeamStockUploadDto> _reachingQuery;
        public BeamStockUploadController(IServiceProvider serviceProvider, IBeamStockQuery<BeamStockUploadDto> reachingQuery) : base(serviceProvider)
        {
            _reachingQuery = reachingQuery ?? throw new ArgumentNullException(nameof(reachingQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            VerifyUser();
            var dailyOperationReachings = await _reachingQuery.GetAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                dailyOperationReachings =
                   dailyOperationReachings
                       .Where(x => x.CreatedDate.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                   x.MonthPeriode.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                   x.YearPeriode.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)); //||

            }
            var total = dailyOperationReachings.Count();
            var result = dailyOperationReachings.Skip((page - 1) * size).Take(size);

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("monthYear")]
        public async Task<IActionResult> GetByMonthYear(int page = 1, int size = 100, int monthId = 0, string year = "", int datestart=0, int datefinish=0, string shift=null)
        {
            var weavingDailyOperations = await _reachingQuery.GetByMonthYear(monthId, year,datestart,datefinish,shift);

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
    
    }
}
