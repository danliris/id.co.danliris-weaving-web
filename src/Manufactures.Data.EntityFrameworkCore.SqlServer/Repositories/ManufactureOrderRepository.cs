using ExtCore.Data.EntityFramework;
using System.Linq;
using System.Threading.Tasks;
using Manufactures.Domain.ReadModels;

namespace Manufactures.Domain.Repositories
{
    public class ManufactureOrderRepository : RepositoryBase<ManufactureOrderReadModel>, IManufactureOrderRepository
    {
        public string CurrentUser { get; private set; }

        public IQueryable<ManufactureOrder> Query => dbSet.Select(o=> new ManufactureOrder(o));

        public Task Insert(ManufactureOrder order)
        {
            dbSet.Add(order.ReadModel);

            return Task.CompletedTask;
        }

        public void SetCurrentUser(string userId)
        {
            CurrentUser = userId;
        }

        public Task Update(ManufactureOrder order)
        {
            dbSet.Update(order.ReadModel);

            order.MarkChanged();

            return Task.CompletedTask;
        }
    }
}
