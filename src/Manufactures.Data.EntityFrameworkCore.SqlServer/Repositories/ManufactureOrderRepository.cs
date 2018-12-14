using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.ReadModels;
using System.Linq;

namespace Manufactures.Domain.Repositories
{
    public class ManufactureOrderRepository : AggregateRepostory<ManufactureOrder, ManufactureOrderReadModel>, IManufactureOrderRepository
    {
        public override IQueryable<ManufactureOrder> Query => dbSet.Select(o=> new ManufactureOrder(o));
    }
}
