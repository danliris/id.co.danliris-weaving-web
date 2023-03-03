using Infrastructure.Domain.Repositories;
using Manufactures.Domain.BrokenCauses.Warping.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.BrokenCauses.Warping.Repositories
{
    public interface IWarpingBrokenCauseRepository : IAggregateRepository<WarpingBrokenCauseDocument, WarpingBrokenCauseReadModel>
    {
    }
}
