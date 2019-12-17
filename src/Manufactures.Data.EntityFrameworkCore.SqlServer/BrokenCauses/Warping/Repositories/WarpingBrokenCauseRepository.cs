using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.BrokenCauses.Warping;
using Manufactures.Domain.BrokenCauses.Warping.ReadModels;
using Manufactures.Domain.BrokenCauses.Warping.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.BrokenCauses.Warping.Repositories
{
    public class WarpingBrokenCauseRepository : AggregateRepostory<WarpingBrokenCauseDocument, WarpingBrokenCauseReadModel>, IWarpingBrokenCauseRepository
    {
        protected override WarpingBrokenCauseDocument Map(WarpingBrokenCauseReadModel readModel)
        {
            return new WarpingBrokenCauseDocument(readModel);
        }
    }
}
