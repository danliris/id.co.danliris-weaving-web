using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;

namespace Manufactures.Domain.DailyOperations.Loom.Repositories
{
    public interface IDailyOperationLoomRepository : IAggregateRepository<DailyOperationLoomDocument, DailyOperationMachineLoomReadModel>
    {
    }
}
