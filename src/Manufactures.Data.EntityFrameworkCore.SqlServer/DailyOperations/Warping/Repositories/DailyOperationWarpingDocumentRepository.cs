using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Warping.Repositories
{
    public class DailyOperationWarpingDocumentRepository : AggregateRepostory<DailyOperationWarpingDocument, DailyOperationWarpingDocumentReadModel>, IDailyOperationWarpingRepository
    {
        protected override DailyOperationWarpingDocument Map(DailyOperationWarpingDocumentReadModel readModel)
        {
            return new DailyOperationWarpingDocument(readModel);
        }
    }
}
