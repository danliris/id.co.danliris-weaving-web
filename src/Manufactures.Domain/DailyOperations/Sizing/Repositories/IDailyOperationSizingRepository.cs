using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;

namespace Manufactures.Domain.DailyOperations.Sizing.Repositories
{
    public interface IDailyOperationSizingRepository : IAggregateRepository<DailyOperationSizingDocument, DailyOperationSizingReadModel>
    {
    }
}
