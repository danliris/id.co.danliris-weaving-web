using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.Queries;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Reaching.Repositories
{
    public class DailyOperationMachineReachingRepository : AggregateRepostory<DailyOperationMachineReaching, DailyOperationMachineReachingReadModel>, IDailyOperationReachingMachineRepository
    {
        protected override DailyOperationMachineReaching Map(DailyOperationMachineReachingReadModel readModel)
        {
            return new DailyOperationMachineReaching(readModel);
        }
    }
}
