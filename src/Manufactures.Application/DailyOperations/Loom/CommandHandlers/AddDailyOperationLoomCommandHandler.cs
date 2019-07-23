using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Movements;
using Manufactures.Domain.Movements.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
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
        private readonly IMovementRepository
            _movementRepository;

        public AddDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _movementRepository =
                _storage.GetRepository<IMovementRepository>();
        }

        public async Task<DailyOperationLoomDocument>
            Handle(AddNewDailyOperationLoomCommand request,
                   CancellationToken cancellationToken)
        {
            //Create new daily operation
            var dailyOperationMachineDocument =
                new DailyOperationLoomDocument(Guid.NewGuid(),
                                               request.UnitId, 
                                               request.MachineId, 
                                               request.BeamId, 
                                               request.OrderId, 
                                               request.DailyOperationMonitoringId, 
                                               OperationStatus.ONPROCESS);
            //Break datetime to match timezone
            var year = request.PreparationDate.Year;
            var month = request.PreparationDate.Month;
            var day = request.PreparationDate.Day;
            var hour = request.PreparationTime.Hours;
            var minutes = request.PreparationTime.Minutes;
            var seconds = request.PreparationTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));
            //Create new Operation / detail
            var newOperation =
                new DailyOperationLoomDetail(Guid.NewGuid(),
                                             request.ShiftId, 
                                             request.OperatorId,
                                             dateTimeOperation, 
                                             MachineStatus.ONENTRY,
                                             true,
                                             false);
            newOperation.AddWarpOrigin(request.WarpOrigin);
            newOperation.AddWeftOrigin(request.WeftOrigin);

            dailyOperationMachineDocument.AddDailyOperationMachineDetail(newOperation);

            await _dailyOperationalDocumentRepository.Update(dailyOperationMachineDocument);

            //Add new Movement
            var newMovement = 
                new MovementDocument(Guid.NewGuid(), 
                                     new DailyOperationId(dailyOperationMachineDocument.Identity), 
                                     MovementStatus.LOOM, 
                                     true);

            await _movementRepository.Update(newMovement);

            _storage.Save();

            return dailyOperationMachineDocument;
        }
    }
}
