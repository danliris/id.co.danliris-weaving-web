using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Reaching.Repositories
{
    public class DailyOperationReachingTyingRepository : AggregateRepostory<DailyOperationReachingTyingDocument, DailyOperationReachingTyingReadModel>, IDailyOperationReachingTyingRepository
    {
        protected override DailyOperationReachingTyingDocument Map(DailyOperationReachingTyingReadModel readModel)
        {
            return new DailyOperationReachingTyingDocument(readModel);
        }
    }
}
