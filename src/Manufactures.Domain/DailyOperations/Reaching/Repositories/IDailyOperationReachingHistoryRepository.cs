using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.Repositories
{
    public interface IDailyOperationReachingHistoryRepository : IAggregateRepository<DailyOperationReachingHistory, DailyOperationReachingHistoryReadModel>
    {
    }
}
