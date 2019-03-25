using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Machines;
using Manufactures.Domain.Machines.Commands;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Machines.CommandHandlers
{
    public class UpdateExistingMachineCommandHandler : ICommandHandler<UpdateExistingMachineCommand,
                                                                MachineDocument>
    {
        private readonly IStorage _storage;
        private readonly IMachineRepository _machineRepository;

        public UpdateExistingMachineCommandHandler(IStorage storage)
        {
            _storage = storage;
            _machineRepository = _storage.GetRepository<IMachineRepository>();
        }

        public async Task<MachineDocument> Handle(UpdateExistingMachineCommand request, CancellationToken cancellationToken)
        {
            var existingMachine = _machineRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (existingMachine == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Ring Id: " + request.Id));
            }

            if(existingMachine.MachineNumber != request.MachineNumber)
            {
                // Check if any have same number
                var sameNumber = _machineRepository.Find(o => o.MachineNumber.Equals(request.MachineNumber) && o.Deleted.Value == false).FirstOrDefault();

                if(sameNumber == null)
                {
                    existingMachine.SetMachineNumber(request.MachineNumber);
                }
                else
                {
                    throw Validator.ErrorValidation(("MachineNumber", "Has available machine number"));
                }
            }


            existingMachine.SetLocation(request.Location);
            existingMachine.SetMachineTypeId(new MachineTypeId(Guid.Parse(request.MachineTypeId)));
            existingMachine.SetWeavingUnitId(new UnitId(int.Parse(request.WeavingUnitId)));

            await _machineRepository.Update(existingMachine);

            _storage.Save();

            return existingMachine;
        }
    }
}
