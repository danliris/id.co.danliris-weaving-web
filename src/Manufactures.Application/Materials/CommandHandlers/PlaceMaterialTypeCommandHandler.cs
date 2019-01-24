using System;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Materials;
using Manufactures.Domain.Materials.Commands;
using Manufactures.Domain.Materials.Repositories;

namespace Manufactures.Application.Materials.CommandHandlers
{
    public class PlaceMaterialTypeCommandHandler : ICommandHandler<PlaceMaterialTypeCommand, MaterialType>
    {
        private readonly IStorage _storage;
        private readonly IMaterialTypeRepository _materialTypeRepository;

        public PlaceMaterialTypeCommandHandler(IStorage storage)
        {
            _storage = storage;
            _materialTypeRepository = storage.GetRepository<IMaterialTypeRepository>();
        }

        public async Task<MaterialType> Handle(PlaceMaterialTypeCommand request, 
                                               CancellationToken cancellationToken)
        {
            var materialType = new MaterialType(id: Guid.NewGuid(), 
                                                code: request.Code, 
                                                name: request.Name, 
                                                description: request.Description);

            await _materialTypeRepository.Update(materialType);

            _storage.Save();

            return materialType;
        }
    }
}
