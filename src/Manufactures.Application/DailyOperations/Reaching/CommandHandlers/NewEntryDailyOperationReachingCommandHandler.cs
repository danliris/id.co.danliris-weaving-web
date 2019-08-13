using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.CommandHandlers
{
    public class NewEntryDailyOperationReachingCommandHandler : ICommandHandler<NewEntryDailyOperationReachingCommand, DailyOperationReachingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingDocumentRepository;

        public NewEntryDailyOperationReachingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
        }

        public async Task<DailyOperationReachingDocument> 
            Handle(NewEntryDailyOperationReachingCommand request, CancellationToken cancellationToken)
        {
            var dailyOperationReachingDocument =
                new DailyOperationReachingDocument(Guid.NewGuid(),
                                                   request.MachineDocumentId,
                                                   request.WeavingUnitId,
                                                   request.ConstructionDocumentId,
                                                   request.SizingBeamId,
                                                   request.PISPieces,
                                                   OperationStatus.ONPROCESS);

            var year = request.PreparationDate.Year;
            var month = request.PreparationDate.Month;
            var day = request.PreparationDate.Day;
            var hour = request.PreparationTime.Hours;
            var minutes = request.PreparationTime.Minutes;
            var seconds = request.PreparationTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            var newOperationDetail =
                    new DailyOperationReachingDetail(Guid.NewGuid(),
                                                     request.OperatorDocumentId,
                                                     dateTimeOperation,
                                                     request.ShiftDocumentId,
                                                     MachineStatus.ONENTRY);

            dailyOperationReachingDocument.AddDailyOperationReachingDetail(newOperationDetail);

            await _dailyOperationReachingDocumentRepository.Update(dailyOperationReachingDocument);

            _storage.Save();

            return dailyOperationReachingDocument;
        }
    }
}
