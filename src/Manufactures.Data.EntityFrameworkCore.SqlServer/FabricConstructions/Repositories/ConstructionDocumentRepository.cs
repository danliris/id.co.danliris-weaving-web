using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.FabricConstructions.ReadModels;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.FabricConstructions.Repositories
{
    public class ConstructionDocumentRepository : AggregateRepostory<FabricConstructionDocument, FabricConstructionReadModel>, IFabricConstructionRepository
    {
        protected override FabricConstructionDocument Map(FabricConstructionReadModel readModel)
        {
            return new FabricConstructionDocument(readModel);
        }
    }
}
