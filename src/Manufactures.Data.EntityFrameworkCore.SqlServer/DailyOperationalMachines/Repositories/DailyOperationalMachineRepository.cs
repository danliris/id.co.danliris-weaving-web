using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations;
using Manufactures.Domain.DailyOperations.ReadModels;
using Manufactures.Domain.DailyOperations.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperationalMachines.Repositories
{
    public class DailyOperationalMachineRepository : AggregateRepostory<DailyOperationalMachineDocument, DailyOperationalMachineDocumentReadModel>, IDailyOperationalMachineRepository
    {
        protected override DailyOperationalMachineDocument Map(DailyOperationalMachineDocumentReadModel readModel)
        {
            return new DailyOperationalMachineDocument(readModel);
        }
    }
}
