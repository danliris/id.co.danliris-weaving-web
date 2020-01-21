using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;

namespace Manufactures.Domain.DailyOperations.Warping.Repositories
{
    public interface IDailyOperationWarpingRepository
        : IAggregateRepository<DailyOperationWarpingDocument, 
                               DailyOperationWarpingDocumentReadModel>
    {
    }
}
