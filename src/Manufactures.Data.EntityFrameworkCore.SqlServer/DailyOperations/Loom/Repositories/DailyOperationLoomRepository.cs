using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Loom.Repositories
{
    public class DailyOperationLoomRepository : AggregateRepostory<DailyOperationalLoomDocument, DailyOperationalMachineLoomReadModel>, IDailyOperationalLoomRepository
    {
        protected override DailyOperationalLoomDocument Map(DailyOperationalMachineLoomReadModel readModel)
        {
            return new DailyOperationalLoomDocument(readModel);
        }
    }
}
