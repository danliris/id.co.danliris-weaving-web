using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.ProductionResults;
using Manufactures.Domain.ProductionResults.ReadModels;
using Manufactures.Domain.ProductionResults.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.ProductionResults.Repositories
{
    public class ProductionResultRepository : AggregateRepostory<ProductionResultDocument, ProductionResultReadModel>, IProductionResultRepository
    {
        protected override ProductionResultDocument Map(ProductionResultReadModel readModel)
        {
            return new ProductionResultDocument(readModel);
        }
    }
}
