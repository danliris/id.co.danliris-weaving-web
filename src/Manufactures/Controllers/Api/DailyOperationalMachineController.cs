using Barebone.Controllers;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.DailyOperations.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Dtos.DailyOperationalMachine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Route("weaving/daily-operational-machine")]
    [ApiController]
    [Authorize]
    public class DailyOperationalMachineController : ControllerApiBase
    {
        private readonly IDailyOperationalMachineRepository _dailyOperationalDocumentRepository;
        private readonly IWeavingOrderDocumentRepository _weavingOrderDocumentRepository;
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;
        private readonly IMachineRepository _machineRepository;

        public DailyOperationalMachineController(IServiceProvider serviceProvider, IWorkContext workContext) : base(serviceProvider)
        {
            _dailyOperationalDocumentRepository = this.Storage.GetRepository<IDailyOperationalMachineRepository>();
            _weavingOrderDocumentRepository = this.Storage.GetRepository<IWeavingOrderDocumentRepository>();
            _constructionDocumentRepository = this.Storage.GetRepository<IConstructionDocumentRepository>();
            _machineRepository = this.Storage.GetRepository<IMachineRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            page = page - 1;
            var domQuery =
                _dailyOperationalDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
            var dailyOperationalMachineDocuments =
                _dailyOperationalDocumentRepository.Find(domQuery.Include(d => d.DailyOperationMachineDetails));

            var resultDto = new List<DailyOperationalMachineListDto>();

            foreach (var dailyOperation in dailyOperationalMachineDocuments)
            {
                var machineDocument = _machineRepository.Find(d => d.Identity.Equals(dailyOperation.MachineId.Value)).FirstOrDefault();
                var dto = new DailyOperationalMachineListDto(dailyOperation, machineDocument.MachineNumber);

                foreach (var detail in dailyOperation.DailyOperationMachineDetails)
                {
                    var orderDocument = _weavingOrderDocumentRepository.Find(d => d.Identity.Equals(detail.OrderDocumentId)).FirstOrDefault();
                    var constructionDocument = _constructionDocumentRepository.Find(c => c.Identity.Equals(orderDocument.ConstructionId)).FirstOrDefault();

                    dto.ConstructionNumber = constructionDocument.ConstructionNumber;
                }

                resultDto.Add(dto);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                resultDto =
                    resultDto
                        .Where(entity => entity.MachineNumber.Contains(keyword,
                                                                          StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(DailyOperationalMachineListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    resultDto =
                        resultDto.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    resultDto =
                        resultDto.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            resultDto =
                resultDto.Skip(page * size).Take(size).ToList();
            int totalRows = resultDto.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(resultDto, info: new
            {
                page,
                size,
                total = totalRows
            });
        }
    }
}
