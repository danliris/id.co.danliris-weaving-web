using ExtCore.Data.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Domain.Repositories
{
    public interface IEntityRepository<TEntity> : IRepository
    {
        Task Insert(TEntity aggregate);

        Task Update(TEntity aggregate);

        Task Removed(TEntity aggregate);

        IQueryable<TEntity> Query { get; }
    }
}
