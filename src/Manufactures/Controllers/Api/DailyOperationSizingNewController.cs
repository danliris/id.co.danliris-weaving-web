using Barebone.Controllers;

using Manufactures.Application.DailyOperations.Production.DataTransferObjects;

using Manufactures.Domain.DailyOperations.Productions.Commands;

using Manufactures.Domain.DailyOperations.Productions.Queries;

using Manufactures.Domain.DailyOperations.WeavingDailyOperationMachineSizing.Queries;


using Manufactures.Helpers.XlsTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using OfficeOpenXml;
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
    [Route("weaving/daily-operation-sizing-machine")]
    [ApiController]
    [Authorize]
    public class DailyOperationSizingNewController : ControllerApiBase
    {


        private readonly IDailyOperationMachineSizingDocumentQuery<DailyOperationMachineSizingListDto> _dailyOperationMachineSizingDocumentQuery;
       
        private readonly IWeavingDailyOperationMachineSizingQuery<WeavingDailyOperationMachineSizingDto> _weavingDailyOperationMachineSizingQuery;
        public DailyOperationSizingNewController(IServiceProvider serviceProvider,

                                    IDailyOperationMachineSizingDocumentQuery<DailyOperationMachineSizingListDto> estimatedProductionDocumentQuery,
                                    IWeavingDailyOperationMachineSizingQuery<WeavingDailyOperationMachineSizingDto> weavingEstimatedProductionQuery
                                    ) : base(serviceProvider)
        {
           
            _dailyOperationMachineSizingDocumentQuery = estimatedProductionDocumentQuery ?? throw new ArgumentNullException(nameof(estimatedProductionDocumentQuery));
            _weavingDailyOperationMachineSizingQuery = weavingEstimatedProductionQuery ?? throw new ArgumentNullException(nameof(weavingEstimatedProductionQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, 
                                             int size = 25, 
                                             string order = "{}", 
                                             string keyword = null, 
                                             string filter = "{}")
        {
            var estimatedProductionDocuments = await _dailyOperationMachineSizingDocumentQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                estimatedProductionDocuments = 
                    estimatedProductionDocuments
                        .Where(o => o.EstimatedNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    o.DateEstimated.ToString("DD MMMM YYYY", CultureInfo.CreateSpecificCulture("id")).Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = 
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() + 
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(DailyOperationMachineSizingListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    estimatedProductionDocuments =
                        estimatedProductionDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    estimatedProductionDocuments =
                        estimatedProductionDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            var result = estimatedProductionDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var Identity = Guid.Parse(Id);
            var estimatedProductionDocument = await _dailyOperationMachineSizingDocumentQuery.GetById(Identity);

            await Task.Yield();
            if (Identity == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(estimatedProductionDocument);
            }
        }

        [HttpGet("edit/{Id}")]
        public async Task<IActionResult> GetEdit(string Id)
        {
            var Identity = Guid.Parse(Id);
            var estimatedProductionDocument = await _dailyOperationMachineSizingDocumentQuery.GetByIdUpdate(Identity);

            await Task.Yield();
            if (Identity == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(estimatedProductionDocument);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddNewDailyOperationMachineSizingCommand command)
        {
            var newEstimationDocument = await Mediator.Send(command);

            return Ok(newEstimationDocument.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id, 
                                             [FromBody] UpdateDailyOperationMachineSizingCommand command)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }

            command.SetId(documentId);
            var updateEstimationDocument = await Mediator.Send(command);

            return Ok(updateEstimationDocument.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid documentId))
            {
                return NotFound();
            }

            var command = new RemoveDailyOperationMachineSizingCommand();
            command.SetId(documentId);

            var deletedEstimationDocument = await Mediator.Send(command);

            return Ok(deletedEstimationDocument.Identity);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(string month, string year, int monthId)
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


                        var weavingMachine = await _weavingDailyOperationMachineSizingQuery.Upload(sheet, month, year, monthId);
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
        [HttpGet("WeavingDailyOperationMachineSizing")]
        public async Task<IActionResult> GetWeavingDailyOperationMachineSizing(int page = 1,
                                           int size = 25,
                                           string order = "{}",
                                           string keyword = null,
                                           string filter = "{}")
        {
            VerifyUser();
            var weavingDailyOperationMachineSizing = await _weavingDailyOperationMachineSizingQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                weavingDailyOperationMachineSizing =
                   weavingDailyOperationMachineSizing
                       .Where(x => x.CreatedDate.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                   x.Month.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                   x.YearPeriode.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)); //||

            }
            
            var result = weavingDailyOperationMachineSizing.Select(y=> new 
            {
                Month = y.Month,
                YearPeriode = y.YearPeriode,
                CreatedDate = y.CreatedDate

            }).Distinct().Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }
        [HttpGet("WeavingDailyOperationMachineSizing/monthYear")]
        public async Task<IActionResult> GetWeavingDailyOperationMachineSizing(string month, string yearPeriode)
        {
            var weavingDailyOperations = _weavingDailyOperationMachineSizingQuery.GetDataByFilter(month, yearPeriode);
 
            return Ok(weavingDailyOperations);
        }

        [HttpGet("WeavingEstimated/download")]
        public async Task<IActionResult> GetWeavingEstimatedExcel(string month, string yearPeriode)
        {

            try
            {
                VerifyUser();
                var acceptRequest = Request.Headers.Values.ToList();

                var weavingDailyOperations = _weavingDailyOperationMachineSizingQuery.GetDataByFilter(month, yearPeriode);

                byte[] xlsInBytes;


                var fileName = "Laporan Estimasi Produksi" + month + "_" + yearPeriode;
                WeavingDailyOperationMachineSizingReportXlsTemplate xlsTemplate = new WeavingDailyOperationMachineSizingReportXlsTemplate();
                MemoryStream xls = xlsTemplate.GenerateXls(weavingDailyOperations,month, yearPeriode);
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
