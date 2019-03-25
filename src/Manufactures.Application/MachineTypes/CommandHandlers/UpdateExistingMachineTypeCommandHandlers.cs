using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.MachineTypes;
using Manufactures.Domain.MachineTypes.Commands;
using Manufactures.Domain.MachineTypes.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.MachineTypes.CommandHandlers
{
    public class UpdateExistingMachineTypeCommandHandlers
        : ICommandHandler<UpdateExistingMachineTypeCommand, MachineTypeDocument>
    {
        private readonly IStorage _storage;
        private readonly IMachineTypeRepository _machineTypeRepository;

        public UpdateExistingMachineTypeCommandHandlers(IStorage storage)
        {
            _storage = storage;
            _machineTypeRepository =
                _storage.GetRepository<IMachineTypeRepository>();
        }

        public async Task<MachineTypeDocument> Handle(UpdateExistingMachineTypeCommand request,
                                                CancellationToken cancellationToken)
        {
            var machineType =
                _machineTypeRepository.Find(o => o.Identity.Equals(request.Id))
                                      .FirstOrDefault();

            if (machineType == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Machine Type with : " + request.Id));
            }

            machineType.SetTypeName(request.TypeName);
            machineType.SetMachineSpeed(request.Speed);
            machineType.SetMachineUnit(request.MachineUnit);

            await _machineTypeRepository.Update(machineType);

            _storage.Save();

            return machineType;

        }
    }
}
