using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Estimations.Productions.ReadModels;

namespace Manufactures.Domain.Estimations.Productions.Repositories
{
    public interface IEstimationProductRepository : IAggregateRepository<EstimatedProductionDocument, EstimatedProductionDocumentReadModel>
    {
    }
}
