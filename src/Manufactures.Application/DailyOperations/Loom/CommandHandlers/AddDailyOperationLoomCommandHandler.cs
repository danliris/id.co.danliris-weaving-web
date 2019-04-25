using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.DailyOperations.Loom.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class AddDailyOperationLoomCommandHandler
        : ICommandHandler<AddNewDailyOperationLoomCommand, 
                          DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository 
            _dailyOperationalDocumentRepository;

        public AddDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository = 
                _storage.GetRepository<IDailyOperationLoomRepository>();
        }

        public async Task<DailyOperationLoomDocument> 
            Handle(AddNewDailyOperationLoomCommand request, 
                   CancellationToken cancellationToken)
        {
            var dailyOperationMachineDocument = 
                new DailyOperationLoomDocument(Guid.NewGuid(),
                                                    request.MachineId, 
                                                    request.UnitId, 
                                                    request.Status);
            var listOfDailyOperationDetail = new List<DailyOperationLoomDetail>();

            foreach (var operationDetail in request.DailyOperationMachineDetails)
            {
                var newOperation = 
                    new DailyOperationLoomDetail(Guid.NewGuid(),
                                                      operationDetail.OrderDocumentId,
                                                      operationDetail.WarpsOrigin,
                                                      operationDetail.WeftsOrigin, 
                                                      operationDetail.BeamId,
                                                      new DailyOperationLoomTimeValueObject(operationDetail.DOMTime), 
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
