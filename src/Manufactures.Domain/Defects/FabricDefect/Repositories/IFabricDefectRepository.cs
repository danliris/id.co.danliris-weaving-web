using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Defects.FabricDefect.ReadModels;

namespace Manufactures.Domain.Defects.FabricDefect.Repositories
{
    public interface IFabricDefectRepository : IAggregateRepository<FabricDefectDocument, FabricDefectReadModel>
    {
    }
}
