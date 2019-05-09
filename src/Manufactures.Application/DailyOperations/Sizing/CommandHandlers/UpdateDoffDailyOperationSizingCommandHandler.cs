using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class UpdateDoffDailyOperationSizingCommandHandler : ICommandHandler<UpdateDoffFinishDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public UpdateDoffDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateDoffFinishDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.DailyOperationSizingDetails).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var lastHistory = existingDailyOperation.DailyOperationSizingDetails.Last();
            
            var dailyOperationSizingDocument =
                new DailyOperationSizingDocument(Guid.NewGuid(),
                                                 existingDailyOperation.MachineDocumentId,
                                                 existingDailyOperation.WeavingUnitId,
                                                 existingDailyOperation.ConstructionDocumentId,
                                                 new DailyOperationSizingCounterValueObject(existingDailyOperation.Counter.Deserialize<DailyOperationSizingCounterCommand>()),
                                                 new DailyOperationSizingWeightValueObject(existingDailyOperation.Weight.Deserialize<DailyOperationSizingWeightCommand>()),
                                                 new List<BeamId>(existingDailyOperation.WarpingBeamCollectionDocumentId.Deserialize<List<BeamId>>()),
                                                 request.MachineSpeed,
                                                 request.TexSQ,
                                                 request.Visco,
                                                 request.PIS,
                                                 request.SPU,
                                                 new BeamId(request.SizingBeamDocumentId.Value));

            var newOperation =
                        new DailyOperationSizingDetail(Guid.NewGuid(),
                                                       new ShiftId(lastHistory.ShiftId),
                                                       new OperatorId(lastHistory.OperatorDocumentId),
                                                       new DailyOperationSizingHistoryValueObject(request.UpdateDoffFinishDailyOperationSizingDetails.History.TimeOnMachine, DailyOperationMachineStatus.ONFINISH, request.UpdateDoffFinishDailyOperationSizingDetails.History.Information),
                                                       new DailyOperationSizingCausesValueObject(lastHistory.Causes.Deserialize<DailyOperationSizingCausesCommand>()));

                await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                _storage.Save();

            return existingDailyOperation;
        }
    }
}
