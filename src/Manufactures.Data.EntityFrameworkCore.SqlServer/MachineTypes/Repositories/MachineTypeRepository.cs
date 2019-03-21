using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.MachineTypes;
using Manufactures.Domain.MachineTypes.ReadModels;
using Manufactures.Domain.MachineTypes.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.MachineTypes.Repositories
{
    public class MachineTypeRepository : AggregateRepostory<MachineTypeDocument, MachineTypeReadModel>, IMachineTypeRepository
    {
        protected override MachineTypeDocument Map(MachineTypeReadModel readModel)
        {
            return new MachineTypeDocument(readModel);
        }
    }
}
