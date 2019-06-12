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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class UpdateStartDailyOperationSizingCommandHandler : ICommandHandler<UpdateStartDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public UpdateStartDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateStartDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.Details).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var histories = existingDailyOperation.Details.OrderByDescending(e => e.DateTimeOperation);
            var lastHistory = histories.FirstOrDefault();

            var dateTimeOperation =
                request.Details.StartDate.ToUniversalTime().AddHours(7).Date + request.Details.StartTime;

            if (histories.FirstOrDefault().MachineStatus == DailyOperationMachineStatus.ONENTRY)
            {
                var Causes = JsonConvert.DeserializeObject<DailyOperationSizingCausesValueObject>(lastHistory.Causes);

                var newOperation =
                        new DailyOperationSizingDetail(Guid.NewGuid(),
                                                       new ShiftId(request.Details.ShiftId.Value),
                                                       new OperatorId(lastHistory.OperatorDocumentId),
                                                       dateTimeOperation,
                                                       DailyOperationMachineStatus.ONSTART,
                                                       "-",
                                                       //new DailyOperationSizingHistoryValueObject(History.MachineDate, History.MachineTime, DailyOperationMachineStatus.ONPROCESS, "-"),
                                                       new DailyOperationSizingCausesValueObject(Causes.BrokenBeam, Causes.MachineTroubled));

                existingDailyOperation.AddDailyOperationSizingDetail(newOperation);

                await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                _storage.Save();


                return existingDailyOperation;
            } else
            {
                throw Validator.ErrorValidation(("Status", "Can't start, latest status is not on ENTRY"));
            }

            //var countStartStatus =
            //    existingDailyOperation
            //        .Details
            //        .Where(e => e.MachineStatus == DailyOperationMachineStatus.ONSTART)
            //        .Count();

            //if (countStartStatus > 0)
            //{
            //    throw Validator.ErrorValidation(("Status", "Start status has available"));
            //}
        }
    }
}
