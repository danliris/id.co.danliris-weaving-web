using Barebone.Controllers;
using Manufactures.Application.TroubleMachineMonitoring.DTOs;
using Manufactures.Application.TroubleMachineMonitoring.Queries;
using Manufactures.Domain.TroubleMachineMonitoring.Commands;
using Manufactures.Domain.TroubleMachineMonitoring.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/trouble-machine-monitoring")]
    [ApiController]
    [Authorize]
    public class TroubleMachineMonitoringController : ControllerApiBase
    {
        private readonly ITroubleMachineMonitoringQuery _troubleMachineMonitoring;
        private readonly IWeavingTroubleMachineTreeLosesQuery<WeavingTroubleMachingTreeLosesDto> _losesQuery;

        public TroubleMachineMonitoringController(IServiceProvider serviceProvider,
                                             //IWorkContext workContext,
                                             ITroubleMachineMonitoringQuery TroubleMachineMonitoringQuery,
                                             IWeavingTroubleMachineTreeLosesQuery<WeavingTroubleMachingTreeLosesDto> losesQuery
                                             ) : base(serviceProvider)
        {
            _troubleMachineMonitoring = TroubleMachineMonitoringQuery ?? throw new ArgumentNullException(nameof(TroubleMachineMonitoringQuery));
            _losesQuery =  losesQuery?? throw new ArgumentNullException(nameof(IWeavingTroubleMachineTreeLosesQuery<WeavingTroubleMachingTreeLosesDto>));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            VerifyUser();
            var troubleMachineMonitoringDocuments = await _troubleMachineMonitoring.GetAll();


            if (!string.IsNullOrEmpty(keyword))
            {
                troubleMachineMonitoringDocuments =
                    troubleMachineMonitoringDocuments
                        .Where(t => t.OrderNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    t.ConstructionNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    t.Operator.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    t.Trouble.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        .ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(TroubleMachineMonitoringListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    troubleMachineMonitoringDocuments =
                        troubleMachineMonitoringDocuments.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    troubleMachineMonitoringDocuments =
                        troubleMachineMonitoringDocuments.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            var result = troubleMachineMonitoringDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var Identity = Guid.Parse(Id);

            var troubleMachineMonitoringDocument = await _troubleMachineMonitoring.GetById(Identity);

            await Task.Yield();

            if (troubleMachineMonitoringDocument == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(troubleMachineMonitoringDocument);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddTroubleMachineMonitoringCommand command)
        {
            var troubleMachine = await Mediator.Send(command);

            return Ok(troubleMachine.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateExistingTroubleMachineMonitoringCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var troubleMachine = await Mediator.Send(command);

            return Ok(troubleMachine.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveExistingTroubleMachineMonitoringCommand();
            command.SetId(Identity);

            var troubleMachine = await Mediator.Send(command);

            return Ok(troubleMachine.Identity);
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


                        var weavingMachine = await _losesQuery.Upload(sheet, month, year, monthId);
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
        [HttpGet("treeLoses")]
        public async Task<IActionResult> GetWarpingMachine(int page = 1,
                                           int size = 25,
                                           string order = "{}",
                                           string keyword = null,
                                           string filter = "{}")
        {
            VerifyUser();
            var weavingDailyOperations = await _losesQuery.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                weavingDailyOperations =
                   weavingDailyOperations
                       .Where(x => x.CreatedDate.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                   x.Month.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.YearPeriode.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)); //||
                                                                                                                
            }
             
            var result = weavingDailyOperations.Select(y => new
            {
                Month = y.Month,
                YearPeriode = y.YearPeriode,
                CreatedDate = y.CreatedDate

            }).Distinct().Skip((page - 1) * size).Take(size);
            var total = result.Count(); 
           

            return Ok(result, info: new { page, size, total });
        }
        [HttpGet("monthYear")]
        public async Task<IActionResult> GetDataByFilter(int page = 1, int size = 100, string month="0", string yearPeriode="")
        {
            var weavingDailyOperations = _losesQuery.GetDataByFilter(month, yearPeriode);
            var data = weavingDailyOperations.OrderBy(a => a.YearPeriode).ThenBy(a => a.MonthId).ThenBy(s => s.Date).ThenBy(a => a.Shift);
            var total = data.Count();
            var result = data.Skip((page - 1) * size).Take(size);
            return Ok(result, info: new { page, size, total });
        }
    }
   
}
