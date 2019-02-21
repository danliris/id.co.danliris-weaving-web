using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Materials;
using Manufactures.Domain.Materials.Commands;
using Manufactures.Domain.Materials.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Materials.CommandHandlers
{
    public class UpdateMaterialTypeCommandHandler : ICommandHandler<UpdateMaterialTypeCommand, MaterialTypeDocument>
    {
        private readonly IStorage _storage;
        private readonly IMaterialTypeRepository _materialTypeRepository;

        public UpdateMaterialTypeCommandHandler(IStorage storage)
        {
            _storage = storage;
            _materialTypeRepository = _storage.GetRepository<IMaterialTypeRepository>();
        }


        public async Task<MaterialTypeDocument> Handle(UpdateMaterialTypeCommand request,
                                               CancellationToken cancellationToken)
        {
            var materialType = _materialTypeRepository.Find(entity => entity.Identity.Equals(request.Id))
                                                      .FirstOrDefault();
            var exsistingMaterialCode = _materialTypeRepository.Find(material => material.Code.Equals(request.Code) &&
                                                                                 material.Deleted.Equals(false)).Count() > 1;

            // Check if material does't exsist
            if (materialType == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Order: " + request.Id));
            }

            // Check if has same material code
            if (exsistingMaterialCode && !materialType.Code.Equals(request.Code))
            {
                throw Validator.ErrorValidation(("Code", "Code with " + request.Code + " has available"));
            }

            materialType.SetCode(request.Code);
            materialType.SetName(request.Name);
            materialType.SetDescription(request.Description);

            if (request.RingDocuments.Count > 0)
            {
                if (materialType.RingDocuments.Count > 0)
                {
                    foreach (var exsistingRingDocument in materialType.RingDocuments)
                    {
                        var removeDocument = request.RingDocuments.ToList().Where(o => o.Code == exsistingRingDocument.Code && o.Number == exsistingRingDocument.Number).FirstOrDefault();

                        if(removeDocument == null)
                        {
                            materialType.RemoveRingNumber(exsistingRingDocument);
                        }
                    }

                }

                foreach (var ringDocument in request.RingDocuments)
                {
                    var exsistingDocument = materialType.RingDocuments.ToList().Where(o => o.Code == ringDocument.Code && o.Number == ringDocument.Number).FirstOrDefault();

                    if (exsistingDocument == null)
                    {
                        materialType.SetRingNumber(ringDocument);
                    }

                }

            }

            await _materialTypeRepository.Update(materialType);

            _storage.Save();

            return materialType;
        }
    }
}
