using Barebone.Controllers;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.YarnNumbers.Repositories;
using Manufactures.Domain.Yarns.Commands;
using Manufactures.Domain.Yarns.Repositories;
using Manufactures.Dtos.Yarn;
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
    [Route("weaving/yarns")]
    [ApiController]
    [Authorize]
    public class YarnDocumentController : ControllerApiBase
    {
        public readonly IYarnDocumentRepository _yarnDocumentRepository;
        public readonly IMaterialTypeRepository _materialTypeRepository;
        public readonly IYarnNumberRepository _yarnNumberRepository;

        public YarnDocumentController(IServiceProvider serviceProvider,
                                      IWorkContext workContext) : base(serviceProvider)
        {
            _yarnDocumentRepository =
                this.Storage.GetRepository<IYarnDocumentRepository>();
            _materialTypeRepository =
                this.Storage.GetRepository<IMaterialTypeRepository>();
            _yarnNumberRepository =
                this.Storage.GetRepository<IYarnNumberRepository>();
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
                _yarnDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
            var yarns =
                _yarnDocumentRepository.Find(query);

            var yarnDocuments = new List<YarnDocumentListDto>();

            foreach (var yarn in yarns)
            {

                var materialType =
                    _materialTypeRepository.Find(o => o.Identity == yarn.MaterialTypeId.Value)
                                           .Select(x => new MaterialTypeValueObject(x.Identity, x.Code, x.Name))
                                           .FirstOrDefault();
                var yarnNumber =
                    _yarnNumberRepository.Find(o => o.Identity == yarn.YarnNumberId.Value)
                                         .Select(x => new YarnNumberValueObject(x.Identity, x.Code, x.Number, x.RingType, x.AdditionalNumber))
                                         .FirstOrDefault();

                var data = new YarnDocumentListDto(yarn, materialType, yarnNumber);

                yarnDocuments.Add(data);
            }


            if (!string.IsNullOrEmpty(keyword))
            {
                yarnDocuments =
                    yarnDocuments.Where(entity => entity.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                          entity.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(YarnDocumentListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    yarnDocuments = yarnDocuments.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    yarnDocuments = yarnDocuments.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            var ResultYarnDocuments = yarnDocuments.Skip(page * size).Take(size).ToList();
            int totalRows = yarnDocuments.Count();
            int resultCount = ResultYarnDocuments.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(ResultYarnDocuments, info: new
            {
                page,
                size,
                total = totalRows,
                count = resultCount
            });
        }

        [HttpGet("get-yarns-by-ids")]
        public async Task<IActionResult> GetYarns(List<string> yarnIds,
                                                  int page = 1,
                                                  int size = 25,
                                                  string order = "{}",
                                                  string keyword = null,
                                                  string filter = "{}")
        {
            page = page - 1;

            var results = new List<YarnDocumentListDto>();
            foreach (var yarnId in yarnIds)
            {
                var yarnGuid = Guid.Parse(yarnId);
                var yarnDocuments =
                    _yarnDocumentRepository
                    .Find(o=>o.Identity.Equals(yarnGuid));

                foreach (var yarnDocument in yarnDocuments)
                {
                    var materialType =
                        _materialTypeRepository.Find(o => o.Identity == yarnDocument.MaterialTypeId.Value)
                                               .Select(x => new MaterialTypeValueObject(x.Identity, x.Code, x.Name))
                                               .FirstOrDefault();
                    var yarnNumber =
                        _yarnNumberRepository.Find(o => o.Identity == yarnDocument.YarnNumberId.Value)
                                             .Select(x => new YarnNumberValueObject(x.Identity, x.Code, x.Number, x.RingType, x.AdditionalNumber))
                                             .FirstOrDefault();

                    var data = new YarnDocumentListDto(yarnDocument, materialType, yarnNumber);

                    results.Add(data);
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    results =
                        results.Where(entity => entity.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                entity.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!order.Contains("{}"))
                {
                    Dictionary<string, string> orderDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              orderDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(YarnDocumentListDto).GetProperty(key);

                    if (orderDictionary.Values.Contains("asc"))
                    {
                        results = results.OrderBy(x => prop.GetValue(x, null)).ToList();
                    }
                    else
                    {
                        results = results.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                    }
                }
            }

            var ResultYarnDocuments = results.Skip(page * size).Take(size).ToList();
            int totalRows = results.Count();
            int resultCount = ResultYarnDocuments.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(ResultYarnDocuments, info: new
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
            var yarn =
                _yarnDocumentRepository.Find(item => item.Identity == Identity)
                                       .FirstOrDefault();
            var materialType =
                _materialTypeRepository.Find(item => item.Identity == yarn.MaterialTypeId.Value)
                                       .Select(x => new MaterialTypeValueObject(x.Identity, x.Code, x.Name))
                                       .FirstOrDefault();
            var yarnNumberDocument =
                _yarnNumberRepository.Find(item => item.Identity == yarn.YarnNumberId.Value)
                                     .Select(x => new YarnNumberValueObject(x.Identity, x.Code, x.Number, x.RingType, x.AdditionalNumber))
                                     .FirstOrDefault();
            await Task.Yield();

            if (yarn == null || materialType == null || yarnNumberDocument == null)
            {
                return NotFound();
            }
            else
            {
                var result = new YarnDocumentDto(yarn, materialType, yarnNumberDocument);
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateNewYarnCommand command)
        {
            var yarnDocument = await Mediator.Send(command);

            return Ok(yarnDocument.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateExsistingYarnCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var existingCode =
                _yarnDocumentRepository.Find(o => o.Code.Equals(command.Code) &&
                                                  o.Deleted.Equals(false)).Count >= 1;

            if(existingCode)
            {
                throw Validator.ErrorValidation(("Code", "Code with " + command.Code + " has available"));
            }

            command.SetId(Identity);
            var yarnDocument = await Mediator.Send(command);

            return Ok(yarnDocument.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveExsistingYarnCommand();
            command.SetId(Identity);

            var yarnDocument = await Mediator.Send(command);

            return Ok(yarnDocument.Identity);
        }
    }
}
