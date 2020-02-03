using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.Repositories
{
    public interface IDailyOperationWarpingBrokenCauseRepository
        : IAggregateRepository<DailyOperationWarpingBrokenCause,
                               DailyOperationWarpingBrokenCauseReadModel>
    {
    }
}
