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
            //Create new daily operation
            var dailyOperationMachineDocument =
                new DailyOperationLoomDocument(Guid.NewGuid(),
                                               request.UnitId, 
                                               request.MachineId, 
                                               request.BeamId, 
                                               request.OrderId, 
                                               request.DailyOperationMonitoringId, 
                                               DailyOperationMachineStatus.ONPROCESS);
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
                                             DailyOperationMachineStatus.ONENTRY,
                                             true,
                                             false);
            newOperation.AddWarpOrigin(request.WarpOrigin);
            newOperation.AddWeftOrigin(request.WeftOrigin);

            dailyOperationMachineDocument.AddDailyOperationMachineDetail(newOperation);

            await _dailyOperationalDocumentRepository.Update(dailyOperationMachineDocument);

            _storage.Save();

            return dailyOperationMachineDocument;
        }
    }
}
