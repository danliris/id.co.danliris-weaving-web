using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.MachinesPlanning;
using Manufactures.Domain.MachinesPlanning.ReadModels;
using Manufactures.Domain.MachinesPlanning.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.MachinesPlanning.Repositories
{
    public class EnginePlanningRepository : AggregateRepostory<MachinesPlanningDocument, MachinesPlanningReadModel>, IMachinesPlanningRepository
    {
        protected override MachinesPlanningDocument Map(MachinesPlanningReadModel readModel)
        {
            return new MachinesPlanningDocument(readModel);
        }
    }
}
