using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.ReadModels;

namespace Manufactures.Domain.Repositories
{
    public class ManufactureOrderRepository : AggregateRepostory<ManufactureOrder, ManufactureOrderReadModel>, IManufactureOrderRepository
    {
        protected override ManufactureOrder Map(ManufactureOrderReadModel readModel)
        {
            return new ManufactureOrder(readModel);
        }
    }
}