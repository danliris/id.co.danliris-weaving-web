using Barebone.Controllers;
using Manufactures.Application.BeamStockMonitoring.DataTransferObjects;
using Manufactures.Domain.BeamStockMonitoring.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/beam-stock-monitoring")]
    [ApiController]
    [Authorize]
    public class BeamStockMonitoringController: ControllerApiBase
    {
        private readonly IBeamStockMonitoringQuery<BeamStockMonitoringDto> _beamStockMonitoring;

        public BeamStockMonitoringController(IServiceProvider serviceProvider, 
                                             IWorkContext workContext,
                                             IBeamStockMonitoringQuery<BeamStockMonitoringDto> beamStockMonitoringQuery) : base(serviceProvider)
        {
            _beamStockMonitoring = beamStockMonitoringQuery ?? throw new ArgumentNullException(nameof(beamStockMonitoringQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            var beamStockMonitoringDocuments = await _beamStockMonitoring.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                await Task.Yield();
                beamStockMonitoringDocuments =
                    beamStockMonitoringDocuments
                        .Where(x => x.BeamNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.OrderNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.ConstructionNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                //Extract Dictionary
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(BeamStockMonitoringDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    await Task.Yield();
                    beamStockMonitoringDocuments =
                        beamStockMonitoringDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    await Task.Yield();
                    beamStockMonitoringDocuments =
                        beamStockMonitoringDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            int totalRows = beamStockMonitoringDocuments.Count();
            var result = beamStockMonitoringDocuments.Skip((page - 1) * size).Take(size);
            var resultCount = result.Count();

            return Ok(result, info: new { page, size, totalRows, resultCount });
        }
    }
}
