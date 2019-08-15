using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Reaching.Repositories
{
    public class DailyOperationReachingRepository : AggregateRepostory<DailyOperationReachingDocument, DailyOperationReachingReadModel>, IDailyOperationReachingRepository
    {
        protected override DailyOperationReachingDocument Map(DailyOperationReachingReadModel readModel)
        {
            return new DailyOperationReachingDocument(readModel);
        }
    }
}
