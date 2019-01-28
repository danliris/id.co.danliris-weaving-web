using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Yarns;
using Manufactures.Domain.Yarns.ReadModels;
using Manufactures.Domain.Yarns.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.Yarns.Repositories
{
    public class YarnRepository : AggregateRepostory<YarnDocument, YarnDocumentReadModel>, IYarnDocumentRepository
    {
        protected override YarnDocument Map(YarnDocumentReadModel readModel)
        {
            return new YarnDocument(readModel);
        }
    }
}
