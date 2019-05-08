using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
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
    public class AddDailyOperationSizingCommandHandler : ICommandHandler<AddNewDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public AddDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument>
            Handle(AddNewDailyOperationSizingCommand request,
                   CancellationToken cancellationToken)
        {
            var dailyOperationSizingDocument =
                new DailyOperationSizingDocument(Guid.NewGuid(),
                                                    request.MachineDocumentId,
                                                    request.WeavingUnitId);

            //var startTime = request.DailyOperationSizingDetails.ProductionTime.Start;
            //var morningShift = new TimeSpan(6, 0, 0);
            //var afternoonShift = new TimeSpan(14, 0, 0);
            //var nightShift = new TimeSpan(22,0,0);

                var newOperation =
                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                   request.DailyOperationSizingDetails.ConstructionDocumentId,
                                                   request.DailyOperationSizingDetails.ShiftId,
                                                   request.DailyOperationSizingDetails.OperatorDocumentId,
                                                   new DailyOperationSizingProductionTimeValueObject(request.DailyOperationSizingDetails.ProductionTime.Start, DateTimeOffset.MinValue, DateTimeOffset.MinValue, DateTimeOffset.MinValue),
                                                   new DailyOperationSizingCounterValueObject(request.DailyOperationSizingDetails.Counter.Start,""),
                                                   new DailyOperationSizingWeightValueObject(request.DailyOperationSizingDetails.Weight.Netto,""),
                                                   new List<BeamId>(request.DailyOperationSizingDetails.WarpingBeamCollectionDocumentId),
                                                   new DailyOperationSizingCausesValueObject("",""),
                                                   "",
                                                   0,
                                                   0,
                                                   0,
                                                   0,
                                                   0,
                                                   new BeamId(Guid.Empty));

                dailyOperationSizingDocument.AddDailyOperationSizingDetail(newOperation);

            await _dailyOperationSizingDocumentRepository.Update(dailyOperationSizingDocument);

            _storage.Save();

            return dailyOperationSizingDocument;
        }
    }
}
