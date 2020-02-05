using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Repositories
{
    public interface IDailyOperationLoomBeamHistoryRepository : IAggregateRepository<DailyOperationLoomBeamHistory, DailyOperationLoomBeamHistoryReadModel>
    {
    }
}
