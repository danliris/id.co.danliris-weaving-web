using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.FabricConstruction.ReadModels;
using Manufactures.Domain.FabricConstruction;
using Manufactures.Domain.FabricConstruction.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.FabricConstruction.Repositories
{
    public class ConstructionDocumentRepository : AggregateRepostory<ConstructionDocument, ConstructionDocumentReadModel>, IFabricConstructionRepository
    {
        protected override ConstructionDocument Map(ConstructionDocumentReadModel readModel)
        {
            return new ConstructionDocument(readModel);
        }
    }
}
