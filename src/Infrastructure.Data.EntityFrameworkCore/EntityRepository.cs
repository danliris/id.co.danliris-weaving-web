using ExtCore.Data.EntityFramework;
using Infrastructure.Domain;
using Infrastructure.Domain.Repositories;
using Moonlay;
using Moonlay.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFrameworkCore
{
    public class EntityRepository<TEntity> : RepositoryBase<TEntity>, IEntityRepository<TEntity> where TEntity : EntityBase<TEntity>
    {
        public IQueryable<TEntity> Query => dbSet;

        public virtual Task Update(TEntity entity)
        {
            if (entity.IsTransient())
                dbSet.Add(entity);
            else if (entity.IsModified())
                dbSet.Update(entity);
            else if (entity.IsRemoved())
            {
                if (entity is ISoftDelete)
                {
                    entity.Deleted = true;
                    dbSet.Update(entity);
                }
                else
                    dbSet.Remove(entity);
            }
            else
                Validator.ThrowWhenTrue(() => true, "Invalid action");

            return Task.CompletedTask;
        }
    }
}