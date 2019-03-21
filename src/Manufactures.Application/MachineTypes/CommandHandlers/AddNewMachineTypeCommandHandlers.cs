using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.MachineTypes;
using Manufactures.Domain.MachineTypes.Commands;
using Manufactures.Domain.MachineTypes.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.MachineTypes.CommandHandlers
{
    public class AddNewMachineTypeCommandHandlers : ICommandHandler<AddNewMachineTypeCommand,
                                                                    MachineTypeDocument>
    {
        private readonly IStorage _storage;
        private readonly IMachineTypeRepository _machineTypeRepository;

        public AddNewMachineTypeCommandHandlers(IStorage storage)
        {
            _storage = storage;
            _machineTypeRepository = 
                _storage.GetRepository<IMachineTypeRepository>();
        }

        public async Task<MachineTypeDocument> Handle(AddNewMachineTypeCommand request, 
                                                CancellationToken cancellationToken)
        {
            var machineType = new MachineTypeDocument(Guid.NewGuid(), 
                                                      request.TypeName, 
                                                      request.Speed, 
                                                      request.MachineUnit);

            await _machineTypeRepository.Update(machineType);

            _storage.Save();

            return machineType;
        }
    }
}
