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
            var materialType = 
                _materialTypeRepository
                    .Find(entity => entity.Identity.Equals(request.Id))
                    .FirstOrDefault();
            var existingMaterialCode = 
                _materialTypeRepository
                    .Find(material => material.Code.Equals(request.Code) && material.Deleted.Equals(false))
                    .Count() > 1;

            // Check if material does't exsist
            if (materialType == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Order: " + request.Id));
            }

            // Check if has same material code
            if (existingMaterialCode && !materialType.Code.Equals(request.Code))
            {
                throw Validator.ErrorValidation(("Code", "Code with " + request.Code + " has available"));
            }

            materialType.SetCode(request.Code);
            materialType.SetName(request.Name);
            materialType.SetDescription(request.Description);

            if (request.RingDocuments.Count > 0)
            {
                foreach(var existingRing in materialType.RingDocuments)
                {
                    var requestRing = request.RingDocuments.Where(e => e.Code.Equals(existingRing.Code) && e.Number.Equals(existingRing.Number)).FirstOrDefault();

                    if(requestRing == null)
                    {
                        materialType.RemoveRingNumber(existingRing);
                    }
                }

                foreach (var requestRing in request.RingDocuments)
                {
                    var existingRing = materialType.RingDocuments.Where(e => e.Code.Equals(requestRing.Code) && e.Number.Equals(requestRing.Number)).FirstOrDefault();

                    if (existingRing == null)
                    {
                        materialType.SetRingNumber(requestRing);
                    }
                }
            } else
            {
                foreach (var existingRing in materialType.RingDocuments)
                {
                    materialType.RemoveRingNumber(existingRing);
                }
            }

            await _materialTypeRepository.Update(materialType);

            _storage.Save();

            return materialType;
        }
    }
}
