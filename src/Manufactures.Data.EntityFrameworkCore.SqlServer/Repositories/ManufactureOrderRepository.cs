using ExtCore.Data.EntityFramework;
using System.Linq;
using System.Threading.Tasks;
using Manufactures.Domain.ReadModels;

namespace Manufactures.Domain.Repositories
{
    public class ManufactureOrderRepository : RepositoryBase<ManufactureOrderReadModel>, IManufactureOrderRepository
    {
        public IQueryable<ManufactureOrder> Query => dbSet.Select(o=> new ManufactureOrder(o));

        public Task Insert(ManufactureOrder order)
        {
            dbSet.Add(order.GetReadModel());

            return Task.CompletedTask;
        }

        public Task Update(ManufactureOrder order)
        {
            if (order.IsModified())
                dbSet.Update(order.GetReadModel());

            return Task.CompletedTask;
        }

        public Task Removed(ManufactureOrder order)
        {
            order.MarkRemoved();

            return Task.CompletedTask;
        }
    }
}
