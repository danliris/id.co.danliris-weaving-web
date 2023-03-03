using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Loom.Repositories
{
    public class DailyOperationLoomRepository : AggregateRepostory<DailyOperationLoomDocument, DailyOperationLoomDocumentReadModel>, IDailyOperationLoomRepository
    {
        protected override DailyOperationLoomDocument Map(DailyOperationLoomDocumentReadModel readModel)
        {
            return new DailyOperationLoomDocument(readModel);
        }
    }
}
