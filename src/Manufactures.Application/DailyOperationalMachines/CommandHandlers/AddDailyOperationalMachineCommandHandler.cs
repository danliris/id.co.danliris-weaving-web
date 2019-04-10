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
            var dailyOperationMachineDocument = new DailyOperationalMachineDocument(Guid.NewGuid(), request.MachineId, request.UnitId, request.Status);
            var listOfDailyOperationDetail = new List<DailyOperationalMachineDetail>();

            foreach (var operation in request.DailyOperationMachineDetails)
            {
                var existingOrderDocument = _weavingOrderDocumentRepository.Find(o => o.Identity.Equals(operation.OrderDocument.Identity)).FirstOrDefault();
                var existingConstructionDocument = _constructionDocumentRepository.Find(o => o.Identity.Equals(existingOrderDocument.ConstructionId)).FirstOrDefault();

                var warpsOrigin = new List<Origin>();
                warpsOrigin.Add(new Origin(operation.OrderDocument.WarpOrigin));
                var weftsOrigin = new List<Origin>();
                weftsOrigin.Add(new Origin(operation.OrderDocument.WeftOrigin));

                var newOperation = new DailyOperationalMachineDetail(Guid.NewGuid(), existingOrderDocument.Identity, warpsOrigin, weftsOrigin, operation.BeamDocument.Identity, operation.DOMTime, operation.ShiftDocument.Identity, operation.BeamOperatorDocument.Identity, operation.SizingOperatorDocument.Identity, operation.LoomGroup, operation.SizingGroup, operation.Information, operation.DetailStatus);
                dailyOperationMachineDocument.AddDailyOperationMachineDetail(newOperation);
            }
            await _dailyOperationalDocumentRepository.Update(dailyOperationMachineDocument);

            _storage.Save();

            return dailyOperationMachineDocument;
        }
    }
}
