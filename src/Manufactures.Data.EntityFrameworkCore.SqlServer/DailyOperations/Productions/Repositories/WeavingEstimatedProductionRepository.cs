using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Productions.ReadModels;
using Manufactures.Domain.DailyOperations.WeavingDailyOperationMachineSizing.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Productions.Repositories
{
    public class WeavingDailyOperationMachineSizingRepository : AggregateRepostory<WeavingDailyOperationMachineSizings, WeavingDailyOperationMachineSizingReadModel>, IWeavingDailyOperationMachineSizingRepository
    {
        protected override WeavingDailyOperationMachineSizings Map(WeavingDailyOperationMachineSizingReadModel readModel)
        {
            return new WeavingDailyOperationMachineSizings(readModel);
        }
    }
}
