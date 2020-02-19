using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Loom.Repositories
{
    public class DailyOperationLoomBeamHistoryRepository : AggregateRepostory<DailyOperationLoomBeamHistory, DailyOperationLoomBeamHistoryReadModel>, IDailyOperationLoomBeamHistoryRepository
    {
        protected override DailyOperationLoomBeamHistory Map(DailyOperationLoomBeamHistoryReadModel readModel)
        {
            return new DailyOperationLoomBeamHistory(readModel);
        }
    }
}
