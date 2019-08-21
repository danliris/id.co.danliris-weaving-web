using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.ReachingTying;
using Manufactures.Domain.DailyOperations.ReachingTying.ReadModels;
using Manufactures.Domain.DailyOperations.ReachingTying.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.ReachingTying.Repositories
{
    public class DailyOperationReachingTyingRepository : AggregateRepostory<DailyOperationReachingTyingDocument, DailyOperationReachingTyingReadModel>, IDailyOperationReachingTyingRepository
    {
        protected override DailyOperationReachingTyingDocument Map(DailyOperationReachingTyingReadModel readModel)
        {
            return new DailyOperationReachingTyingDocument(readModel);
        }
    }
}
