using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Estimations.Productions.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.Estimations.Productions.Repositories
{
    public class EstimationProductionRepository : AggregateRepostory<EstimatedProductionDocument, EstimatedProductionDocumentReadModel>, IEstimationProductRepository
    {
        protected override EstimatedProductionDocument Map(EstimatedProductionDocumentReadModel readModel)
        {
            return new EstimatedProductionDocument(readModel);
        }
    }
}
