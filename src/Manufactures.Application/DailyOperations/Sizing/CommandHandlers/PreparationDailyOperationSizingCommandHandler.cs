using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Movements.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class PreparationDailyOperationSizingCommandHandler : ICommandHandler<PreparationDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IDailyOperationSizingHistoryRepository
            _dailyOperationSizingHistoryRepository;
        private readonly IDailyOperationSizingBeamsWarpingRepository
            _dailyOperationSizingBeamsWarpingRepository;
        private readonly IMovementRepository
            _movementRepository;

        public PreparationDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationSizingHistoryRepository =
                _storage.GetRepository<IDailyOperationSizingHistoryRepository>();
            _dailyOperationSizingBeamsWarpingRepository =
                _storage.GetRepository<IDailyOperationSizingBeamsWarpingRepository>();
            _movementRepository =
               _storage.GetRepository<IMovementRepository>();
        }

        public async Task<DailyOperationSizingDocument>
            Handle(PreparationDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            //Check if any Daily Operation using Selected Order (SOP)
            var existingDailyOperationWarpingDocument = _dailyOperationSizingDocumentRepository
                                                        .Find(o => o.OrderDocumentId.Equals(request.OrderDocumentId.Value))
                                                        .Any();

            //if (existingDailyOperationWarpingDocument == true)
            //{
            //    throw Validator.ErrorValidation(("OrderDocument", "No. Produksi Sudah Digunakan"));
            //}

            var year = request.PreparationDate.Year;
            var month = request.PreparationDate.Month;
            var day = request.PreparationDate.Day;
            var hour = request.PreparationTime.Hours;
            var minutes = request.PreparationTime.Minutes;
            var seconds = request.PreparationTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            var newSizingDocument =
                new DailyOperationSizingDocument(Guid.NewGuid(),
                                                 request.MachineDocumentId,
                                                 request.OrderDocumentId,
                                                 request.EmptyWeight,
                                                 request.YarnStrands,
                                                 request.RecipeCode,
                                                 request.NeReal,
                                                 dateTimeOperation,
                                                 request.BeamProductResult,
                                                 OperationStatus.ONPROCESS);

            await _dailyOperationSizingDocumentRepository.Update(newSizingDocument);

            foreach (var beamWarping in request.BeamsWarping)
            {
                var newBeamWarping =
                    new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                         beamWarping.BeamDocumentId,
                                                         beamWarping.YarnStrands,
                                                         beamWarping.EmptyWeight,
                                                         newSizingDocument.Identity);

                await _dailyOperationSizingBeamsWarpingRepository.Update(newBeamWarping);
            }

            var newSizingHistory =
                    new DailyOperationSizingHistory(Guid.NewGuid(),
                                                    request.PreparationShift,
                                                    request.PreparationOperator,
                                                    dateTimeOperation,
                                                    MachineStatus.ONENTRY,
                                                    newSizingDocument.Identity);

            await _dailyOperationSizingHistoryRepository.Update(newSizingHistory);

            _storage.Save();

            return newSizingDocument;
        }
    }
}
