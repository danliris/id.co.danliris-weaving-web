using Barebone.Controllers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Orders.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
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

        [HttpGet("order-by-period/{month}/{year}/unit/{unitId}/status/{status}")]
        public async Task<IActionResult> Get(string month,
                                             string year,
                                             int unitId,
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
                                                 entity.UnitId.Value.Equals(unitId))
                    .ToArray();

            if (status.Equals(Constants.ONORDER))
            {
                orderDto = orderDto.Where(e => e.OrderStatus == Constants.ONORDER).ToArray();
            }

            foreach (var order in orderDto)
            {
                var constructionDocument = 
                    _constructionDocumentRepository
                        .Find(e => e.Identity.Equals(order.ConstructionId.Value))
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

            return Ok(resultData);
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
                _weavingOrderDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
            var weavingOrderDocuments = 
                _weavingOrderDocumentRepository.Find(query);

            var resultData = new List<ListWeavingOrderDocumentDto>();

            foreach(var weavingOrder in weavingOrderDocuments)
            {
                var construction =
                    _constructionDocumentRepository.Find(o => o.Identity == weavingOrder.ConstructionId.Value).FirstOrDefault();

                var orderData = 
                    new ListWeavingOrderDocumentDto(weavingOrder, 
                                                    new FabricConstructionDocument(construction.Identity, construction.ConstructionNumber));

                resultData.Add(orderData);
            }


            if (!string.IsNullOrEmpty(keyword))
            {
                resultData =
                    resultData
                        .Where(entity => entity.OrderNumber.Contains(keyword,
                                                                     StringComparison.OrdinalIgnoreCase) ||
                                         entity.ConstructionNumber.Contains(keyword,
                                                                            StringComparison.OrdinalIgnoreCase) ||
                                         entity.UnitId.Value.ToString().Contains(keyword,
                                                                          StringComparison.OrdinalIgnoreCase) ||
                                         entity.DateOrdered.LocalDateTime
                                                           .ToString("dd MMMM yyyy")
                                                           .Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
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
                    resultData =
                        resultData.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    resultData =
                        resultData.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            resultData =
                resultData.Skip(page * size).Take(size).ToList();
            int totalRows = resultData.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(resultData, info: new
            {
                page,
                size,
                total = totalRows
            });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var orderId = Guid.Parse(Id);
            var order = 
                _weavingOrderDocumentRepository.Find(item => item.Identity == orderId)
                                               .FirstOrDefault();
            var construction =
                _constructionDocumentRepository.Find(item => item.Identity == order.ConstructionId.Value)
                                               .FirstOrDefault();
            var orderDto = new WeavingOrderDocumentDto(order, 
                                                       new UnitId(order.UnitId.Value), 
                                                       new FabricConstructionDocument(construction.Identity, 
                                                                                      construction.ConstructionNumber));

            await Task.Yield();

            if (orderId == null)
            {
                return NoContent();
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

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id, 
                                             [FromBody]UpdateWeavingOrderCommand command)
        {
            if (!Guid.TryParse(Id, out Guid orderId))
            {
                return NotFound();
            }

            command.SetId(orderId);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid orderId))
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
