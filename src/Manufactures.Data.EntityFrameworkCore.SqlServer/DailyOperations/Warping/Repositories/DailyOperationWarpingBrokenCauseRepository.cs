using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Warping.Repositories
{
    public class DailyOperationWarpingBrokenCauseRepository : AggregateRepostory<DailyOperationWarpingBrokenCause, DailyOperationWarpingBrokenCauseReadModel>, IDailyOperationWarpingBrokenCauseRepository
    {
        protected override DailyOperationWarpingBrokenCause Map(DailyOperationWarpingBrokenCauseReadModel readModel)
        {
            return new DailyOperationWarpingBrokenCause(readModel);
        }
    }
}
