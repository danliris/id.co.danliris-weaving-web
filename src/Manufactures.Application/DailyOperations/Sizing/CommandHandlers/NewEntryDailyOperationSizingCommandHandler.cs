using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Movements.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class NewEntryDailyOperationSizingCommandHandler : ICommandHandler<NewEntryDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IMovementRepository
            _movementRepository;

        public NewEntryDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
            _movementRepository =
               _storage.GetRepository<IMovementRepository>();
        }

        public async Task<DailyOperationSizingDocument>
            Handle(NewEntryDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            //Check if any Daily Operation using Selected Order (SOP)
            var existingDailyOperationWarpingDocument = _dailyOperationSizingDocumentRepository
                                                        .Find(o => o.OrderDocumentId.Equals(request.OrderDocumentId.Value))
                                                        .Any();

            if (existingDailyOperationWarpingDocument == true)
            {
                throw Validator.ErrorValidation(("OrderDocument", "No. Produksi Sudah Digunakan"));
            }

            var dailyOperationSizingDocument =
                new DailyOperationSizingDocument(Guid.NewGuid(),
                                                    request.MachineDocumentId,
                                                    request.OrderDocumentId,
                                                    request.BeamsWarping, 
                                                    request.EmptyWeight,
                                                    request.YarnStrands,
                                                    request.RecipeCode,
                                                    request.NeReal,
                                                    0,
                                                    "0",
                                                    "0",
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
                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                   request.PreparationShift,
                                                   request.PreparationOperator,
                                                   dateTimeOperation,
                                                   MachineStatus.ONENTRY,
                                                   "-",
                                                   new DailyOperationSizingCauseValueObject("0","0"),
                                                   " ");

            dailyOperationSizingDocument.AddDailyOperationSizingDetail(newOperationDetail);

            await _dailyOperationSizingDocumentRepository.Update(dailyOperationSizingDocument);

            _storage.Save();

            return dailyOperationSizingDocument;
        }
    }
}
