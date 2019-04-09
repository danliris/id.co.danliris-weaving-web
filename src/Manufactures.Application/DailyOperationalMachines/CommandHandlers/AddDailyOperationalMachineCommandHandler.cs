using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.DailyOperations;
using Manufactures.Domain.DailyOperations.Commands;
using Manufactures.Domain.DailyOperations.Entities;
using Manufactures.Domain.DailyOperations.Repositories;
using Manufactures.Domain.DailyOperations.ValueObjects;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperationalMachines.CommandHandlers
{
    public class AddDailyOperationalMachineCommandHandler : ICommandHandler<AddNewDailyOperationalMachineCommand, DailyOperationalMachineDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationalMachineRepository _dailyOperationalDocumentRepository;
        private readonly IWeavingOrderDocumentRepository _weavingOrderDocumentRepository;
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;
        private readonly IMachineRepository _machineRepository;

        public AddDailyOperationalMachineCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository = _storage.GetRepository<IDailyOperationalMachineRepository>();
            _weavingOrderDocumentRepository = _storage.GetRepository<IWeavingOrderDocumentRepository>();
            _constructionDocumentRepository = _storage.GetRepository<IConstructionDocumentRepository>();
            _machineRepository = _storage.GetRepository<IMachineRepository>();
        }

        public async Task<DailyOperationalMachineDocument> Handle(AddNewDailyOperationalMachineCommand request, CancellationToken cancellationToken)
        {
            var dailyOperationMachineDocument = new DailyOperationalMachineDocument(Guid.NewGuid(), request.MachineId, request.UnitId);
            var listOfDailyOperationDetail = new List<DailyOperationalMachineDetail>();

            foreach (var operation in request.DailyOperationMachineDetails)
            {
                var existingOrderDocument = _weavingOrderDocumentRepository.Find(o => o.OrderNumber.Equals(operation.OrderNumber)).FirstOrDefault();
                var existingConstructionDocument = _constructionDocumentRepository.Find(o => o.Identity.Equals(existingOrderDocument.ConstructionId)).FirstOrDefault();
                var newOperation = new DailyOperationalMachineDetail(Guid.NewGuid(), new OrderDocumentId(existingOrderDocument.Identity), operation.Shift, operation.DOMTime, operation.BeamNumber, operation.BeamOperator, operation.LoomGroup, operation.SizingNumber, operation.SizingOperator, operation.SizingGroup, operation.Information, operation.WarpOrigin, operation.WeftOrigin);
                dailyOperationMachineDocument.AddDailyOperationMachineDetail(newOperation);
            }
            await _dailyOperationalDocumentRepository.Update(dailyOperationMachineDocument);

            _storage.Save();

            return dailyOperationMachineDocument;
        }
    }
}
