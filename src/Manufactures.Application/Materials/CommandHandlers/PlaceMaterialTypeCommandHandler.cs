using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Materials;
using Manufactures.Domain.Materials.Commands;
using Manufactures.Domain.Materials.Repositories;
using Moonlay;

namespace Manufactures.Application.Materials.CommandHandlers
{
    public class PlaceMaterialTypeCommandHandler : ICommandHandler<PlaceMaterialTypeCommand, MaterialTypeDocument>
    {
        private readonly IStorage _storage;
        private readonly IMaterialTypeRepository _materialTypeRepository;

        public PlaceMaterialTypeCommandHandler(IStorage storage)
        {
            _storage = storage;
            _materialTypeRepository = storage.GetRepository<IMaterialTypeRepository>();
        }

        public async Task<MaterialTypeDocument> Handle(PlaceMaterialTypeCommand request,
                                               CancellationToken cancellationToken)
        {
            var exsistingMaterialCode = _materialTypeRepository.Find(material => material.Code.Equals(request.Code) &&
                                                                                 material.Deleted.Equals(false)).Count() > 1;
            // Check if has same material code
            if (exsistingMaterialCode)
            {
                throw Validator.ErrorValidation(("Code", "Code with " + request.Code + " has available"));
            }
            
            var materialType = new MaterialTypeDocument(id: Guid.NewGuid(),
                                                code: request.Code,
                                                name: request.Name,
                                                description: request.Description);

            if(request.RingDocuments.Count > 0)
            {
                foreach(var ringDocument in request.RingDocuments)
                {
                    materialType.SetRingNumber(ringDocument);
                }
            }

            await _materialTypeRepository.Update(materialType);

            _storage.Save();

            return materialType;
        }
    }
}
