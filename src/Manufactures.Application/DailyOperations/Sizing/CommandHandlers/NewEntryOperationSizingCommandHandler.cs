using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
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

        public NewEntryOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument>
            Handle(NewEntryDailyOperationSizingCommand request,
                   CancellationToken cancellationToken)
        {
            //var startTime = request.Details.ProductionTime.Start;
            //var morningShift = new TimeSpan(6, 0, 0);
            //var afternoonShift = new TimeSpan(14, 0, 0);
            //var nightShift = new TimeSpan(22,0,0);

            var dailyOperationSizingDocument =
                new DailyOperationSizingDocument(Guid.NewGuid(),
                                                    request.MachineDocumentId,
                                                    request.WeavingUnitId,
                                                    request.ConstructionDocumentId,
                                                    request.RecipeCode,
                                                    new DailyOperationSizingCounterValueObject(request.Counter.Start, ""),
                                                    new DailyOperationSizingWeightValueObject(request.Weight.Netto, ""),
                                                    request.WarpingBeamsId,
                                                    0,
                                                    0,
                                                    0,
                                                    0,
                                                    0,
                                                    new BeamId(Guid.Empty));

                var newOperation =
                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                   request.Details.ShiftId,
                                                   request.Details.OperatorDocumentId,
                                                   new DailyOperationSizingHistoryValueObject(request.Details.History.MachineDate, request.Details.History.MachineTime, DailyOperationMachineStatus.ONENTRY, request.Details.History.Information),
                                                   new DailyOperationSizingCausesValueObject("0","0"));

                dailyOperationSizingDocument.AddDailyOperationSizingDetail(newOperation);

            await _dailyOperationSizingDocumentRepository.Update(dailyOperationSizingDocument);

            _storage.Save();

            return dailyOperationSizingDocument;
        }
    }
}
