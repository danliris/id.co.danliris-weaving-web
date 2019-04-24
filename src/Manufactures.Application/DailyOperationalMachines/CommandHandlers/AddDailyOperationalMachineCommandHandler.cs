using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations;
using Manufactures.Domain.DailyOperations.Commands;
using Manufactures.Domain.DailyOperations.Entities;
using Manufactures.Domain.DailyOperations.Repositories;
using Manufactures.Domain.DailyOperations.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperationalMachines.CommandHandlers
{
    public class AddDailyOperationalMachineCommandHandler
        : ICommandHandler<AddNewDailyOperationalMachineCommand, 
                          DailyOperationalMachineDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationalMachineRepository 
            _dailyOperationalDocumentRepository;

        public AddDailyOperationalMachineCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository = 
                _storage.GetRepository<IDailyOperationalMachineRepository>();
        }

        public async Task<DailyOperationalMachineDocument> 
            Handle(AddNewDailyOperationalMachineCommand request, 
                   CancellationToken cancellationToken)
        {
            var dailyOperationMachineDocument = 
                new DailyOperationalMachineDocument(Guid.NewGuid(),
                                                    request.MachineId, 
                                                    request.UnitId, 
                                                    request.Status);
            var listOfDailyOperationDetail = new List<DailyOperationalMachineDetail>();

            foreach (var operationDetail in request.DailyOperationMachineDetails)
            {
                var newOperation = 
                    new DailyOperationalMachineDetail(Guid.NewGuid(),
                                                      operationDetail.OrderDocumentId,
                                                      operationDetail.WarpsOrigin,
                                                      operationDetail.WeftsOrigin, 
                                                      operationDetail.BeamId,
                                                      new DOMTimeValueObject(operationDetail.DOMTime), 
                                                      operationDetail.ShiftId,
                                                      operationDetail.BeamOperatorId,
                                                      operationDetail.SizingOperatorId, 
                                                      operationDetail.Information, 
                                                      operationDetail.DetailStatus);

                dailyOperationMachineDocument.AddDailyOperationMachineDetail(newOperation);
            }

            await _dailyOperationalDocumentRepository.Update(dailyOperationMachineDocument);

            _storage.Save();

            return dailyOperationMachineDocument;
        }
    }
}
