using Infrastructure.Domain.Repositories;
using Manufactures.Domain.YarnNumbers.ReadModels;

namespace Manufactures.Domain.YarnNumbers.Repositories
{
    public interface IYarnNumberRepository : IAggregateRepository<YarnNumberDocument, YarnNumberDocumentReadModel>
    {
    }
}
