using Infrastructure.Domain.Repositories;
using Manufactures.Domain.MachinesPlanning.ReadModels;

namespace Manufactures.Domain.MachinesPlanning.Repositories
{
    public interface IMachinesPlanningRepository : IAggregateRepository<MachinesPlanningDocument, MachinesPlanningReadModel>
    {
    }
}
