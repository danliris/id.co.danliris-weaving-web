using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Machines;
using Manufactures.Domain.Machines.Commands;
using Manufactures.Domain.Machines.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Machines.CommandHandlers
{
    public class RemoveExistingMachineCommandHandlers : ICommandHandler<RemoveExistingMachineCommand,
                                                                MachineDocument>
    {
        private readonly IStorage _storage;
        private readonly IMachineRepository _machineRepository;

        public RemoveExistingMachineCommandHandlers(IStorage storage)
        {
            _storage = storage;
            _machineRepository = _storage.GetRepository<IMachineRepository>();
        }

        public async Task<MachineDocument> Handle(RemoveExistingMachineCommand request, CancellationToken cancellationToken)
        {
            var existingMachine = _machineRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (existingMachine == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Ring Id: " + request.Id));
            }

            existingMachine.Remove();

            await _machineRepository.Update(existingMachine);

            _storage.Save();

            return existingMachine;
        }
    }
}
