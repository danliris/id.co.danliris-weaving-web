using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Spu.Entities;
using Manufactures.Domain.DailyOperations.Spu.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Spu.Repositories
{
    public interface IWeavingDailyOperationSpuMachineRepository
     : IAggregateRepository<WeavingDailyOperationWarpingMachine,
                               WeavingDailyOperationWarpingMachineReadModel>
    {
    }
}
