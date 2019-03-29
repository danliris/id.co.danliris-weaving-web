using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.OperationalMachinesPlanning;
using Manufactures.Domain.OperationalMachinesPlanning.ReadModels;
using Manufactures.Domain.OperationalMachinesPlanning.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.OperationalEnginePlanning.Repositories
{
    public class EnginePlanningRepository : AggregateRepostory<MachinesPlanningDocument, MachinesPlanningReadModel>, IMachinesPlanningRepository
    {
        protected override MachinesPlanningDocument Map(MachinesPlanningReadModel readModel)
        {
            return new MachinesPlanningDocument(readModel);
        }
    }
}
