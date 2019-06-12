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
    public class UpdateResumeDailyOperationSizingCommandHandler : ICommandHandler<UpdateResumeDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public UpdateResumeDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateResumeDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.Details).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var histories = existingDailyOperation.Details.OrderByDescending(e => e.DateTimeOperation);
            var lastHistory = histories.FirstOrDefault();

            //if (histories.FirstOrDefault().MachineStatus == DailyOperationMachineStatus.ONSTOP)
            //{
            //    throw Validator.ErrorValidation(("Status", "Can't continue, check your latest status"));
            //}

            if (histories.FirstOrDefault().MachineStatus == DailyOperationMachineStatus.ONSTOP)
            {
                var dateTimeOperation =
            request.Details.ResumeDate.ToUniversalTime().AddHours(7).Date + request.Details.ResumeTime;

                //var History = request.Details.History;
                var Causes = JsonConvert.DeserializeObject<DailyOperationSizingCausesValueObject>(lastHistory.Causes);

                var newOperation =
                            new DailyOperationSizingDetail(Guid.NewGuid(),
                                                           new ShiftId(request.Details.ShiftId.Value),
                                                           new OperatorId(request.Details.OperatorDocumentId.Value),
                                                           dateTimeOperation,
                                                           DailyOperationMachineStatus.ONRESUME,
                                                           "-",
                                                           //new DailyOperationSizingHistoryValueObject(History.MachineDate, History.MachineTime, DailyOperationMachineStatus.ONRESUME, "-"),
                                                           new DailyOperationSizingCausesValueObject(Causes.BrokenBeam, Causes.MachineTroubled));

                existingDailyOperation.AddDailyOperationSizingDetail(newOperation);

                await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                _storage.Save();

                return existingDailyOperation;
            }
            else
            {
                throw Validator.ErrorValidation(("Status", "Can't continue, latest status is not on STOP"));
            }
        }
    }
}
