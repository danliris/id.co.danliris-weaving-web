using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Suppliers.ReadModels;

namespace Manufactures.Domain.Suppliers.Repositories
{
    public interface IWeavingSupplierRepository : IAggregateRepository<WeavingSupplierDocument, WeavingSupplierDocumentReadModel>
    {
    }
}
