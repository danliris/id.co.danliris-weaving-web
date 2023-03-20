using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Estimations.WeavingEstimationProductions.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.Estimations.Productions.Repositories
{
    public class WeavingEstimatedProductionRepository : AggregateRepostory<WeavingEstimatedProduction, WeavingEstimatedProductionReadModel>, IWeavingEstimatedProductionRepository
    {
        protected override WeavingEstimatedProduction Map(WeavingEstimatedProductionReadModel readModel)
        {
            return new WeavingEstimatedProduction(readModel);
        }
    }
}
