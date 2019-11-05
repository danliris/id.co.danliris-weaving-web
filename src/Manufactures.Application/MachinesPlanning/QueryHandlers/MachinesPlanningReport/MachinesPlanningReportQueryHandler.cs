using ExtCore.Data.Abstractions;
using Manufactures.Application.MachinesPlanning.DataTransferObjects;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachinesPlanning.Queries;
using Manufactures.Domain.MachinesPlanning.Repositories;
using Manufactures.Domain.Operators.Repositories;
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

        public MachinesPlanningReportQueryHandler(IStorage storage)
        {
            _storage = storage;
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _machinePlanningRepository =
                _storage.GetRepository<IMachinesPlanningRepository>();
        }

        public async Task<IEnumerable<MachinesPlanningReportListDto>> GetAll()
        {
            try
            {
                //Query for Machine Planning
                var machinePlanningQuery =
                    _machinePlanningRepository
                        .Query
                        .OrderByDescending(o => o.CreatedDate);

                //Get Machine Planning Data from Machine Planning
                await Task.Yield();
                var machinePlanningDocuments =
                    _machinePlanningRepository
                        .Find(machinePlanningQuery);
                var result = new List<MachinesPlanningReportListDto>();

                foreach (var document in machinePlanningDocuments)
                {
                    await Task.Yield();
                    var weavingUnit = document.UnitDepartementId.Value;

                    var machineQuery =
                        _machineRepository
                            .Query
                            .OrderByDescending(o => o.CreatedDate);

                    await Task.Yield();
                    var machineDocument =
                        _machineRepository
                            .Find(machineQuery)
                            .Where(o => o.Identity.Equals(document.MachineId.Value))
                            .FirstOrDefault();
                    var machineNumber = machineDocument.MachineNumber ?? "Machine Number Not Found";
                    var location = machineDocument.Location ?? "Location Not Found";

                    await Task.Yield();
                    var userMaintenanceQuery =
                        _operatorRepository
                            .Query
                            .Where(o => o.Identity.ToString().Equals(document.UserMaintenanceId.Value))
                            .OrderByDescending(o => o.CreatedDate);

                    await Task.Yield();
                    var userMaintenanceDocument =
                        _operatorRepository 
                            .Find(userMaintenanceQuery)
                            .FirstOrDefault();
                    var userMaintenanceName = userMaintenanceDocument.CoreAccount.Name;


                    await Task.Yield();
                    var userOperatorQuery =
                        _operatorRepository
                            .Query
                            .Where(o => o.Identity.ToString().Equals(document.UserOperatorId.Value))
                            .OrderByDescending(o => o.CreatedDate);

                    await Task.Yield();
                    var userOperatorDocument =
                        _operatorRepository
                            .Find(userOperatorQuery)
                            .FirstOrDefault();
                    var userOperatorName = userOperatorDocument.CoreAccount.Name;

                    //Instantiate Value to MachinePlanningDto
                    var machinePlanning = new MachinesPlanningReportListDto(document, weavingUnit, machineNumber, location, userMaintenanceName, userOperatorName);

                    //Add MachinePlanningDto to List of MachinePlanningDto
                    result.Add(machinePlanning);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<MachinesPlanningReportListDto>> GetByWeavingUnit(int weavingUnitId)
        {
            try
            {
                //Query for Machine Planning
                var machinePlanningQuery =
                    _machinePlanningRepository
                        .Query
                        .Where(o=>o.UnitDepartementId.Value.Equals(weavingUnitId))
                        .OrderByDescending(o => o.CreatedDate);

                //Get Machine Planning Data from Machine Planning
                await Task.Yield();
                var machinePlanningDocuments =
                    _machinePlanningRepository
                        .Find(machinePlanningQuery);
                var result = new List<MachinesPlanningReportListDto>();

                foreach (var document in machinePlanningDocuments)
                {
                    await Task.Yield();
                    var weavingUnit = document.UnitDepartementId.Value;

                    var machineQuery =
                        _machineRepository
                            .Query
                            .OrderByDescending(o => o.CreatedDate);

                    await Task.Yield();
                    var machineDocument =
                        _machineRepository
                            .Find(machineQuery)
                            .Where(o => o.Identity.Equals(document.MachineId.Value))
                            .FirstOrDefault();
                    var machineNumber = machineDocument.MachineNumber ?? "Machine Number Not Found";
                    var location = machineDocument.Location??"Location Not Found";

                    await Task.Yield();
                    var userMaintenanceQuery =
                        _operatorRepository
                            .Query
                            .Where(o => o.Identity.ToString().Equals(document.UserMaintenanceId.Value))
                            .OrderByDescending(o => o.CreatedDate);

                    await Task.Yield();
                    var userMaintenanceDocument =
                        _operatorRepository
                            .Find(userMaintenanceQuery)
                            .FirstOrDefault();
                    var userMaintenanceName = userMaintenanceDocument.CoreAccount.Name;


                    await Task.Yield();
                    var userOperatorQuery =
                        _operatorRepository
                            .Query
                            .Where(o => o.Identity.ToString().Equals(document.UserOperatorId.Value))
                            .OrderByDescending(o => o.CreatedDate);

                    await Task.Yield();
                    var userOperatorDocument =
                        _operatorRepository
                            .Find(userOperatorQuery)
                            .FirstOrDefault();
                    var userOperatorName = userOperatorDocument.CoreAccount.Name;

                    //Instantiate Value to MachinePlanningDto
                    var machinePlanning = new MachinesPlanningReportListDto(document, weavingUnit, machineNumber, location, userMaintenanceName, userOperatorName);

                    //Add MachinePlanningDto to List of MachinePlanningDto
                    result.Add(machinePlanning);
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<IEnumerable<MachinesPlanningReportListDto>> GetByMachine(Guid machineId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MachinesPlanningReportListDto>> GetByBlock(string block)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MachinesPlanningReportListDto>> GetByWeavingUnitMachine(int weavingUnitId, Guid machineId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MachinesPlanningReportListDto>> GetByWeavingUnitBlock(int weavingUnitId, string block)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MachinesPlanningReportListDto>> GetByMachineBlock(Guid machineId, string block)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MachinesPlanningReportListDto>> GetAllSpecified(int weavingUnitId, Guid machineId, string block)
        {
            throw new NotImplementedException();
        }
    }
}
