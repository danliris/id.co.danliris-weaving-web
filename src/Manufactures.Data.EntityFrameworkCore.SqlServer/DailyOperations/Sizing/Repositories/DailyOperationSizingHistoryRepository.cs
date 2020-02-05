using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Sizing.Repositories
{
    public class DailyOperationSizingHistoryRepository : AggregateRepostory<DailyOperationSizingHistory, DailyOperationSizingHistoryReadModel>, IDailyOperationSizingHistoryRepository
    {
        protected override DailyOperationSizingHistory Map(DailyOperationSizingHistoryReadModel readModel)
        {
            return new DailyOperationSizingHistory(readModel);
        }
    }
}
