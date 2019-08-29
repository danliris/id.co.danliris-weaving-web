using Barebone.Controllers;
using Manufactures.Domain.StockCard.Commands;
using Manufactures.Domain.StockCard.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/stock-cards")]
    [ApiController]
    [Authorize]
    public class StockCardController : ControllerApiBase
    {
        private readonly IStockCardRepository
           _stockCardRepository;

        public StockCardController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _stockCardRepository =
                this.Storage.GetRepository<IStockCardRepository>();
        }

        [HttpPost("adjustment")]
        public async Task<IActionResult> Post([FromBody]CreateStockAdjustmentCommand command)
        {
            var stockCardDoucment = await Mediator.Send(command);

            return Ok(stockCardDoucment.Identity);
        }


    }
}
