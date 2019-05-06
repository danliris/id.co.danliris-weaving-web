using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Sizing.Repositories
{
    public class DailyOperationSizingRepository : AggregateRepostory<DailyOperationSizingDocument, DailyOperationSizingReadModel>, IDailyOperationSizingRepository
    {
        protected override DailyOperationSizingDocument Map(DailyOperationSizingReadModel readModel)
        {
            return new DailyOperationSizingDocument(readModel);
        }
    }
}
