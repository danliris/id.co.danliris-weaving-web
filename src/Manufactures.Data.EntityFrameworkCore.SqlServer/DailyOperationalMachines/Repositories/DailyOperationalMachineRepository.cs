using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations;
using Manufactures.Domain.DailyOperations.ReadModels;
using Manufactures.Domain.DailyOperations.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperationalMachines.Repositories
{
    public class DailyOperationalMachineRepository : AggregateRepostory<DailyOperationMachineDocument, DailyOperationMachineDocumentReadModel>, IDailyOperationalMachineRepository
    {
        protected override DailyOperationMachineDocument Map(DailyOperationMachineDocumentReadModel readModel)
        {
            return new DailyOperationMachineDocument(readModel);
        }
    }
}
