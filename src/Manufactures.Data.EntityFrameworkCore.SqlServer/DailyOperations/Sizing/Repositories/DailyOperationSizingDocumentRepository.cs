using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Sizing.Repositories
{
    public class DailyOperationSizingDocumentRepository : AggregateRepostory<DailyOperationSizingDocument, DailyOperationSizingDocumentReadModel>, IDailyOperationSizingDocumentRepository
    {
        protected override DailyOperationSizingDocument Map(DailyOperationSizingDocumentReadModel readModel)
        {
            return new DailyOperationSizingDocument(readModel);
        }
    }
}
