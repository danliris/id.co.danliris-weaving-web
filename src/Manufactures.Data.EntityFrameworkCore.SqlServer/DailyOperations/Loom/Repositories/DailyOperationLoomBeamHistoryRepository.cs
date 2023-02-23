using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Loom.Repositories
{
    public class DailyOperationLoomBeamHistoryRepository : AggregateRepostory<DailyOperationLoomHistory, DailyOperationLoomHistoryReadModel>, IDailyOperationLoomHistoryRepository
    {
        protected override DailyOperationLoomHistory Map(DailyOperationLoomHistoryReadModel readModel)
        {
            return new DailyOperationLoomHistory(readModel);
        }
    }
}
