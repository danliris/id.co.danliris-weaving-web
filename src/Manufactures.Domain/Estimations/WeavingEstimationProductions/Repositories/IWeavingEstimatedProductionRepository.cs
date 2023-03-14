using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Estimations.WeavingEstimationProductions.Repositories
{
    public interface IWeavingEstimatedProductionRepository : IAggregateRepository<WeavingEstimatedProduction,
                               WeavingEstimatedProductionReadModel>
    {
    }
}
