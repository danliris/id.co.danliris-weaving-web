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
    public class DailyOperationSizingBeamsWarpingRepository : AggregateRepostory<DailyOperationSizingBeamsWarping, DailyOperationSizingBeamsWarpingReadModel>, IDailyOperationSizingBeamsWarpingRepository
    {
        protected override DailyOperationSizingBeamsWarping Map(DailyOperationSizingBeamsWarpingReadModel readModel)
        {
            return new DailyOperationSizingBeamsWarping(readModel);
        }
    }
}
