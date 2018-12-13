using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Weaving.Dtos;

namespace Weaving.Controllers
{
    [Route("weaving/manufacture-orders")]
    [ApiController]
    public class ManufactureOrderController : Barebone.Controllers.ControllerApiBase
    {
        public ManufactureOrderController(IStorage storage) : base(storage)
        {
        }

        [HttpPost]
        public IActionResult Post([FromBody]ManufactureOrderForm form)
        {
            return Ok();
        }
    }
}