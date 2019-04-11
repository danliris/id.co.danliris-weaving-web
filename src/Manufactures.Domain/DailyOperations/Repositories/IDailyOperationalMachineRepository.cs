using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Repositories
{
    public interface IDailyOperationalMachineRepository : IAggregateRepository<DailyOperationalMachineDocument, DailyOperationalMachineDocumentReadModel>
    {
    }
}
