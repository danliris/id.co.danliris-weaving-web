using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Construction.ReadModels;
using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.Repositories;
using System.Threading.Tasks;
using System.Linq;

namespace Manufactures.Data.EntityFrameworkCore.Construction.Repositories
{
    public class ConstructionDocumentRepository : AggregateRepostory<ConstructionDocument, ConstructionDocumentReadModel>, IConstructionDocumentRepository
    {
        protected override ConstructionDocument Map(ConstructionDocumentReadModel readModel)
        {
            return new ConstructionDocument(readModel);
        }

        public Task<bool> IsAvailableConstructionNumber(string constructionNumber)
        {
            var hasNumber = this.dbSet.Where(entity => entity.ConstructionNumber.Equals(constructionNumber)).Count() > 1;

            return Task.FromResult(hasNumber);
        }
    }
}
