using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Suppliers;
using Manufactures.Domain.Suppliers.Commands;
using Manufactures.Domain.Suppliers.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Suppliers.CommandHandlers
{
    class UpdateAvailableSupplierCommandHandler : ICommandHandler<UpdateExsistingSupplierCommand, WeavingSupplierDocument>
    {
        private readonly IStorage _storage;
        private readonly IWeavingSupplierRepository _weavingSupplierRepository;

        public UpdateAvailableSupplierCommandHandler(IStorage storage)
        {
            _storage = storage;
            _weavingSupplierRepository = _storage.GetRepository<IWeavingSupplierRepository>();
        }

        public async Task<WeavingSupplierDocument> Handle(UpdateExsistingSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplierDocument = _weavingSupplierRepository.Find(supplier => supplier.Identity.Equals(request.Id)).FirstOrDefault();

            if(supplierDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Order: " + request.Id));
            }

            var hasExsistingCode = _weavingSupplierRepository.Find(supplier => supplier.Code.Equals(request.Code)).Count() >= 1;

            if(hasExsistingCode)
            {
                throw Validator.ErrorValidation(("Code", "This Code: " + request.Code + " has available"));
            }

            supplierDocument.SetCode(request.Code);
            supplierDocument.SetCode(request.Name);
            supplierDocument.SetCoreSupplierId(request.CoreSupplierId);

            await _weavingSupplierRepository.Update(supplierDocument);
            _storage.Save();

            return supplierDocument;
        }
    }
}
