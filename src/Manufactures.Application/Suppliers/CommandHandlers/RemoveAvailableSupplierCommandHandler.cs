using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Suppliers;
using Manufactures.Domain.Suppliers.Commands;
using Manufactures.Domain.Suppliers.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Suppliers.CommandHandlers
{
    public class RemoveAvailableSupplierCommandHandler : ICommandHandler<RemoveSupplierCommand, WeavingSupplierDocument>
    {
        private readonly IStorage _storage;
        private readonly IWeavingSupplierRepository _weavingSupplierRepository;

        public RemoveAvailableSupplierCommandHandler(IStorage storage)
        {
            _storage = storage;
            _weavingSupplierRepository = _storage.GetRepository<IWeavingSupplierRepository>();
        }
        public async Task<WeavingSupplierDocument> Handle(RemoveSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplierDocument = _weavingSupplierRepository.Find(supplier => supplier.Identity.Equals(request.Id)).FirstOrDefault();

            if (supplierDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Supplier Id: " + request.Id));
            }

            supplierDocument.Remove();
            await _weavingSupplierRepository.Update(supplierDocument);
            _storage.Save();

            return supplierDocument;
        }
    }
}
