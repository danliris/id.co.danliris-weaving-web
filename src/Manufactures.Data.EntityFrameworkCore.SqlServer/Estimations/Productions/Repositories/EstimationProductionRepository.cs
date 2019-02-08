using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.ReadModels;

namespace Manufactures.Data.EntityFrameworkCore.Estimations.Productions.Repositories
{
    public class EstimationProductionRepository : AggregateRepostory<EstimatedProductionDocument, EstimatedProductionDocumentReadModel>
    {
        protected override EstimatedProductionDocument Map(EstimatedProductionDocumentReadModel readModel)
        {
            return new EstimatedProductionDocument(readModel);
        }
    }
}
