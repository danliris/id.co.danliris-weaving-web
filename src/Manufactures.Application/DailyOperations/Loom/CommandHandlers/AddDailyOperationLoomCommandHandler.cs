using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using System;
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
                                               request.UnitId, 
                                               request.MachineId, 
                                               request.BeamId, 
                                               request.OrderId, 
                                               request.DailyOperationMonitoringId, 
                                               DailyOperationMachineStatus.ONPROCESS);
            var dateTimeOperation = 
                request.PreparationDate.ToUniversalTime().AddHours(7).Date + request.PreparationTime;
            var newOperation =
                new DailyOperationLoomDetail(Guid.NewGuid(),
                                             request.ShiftId, 
                                             request.OperatorId, 
                                             request.WarpOrigin, 
                                             request.WeftOrigin,
                                             dateTimeOperation, 
                                             DailyOperationMachineStatus.ONENTRY,
                                             true,
                                             false);

            dailyOperationMachineDocument.AddDailyOperationMachineDetail(newOperation);

            await _dailyOperationalDocumentRepository.Update(dailyOperationMachineDocument);

            _storage.Save();

            return dailyOperationMachineDocument;
        }
    }
}
