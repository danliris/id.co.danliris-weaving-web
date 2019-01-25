using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Materials;
using Manufactures.Domain.Materials.ReadModels;
using Manufactures.Domain.Materials.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Data.EntityFrameworkCore.Materials.Repositories
{
    public class MaterialTypeRepository : AggregateRepostory<MaterialTypeDocument, MaterialTypeReadModel>, IMaterialTypeRepository
    {
        protected override MaterialTypeDocument Map(MaterialTypeReadModel readModel)
        {
            return new MaterialTypeDocument(readModel);
        }

        public Task<bool> ChekAvailableMaterialCode(string code)
        {
            var hasCode = this.dbSet.Where(entity => entity.Code.Equals(code)).Count() >= 1;

            return Task.FromResult(hasCode);
        }
    }
}
