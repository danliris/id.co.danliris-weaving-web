﻿using Manufactures.Domain.Commands;
using Manufactures.Domain.Repositories;
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

            var query = _manufactureOrderRepo.Query.Include(o => o.Composition).OrderByDescending(o => o.CreatedDate).Take(pageSize).Skip(page * pageSize);

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
        public async Task<IActionResult> Post([FromBody]ManufactureOrderForm form)
        {
            var command = new PlaceOrderCommand(form.OrderDate.Date, form.UnitDepartmentId, form.YarnCodes, form.CompositionId, form.Blended, form.MachineId, form.UserId);

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]ManufactureOrderForm form)
        {
            var orderId = Guid.Parse(id);
            var order = _manufactureOrderRepo.Find(o => o.Identity == orderId).FirstOrDefault();

            if (order == null)
                return NotFound();

            order.SetBlended(form.Blended);
            order.SetMachineId(form.MachineId);
            order.SetUnitDepartment(form.UnitDepartmentId);
            order.SetUserId(form.UserId);
            order.SetYarnCodes(form.YarnCodes);

            await _manufactureOrderRepo.Update(order);

            // Save Changes
            Storage.Save();

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var orderId = Guid.Parse(id);
            var order = _manufactureOrderRepo.Find(o => o.Identity == orderId).FirstOrDefault();

            if (order == null)
                return NotFound();

            order.Remove();

            await _manufactureOrderRepo.Update(order);

            Storage.Save();

            return Ok(order.Identity);
        }
    }
}