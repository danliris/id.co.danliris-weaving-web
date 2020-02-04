using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Reaching.Repositories
{
    public class DailyOperationReachingHistoryRepository : AggregateRepostory<DailyOperationReachingHistory, DailyOperationReachingHistoryReadModel>, IDailyOperationReachingHistoryRepository
    {
        protected override DailyOperationReachingHistory Map(DailyOperationReachingHistoryReadModel readModel)
        {
            return new DailyOperationReachingHistory(readModel);
        }
    }
}
