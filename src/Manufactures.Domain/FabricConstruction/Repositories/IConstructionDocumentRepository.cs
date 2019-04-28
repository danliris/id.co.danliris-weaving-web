using Infrastructure.Domain.Repositories;
using Manufactures.Domain.FabricConstruction.ReadModels;

namespace Manufactures.Domain.FabricConstruction.Repositories
{
    public interface IConstructionDocumentRepository : IAggregateRepository<ConstructionDocument, ConstructionDocumentReadModel>
    {
    }
}
