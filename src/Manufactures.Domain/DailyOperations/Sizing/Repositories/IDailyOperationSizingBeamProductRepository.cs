using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;

namespace Manufactures.Domain.DailyOperations.Sizing.Repositories
{
    public interface IDailyOperationSizingBeamProductRepository : IAggregateRepository<DailyOperationSizingBeamProduct, DailyOperationSizingBeamProductReadModel>
    {
    }
}
