using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Rings.ReadModels;

namespace Manufactures.Domain.Rings.repositories
{
    public interface IRingRepository : IAggregateRepository<RingDocument, RingDocumentReadModel>
    {
    }
}
