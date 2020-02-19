using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Loom.Repositories
{
    public class DailyOperationLoomBeamProductRepository : AggregateRepostory<DailyOperationLoomBeamProduct, DailyOperationLoomBeamProductReadModel>, IDailyOperationLoomBeamProductRepository
    {

        protected override DailyOperationLoomBeamProduct Map(DailyOperationLoomBeamProductReadModel readModel)
        {
            return new DailyOperationLoomBeamProduct(readModel);
        }
    }
}
