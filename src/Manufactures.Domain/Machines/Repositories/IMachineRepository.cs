using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Machines.ReadModels;

namespace Manufactures.Domain.Machines.Repositories
{
    public interface IMachineRepository : IAggregateRepository<MachineDocument, MachineDocumentReadModel>
    {
    }
}
