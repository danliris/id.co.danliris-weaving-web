using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Estimations.Productions.Repositories
{
    public interface IEstimatedProductionDetailRepository : IAggregateRepository<EstimatedProductionDetail, EstimatedProductionDetailReadModel>
    {
    }
}
