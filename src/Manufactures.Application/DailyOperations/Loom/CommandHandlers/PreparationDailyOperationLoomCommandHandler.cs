using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.BeamStockMonitoring.Repositories;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class PreparationDailyOperationLoomCommandHandler : ICommandHandler<PreparationDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomDocumentRepository;
        private readonly IBeamStockMonitoringRepository
             _beamStockMonitoringRepository;

        public PreparationDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationLoomDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _beamStockMonitoringRepository =
                _storage.GetRepository<IBeamStockMonitoringRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(PreparationDailyOperationLoomCommand request, CancellationToken cancellationToken)
        {
            var dailyOperationLoomDocument =
                new DailyOperationLoomDocument(Guid.NewGuid(),
                                               request.OrderDocumentId,
                                               OperationStatus.ONPROCESS);

            foreach(var beamHistory in request.LoomBeamHistories)
            {
                var year = beamHistory.DateMachine.Year;
                var month = beamHistory.DateMachine.Month;
                var day = beamHistory.DateMachine.Day;
                var hour = beamHistory.TimeMachine.Hours;
                var minutes = beamHistory.TimeMachine.Minutes;
                var seconds = beamHistory.TimeMachine.Seconds;
                var dateTimeBeamHistory =
                    new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

                var newBeamHistory =
                        new DailyOperationLoomBeamHistory(Guid.NewGuid(),
                                                          new BeamId(beamHistory.BeamDocumentId.Value),
                                                          new MachineId(beamHistory.MachineDocumentId.Value),
                                                          new OperatorId(beamHistory.OperatorDocumentId.Value),
                                                          dateTimeBeamHistory,
                                                          new ShiftId(beamHistory.ShiftDocumentId.Value),
                                                          beamHistory.Process,
                                                          beamHistory.Information,
                                                          MachineStatus.ONENTRY);

                dailyOperationLoomDocument.AddDailyOperationLoomHistory(newBeamHistory);
            }

            await _dailyOperationLoomDocumentRepository.Update(dailyOperationLoomDocument);

            _storage.Save();

            return dailyOperationLoomDocument;
        }
    }
}
