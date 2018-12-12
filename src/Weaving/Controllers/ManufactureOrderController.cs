using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Weaving.Dtos;

namespace Weaving.Controllers
{
    [Route("weaving/manufacture-orders")]
    [ApiController]
    public class ManufactureOrderController : ControllerBase
    {
        public ManufactureOrderController()
        {
        }

        //public ActionResult Index()
        //{
        //    return this.View();
        //}

        [HttpPost]
        public IActionResult Post([FromBody]ManufactureOrderForm form)
        {
            return Ok();
        }
    }
}
