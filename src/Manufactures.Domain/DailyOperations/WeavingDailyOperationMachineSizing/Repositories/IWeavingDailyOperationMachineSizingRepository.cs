using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Productions.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.WeavingDailyOperationMachineSizing.Repositories
{
    public interface IWeavingDailyOperationMachineSizingRepository 
        : IAggregateRepository<WeavingDailyOperationMachineSizings,
        WeavingDailyOperationMachineSizingReadModel>
    {
    }
}
