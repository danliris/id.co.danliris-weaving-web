using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.YarnNumbers;
using Manufactures.Domain.YarnNumbers.ReadModels;
using Manufactures.Domain.YarnNumbers.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Data.EntityFrameworkCore.YarnNumbers.Repositories
{
    public class RingDocumentRepository : AggregateRepostory<YarnNumberDocument, YarnNumberDocumentReadModel>, IYarnNumberRepository
    {
        protected override YarnNumberDocument Map(YarnNumberDocumentReadModel readModel)
        {
            return new YarnNumberDocument(readModel);
        }
    }
}
