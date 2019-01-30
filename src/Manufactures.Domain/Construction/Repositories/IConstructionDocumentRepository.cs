using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Construction.ReadModels;

namespace Manufactures.Domain.Construction.Repositories
{
    public interface IConstructionDocumentRepository : IAggregateRepository<ConstructionDocument, ConstructionDocumentReadModel>
    {
    }
}
