using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.BeamStockMonitoring.Repositories;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.CommandHandlers
{
    public class PreparationDailyOperationReachingCommandHandler : ICommandHandler<PreparationDailyOperationReachingCommand, DailyOperationReachingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingDocumentRepository;
        private readonly IBeamStockMonitoringRepository
             _beamStockMonitoringRepository;

        public PreparationDailyOperationReachingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
            _beamStockMonitoringRepository =
                _storage.GetRepository<IBeamStockMonitoringRepository>();
        }

        public async Task<DailyOperationReachingDocument>
            Handle(PreparationDailyOperationReachingCommand request, CancellationToken cancellationToken)
        {
            var dailyOperationReachingDocument =
                new DailyOperationReachingDocument(Guid.NewGuid(),
                                                        new MachineId(request.MachineDocumentId.Value),
                                                        new OrderId(request.OrderDocumentId.Value),
                                                        new BeamId(request.SizingBeamId.Value),
                                                        OperationStatus.ONPROCESS);

            var year = request.PreparationDate.Year;
            var month = request.PreparationDate.Month;
            var day = request.PreparationDate.Day;
            var hour = request.PreparationTime.Hours;
            var minutes = request.PreparationTime.Minutes;
            var seconds = request.PreparationTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            var newHistory =
                    new DailyOperationReachingHistory(Guid.NewGuid(),
                                                          new OperatorId(request.OperatorDocumentId.Value),
                                                          dateTimeOperation,
                                                          new ShiftId(request.ShiftDocumentId.Value),
                                                          MachineStatus.ONENTRY);

            dailyOperationReachingDocument.AddDailyOperationReachingHistory(newHistory);

            await _dailyOperationReachingDocumentRepository.Update(dailyOperationReachingDocument);

            _storage.Save();

            return dailyOperationReachingDocument;
        }
    }
}
