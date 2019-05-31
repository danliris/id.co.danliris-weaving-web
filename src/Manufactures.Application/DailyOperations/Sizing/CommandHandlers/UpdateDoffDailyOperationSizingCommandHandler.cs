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
using Newtonsoft.Json;
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
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.Details).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var lastHistory = existingDailyOperation.Details.FirstOrDefault();

            var Counter = JsonConvert.DeserializeObject<DailyOperationSizingCounterValueObject>(existingDailyOperation.Counter);
            //var Counter = existingDailyOperation.Weight.Deserialize<DailyOperationSizingCounterValueObject>();
            var Weight = JsonConvert.DeserializeObject<DailyOperationSizingWeightValueObject>(existingDailyOperation.Weight);
            //var Weight = existingDailyOperation.Weight.Deserialize<DailyOperationSizingWeightValueObject>();
            
            var dailyOperationSizingDocument =
                new DailyOperationSizingDocument(Guid.NewGuid(),
                                                 existingDailyOperation.MachineDocumentId,
                                                 existingDailyOperation.WeavingUnitId,
                                                 existingDailyOperation.ConstructionDocumentId,
                                                 existingDailyOperation.RecipeCode,
                                                 new DailyOperationSizingCounterValueObject(Counter.Start, request.Counter.Finish),
                                                 new DailyOperationSizingWeightValueObject(Weight.Netto, request.Weight.Bruto),
                                                 existingDailyOperation.WarpingBeamsId,
                                                 request.MachineSpeed,
                                                 request.TexSQ,
                                                 request.Visco,
                                                 request.PIS,
                                                 request.SPU,
                                                 new BeamId(request.SizingBeamDocumentId.Value));

            var History = request.Details.History;
            var Causes = JsonConvert.DeserializeObject<DailyOperationSizingCausesValueObject>(lastHistory.Causes);

            var newOperation =
                        new DailyOperationSizingDetail(Guid.NewGuid(),
                                                       new ShiftId(lastHistory.ShiftDocumentId),
                                                       new OperatorId(lastHistory.OperatorDocumentId),
                                                       new DailyOperationSizingHistoryValueObject(History.MachineDate, History.MachineTime, DailyOperationMachineStatus.ONFINISH, History.Information),
                                                       new DailyOperationSizingCausesValueObject(Causes.BrokenBeam, Causes.MachineTroubled));

            dailyOperationSizingDocument.AddDailyOperationSizingDetail(newOperation);

                await _dailyOperationSizingDocumentRepository.Update(dailyOperationSizingDocument);
                _storage.Save();

            return existingDailyOperation;
        }
    }
}
