using Infrastructure.Domain.Repositories;
using Manufactures.Domain.FabricConstruction.ReadModels;

namespace Manufactures.Domain.FabricConstruction.Repositories
{
    public interface IFabricConstructionRepository : IAggregateRepository<ConstructionDocument, ConstructionDocumentReadModel>
    {
    }
}
