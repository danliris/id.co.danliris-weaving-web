using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Rings;
using Manufactures.Domain.Rings.ReadModels;
using Manufactures.Domain.Rings.repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Data.EntityFrameworkCore.Rings.Repositories
{
    public class RingDocumentRepository : AggregateRepostory<RingDocument, RingDocumentReadModel>, IRingRepository
    {
        protected override RingDocument Map(RingDocumentReadModel readModel)
        {
            return new RingDocument(readModel);
        }

        public Task<bool> isAvailableRingCode(string code)
        {
            var hasCode = this.dbSet.Where(entity => entity.Code.Equals(code)).Count() >= 1;

            return Task.FromResult(hasCode);
        }
    }
}
