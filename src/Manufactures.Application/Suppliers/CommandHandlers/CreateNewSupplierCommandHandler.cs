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
    public class CreateNewSupplierCommandHandler : ICommandHandler<PlaceNewSupplierCommand, WeavingSupplierDocument>
    {
        private readonly IStorage _storage;
        private readonly IWeavingSupplierRepository _weavingSupplierRepository;

        public CreateNewSupplierCommandHandler(IStorage storage)
        {
            _storage = storage;
            _weavingSupplierRepository = _storage.GetRepository<IWeavingSupplierRepository>();
        }

        public async Task<WeavingSupplierDocument> Handle(PlaceNewSupplierCommand request, CancellationToken cancellationToken)
        {
            var hasSupplier = _weavingSupplierRepository.Find(supplier => supplier.Code.Equals(request.Code) && 
                                                                          supplier.Deleted.Equals(false)).Count() >= 1;

            // Check for exsisting supplier
            if(hasSupplier)
            {
                throw Validator.ErrorValidation(("Supplier with Code", request.Code + " has available"));
            }

            var supplierDocument = new WeavingSupplierDocument(Guid.NewGuid(), request.Code, request.Name, request.CoreSupplierId);

            await _weavingSupplierRepository.Update(supplierDocument);
            _storage.Save();

            return supplierDocument;
        }
    }
}
