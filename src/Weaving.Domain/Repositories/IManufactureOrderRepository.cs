using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Weaving.Domain;

namespace Weaving.Domain.Repositories
{
    public interface IManufactureOrderRepository : IRepository
    {
        void SetCurrentUser(string userId);

        Task Insert(ManufactureOrder order);

        Task Update(ManufactureOrder order);

        IQueryable<ManufactureOrder> Query { get; }
    }
}
