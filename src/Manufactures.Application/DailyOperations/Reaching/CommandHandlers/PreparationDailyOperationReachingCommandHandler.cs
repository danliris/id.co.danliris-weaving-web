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
        private readonly IDailyOperationReachingHistoryRepository
            _dailyOperationReachingHistoryRepository;

        public PreparationDailyOperationReachingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
            _beamStockMonitoringRepository =
                _storage.GetRepository<IBeamStockMonitoringRepository>();
            _dailyOperationReachingHistoryRepository = _storage.GetRepository<IDailyOperationReachingHistoryRepository>();
        }

        public async Task<DailyOperationReachingDocument>
            Handle(PreparationDailyOperationReachingCommand request, CancellationToken cancellationToken)
        {
            var dailyOperationReachingDocument =
                new DailyOperationReachingDocument(Guid.NewGuid(),
                                                   request.MachineDocumentId,
                                                   request.OrderDocumentId,
                                                   request.SizingBeamId,
                                                   OperationStatus.ONPROCESS);

            await _dailyOperationReachingDocumentRepository.Update(dailyOperationReachingDocument);

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
                                                      request.OperatorDocumentId,
                                                      0,
                                                      dateTimeOperation,
                                                      request.ShiftDocumentId,
                                                      MachineStatus.ONENTRY,
                                                      dailyOperationReachingDocument.Identity);
            
            await _dailyOperationReachingHistoryRepository.Update(newHistory);

            _storage.Save();

            return dailyOperationReachingDocument;
        }
    }
}
