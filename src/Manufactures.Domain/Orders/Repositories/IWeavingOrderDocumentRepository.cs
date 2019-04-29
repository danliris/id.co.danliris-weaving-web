using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Orders.ReadModels;
using System.Threading.Tasks;

namespace Manufactures.Domain.Orders.Repositories
{
    public interface IWeavingOrderDocumentRepository : IAggregateRepository<OrderDocument, OrderDocumentReadModel>
    {
        Task<string> GetWeavingOrderNumber();
    }
}
