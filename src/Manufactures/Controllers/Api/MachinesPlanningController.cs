using Barebone.Controllers;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachineTypes.Repositories;
using Manufactures.Domain.MachinesPlanning.Commands;
using Manufactures.Domain.MachinesPlanning.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.MachinesPlanning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manufactures.Domain.MachinesPlanning.Queries;
using Manufactures.Application.MachinesPlanning.DataTransferObjects;
using Manufactures.Helpers.XlsTemplates;
using System.IO;
using Moonlay.ExtCore.Mvc.Abstractions;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/machines-planning")]
    [ApiController]
    [Authorize]
    public class MachinesPlanningController : ControllerApiBase
    {
        private readonly IMachinesPlanningRepository _machinesPlanningRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IMachineTypeRepository _machineTypeRepository;

        private readonly IMachinesPlanningReportQuery<MachinesPlanningReportListDto> _machinesPlanningReportQuery;

        public MachinesPlanningController(IServiceProvider serviceProvider,
                                          IMachinesPlanningReportQuery<MachinesPlanningReportListDto> machinesPlanningReportQuery) : base(serviceProvider)
        {
            _machinesPlanningRepository =
                this.Storage.GetRepository<IMachinesPlanningRepository>();
            _machineRepository =
                this.Storage.GetRepository<IMachineRepository>();
            _machineTypeRepository =
                 this.Storage.GetRepository<IMachineTypeRepository>();

            _machinesPlanningReportQuery = machinesPlanningReportQuery ?? throw new ArgumentNullException(nameof(machinesPlanningReportQuery));
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
                _machinesPlanningRepository.Query.OrderByDescending(item => item.CreatedDate);
            var machinesPlanning =
                _machinesPlanningRepository.Find(query);

            var machinePlanningDtos = new List<MachinesPlanningListDto>();

            foreach (var machinePlanning in machinesPlanning)
            {
                var manufacturingMachine = 
                    _machineRepository.Find(item => item.Identity == machinePlanning.MachineId.Value)
                                      .FirstOrDefault();
                var manufacturingMachineType = 
                    _machineTypeRepository.Find(item => item.Identity == manufacturingMachine.MachineTypeId.Value)
                                          .FirstOrDefault();

                var machine = new ManufactureMachine(manufacturingMachine, manufacturingMachineType);
                var machineDto = new MachinesPlanningListDto(machinePlanning, machine);

                machinePlanningDtos.Add(machineDto);
            }


            if (!string.IsNullOrEmpty(keyword))
            {
                machinePlanningDtos =
                    machinePlanningDtos.Where(entity => entity.Area.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                entity.Blok.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                entity.BlokKaizen.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                               .ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(MachinesPlanningListDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    machinePlanningDtos = machinePlanningDtos.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    machinePlanningDtos = machinePlanningDtos.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            var ResultMachinePlanningDtos = machinePlanningDtos.Skip(page * size).Take(size).ToList();
            int totalRows = machinePlanningDtos.Count();
            int resultCount = ResultMachinePlanningDtos.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(ResultMachinePlanningDtos, info: new
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
            var machinePlanning =
                _machinesPlanningRepository.Find(item => item.Identity == Identity).FirstOrDefault();

            var manufacturingMachine = 
                _machineRepository.Find(item => item.Identity == machinePlanning.MachineId.Value)
                                  .FirstOrDefault();
            var manufacturingMachineType = 
                _machineTypeRepository.Find(item => item.Identity == manufacturingMachine.MachineTypeId.Value)
                                      .FirstOrDefault();

            var machine = new ManufactureMachine(manufacturingMachine, manufacturingMachineType);
            var resultDto = new MachinesPlanningDocumentDto(machinePlanning, machine);

            await Task.Yield();

            if (resultDto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(resultDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddNewEnginePlanningCommand command)
        {
            var existingMachinePlanning = 
                _machinesPlanningRepository.Find(o => o.MachineId.Equals(command.MachineId) && o.Blok.Equals(command.Blok))
                                           .FirstOrDefault();

            if (existingMachinePlanning != null)
            {
                throw Validator.ErrorValidation(("MachineId", "Has available machine planning on same blok"));
            }

            var machinePlanning = await Mediator.Send(command);

            return Ok(machinePlanning.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateEnginePlanningCommand command)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var existingMachinePlanning = 
                _machinesPlanningRepository.Find(o => o.MachineId == command.MachineId && o.Blok == command.Blok)
                                           .FirstOrDefault();

            if (existingMachinePlanning != null)
            {
                throw Validator.ErrorValidation(("MachineId", "Has available machine planning on same blok"));
            }

            command.SetId(Identity);
            var machinePlanning = await Mediator.Send(command);

            return Ok(machinePlanning.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid Identity))
            {
                return NotFound();
            }

            var command = new RemoveEnginePlanningCommand();
            command.SetId(Identity);

            var machinePlanning = await Mediator.Send(command);

            return Ok(machinePlanning.Identity);
        }

        //Controller for Machine Planning Report
        [HttpGet("get-report")]
        public async Task<IActionResult> GetReport(string machineId,
                                                   string block, 
                                                   int unitId = 0,
                                                   int page = 1,
                                                   int size = 25,
                                                   string order = "{}")
        {
            var acceptRequest = Request.Headers.Values.ToList();
            var index = acceptRequest.IndexOf("application/xls") > 0;

            var machinePlanningReport = await _machinesPlanningReportQuery.GetReports(machineId,
                                                                                      block,
                                                                                      unitId,
                                                                                      page,
                                                                                      size,
                                                                                      order);

            await Task.Yield();
            if (index.Equals(true))
            {
                byte[] xlsInBytes;

                MachinePlanningReportXlsTemplate xlsTemplate = new MachinePlanningReportXlsTemplate();
                MemoryStream xls = xlsTemplate.GenerateMachinePlanningReportXls(machinePlanningReport.Item1.ToList());
                xlsInBytes = xls.ToArray();
                var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Perencanaan Mesin");
                return xlsFile;
            }
            else
            {
                return Ok(machinePlanningReport.Item1, info: new
                {
                    count = machinePlanningReport.Item2
                });
            }
        }

        //[HttpGet("get-all")]
        //public async Task<IActionResult> GetAllReport()
        //{
        //    var acceptRequest = Request.Headers.Values.ToList();
        //    var index = acceptRequest.IndexOf("application/xls") > 0;

        //    var machinePlanning = await _machinesPlanningReportQuery.GetAll();

        //    await Task.Yield();
        //    if (index.Equals(true))
        //    {
        //        byte[] xlsInBytes;

        //        MachinePlanningReportXlsTemplate xlsTemplate = new MachinePlanningReportXlsTemplate();
        //        MemoryStream xls = xlsTemplate.GenerateMachinePlanningReportXls(machinePlanning.ToList());
        //        xlsInBytes = xls.ToArray();
        //        var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Perencanaan Mesin");
        //        return xlsFile;
        //    }
        //    else
        //    {
        //        return Ok(machinePlanning, info: new
        //        {
        //            count = machinePlanning.Count()
        //        });
        //    }
        //}

        //[HttpGet("get-by-weaving-unit/{weavingUnitId}")]
        //public async Task<IActionResult> GetByWeavingUnit(int weavingUnitId)
        //{
        //    var acceptRequest = Request.Headers.Values.ToList();
        //    var index = acceptRequest.IndexOf("application/xls") > 0;

        //    var machinePlanning = await _machinesPlanningReportQuery.GetByWeavingUnit(weavingUnitId);

        //    await Task.Yield();
        //    if (index.Equals(true))
        //    {
        //        byte[] xlsInBytes;

        //        MachinePlanningReportXlsTemplate xlsTemplate = new MachinePlanningReportXlsTemplate();
        //        MemoryStream xls = xlsTemplate.GenerateMachinePlanningReportXls(machinePlanning.ToList());
        //        xlsInBytes = xls.ToArray();
        //        var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Perencanaan Mesin");
        //        return xlsFile;
        //    }
        //    else
        //    {
        //        return Ok(machinePlanning, info: new
        //        {
        //            count = machinePlanning.Count()
        //        });
        //    }
        //}

        //[HttpGet("get-by-machine/{machineId}")]
        //public async Task<IActionResult> GetByMachine(Guid machineId)
        //{
        //    var acceptRequest = Request.Headers.Values.ToList();
        //    var index = acceptRequest.IndexOf("application/xls") > 0;

        //    var machinePlanning = await _machinesPlanningReportQuery.GetByMachine(machineId);

        //    await Task.Yield();
        //    if (index.Equals(true))
        //    {
        //        byte[] xlsInBytes;

        //        MachinePlanningReportXlsTemplate xlsTemplate = new MachinePlanningReportXlsTemplate();
        //        MemoryStream xls = xlsTemplate.GenerateMachinePlanningReportXls(machinePlanning.ToList());
        //        xlsInBytes = xls.ToArray();
        //        var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Perencanaan Mesin");
        //        return xlsFile;
        //    }
        //    else
        //    {
        //        return Ok(machinePlanning, info: new
        //        {
        //            count = machinePlanning.Count()
        //        });
        //    }
        //}

        //[HttpGet("get-by-block/{block}")]
        //public async Task<IActionResult> GetByBlock(string block)
        //{
        //    var acceptRequest = Request.Headers.Values.ToList();
        //    var index = acceptRequest.IndexOf("application/xls") > 0;

        //    var machinePlanning = await _machinesPlanningReportQuery.GetByBlock(block);

        //    await Task.Yield();
        //    if (index.Equals(true))
        //    {
        //        byte[] xlsInBytes;

        //        MachinePlanningReportXlsTemplate xlsTemplate = new MachinePlanningReportXlsTemplate();
        //        MemoryStream xls = xlsTemplate.GenerateMachinePlanningReportXls(machinePlanning.ToList());
        //        xlsInBytes = xls.ToArray();
        //        var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Perencanaan Mesin");
        //        return xlsFile;
        //    }
        //    else
        //    {
        //        return Ok(machinePlanning, info: new
        //        {
        //            count = machinePlanning.Count()
        //        });
        //    }
        //}

        //[HttpGet("get-by-weaving-unit-machine/unit/{weavingUnitId}/machine/{machineId}")]
        //public async Task<IActionResult> GetByWeavingUnitMachine(int weavingUnitId, Guid machineId)
        //{
        //    var acceptRequest = Request.Headers.Values.ToList();
        //    var index = acceptRequest.IndexOf("application/xls") > 0;

        //    var machinePlanning = await _machinesPlanningReportQuery.GetByWeavingUnitMachine(weavingUnitId, machineId);

        //    await Task.Yield();
        //    if (index.Equals(true))
        //    {
        //        byte[] xlsInBytes;

        //        MachinePlanningReportXlsTemplate xlsTemplate = new MachinePlanningReportXlsTemplate();
        //        MemoryStream xls = xlsTemplate.GenerateMachinePlanningReportXls(machinePlanning.ToList());
        //        xlsInBytes = xls.ToArray();
        //        var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Perencanaan Mesin");
        //        return xlsFile;
        //    }
        //    else
        //    {
        //        return Ok(machinePlanning, info: new
        //        {
        //            count = machinePlanning.Count()
        //        });
        //    }
        //}

        //[HttpGet("get-by-weaving-unit-block/unit/{weavingUnitId}/block/{block}")]
        //public async Task<IActionResult> GetByWeavingUnitBlock(int weavingUnitId, string block)
        //{
        //    var acceptRequest = Request.Headers.Values.ToList();
        //    var index = acceptRequest.IndexOf("application/xls") > 0;

        //    var machinePlanning = await _machinesPlanningReportQuery.GetByWeavingUnitBlock(weavingUnitId, block);

        //    await Task.Yield();
        //    if (index.Equals(true))
        //    {
        //        byte[] xlsInBytes;

        //        MachinePlanningReportXlsTemplate xlsTemplate = new MachinePlanningReportXlsTemplate();
        //        MemoryStream xls = xlsTemplate.GenerateMachinePlanningReportXls(machinePlanning.ToList());
        //        xlsInBytes = xls.ToArray();
        //        var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Perencanaan Mesin");
        //        return xlsFile;
        //    }
        //    else
        //    {
        //        return Ok(machinePlanning, info: new
        //        {
        //            count = machinePlanning.Count()
        //        });
        //    }
        //}

        //[HttpGet("get-by-machine-block/machine/{machineId}/block/{block}")]
        //public async Task<IActionResult> GetByMachineBlock(Guid machineId, string block)
        //{
        //    var acceptRequest = Request.Headers.Values.ToList();
        //    var index = acceptRequest.IndexOf("application/xls") > 0;

        //    var machinePlanning = await _machinesPlanningReportQuery.GetByMachineBlock(machineId, block);

        //    await Task.Yield();
        //    if (index.Equals(true))
        //    {
        //        byte[] xlsInBytes;

        //        MachinePlanningReportXlsTemplate xlsTemplate = new MachinePlanningReportXlsTemplate();
        //        MemoryStream xls = xlsTemplate.GenerateMachinePlanningReportXls(machinePlanning.ToList());
        //        xlsInBytes = xls.ToArray();
        //        var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Perencanaan Mesin");
        //        return xlsFile;
        //    }
        //    else
        //    {
        //        return Ok(machinePlanning, info: new
        //        {
        //            count = machinePlanning.Count()
        //        });
        //    }
        //}

        //[HttpGet("get-all-specified/unit/{weavingUnitId}/machine/{machineId}/block/{block}")]
        //public async Task<IActionResult> GetAllSpecified(int weavingUnitId, Guid machineId, string block)
        //{
        //    var acceptRequest = Request.Headers.Values.ToList();
        //    var index = acceptRequest.IndexOf("application/xls") > 0;

        //    var machinePlanning = await _machinesPlanningReportQuery.GetAllSpecified(weavingUnitId, machineId, block);

        //    await Task.Yield();
        //    if (index.Equals(true))
        //    {
        //        byte[] xlsInBytes;

        //        MachinePlanningReportXlsTemplate xlsTemplate = new MachinePlanningReportXlsTemplate();
        //        MemoryStream xls = xlsTemplate.GenerateMachinePlanningReportXls(machinePlanning.ToList());
        //        xlsInBytes = xls.ToArray();
        //        var xlsFile = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Perencanaan Mesin");
        //        return xlsFile;
        //    }
        //    else
        //    {
        //        return Ok(machinePlanning, info: new
        //        {
        //            count = machinePlanning.Count()
        //        });
        //    }
        //}
    }
}
