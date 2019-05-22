using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.DailyOperations.Loom.ValueObjects;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
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
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingRepository;

        public AddDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _dailyOperationSizingRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
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
                                               DailyOperationMachineStatus.ONENTRY);

            var newDailyOperationHistory =
                new DailyOperationLoomHistory(request.PreparationDate.Date, 
                                              TimeSpan.Parse(request.PreparationTime), 
                                              DailyOperationMachineStatus.ONENTRY, 
                                              true, 
                                              false);

            var newOperation =
                   new DailyOperationLoomDetail(Guid.NewGuid(), 
                                                request.ShiftId, 
                                                request.OperatorId, 
                                                request.WarpOrigin, 
                                                request.WeftOrigin, 
                                                newDailyOperationHistory);

            dailyOperationMachineDocument.AddDailyOperationMachineDetail(newOperation);

            await _dailyOperationalDocumentRepository.Update(dailyOperationMachineDocument);

            _storage.Save();

            return dailyOperationMachineDocument;
        }
    }
}
