using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Orders.ReadModels;

namespace Manufactures.Domain.Orders.Repositories
{
    public class ManufactureOrderRepository : AggregateRepostory<ManufactureOrder, ManufactureOrderReadModel>, IManufactureOrderRepository
    {
        protected override ManufactureOrder Map(ManufactureOrderReadModel readModel)
        {
            return new ManufactureOrder(readModel);
        }
    }
}