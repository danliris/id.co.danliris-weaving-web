using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations;
using Manufactures.Domain.DailyOperations.Commands;
using Manufactures.Domain.DailyOperations.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Orders.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperationalMachines.CommandHandlers
{
    public class AddDailyOperationalMachineCommandHandler : ICommandHandler<AddNewDailyOperationCommand, DailyOperationMachineDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationalMachineRepository _dailyOperationalDocumentRepository;
        private readonly IWeavingOrderDocumentRepository _weavingOrderDocumentRepository;
        private readonly IMachineRepository _machineRepository;

        public AddDailyOperationalMachineCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository = _storage.GetRepository<IDailyOperationalMachineRepository>();
            _weavingOrderDocumentRepository = _storage.GetRepository<IWeavingOrderDocumentRepository>();
            _machineRepository = _storage.GetRepository<IMachineRepository>();
        }

        public async Task<DailyOperationMachineDocument> Handle(AddNewDailyOperationCommand request, CancellationToken cancellationToken)
        {
            var dailyOperationMachineDocument = new DailyOperationMachineDocument(Guid.NewGuid(), request.MachineId, request.UnitId);

            foreach (var operation in request.DailyOperationMachineDetails)
            {

            }

            _storage.Save();

            return dailyOperationMachineDocument;
        }
    }
}
