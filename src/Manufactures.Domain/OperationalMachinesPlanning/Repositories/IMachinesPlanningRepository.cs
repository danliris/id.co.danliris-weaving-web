using Infrastructure.Domain.Repositories;
using Manufactures.Domain.OperationalMachinesPlanning.ReadModels;

namespace Manufactures.Domain.OperationalMachinesPlanning.Repositories
{
    public interface IMachinesPlanningRepository : IAggregateRepository<MachinesPlanningDocument, MachinesPlanningReadModel>
    {
    }
}
