using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.ReachingTying.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ReachingTying.Repositories
{
    public interface IDailyOperationReachingTyingRepository: IAggregateRepository<DailyOperationReachingTyingDocument, DailyOperationReachingTyingReadModel>
    {
    }
}
