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
    public class AddNewMachineCommandHandlers : ICommandHandler<AddNewMachineCommand, 
                                                                MachineDocument>
    {
        private readonly IStorage _storage;
        private readonly IMachineRepository _machineRepository;

        public AddNewMachineCommandHandlers(IStorage storage)
        {
            _storage = storage;
            _machineRepository = _storage.GetRepository<IMachineRepository>();
        }
        public async Task<MachineDocument> Handle(AddNewMachineCommand request, 
                                                                   CancellationToken cancellationToken)
        {
            var existingMachine = _machineRepository.Find(o => o.MachineNumber.Equals(request.MachineNumber) && 
                                                                o.Deleted.Value.Equals(false))
                                                     .FirstOrDefault();


            if(existingMachine != null)
            {
                throw Validator.ErrorValidation(("MachineNumber", "Has available machine number"));
            }


            var machineDocument = new MachineDocument(Guid.NewGuid(),
                                                      request.MachineNumber,
                                                      request.Location,
                                                      new MachineTypeId(request.MachineTypeId),
                                                      new UnitId(request.WeavingUnitId),
                                                      request.Process,
                                                      request.Area,
                                                      request.Block);

            machineDocument.SetCutmark(request.Cutmark);
            machineDocument.SetCutmarkUom(request.CutmarkUom ?? "");

            await _machineRepository.Update(machineDocument);

            _storage.Save();

            return machineDocument;
        }
    }
}
