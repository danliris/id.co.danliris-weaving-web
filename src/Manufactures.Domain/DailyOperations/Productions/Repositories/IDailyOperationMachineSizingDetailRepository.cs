using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Productions.Entities;
using Manufactures.Domain.DailyOperations.Productions.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Productions.Repositories
{
    public interface IDailyOperationMachineSizingDetailRepository : IAggregateRepository<DailyOperationMachineSizingDetail, DailyOperationMachineSizingDetailReadModel>
    {
    }
}
