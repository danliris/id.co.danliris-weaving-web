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
            var materialType = _materialTypeRepository.Find(entity => entity.Identity == request.Id)
                                                      .FirstOrDefault();

            if(materialType == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Order: " + request.Id));
            }

            var hasExistingCode = _materialTypeRepository.Find(material => material.Code.Equals(request.Code)).Count() >= 1;

            if(hasExistingCode)
            {
                throw Validator.ErrorValidation(("Code", "This Code: " + request.Code + " has available"));
            }

            materialType.SetCode(request.Code);
            materialType.SetName(request.Name);
            materialType.SetDescription(request.Description);

            await _materialTypeRepository.Update(materialType);

            _storage.Save();

            return materialType;
        }
    }
}
