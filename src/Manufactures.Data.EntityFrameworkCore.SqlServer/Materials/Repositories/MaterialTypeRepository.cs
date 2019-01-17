using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Materials;
using Manufactures.Domain.Materials.ReadModels;
using Manufactures.Domain.Materials.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.Materials.Repositories
{
    public class MaterialTypeRepository : AggregateRepostory<MaterialType, MaterialTypeReadModel>, IMaterialTypeRepository
    {
        protected override MaterialType Map(MaterialTypeReadModel readModel)
        {
            return new MaterialType(readModel);
        }
    }
}
