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
using Moonlay;
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
            var histories = existingDailyOperation.Details.OrderByDescending(e => e.DateTimeOperation);
            var lastHistory = histories.FirstOrDefault();

            var countFinishStatus =
                existingDailyOperation
                    .Details
                    .Where(e => e.OperationStatus == DailyOperationMachineStatus.ONCOMPLETE)
                    .Count();

            if (countFinishStatus > 0)
            {
                throw Validator.ErrorValidation(("Status", "Start status has available"));
            }

            var dateTimeOperation =
                request.Details.FinishDate.ToUniversalTime().AddHours(7).Date + request.Details.FinishTime;

            //var Counter = JsonConvert.DeserializeObject<DailyOperationSizingCounterValueObject>(existingDailyOperation.Counter);
            //var Weight = JsonConvert.DeserializeObject<DailyOperationSizingWeightValueObject>(existingDailyOperation.Weight);

            var Counter = existingDailyOperation.Counter;
            var Weight = existingDailyOperation.Weight;

            //var dailyOperationSizingDocument =
                //new DailyOperationSizingDocument(Guid.NewGuid(),
                //                                 existingDailyOperation.MachineDocumentId,
                //                                 existingDailyOperation.WeavingUnitId,
                //                                 existingDailyOperation.ConstructionDocumentId,
                //                                 existingDailyOperation.RecipeCode,
                //                                 new DailyOperationSizingCounterValueObject(Counter.Start, request.Counter.Finish),
                //                                 new DailyOperationSizingWeightValueObject(Weight.Netto, request.Weight.Bruto),
                //                                 existingDailyOperation.WarpingBeamsId,
                //                                 request.MachineSpeed,
                //                                 request.TexSQ,
                //                                 request.Visco,
                //                                 request.PIS,
                //                                 request.SPU,
                //                                 new BeamId(request.SizingBeamDocumentId.Value));

            existingDailyOperation.SetCounter(new DailyOperationSizingCounterValueObject(Counter.Start, request.Counter.Finish));
            existingDailyOperation.SetWeight(new DailyOperationSizingWeightValueObject(Weight.Netto, request.Weight.Bruto));
            existingDailyOperation.SetMachineSpeed(request.MachineSpeed);
            existingDailyOperation.SetTexSQ(request.TexSQ);
            existingDailyOperation.SetVisco(request.Visco);
            existingDailyOperation.SetPIS(request.PIS);
            existingDailyOperation.SetSPU(request.SPU);
            existingDailyOperation.SetSizingBeamDocumentId(request.SizingBeamDocumentId);

            //var History = request.Details.History;
            var Causes = JsonConvert.DeserializeObject<DailyOperationSizingCausesValueObject>(lastHistory.Causes);

            var newOperation =
                        new DailyOperationSizingDetail(Guid.NewGuid(),
                                                       new ShiftId(lastHistory.ShiftDocumentId),
                                                       new OperatorId(lastHistory.OperatorDocumentId),
                                                       dateTimeOperation,
                                                       DailyOperationMachineStatus.ONFINISH, 
                                                       "-",
                                                       //new DailyOperationSizingHistoryValueObject(History.MachineDate, History.MachineTime, DailyOperationMachineStatus.ONFINISH, "-"),
                                                       new DailyOperationSizingCausesValueObject(Causes.BrokenBeam, Causes.MachineTroubled));

            existingDailyOperation.AddDailyOperationSizingDetail(newOperation);

                await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                _storage.Save();

            return existingDailyOperation;
        }
    }
}
