using Infrastructure.Domain.Repositories;
using Manufactures.Domain.ReadModels;

namespace Manufactures.Domain.Repositories
{
    public interface IManufactureOrderRepository : IAggregateRepository<ManufactureOrder, ManufactureOrderReadModel>
    {
        
    }
}
