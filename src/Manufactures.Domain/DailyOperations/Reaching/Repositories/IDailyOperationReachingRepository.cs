using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;

namespace Manufactures.Domain.DailyOperations.Reaching.Repositories
{
    public interface IDailyOperationReachingRepository: IAggregateRepository<DailyOperationReachingDocument, DailyOperationReachingReadModel>
    {
    }
}
