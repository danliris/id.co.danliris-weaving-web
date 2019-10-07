using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers
{
    [Route("manufacture/orders")]
    [ApiController]
    public class ManufactureOrderController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IManufactureOrderRepository _manufactureOrderRepo;

        public ManufactureOrderController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _manufactureOrderRepo = this.Storage.GetRepository<IManufactureOrderRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int page = 0, pageSize = 25;
            int totalRows = _manufactureOrderRepo.Query.Count();

            var query = _manufactureOrderRepo.Query.OrderByDescending(o => o.CreatedDate).Take(pageSize).Skip(page * pageSize);

            var ordersDto = _manufactureOrderRepo.Find(query).Select(o => new ManufactureOrderDto(o)).ToArray();

            await Task.Yield();

            return Ok(ordersDto, info: new
            {
                page,
                pageSize,
                count = totalRows
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var orderId = Guid.Parse(id);

            var orderDto = _manufactureOrderRepo.Find(o => o.Identity == orderId).Select(o => new ManufactureOrderDto(o)).FirstOrDefault();

            await Task.Yield();

            if (orderDto == null)
                return NotFound();
            else
                return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceOrderCommandOld command)
        {
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]UpdateOrderCommandOld command)
        {
            if (!Guid.TryParse(id, out Guid orderId))
                return NotFound();

            command.SetId(orderId);

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid orderId))
                return NotFound();

            var command = new RemoveOrderCommandOld();
            command.SetId(orderId);

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }
    }
}