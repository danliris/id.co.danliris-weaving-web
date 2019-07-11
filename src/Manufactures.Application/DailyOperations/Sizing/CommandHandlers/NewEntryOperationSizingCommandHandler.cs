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
            var dailyOperationSizingDocument =
                new DailyOperationSizingDocument(Guid.NewGuid(),
                                                    request.MachineDocumentId,
                                                    request.WeavingUnitId,
                                                    request.ConstructionDocumentId,
                                                    request.WarpingBeamsId,
                                                    request.RecipeCode,
                                                    request.NeReal,
                                                    0,
                                                    0,
                                                    0,
                                                    DailyOperationMachineStatus.ONPROCESS);

            //var counter = new DailyOperationSizingCounterValueObject(request.SizingBeam.Counter.Start, 0);
            //var weight = new DailyOperationSizingWeightValueObject(0, 0, 0);
            var beam = new DailyOperationSizingBeamDocumentValueObject(request.SizingBeam.SizingBeamId,
                                                         request.SizingBeam.Counter.Start, 
                                                         0,
                                                         0, 
                                                         0, 
                                                         0,
                                                         0,
                                                         0,
                                                         DailyOperationMachineStatus.ONPROCESS);

            dailyOperationSizingDocument.AddSizingBeam(beam);

            var year = request.Details.PreparationDate.Year;
            var month = request.Details.PreparationDate.Month;
            var day = request.Details.PreparationDate.Day;
            var hour = request.Details.PreparationTime.Hours;
            var minutes = request.Details.PreparationTime.Minutes;
            var seconds = request.Details.PreparationTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            var newOperation =
                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                   request.Details.ShiftId,
                                                   request.Details.OperatorDocumentId,
                                                   dateTimeOperation,
                                                   DailyOperationMachineStatus.ONENTRY,
                                                   "-",
                                                   new DailyOperationSizingCauseValueObject("0","0"));

                dailyOperationSizingDocument.AddDailyOperationSizingDetail(newOperation);

            await _dailyOperationSizingDocumentRepository.Update(dailyOperationSizingDocument);

            //Add new Movement
            var newMovement =
                new MovementDocument(Guid.NewGuid(),
                                     new DailyOperationId(dailyOperationSizingDocument.Identity),
                                     MovementStatusConstant.SIZING,
                                     true);

            await _movementRepository.Update(newMovement);

            _storage.Save();

            return dailyOperationSizingDocument;
        }
    }
}
