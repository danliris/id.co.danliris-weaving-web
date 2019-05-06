using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.DailyOperations.Loom.ValueObjects;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Linq;
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
                                                    request.MachineId, 
                                                    request.UnitId, 
                                                    request.DailyOperationStatus);

            var newDailyOperationHistory = new DailyOperationLoomHistory();
            newDailyOperationHistory.SetTimeOnMachine(request.Detail.DailyOperationLoomHistory.TimeOnMachine);
            newDailyOperationHistory.SetMachineStatus(Constants.PROCESS);
            newDailyOperationHistory.SetInformation(request.Detail.DailyOperationLoomHistory.Information);

            var sizingOperatorDocumentId = 
                _dailyOperationSizingRepository
                    .Query
                    .Where(o => o.Identity == request.DailyOperationSizingId.Value)
                    .FirstOrDefault()
                    .Identity;

            var newOperation =
                   new DailyOperationLoomDetail(Guid.NewGuid(),
                                                     request.Detail.OrderDocumentId,
                                                     request.Detail.WarpOrigin,
                                                     request.Detail.WeftOrigin,
                                                     request.Detail.BeamId,
                                                     newDailyOperationHistory,
                                                     request.Detail.ShiftId,
                                                     request.Detail.BeamOperatorId,
                                                     new OperatorId(sizingOperatorDocumentId));

            dailyOperationMachineDocument.AddDailyOperationMachineDetail(newOperation);

            await _dailyOperationalDocumentRepository.Update(dailyOperationMachineDocument);

            _storage.Save();

            return dailyOperationMachineDocument;
        }
    }
}
