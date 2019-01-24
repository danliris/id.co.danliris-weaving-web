using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Materials;
using Manufactures.Domain.Materials.ReadModels;
using Manufactures.Domain.Materials.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.Materials.Repositories
{
    public class MaterialTypeRepository : AggregateRepostory<MaterialTypeDocument, MaterialTypeReadModel>, IMaterialTypeRepository
    {
        protected override MaterialTypeDocument Map(MaterialTypeReadModel readModel)
        {
            return new MaterialTypeDocument(readModel);
        }
    }
}
