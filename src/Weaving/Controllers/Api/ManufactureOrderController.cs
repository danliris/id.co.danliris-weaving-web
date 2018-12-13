using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Weaving.Application;
using Weaving.Domain.Repositories;
using Weaving.Dtos;

namespace Weaving.Controllers
{
    [Route("weaving/manufacture-orders")]
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ManufactureOrderForm form)
        {
            var order = await _manufactureOrderService.PlacedOrderAsync(form.OrderDate.Date, form.UnitDepartmentId, form.YarnCodes, null, form.Blended, form.MachineId);

            return Ok(order.Identity);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = _manufactureOrderRepo.Query.Select(o => new ManufactureOrderDto(o)).ToList();

            await Task.Yield();

            return Ok(orders);
        }
    }
}