using Infrastructure.Domain.Repositories;
using Manufactures.Domain.MachineTypes.ReadModels;

namespace Manufactures.Domain.MachineTypes.Repositories
{
    public interface IMachineTypeRepository : IAggregateRepository<MachineTypeDocument, MachineTypeReadModel>
    {
    }
}
