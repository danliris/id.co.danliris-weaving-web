using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Machines;
using Manufactures.Domain.Machines.ReadModels;
using Manufactures.Domain.Machines.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.Machines.Repositories
{
    public class MachineRepository : AggregateRepostory<MachineDocument, MachineDocumentReadModel>, IMachineRepository
    {
        protected override MachineDocument Map(MachineDocumentReadModel readModel)
        {
            return new MachineDocument(readModel);
        }
    }
}
