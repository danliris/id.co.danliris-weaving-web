using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Suppliers;
using Manufactures.Domain.Suppliers.ReadModels;
using Manufactures.Domain.Suppliers.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.Suppliers.Repositories
{
    public class SupplierRepository : AggregateRepostory<WeavingSupplierDocument, WeavingSupplierDocumentReadModel>, IWeavingSupplierRepository
    {
        protected override WeavingSupplierDocument Map(WeavingSupplierDocumentReadModel readModel)
        {
            return new WeavingSupplierDocument(readModel);
        }
    }
}
