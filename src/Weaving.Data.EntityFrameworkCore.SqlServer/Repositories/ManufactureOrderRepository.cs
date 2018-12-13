using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Weaving.Domain.Entities;
using Weaving.Domain.ReadModels;

namespace Weaving.Domain.Repositories
{
    public class ManufactureOrderRepository : RepositoryBase<ManufactureOrderReadModel>, IManufactureOrderRepository
    {
        public string CurrentUser { get; private set; }

        public DbSet<GoodsConstruction> GoodsConstructionSet { get; }

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

            return Task.CompletedTask;
        }
    }
}
