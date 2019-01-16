using Barebone.Controllers;
using Manufactures.Domain.Materials.Commands;
using Manufactures.Domain.Materials.Repositories;
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
    [Route("weaving/material-types")]
    [ApiController]
    [Authorize]
    public class MaterialTypeController : ControllerApiBase
    {
        private readonly IMaterialTypeRepository _materialTypeRepository;

        public MaterialTypeController(IServiceProvider serviceProvider, IWorkContext workContext) : base(serviceProvider)
        {
            _materialTypeRepository = this.Storage.GetRepository<IMaterialTypeRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 0, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            int totalRows = _materialTypeRepository.Query.Count();
            var query = _materialTypeRepository.Query.OrderByDescending(item => item.CreatedDate).Take(size).Skip(page * size);
            var materialTypeDto = _materialTypeRepository.Find(query).Select(item => new MaterialTypeDto(item)).ToArray();

            await Task.Yield();

            return Ok(materialTypeDto, info: new
            {
                page,
                size,
                count = totalRows
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Identity = Guid.Parse(id);
            var materialTypeDto = _materialTypeRepository.Find(item => item.Identity == Identity).Select(item => new MaterialTypeDto(item)).FirstOrDefault();
            await Task.Yield();

            if (materialTypeDto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(materialTypeDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceMaterialTypeCommand command)
        {
            var materialType = await Mediator.Send(command);

            return Ok(materialType.Identity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]UpdateMaterialTypeCommand command)
        {
            if (!Guid.TryParse(id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveMaterialTypeCommand();
            command.SetId(Identity);

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }
    }
}
