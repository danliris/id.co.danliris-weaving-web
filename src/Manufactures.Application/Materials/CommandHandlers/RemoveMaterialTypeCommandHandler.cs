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
    public class RemoveMaterialTypeCommandHandler : ICommandHandler<RemoveMaterialTypeCommand, MaterialType>
    {
        private readonly IStorage _storage;
        private readonly IMaterialTypeRepository _materialTypeRepository;

        public RemoveMaterialTypeCommandHandler(IStorage storage)
        {
            _storage = storage;
            _materialTypeRepository = _storage.GetRepository<IMaterialTypeRepository>();
        }

        public async Task<MaterialType> Handle(RemoveMaterialTypeCommand request, CancellationToken cancellationToken)
        {
            var materialType = _materialTypeRepository.Find(entity => entity.Identity == request.Id).FirstOrDefault();

            if (materialType == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Order: " + request.Id));
            }

            materialType.Remove();

            await _materialTypeRepository.Update(materialType);

            _storage.Save();

            return materialType;
        }
    }
}
