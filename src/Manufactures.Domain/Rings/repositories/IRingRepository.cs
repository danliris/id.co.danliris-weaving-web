using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Rings.ReadModels;
using System.Threading.Tasks;

namespace Manufactures.Domain.Rings.Repositories
{
    public interface IRingRepository : IAggregateRepository<RingDocument, RingDocumentReadModel>
    {
    }
}
