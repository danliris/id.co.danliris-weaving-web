using Infrastructure.Domain.Repositories;

using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Entities;


namespace Manufactures.Domain.DailyOperations.Spu.Repositories
{
    public interface IWeavingDailyOperationSpuMachineRepository
     : IAggregateRepository<WeavingDailyOperationWarpingMachine,
                               WeavingDailyOperationWarpingMachineReadModel>
    {
    }
}
