using ExtCore.Data.EntityFramework;
using Infrastructure.Domain;
using Infrastructure.Domain.ReadModels;
using Infrastructure.Domain.Repositories;
using Moonlay.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFrameworkCore
{
    public abstract class AggregateRepostory<TAggregate, TReadModel> : RepositoryBase<TReadModel>, IAggregateRepository<TAggregate>
        where TAggregate : AggregateRoot<TAggregate, TReadModel>
        where TReadModel : ReadModelBase
    {
        public abstract IQueryable<TAggregate> Query { get; }

        public virtual Task Update(TAggregate aggregate)
        {
            if (aggregate.IsTransient())
                dbSet.Add(aggregate.GetReadModel());

            else if (aggregate.IsModified())
                dbSet.Update(aggregate.GetReadModel());

            else if (aggregate.IsRemoved())
            {
                var readModel = aggregate.GetReadModel();
                if (readModel is ISoftDelete)
                {
                    readModel.Deleted = true;
                    dbSet.Update(readModel);
                }
                else
                    dbSet.Remove(readModel);
            }

            return Task.CompletedTask;
        }
    }
}