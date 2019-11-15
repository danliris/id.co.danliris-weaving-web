using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.MachinesPlanning.DataTransferObjects;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachinesPlanning.Queries;
using Manufactures.Domain.MachinesPlanning.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.MachinesPlanning.QueryHandlers.MachinesPlanningReport
{
    public class MachinesPlanningReportQueryHandler : IMachinesPlanningReportQuery<MachinesPlanningReportListDto>
    {
        private readonly IStorage
            _storage;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IMachinesPlanningRepository
            _machinePlanningRepository;
        protected readonly IHttpClientService _http;

        public MachinesPlanningReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http = serviceProvider.GetService<IHttpClientService>();
            _storage = storage;
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _machinePlanningRepository =
                _storage.GetRepository<IMachinesPlanningRepository>();
        }

        protected SingleUnitResult GetUnit(int id)
        {
            var masterUnitUri = MasterDataSettings.Endpoint + $"master/units/{id}";
            var unitResponse = _http.GetAsync(masterUnitUri).Result;

            if (unitResponse.IsSuccessStatusCode)
            {
                SingleUnitResult unitResult = JsonConvert.DeserializeObject<SingleUnitResult>(unitResponse.Content.ReadAsStringAsync().Result);
                return unitResult;
            }
            else
            {
                return new SingleUnitResult();
            }
        }

        public async Task<(IEnumerable<MachinesPlanningReportListDto>, int)> GetReports(string machineId, 
                                                                                        string block, 
                                                                                        int unitId,
                                                                                        int page, 
                                                                                        int size, 
                                                                                        string order="{}")
        {
            try
            {
                //Add Shell (result) for Machine Planning Report Dto
                var result = new List<MachinesPlanningReportListDto>();

                //Query for Machine Planning Report
                var machinePlanningQuery =
                    _machinePlanningRepository
                        .Query
                        .AsQueryable();

                //Check if Unit Id Null
                //Instantiate New Value if Unit Id not 0
                await Task.Yield();
                if (unitId != 0)
                {
                    machinePlanningQuery = machinePlanningQuery.Where(x => x.UnitDepartementId.Value == unitId);
                }

                //Check if Machine Id Null
                await Task.Yield();
                if (!string.IsNullOrEmpty(machineId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(machineId, out Guid machineGuid))
                    {
                        machinePlanningQuery = machinePlanningQuery.Where(x => x.MachineId.Value == machineGuid);
                    }
                    else
                    {
                        return (result, result.Count);
                    }
                }

                //Check if Block not Empty String
                await Task.Yield();
                if (block != null)
                {
                    machinePlanningQuery = machinePlanningQuery.Where(x => x.Blok == block);
                }

                //Get Machine Planning Data from Machine Planning Repo
                var machinePlanningDocuments =
                   _machinePlanningRepository
                       .Find(machinePlanningQuery
                       .OrderByDescending(x => x.CreatedDate));

                foreach(var document in machinePlanningDocuments)
                {
                    //Get Weaving Unit
                    await Task.Yield();
                    var weavingUnitId = document.UnitDepartementId.Value;

                    SingleUnitResult unitData = GetUnit(weavingUnitId);
                    var weavingUnitName = unitData.data.Name;

                    //Get Machine Number
                    await Task.Yield();
                    var _machineId = document.MachineId.Value;
                    var machineQuery =
                        _machineRepository
                            .Query
                            .OrderByDescending(o => o.CreatedDate);
                    var machineDocument =
                        _machineRepository
                            .Find(machineQuery)
                            .Where(o => o.Identity.Equals(_machineId))
                            .FirstOrDefault();
                    var machineNumber = machineDocument.MachineNumber;

                    //Get Location
                    var machineLocation = machineDocument.Location;

                    //Get User Maintenance Name
                    await Task.Yield();
                    var maintenanceAndOperatorQuery =
                        _operatorRepository
                            .Query
                            .OrderByDescending(o => o.CreatedDate);

                    var userMaintenanceId = document.UserMaintenanceId.Value;
                    var userMaintenanceDocument =
                        _operatorRepository
                            .Find(maintenanceAndOperatorQuery)
                            .Where(o => o.Identity.ToString().Equals(userMaintenanceId))
                            .FirstOrDefault();
                    var userMaintenanceName = userMaintenanceDocument.CoreAccount.Name;

                    //Get User Operator Name
                    await Task.Yield();
                    var userOperatorId = document.UserOperatorId.Value;
                    var userOperatorDocument =
                        _operatorRepository
                            .Find(maintenanceAndOperatorQuery)
                            .Where(o => o.Identity.ToString().Equals(userOperatorId))
                            .FirstOrDefault();
                    var userOperatorName = userOperatorDocument.CoreAccount.Name;

                    //Instantiate Value to DailyOperationWarpingReportListDto
                    var machinePlanningReport = new MachinesPlanningReportListDto(document,
                                                                                  weavingUnitName,
                                                                                  machineNumber, 
                                                                                  machineLocation, 
                                                                                  userMaintenanceName, 
                                                                                  userOperatorName);

                    //Add DailyOperationWarpingDto to List of DailyOperationWarpingDto
                    result.Add(machinePlanningReport);
                }

                if (!order.Contains("{}"))
                {
                    Dictionary<string, string> orderDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              orderDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(MachinesPlanningReportListDto).GetProperty(key);

                    if (orderDictionary.Values.Contains("asc"))
                    {
                        result = result.OrderBy(x => prop.GetValue(x, null)).ToList();
                    }
                    else
                    {
                        result = result.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                    }
                }

                var pagedResult = result.Skip((page - 1) * size).Take(size);

                return (pagedResult, result.Count);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public async Task<IEnumerable<MachinesPlanningReportListDto>> GetAll()
        //{
        //    try
        //    {
        //        //Query for Machine Planning
        //        var machinePlanningQuery =
        //            _machinePlanningRepository
        //                .Query
        //                .OrderByDescending(o => o.CreatedDate);

        //        //Get Machine Planning Data from Machine Planning Repo
        //        await Task.Yield();
        //        var machinePlanningDocuments =
        //            _machinePlanningRepository
        //                .Find(machinePlanningQuery);
        //        var result = new List<MachinesPlanningReportListDto>();

        //        foreach (var document in machinePlanningDocuments)
        //        {
        //            //Get Weaving Unit Name
        //            await Task.Yield();
        //            var weavingUnitId = document.UnitDepartementId.Value;

        //            SingleUnitResult unitData = GetUnit(weavingUnitId);
        //            var weavingUnitName = unitData.data.Name;

        //            //Get Machine Number & Location
        //            await Task.Yield();
        //            var machineQuery =
        //                _machineRepository
        //                    .Query
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var machineDocument =
        //                _machineRepository
        //                    .Find(machineQuery)
        //                    .Where(o => o.Identity.Equals(document.MachineId.Value))
        //                    .FirstOrDefault();
        //            var machineNumber = machineDocument.MachineNumber ?? "Machine Number Not Found";
        //            var location = machineDocument.Location ?? "Location Not Found";

        //            //Get User Maintenance Name
        //            await Task.Yield();
        //            var userMaintenanceQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserMaintenanceId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userMaintenanceDocument =
        //                _operatorRepository 
        //                    .Find(userMaintenanceQuery)
        //                    .FirstOrDefault();
        //            var userMaintenanceName = userMaintenanceDocument.CoreAccount.Name;

        //            //Get User Operator Name
        //            await Task.Yield();
        //            var userOperatorQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserOperatorId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userOperatorDocument =
        //                _operatorRepository
        //                    .Find(userOperatorQuery)
        //                    .FirstOrDefault();
        //            var userOperatorName = userOperatorDocument.CoreAccount.Name;

        //            //Instantiate Value to MachinePlanningDto
        //            var machinePlanning = new MachinesPlanningReportListDto(document, weavingUnitName, machineNumber, location, userMaintenanceName, userOperatorName);

        //            //Add MachinePlanningDto to List of MachinePlanningDto
        //            result.Add(machinePlanning);
        //        }

        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<MachinesPlanningReportListDto>> GetByWeavingUnit(int weavingUnitId)
        //{
        //    try
        //    {
        //        //Query for Machine Planning
        //        var machinePlanningQuery =
        //            _machinePlanningRepository
        //                .Query
        //                .Where(o=>o.UnitDepartementId.Value.Equals(weavingUnitId))
        //                .OrderByDescending(o => o.CreatedDate);

        //        //Get Machine Planning Data from Machine Planning Repo
        //        await Task.Yield();
        //        var machinePlanningDocuments =
        //            _machinePlanningRepository
        //                .Find(machinePlanningQuery);
        //        var result = new List<MachinesPlanningReportListDto>();

        //        foreach (var document in machinePlanningDocuments)
        //        {
        //            //Get Weaving Unit Name
        //            await Task.Yield();
        //            var weavingUnit = document.UnitDepartementId.Value;

        //            SingleUnitResult unitData = GetUnit(weavingUnit);
        //            var weavingUnitName = unitData.data.Name;

        //            //Get Machine Number & Location
        //            await Task.Yield();
        //            var machineQuery =
        //                _machineRepository
        //                    .Query
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var machineDocument =
        //                _machineRepository
        //                    .Find(machineQuery)
        //                    .Where(o => o.Identity.Equals(document.MachineId.Value))
        //                    .FirstOrDefault();
        //            var machineNumber = machineDocument.MachineNumber ?? "Machine Number Not Found";
        //            var location = machineDocument.Location??"Location Not Found";

        //            //Get User Maintenance Name
        //            await Task.Yield();
        //            var userMaintenanceQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserMaintenanceId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userMaintenanceDocument =
        //                _operatorRepository
        //                    .Find(userMaintenanceQuery)
        //                    .FirstOrDefault();
        //            var userMaintenanceName = userMaintenanceDocument.CoreAccount.Name;

        //            //Get User Operator Name
        //            await Task.Yield();
        //            var userOperatorQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserOperatorId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userOperatorDocument =
        //                _operatorRepository
        //                    .Find(userOperatorQuery)
        //                    .FirstOrDefault();
        //            var userOperatorName = userOperatorDocument.CoreAccount.Name;

        //            //Instantiate Value to MachinePlanningDto
        //            var machinePlanning = new MachinesPlanningReportListDto(document, weavingUnitName, machineNumber, location, userMaintenanceName, userOperatorName);

        //            //Add MachinePlanningDto to List of MachinePlanningDto
        //            result.Add(machinePlanning);
        //        }

        //        return result;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<MachinesPlanningReportListDto>> GetByMachine(Guid machineId)
        //{
        //    try
        //    {
        //        //Query for Machine Planning
        //        var machinePlanningQuery =
        //            _machinePlanningRepository
        //                .Query
        //                .Where(o => o.MachineId.Value.Equals(machineId))
        //                .OrderByDescending(o => o.CreatedDate);

        //        //Get Machine Planning Data from Machine Planning Repo
        //        await Task.Yield();
        //        var machinePlanningDocuments =
        //            _machinePlanningRepository
        //                .Find(machinePlanningQuery);
        //        var result = new List<MachinesPlanningReportListDto>();

        //        foreach (var document in machinePlanningDocuments)
        //        {
        //            //Get Weaving Unit Name
        //            await Task.Yield();
        //            var weavingUnitId = document.UnitDepartementId.Value;

        //            SingleUnitResult unitData = GetUnit(weavingUnitId);
        //            var weavingUnitName = unitData.data.Name;

        //            //Get Machine Number & Location
        //            await Task.Yield();
        //            var machineQuery =
        //                _machineRepository
        //                    .Query
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var machineDocument =
        //                _machineRepository
        //                    .Find(machineQuery)
        //                    .Where(o => o.Identity.Equals(document.MachineId.Value))
        //                    .FirstOrDefault();
        //            var machineNumber = machineDocument.MachineNumber ?? "Machine Number Not Found";
        //            var location = machineDocument.Location ?? "Location Not Found";

        //            //Get User Maintenance Name
        //            await Task.Yield();
        //            var userMaintenanceQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserMaintenanceId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userMaintenanceDocument =
        //                _operatorRepository
        //                    .Find(userMaintenanceQuery)
        //                    .FirstOrDefault();
        //            var userMaintenanceName = userMaintenanceDocument.CoreAccount.Name;

        //            //Get User Operator Name
        //            await Task.Yield();
        //            var userOperatorQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserOperatorId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userOperatorDocument =
        //                _operatorRepository
        //                    .Find(userOperatorQuery)
        //                    .FirstOrDefault();
        //            var userOperatorName = userOperatorDocument.CoreAccount.Name;

        //            //Instantiate Value to MachinePlanningDto
        //            var machinePlanning = new MachinesPlanningReportListDto(document, weavingUnitName, machineNumber, location, userMaintenanceName, userOperatorName);

        //            //Add MachinePlanningDto to List of MachinePlanningDto
        //            result.Add(machinePlanning);
        //        }

        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<MachinesPlanningReportListDto>> GetByBlock(string block)
        //{
        //    try
        //    {
        //        //Query for Machine Planning
        //        var machinePlanningQuery =
        //            _machinePlanningRepository
        //                .Query
        //                .Where(o => o.Blok.Equals(block))
        //                .OrderByDescending(o => o.CreatedDate);

        //        //Get Machine Planning Data from Machine Planning Repo
        //        await Task.Yield();
        //        var machinePlanningDocuments =
        //            _machinePlanningRepository
        //                .Find(machinePlanningQuery);
        //        var result = new List<MachinesPlanningReportListDto>();

        //        foreach (var document in machinePlanningDocuments)
        //        {
        //            //Get Weaving Unit Name
        //            await Task.Yield();
        //            var weavingUnitId = document.UnitDepartementId.Value;

        //            SingleUnitResult unitData = GetUnit(weavingUnitId);
        //            var weavingUnitName = unitData.data.Name;

        //            //Get Machine Number & Location
        //            await Task.Yield();
        //            var machineQuery =
        //                _machineRepository
        //                    .Query
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var machineDocument =
        //                _machineRepository
        //                    .Find(machineQuery)
        //                    .Where(o => o.Identity.Equals(document.MachineId.Value))
        //                    .FirstOrDefault();
        //            var machineNumber = machineDocument.MachineNumber ?? "Machine Number Not Found";
        //            var location = machineDocument.Location ?? "Location Not Found";

        //            //Get User Maintenance Name
        //            await Task.Yield();
        //            var userMaintenanceQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserMaintenanceId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userMaintenanceDocument =
        //                _operatorRepository
        //                    .Find(userMaintenanceQuery)
        //                    .FirstOrDefault();
        //            var userMaintenanceName = userMaintenanceDocument.CoreAccount.Name;

        //            //Get User Operator Name
        //            await Task.Yield();
        //            var userOperatorQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserOperatorId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userOperatorDocument =
        //                _operatorRepository
        //                    .Find(userOperatorQuery)
        //                    .FirstOrDefault();
        //            var userOperatorName = userOperatorDocument.CoreAccount.Name;

        //            //Instantiate Value to MachinePlanningDto
        //            var machinePlanning = new MachinesPlanningReportListDto(document, weavingUnitName, machineNumber, location, userMaintenanceName, userOperatorName);

        //            //Add MachinePlanningDto to List of MachinePlanningDto
        //            result.Add(machinePlanning);
        //        }

        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<MachinesPlanningReportListDto>> GetByWeavingUnitMachine(int weavingUnitId, Guid machineId)
        //{
        //    try
        //    {
        //        //Query for Machine Planning
        //        var machinePlanningQuery =
        //            _machinePlanningRepository
        //                .Query
        //                .Where(o => o.UnitDepartementId.Value.Equals(weavingUnitId) && o.MachineId.Value.Equals(machineId))
        //                .OrderByDescending(o => o.CreatedDate);

        //        //Get Machine Planning Data from Machine Planning Repo
        //        await Task.Yield();
        //        var machinePlanningDocuments =
        //            _machinePlanningRepository
        //                .Find(machinePlanningQuery);
        //        var result = new List<MachinesPlanningReportListDto>();

        //        foreach (var document in machinePlanningDocuments)
        //        {
        //            //Get Weaving Unit Name
        //            await Task.Yield();
        //            var weavingUnit = document.UnitDepartementId.Value;

        //            SingleUnitResult unitData = GetUnit(weavingUnit);
        //            var weavingUnitName = unitData.data.Name;

        //            //Get Machine Number & Location
        //            await Task.Yield();
        //            var machineQuery =
        //                _machineRepository
        //                    .Query
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var machineDocument =
        //                _machineRepository
        //                    .Find(machineQuery)
        //                    .Where(o => o.Identity.Equals(document.MachineId.Value))
        //                    .FirstOrDefault();
        //            var machineNumber = machineDocument.MachineNumber ?? "Machine Number Not Found";
        //            var location = machineDocument.Location ?? "Location Not Found";

        //            //Get User Maintenance Name
        //            await Task.Yield();
        //            var userMaintenanceQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserMaintenanceId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userMaintenanceDocument =
        //                _operatorRepository
        //                    .Find(userMaintenanceQuery)
        //                    .FirstOrDefault();
        //            var userMaintenanceName = userMaintenanceDocument.CoreAccount.Name;

        //            //Get User Operator Name
        //            await Task.Yield();
        //            var userOperatorQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserOperatorId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userOperatorDocument =
        //                _operatorRepository
        //                    .Find(userOperatorQuery)
        //                    .FirstOrDefault();
        //            var userOperatorName = userOperatorDocument.CoreAccount.Name;

        //            //Instantiate Value to MachinePlanningDto
        //            var machinePlanning = new MachinesPlanningReportListDto(document, weavingUnitName, machineNumber, location, userMaintenanceName, userOperatorName);

        //            //Add MachinePlanningDto to List of MachinePlanningDto
        //            result.Add(machinePlanning);
        //        }

        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<MachinesPlanningReportListDto>> GetByWeavingUnitBlock(int weavingUnitId, string block)
        //{
        //    try
        //    {
        //        //Query for Machine Planning
        //        var machinePlanningQuery =
        //            _machinePlanningRepository
        //                .Query
        //                .Where(o => o.UnitDepartementId.Value.Equals(weavingUnitId) && o.Blok.Equals(block))
        //                .OrderByDescending(o => o.CreatedDate);

        //        //Get Machine Planning Data from Machine Planning Repo
        //        await Task.Yield();
        //        var machinePlanningDocuments =
        //            _machinePlanningRepository
        //                .Find(machinePlanningQuery);
        //        var result = new List<MachinesPlanningReportListDto>();

        //        foreach (var document in machinePlanningDocuments)
        //        {
        //            //Get Weaving Unit Name
        //            await Task.Yield();
        //            var weavingUnit = document.UnitDepartementId.Value;

        //            SingleUnitResult unitData = GetUnit(weavingUnit);
        //            var weavingUnitName = unitData.data.Name;

        //            //Get Machine Number & Location
        //            await Task.Yield();
        //            var machineQuery =
        //                _machineRepository
        //                    .Query
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var machineDocument =
        //                _machineRepository
        //                    .Find(machineQuery)
        //                    .Where(o => o.Identity.Equals(document.MachineId.Value))
        //                    .FirstOrDefault();
        //            var machineNumber = machineDocument.MachineNumber ?? "Machine Number Not Found";
        //            var location = machineDocument.Location ?? "Location Not Found";

        //            //Get User Maintenance Name
        //            await Task.Yield();
        //            var userMaintenanceQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserMaintenanceId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userMaintenanceDocument =
        //                _operatorRepository
        //                    .Find(userMaintenanceQuery)
        //                    .FirstOrDefault();
        //            var userMaintenanceName = userMaintenanceDocument.CoreAccount.Name;

        //            //Get User Operator Name
        //            await Task.Yield();
        //            var userOperatorQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserOperatorId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userOperatorDocument =
        //                _operatorRepository
        //                    .Find(userOperatorQuery)
        //                    .FirstOrDefault();
        //            var userOperatorName = userOperatorDocument.CoreAccount.Name;

        //            //Instantiate Value to MachinePlanningDto
        //            var machinePlanning = new MachinesPlanningReportListDto(document, weavingUnitName, machineNumber, location, userMaintenanceName, userOperatorName);

        //            //Add MachinePlanningDto to List of MachinePlanningDto
        //            result.Add(machinePlanning);
        //        }

        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<MachinesPlanningReportListDto>> GetByMachineBlock(Guid machineId, string block)
        //{
        //    try
        //    {
        //        //Query for Machine Planning
        //        var machinePlanningQuery =
        //            _machinePlanningRepository
        //                .Query
        //                .Where(o => o.MachineId.Value.Equals(machineId) && o.Blok.Equals(block))
        //                .OrderByDescending(o => o.CreatedDate);

        //        //Get Machine Planning Data from Machine Planning Repo
        //        await Task.Yield();
        //        var machinePlanningDocuments =
        //            _machinePlanningRepository
        //                .Find(machinePlanningQuery);
        //        var result = new List<MachinesPlanningReportListDto>();

        //        foreach (var document in machinePlanningDocuments)
        //        {
        //            //Get Weaving Unit Name
        //            await Task.Yield();
        //            var weavingUnitId = document.UnitDepartementId.Value;

        //            SingleUnitResult unitData = GetUnit(weavingUnitId);
        //            var weavingUnitName = unitData.data.Name;

        //            //Get Machine Number & Location
        //            await Task.Yield();
        //            var machineQuery =
        //                _machineRepository
        //                    .Query
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var machineDocument =
        //                _machineRepository
        //                    .Find(machineQuery)
        //                    .Where(o => o.Identity.Equals(document.MachineId.Value))
        //                    .FirstOrDefault();
        //            var machineNumber = machineDocument.MachineNumber ?? "Machine Number Not Found";
        //            var location = machineDocument.Location ?? "Location Not Found";

        //            //Get User Maintenance Name
        //            await Task.Yield();
        //            var userMaintenanceQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserMaintenanceId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userMaintenanceDocument =
        //                _operatorRepository
        //                    .Find(userMaintenanceQuery)
        //                    .FirstOrDefault();
        //            var userMaintenanceName = userMaintenanceDocument.CoreAccount.Name;

        //            //Get User Operator Name
        //            await Task.Yield();
        //            var userOperatorQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserOperatorId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userOperatorDocument =
        //                _operatorRepository
        //                    .Find(userOperatorQuery)
        //                    .FirstOrDefault();
        //            var userOperatorName = userOperatorDocument.CoreAccount.Name;

        //            //Instantiate Value to MachinePlanningDto
        //            var machinePlanning = new MachinesPlanningReportListDto(document, weavingUnitName, machineNumber, location, userMaintenanceName, userOperatorName);

        //            //Add MachinePlanningDto to List of MachinePlanningDto
        //            result.Add(machinePlanning);
        //        }

        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<MachinesPlanningReportListDto>> GetAllSpecified(int weavingUnitId, Guid machineId, string block)
        //{
        //    try
        //    {
        //        //Query for Machine Planning
        //        var machinePlanningQuery =
        //            _machinePlanningRepository
        //                .Query
        //                .Where(o => o.UnitDepartementId.Value.Equals(weavingUnitId)&& o.MachineId.Value.Equals(machineId) && o.Blok.Equals(block))
        //                .OrderByDescending(o => o.CreatedDate);

        //        //Get Machine Planning Data from Machine Planning Repo
        //        await Task.Yield();
        //        var machinePlanningDocuments =
        //            _machinePlanningRepository
        //                .Find(machinePlanningQuery);
        //        var result = new List<MachinesPlanningReportListDto>();

        //        foreach (var document in machinePlanningDocuments)
        //        {
        //            //Get Weaving Unit Name
        //            await Task.Yield();
        //            var weavingUnit = document.UnitDepartementId.Value;

        //            SingleUnitResult unitData = GetUnit(weavingUnit);
        //            var weavingUnitName = unitData.data.Name;

        //            //Get Machine Number & Location
        //            await Task.Yield();
        //            var machineQuery =
        //                _machineRepository
        //                    .Query
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var machineDocument =
        //                _machineRepository
        //                    .Find(machineQuery)
        //                    .Where(o => o.Identity.Equals(document.MachineId.Value))
        //                    .FirstOrDefault();
        //            var machineNumber = machineDocument.MachineNumber ?? "Machine Number Not Found";
        //            var location = machineDocument.Location ?? "Location Not Found";

        //            //Get User Maintenance Name
        //            await Task.Yield();
        //            var userMaintenanceQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserMaintenanceId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userMaintenanceDocument =
        //                _operatorRepository
        //                    .Find(userMaintenanceQuery)
        //                    .FirstOrDefault();
        //            var userMaintenanceName = userMaintenanceDocument.CoreAccount.Name;

        //            //Get User Operator Name
        //            await Task.Yield();
        //            var userOperatorQuery =
        //                _operatorRepository
        //                    .Query
        //                    .Where(o => o.Identity.ToString().Equals(document.UserOperatorId.Value))
        //                    .OrderByDescending(o => o.CreatedDate);

        //            await Task.Yield();
        //            var userOperatorDocument =
        //                _operatorRepository
        //                    .Find(userOperatorQuery)
        //                    .FirstOrDefault();
        //            var userOperatorName = userOperatorDocument.CoreAccount.Name;

        //            //Instantiate Value to MachinePlanningDto
        //            var machinePlanning = new MachinesPlanningReportListDto(document, weavingUnitName, machineNumber, location, userMaintenanceName, userOperatorName);

        //            //Add MachinePlanningDto to List of MachinePlanningDto
        //            result.Add(machinePlanning);
        //        }

        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}
