using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Movements;
using Manufactures.Domain.Movements.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class NewEntryOperationSizingCommandHandler : ICommandHandler<NewEntryDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IMovementRepository
            _movementRepository;

        public NewEntryOperationSizingCommandHandler(IStorage storage)
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
            //var warpingBeamsCollection = new List<BeamId>();

            //foreach(var beamDocument in request.BeamsWarping)
            //{
            //    var beamValue = new DailyOperationSizingBeamsCollectionValueObject(beamDocument.Id, beamDocument.YarnStrands);
            //    warpingBeamsCollection.Add(beamValue);
            //}

            var dailyOperationSizingDocument =
                new DailyOperationSizingDocument(Guid.NewGuid(),
                                                    request.MachineDocumentId,
                                                    request.WeavingUnitId,
                                                    request.ConstructionDocumentId,
                                                    request.BeamsWarping,
                                                    request.RecipeCode,
                                                    request.NeReal,
                                                    0,
                                                    0,
                                                    0,
                                                    OperationStatus.ONPROCESS);

            //var counter = new DailyOperationSizingCounterValueObject(request.SizingBeamDocuments.Counter.Start, 0);
            //var weight = new DailyOperationSizingWeightValueObject(0, 0, 0);

            var year = request.Details.PreparationDate.Year;
            var month = request.Details.PreparationDate.Month;
            var day = request.Details.PreparationDate.Day;
            var hour = request.Details.PreparationTime.Hours;
            var minutes = request.Details.PreparationTime.Minutes;
            var seconds = request.Details.PreparationTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            var newOperationDetail =
                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                   request.Details.ShiftId,
                                                   request.Details.OperatorDocumentId,
                                                   dateTimeOperation,
                                                   MachineStatus.ONENTRY,
                                                   "-",
                                                   new DailyOperationSizingCauseValueObject("0","0"),
                                                   " ");

            dailyOperationSizingDocument.AddDailyOperationSizingDetail(newOperationDetail);

            await _dailyOperationSizingDocumentRepository.Update(dailyOperationSizingDocument);

            //Add new Movement
            //var newMovement =
            //    new MovementDocument(Guid.NewGuid(),
            //                         new DailyOperationId(dailyOperationSizingDocument.Identity),
            //                         MovementStatusConstant.SIZING,
            //                         true);

            //await _movementRepository.Update(newMovement);

            _storage.Save();

            return dailyOperationSizingDocument;
        }
    }
}
