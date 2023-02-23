using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Materials.ReadModels;

namespace Manufactures.Domain.Materials.Repositories
{
    public interface IMaterialTypeRepository : IAggregateRepository<MaterialTypeDocument, MaterialTypeReadModel>
    {
    }
}
