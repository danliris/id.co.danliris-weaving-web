using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.MachineTypes;
using Manufactures.Domain.MachineTypes.Commands;
using Manufactures.Domain.MachineTypes.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.MachineTypes.CommandHandlers
{
    public class RemoveExistingMachineTypeCommandHandlers 
        : ICommandHandler<RemoveExistingMachineTypeCommand, MachineTypeDocument>
    {
        private readonly IStorage _storage;
        private readonly IMachineTypeRepository _machineTypeRepository;

        public RemoveExistingMachineTypeCommandHandlers(IStorage storage)
        {
            _storage = storage;
            _machineTypeRepository =
                _storage.GetRepository<IMachineTypeRepository>();
        }

        public async Task<MachineTypeDocument> Handle(RemoveExistingMachineTypeCommand request, 
                                                CancellationToken cancellationToken)
        {
            var machineType = 
                _machineTypeRepository.Find(o => o.Identity.Equals(request.Id))
                                      .FirstOrDefault();

            if (machineType == null)
            {
                throw Validator.ErrorValidation(("Id", "Machine Type not found with : " + request.Id));
            }

            machineType.Remove();

            await _machineTypeRepository.Update(machineType);

            _storage.Save();


            return machineType;
        }
    }
}
