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
                                                    request.WeavingUnitId,
                                                    request.ConstructionDocumentId,
                                                    new DailyOperationSizingCounterValueObject(request.Counter.Start, ""),
                                                    new DailyOperationSizingWeightValueObject(request.Weight.Netto, ""),
                                                    new List<BeamId>(request.WarpingBeamCollectionDocumentId),
                                                    0,
                                                    0,
                                                    0,
                                                    0,
                                                    0,
                                                    new BeamId(Guid.Empty));

            //var startTime = request.DailyOperationSizingDetails.ProductionTime.Start;
            //var morningShift = new TimeSpan(6, 0, 0);
            //var afternoonShift = new TimeSpan(14, 0, 0);
            //var nightShift = new TimeSpan(22,0,0);

                var newOperation =
                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                   request.DailyOperationSizingDetails.ShiftId,
                                                   request.DailyOperationSizingDetails.OperatorDocumentId,
                                                   new DailyOperationSizingHistoryValueObject(request.DailyOperationSizingDetails.History.TimeOnMachine, DailyOperationMachineStatus.ONPROCESS, request.DailyOperationSizingDetails.History.Information),
                                                   new DailyOperationSizingCausesValueObject("",""));

                dailyOperationSizingDocument.AddDailyOperationSizingDetail(newOperation);

            await _dailyOperationSizingDocumentRepository.Update(dailyOperationSizingDocument);

            _storage.Save();

            return dailyOperationSizingDocument;
        }
    }
}
