using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Rings;
using Manufactures.Domain.Rings.ReadModels;
using Manufactures.Domain.Rings.repositories;

namespace Manufactures.Data.EntityFrameworkCore.Rings.Repositories
{
    public class RingDocumentRepository : AggregateRepostory<RingDocument, RingDocumentReadModel>, IRingRepository
    {
        protected override RingDocument Map(RingDocumentReadModel readModel)
        {
            return new RingDocument(readModel);
        }
    }
}
