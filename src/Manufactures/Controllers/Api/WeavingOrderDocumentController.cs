﻿using Barebone.Controllers;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/orders")]
    [ApiController]
    [Authorize]
    public class WeavingOrderDocumentController : ControllerApiBase
    {
        private readonly IWeavingOrderDocumentRepository _weavingOrderDocumentRepository;

        public WeavingOrderDocumentController(IServiceProvider serviceProvider, IWorkContext workContext) : base(serviceProvider)
        {
            _weavingOrderDocumentRepository = this.Storage.GetRepository<IWeavingOrderDocumentRepository>();
        }
        
        [HttpGet]
        [Route("request-order-number")]
        public async Task<IActionResult> GetOrderNumber()
        {
            var orderNumber = await _weavingOrderDocumentRepository.GetWeavingOrderNumber();

            return Ok(orderNumber);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 0, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            int totalRows = _weavingOrderDocumentRepository.Query.Count();
            var query = _weavingOrderDocumentRepository.Query.OrderByDescending(item => item.CreatedDate).Take(size).Skip(page * size);
            var weavingOrderDto = _weavingOrderDocumentRepository.Find(query).Select(item => new ListWeavingOrderDocumentDto(item)).ToArray();

            await Task.Yield();

            return Ok(weavingOrderDto, info: new
            {
                page,
                size,
                count = totalRows
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var orderId = Guid.Parse(id);
            var orderDto = _weavingOrderDocumentRepository.Find(item => item.Identity == orderId).Select(item => new WeavingOrderDocumentDto(item)).FirstOrDefault();
            await Task.Yield();

            if (orderId == null)
            {
                return NotFound();
            } else
            {
                return Ok(orderDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceWeavingOrderCommand command)
        {
            command.OrderNumber = await _weavingOrderDocumentRepository.GetWeavingOrderNumber();

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]UpdateWeavingOrderCommand command)
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