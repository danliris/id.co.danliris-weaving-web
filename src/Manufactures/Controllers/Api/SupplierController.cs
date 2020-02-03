using Barebone.Controllers;
using Manufactures.Domain.Suppliers.Commands;
using Manufactures.Domain.Suppliers.Repositories;
using Manufactures.DataTransferObjects.WeavingSupplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/suppliers")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerApiBase
    {
        private readonly IWeavingSupplierRepository _weavingSupplierRepository;

        public SupplierController(IServiceProvider serviceProvider,
                                  IWorkContext workContext) : base(serviceProvider)
        {
            _weavingSupplierRepository =
                this.Storage.GetRepository<IWeavingSupplierRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            page = page - 1;
            var query =
                _weavingSupplierRepository.Query.OrderByDescending(item => item.CreatedDate);
            var suppliers =
                _weavingSupplierRepository.Find(query)
                                          .Select(item => new SupplierListDto(item));

            if (!string.IsNullOrEmpty(keyword))
            {
                suppliers =
                    suppliers.Where(entity => entity.Code.Contains(keyword, 
                                                                   StringComparison.OrdinalIgnoreCase) ||
                                              entity.Name.Contains(keyword, 
                                                                   StringComparison.OrdinalIgnoreCase));
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(SupplierListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    suppliers = suppliers.OrderBy(x => prop.GetValue(x, null));
                }
                else
                {
                    suppliers = suppliers.OrderByDescending(x => prop.GetValue(x, null));
                }
            }

            var ResultSuppliers = suppliers.Skip(page * size).Take(size);
            int totalRows = suppliers.Count();
            int resultCount = ResultSuppliers.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(ResultSuppliers, info: new
            {
                page,
                size,
                total = totalRows,
                count = resultCount
            });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var Identity = Guid.Parse(Id);
            var supplier =
                _weavingSupplierRepository.Find(item => item.Identity == Identity)
                                          .Select(item => new SupplierDocumentDto(item))
                                          .FirstOrDefault();
            await Task.Yield();

            if (supplier == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(supplier);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceNewSupplierCommand command)
        {
            var SupplierDocument = await Mediator.Send(command);

            return Ok(SupplierDocument.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateExsistingSupplierCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            command.SetId(Identity);
            var supplierDocument = await Mediator.Send(command);

            return Ok(supplierDocument.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveSupplierCommand();
            command.SetId(Identity);

            var SupplierDocument = await Mediator.Send(command);

            return Ok(SupplierDocument.Identity);
        }

        [HttpGet("get-code/{id}")]
        public async Task<IActionResult> GetSupplierName(string id)
        {
            var supplierId = new Guid(id);

            var supplierDocument =
                _weavingSupplierRepository
                    .Find(e => e.Identity.Equals(supplierId))
                    .FirstOrDefault();
            var supplierCode = supplierDocument.Code;

            if (supplierDocument != null)
            {
                await Task.Yield();
                return Ok(supplierCode);
            }
            else
            {
                await Task.Yield();
                return NotFound();
                throw Validator.ErrorValidation(("WarpOrigin", "Can't Find Supplier Code"),("WeftOrigin", "Can't Find Supplier Code"));
            }
        }

        [HttpGet("get-supplier/{id}")]
        public async Task<IActionResult> GetSupplier(string id)
        {
            if (!Guid.TryParse(id, out Guid identity))
            {
                return NotFound();
            }

            var supplierDocument =
                _weavingSupplierRepository
                    .Find(o => o.Identity == identity)
                    .Select(o=>new SupplierDocumentDto(o))
                    .FirstOrDefault();

            if (supplierDocument != null)
            {
                await Task.Yield();
                return Ok(supplierDocument);
            }
            else
            {
                await Task.Yield();
                return NotFound();
            }
        }
    }
}
