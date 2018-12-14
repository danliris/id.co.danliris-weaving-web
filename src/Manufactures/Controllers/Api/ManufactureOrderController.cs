using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Moonlay;
using System;
using System.Linq;
using System.Threading.Tasks;
using Manufactures.Application;
using Manufactures.Domain.Repositories;
using Manufactures.Dtos;

namespace Manufactures.Controllers
{
    [Route("manufacture/orders")]
    [ApiController]
    public class ManufactureOrderController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IManufactureOrderService _manufactureOrderService;
        private readonly IManufactureOrderRepository _manufactureOrderRepo;

        public ManufactureOrderController(IManufactureOrderService manufactureOrderService, IStorage storage) : base(storage)
        {
            _manufactureOrderService = manufactureOrderService;
            _manufactureOrderRepo = this.Storage.GetRepository<IManufactureOrderRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ordersDto = _manufactureOrderRepo.Query.Select(o => new ManufactureOrderDto(o)).ToList();

            await Task.Yield();

            return Ok(ordersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var orderId = Guid.Parse(id);

            var orderDto = _manufactureOrderRepo.Query.Select(o => new ManufactureOrderDto(o)).FirstOrDefault(o => o.Identity == orderId);

            await Task.Yield();

            if (orderDto == null)
                return NotFound();
            else
                return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ManufactureOrderForm form)
        {
            var order = await _manufactureOrderService.PlacedOrderAsync(form.OrderDate.Date, form.UnitDepartmentId, form.YarnCodes, null, form.Blended, form.MachineId);

            return Ok(order.Identity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]ManufactureOrderForm form)
        {
            var orderId = Guid.Parse(id);
            var order = _manufactureOrderRepo.Query.FirstOrDefault(o => o.Identity == orderId);

            Validator.ThrowIfNull(() => order);

            order.SetBlended(form.Blended);
            order.SetMachineId(form.MachineId);
            order.SetUnitDepartment(form.UnitDepartmentId);
            order.SetUserId(form.UserId);
            order.SetYarnCodes(form.YarnCodes);

            await _manufactureOrderRepo.Update(order);

            Storage.Save();

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var orderId = Guid.Parse(id);
            var order = _manufactureOrderRepo.Query.FirstOrDefault(o => o.Identity == orderId);

            Validator.ThrowIfNull(() => order);

            await _manufactureOrderRepo.Remove(order);

            Storage.Save();

            return Ok(order.Identity);
        }
    }
}