﻿using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;

namespace Manufactures.Domain.DailyOperations.Loom.Repositories
{
    public interface IDailyOperationLoomBeamProductRepository : IAggregateRepository<DailyOperationLoomBeamUsed, DailyOperationLoomBeamUsedReadModel>
    {
    }
}
