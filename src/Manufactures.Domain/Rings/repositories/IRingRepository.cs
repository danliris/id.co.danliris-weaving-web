using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Rings.ReadModels;
using System.Threading.Tasks;

namespace Manufactures.Domain.Rings.repositories
{
    public interface IRingRepository : IAggregateRepository<RingDocument, RingDocumentReadModel>
    {
        Task<bool> isAvailableRingCode(string code);
    }
}
