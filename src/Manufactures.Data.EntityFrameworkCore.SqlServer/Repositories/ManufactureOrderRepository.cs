using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.ReadModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Manufactures.Domain.Repositories
{
    public class ManufactureOrderRepository : AggregateRepostory<ManufactureOrder, ManufactureOrderReadModel>, IManufactureOrderRepository
    {
        public override IQueryable<ManufactureOrder> Query => dbSet.Include(o => o.Composition).Select(o => new ManufactureOrder(o));
    }
}