using ExtCore.Data.EntityFramework;
using Infrastructure.Domain;
using Moonlay.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFrameworkCore
{
    public class EntityRepository<TEntity> : RepositoryBase<TEntity> where TEntity : EntityBase<TEntity>
    {
        public virtual IQueryable<TEntity> Query => dbSet;

        public Task Insert(TEntity entity)
        {
            if (entity.IsTransient())
                dbSet.Add(entity);

            return Task.CompletedTask;
        }

        public Task Remove(TEntity entity)
        {
            entity.MarkRemoved();

            if (entity.IsDeleted())
            {
                if (entity is ISoftDelete)
                {
                    entity.Deleted = true;
                    dbSet.Update(entity);
                }
                else
                    dbSet.Remove(entity);
            }

            return Task.CompletedTask;
        }

        public Task Update(TEntity entity)
        {
            if (entity.IsModified())
                dbSet.Update(entity);

            return Task.CompletedTask;
        }
    }
}
