using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperationalMachines.Repositories
{
    public class DailyOperationalMachineRepository : AggregateRepostory<DailyOperationalLoomDocument, DailyOperationalMachineLoomReadModel>, IDailyOperationalLoomRepository
    {
        protected override DailyOperationalLoomDocument Map(DailyOperationalMachineLoomReadModel readModel)
        {
            return new DailyOperationalLoomDocument(readModel);
        }
    }
}
