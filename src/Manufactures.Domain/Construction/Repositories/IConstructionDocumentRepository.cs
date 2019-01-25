using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Construction.ReadModels;
using System.Threading.Tasks;

namespace Manufactures.Domain.Construction.Repositories
{
    public interface IConstructionDocumentRepository : IAggregateRepository<ConstructionDocument, ConstructionDocumentReadModel>
    {
        Task<bool> IsAvailableConstructionNumber(string constructionNumber);
    }
}
