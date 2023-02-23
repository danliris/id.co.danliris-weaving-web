using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manufactures.Domain.MachinesPlanning.Queries
{
    public interface IMachinesPlanningReportQuery<TModel>
    {
        Task<(IEnumerable<TModel>, int)> GetReports(string machineId,
                                                    string block,
                                                    int unitId,
                                                    int page,
                                                    int size,
                                                    string order);
        //Task<IEnumerable<TModel>> GetAll();
        //Task<IEnumerable<TModel>> GetByWeavingUnit(int weavingUnitId);
        //Task<IEnumerable<TModel>> GetByMachine(Guid machineId);
        //Task<IEnumerable<TModel>> GetByBlock(string block);
        //Task<IEnumerable<TModel>> GetByWeavingUnitMachine(int weavingUnitId, Guid machineId);
        //Task<IEnumerable<TModel>> GetByWeavingUnitBlock(int weavingUnitId, string block);
        //Task<IEnumerable<TModel>> GetByMachineBlock(Guid machineId, string block);
        //Task<IEnumerable<TModel>> GetAllSpecified(int weavingUnitId, Guid machineId, string block);
    }
}
