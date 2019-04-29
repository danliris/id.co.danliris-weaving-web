using Infrastructure.Domain.Repositories;
using Manufactures.Domain.FabricConstructions.ReadModels;

namespace Manufactures.Domain.FabricConstructions.Repositories
{
    public interface IFabricConstructionRepository : IAggregateRepository<FabricConstructionDocument, FabricConstructionReadModel>
    {
    }
}
