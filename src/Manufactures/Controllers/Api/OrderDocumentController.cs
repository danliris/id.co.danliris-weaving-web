using Barebone.Controllers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Dtos.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/orders")]
    [ApiController]
    [Authorize]
    public class OrderDocumentController : ControllerApiBase
    {
        private readonly IWeavingOrderDocumentRepository
                                               _weavingOrderDocumentRepository;
        private readonly IConstructionDocumentRepository
                                               _constructionDocumentRepository;

        public OrderDocumentController(IServiceProvider serviceProvider,
                                       IWorkContext workContext) : base(serviceProvider)
        {
            _weavingOrderDocumentRepository =
                this.Storage.GetRepository<IWeavingOrderDocumentRepository>();
            _constructionDocumentRepository =
                this.Storage.GetRepository<IConstructionDocumentRepository>();
        }

        [HttpGet]
        [Route("request-order-number")]
        public async Task<IActionResult> GetOrderNumber()
        {
            var orderNumber =
                await _weavingOrderDocumentRepository.GetWeavingOrderNumber();

            return Ok(orderNumber);
        }

        [HttpGet("order-by-period/{month}/{year}/unit/{unitCode}/status/{status}")]
        public async Task<IActionResult> Get(string month,
                                             string year,
                                             string unitCode,
                                             string status)
        {
            var resultData = new List<OrderBySearchDto>();
            var query =
                _weavingOrderDocumentRepository
                    .Query.OrderByDescending(item => item.CreatedDate);
            var orderDto =
                _weavingOrderDocumentRepository
                    .Find(query).Where(entity => entity.Period.Month.Contains(month) &&
                                                 entity.Period.Year.Contains(year) &&
                                                 entity.WeavingUnit.Code.Equals(unitCode))
                    .ToArray();

            if (status.Equals(Constants.ONORDER))
            {
                orderDto = orderDto.Where(e => e.OrderStatus == Constants.ONORDER).ToArray();
            }

            foreach (var order in orderDto)
            {
                var constructionDocument = 
                    _constructionDocumentRepository
                        .Find(e => e.Identity.Equals(order.FabricConstructionDocument.Id))
                        .FirstOrDefault();

                if (constructionDocument == null)
                {
                    throw Validator.ErrorValidation(("Construction Document",
                                                     "Invalid Construction Document with Order Identity " + 
                                                     order.Identity + 
                                                     " Not Found"));
                }

                var newOrder = new OrderBySearchDto(order, constructionDocument);

                resultData.Add(newOrder);
            }
            
            await Task.Yield();

            if (resultData.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(resultData);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            page = page - 1;
            var query = 
                _weavingOrderDocumentRepository.Query.OrderByDescending(item => item.CreatedDate)
                                                     .Take(size)
                                                     .Skip(page * size);
            var weavingOrderDocuments = 
                _weavingOrderDocumentRepository.Find(query)
                                               .Select(item => new ListWeavingOrderDocumentDto(item));

            if (!string.IsNullOrEmpty(keyword))
            {
                weavingOrderDocuments = 
                    weavingOrderDocuments
                        .Where(entity => entity.OrderNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                         entity.ConstructionNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                         entity.WeavingUnit.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                         entity.DateOrdered.LocalDateTime.ToString("dd MMMM yyyy").Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = 
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() + 
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = 
                    typeof(ListWeavingOrderDocumentDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    weavingOrderDocuments = 
                        weavingOrderDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    weavingOrderDocuments = 
                        weavingOrderDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            weavingOrderDocuments = weavingOrderDocuments.ToArray();
            int totalRows = weavingOrderDocuments.Count();

            await Task.Yield();

            return Ok(weavingOrderDocuments, info: new
            {
                page,
                size,
                total = totalRows
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var orderId = Guid.Parse(id);
            var orderDto = 
                _weavingOrderDocumentRepository.Find(item => item.Identity == orderId)
                                               .Select(item => new WeavingOrderById(item))
                                               .FirstOrDefault();

            await Task.Yield();

            if (orderId == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(orderDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceWeavingOrderCommand command)
        {
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, 
                                             [FromBody]UpdateWeavingOrderCommand command)
        {
            if (!Guid.TryParse(id, out Guid orderId))
            {
                return NotFound();
            }

            command.SetId(orderId);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid orderId))
            {
                return NotFound();
            }

            var command = new RemoveWeavingOrderCommand();
            command.SetId(orderId);

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }
    }
}
