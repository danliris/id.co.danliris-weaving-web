using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.ReachingTying;
using Manufactures.Domain.DailyOperations.ReachingTying.Command;
using Manufactures.Domain.DailyOperations.ReachingTying.Entities;
using Manufactures.Domain.DailyOperations.ReachingTying.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.ReachingTying.CommandHandlers
{
    public class NewEntryDailyOperationReachingTyingCommandHandler : ICommandHandler<NewEntryDailyOperationReachingTyingCommand, DailyOperationReachingTyingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingTyingRepository
            _dailyOperationReachingDocumentRepository;

        public NewEntryDailyOperationReachingTyingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingTyingRepository>();
        }

        public async Task<DailyOperationReachingTyingDocument> 
            Handle(NewEntryDailyOperationReachingTyingCommand request, CancellationToken cancellationToken)
        {
            var dailyOperationReachingDocument =
                new DailyOperationReachingTyingDocument(Guid.NewGuid(),
                                                   request.MachineDocumentId,
                                                   request.WeavingUnitId,
                                                   request.OrderDocumentId,
                                                   request.SizingBeamId,
                                                   request.PISPieces,
                                                   OperationStatus.ONPROCESS);

            var year = request.EntryDate.Year;
            var month = request.EntryDate.Month;
            var day = request.EntryDate.Day;
            var hour = request.EntryTime.Hours;
            var minutes = request.EntryTime.Minutes;
            var seconds = request.EntryTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            var newOperationDetail =
                    new DailyOperationReachingTyingDetail(Guid.NewGuid(),
                                                          request.OperatorDocumentId,
                                                          request.YarnStrandsProcessed,
                                                          dateTimeOperation,
                                                          request.ShiftDocumentId,
                                                          MachineStatus.ONENTRY);

            dailyOperationReachingDocument.AddDailyOperationReachingTyingDetail(newOperationDetail);

            await _dailyOperationReachingDocumentRepository.Update(dailyOperationReachingDocument);

            _storage.Save();

            return dailyOperationReachingDocument;
        }
    }
}
