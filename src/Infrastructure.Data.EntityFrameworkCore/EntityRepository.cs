using ExtCore.Data.EntityFramework;
using Infrastructure.Domain.Events;
using Infrastructure.Domain.ReadModels;
using Moonlay.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFrameworkCore
{
    public class EntityRepository<TEntity> : RepositoryBase<TEntity> where TEntity : ReadModelBase
    {
        public virtual IQueryable<TEntity> Query => dbSet;

        public Task Insert(TEntity entity)
        {
            dbSet.Add(entity);

            entity.AddDomainEvent(new OnEntityCreated<TEntity>(entity));

            return Task.CompletedTask;
        }

        public Task Remove(TEntity entity)
        {
            if (entity is ISoftDelete)
            {
                entity.Deleted = true;
                dbSet.Update(entity);
            }
            else
                dbSet.Remove(entity);

            entity.AddDomainEvent(new OnEntityDeleted<TEntity>(entity));

            return Task.CompletedTask;
        }

        public Task Update(TEntity entity)
        {
            dbSet.Update(entity);

            entity.AddDomainEvent(new OnEntityUpdated<TEntity>(entity));

            return Task.CompletedTask;
        }
    }
}
