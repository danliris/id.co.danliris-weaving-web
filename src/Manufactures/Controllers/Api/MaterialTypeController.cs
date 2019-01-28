using Barebone.Controllers;
using Manufactures.Domain.Materials.Commands;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            var query = _materialTypeRepository.Query.OrderByDescending(item => item.CreatedDate).Take(size).Skip(page * size);
            var materialTypeDocuments = _materialTypeRepository.Find(query).Select(item => new MaterialTypeDto(item));

            if (!string.IsNullOrEmpty(keyword))
            {
                materialTypeDocuments = materialTypeDocuments.Where(entity => entity.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                             entity.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                             entity.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() + orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(MaterialTypeDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    materialTypeDocuments = materialTypeDocuments.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    materialTypeDocuments = materialTypeDocuments.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            materialTypeDocuments = materialTypeDocuments.ToArray();
            int totalRows = materialTypeDocuments.Count();

            await Task.Yield();

            return Ok(materialTypeDocuments, info: new
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
            var materialType = await Mediator.Send(command);

            return Ok(materialType.Identity);
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

            var materialType = await Mediator.Send(command);

            return Ok(materialType.Identity);
        }
    }
}
