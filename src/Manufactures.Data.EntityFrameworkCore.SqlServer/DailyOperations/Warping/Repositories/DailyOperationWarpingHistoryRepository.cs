using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Warping.Repositories
{
    public class DailyOperationWarpingHistoryRepository : AggregateRepostory<DailyOperationWarpingHistory, DailyOperationWarpingHistoryReadModel>, IDailyOperationWarpingHistoryRepository
    {
        protected override DailyOperationWarpingHistory Map(DailyOperationWarpingHistoryReadModel readModel)
        {
            return new DailyOperationWarpingHistory(readModel);
        }
    }
}
