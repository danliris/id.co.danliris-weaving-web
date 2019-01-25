using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Materials.ReadModels;
using System.Threading.Tasks;

namespace Manufactures.Domain.Materials.Repositories
{
    public interface IMaterialTypeRepository : IAggregateRepository<MaterialTypeDocument, MaterialTypeReadModel>
    {
        Task<bool> ChekAvailableMaterialCode(string code);
    }
}
