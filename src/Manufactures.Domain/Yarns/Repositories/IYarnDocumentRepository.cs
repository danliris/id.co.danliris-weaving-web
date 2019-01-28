using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Yarns.ReadModels;

namespace Manufactures.Domain.Yarns.Repositories
{
    public interface IYarnDocumentRepository : IAggregateRepository<YarnDocument, YarnDocumentReadModel>
    {
    }
}
