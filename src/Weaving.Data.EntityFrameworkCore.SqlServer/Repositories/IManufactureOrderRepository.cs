using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Weaving.Domain;

namespace Weaving.Application.Repositories
{
    public interface IManufactureOrderRepository : IRepository
    {
        void SetCurrentUser(string userId);

        Task Update(ManufactureOrder order);
    }
}
