using Infrastructure.Domain.Repositories;
using Manufactures.Domain.ProductionResults.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.ProductionResults.Repositories
{
    public interface IProductionResultRepository : IAggregateRepository<ProductionResultDocument, ProductionResultReadModel>
    {
    }
}
