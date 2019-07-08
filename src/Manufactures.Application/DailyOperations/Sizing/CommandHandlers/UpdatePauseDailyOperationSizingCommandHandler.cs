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
    public class UpdatePauseDailyOperationSizingCommandHandler : ICommandHandler<UpdatePauseDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public UpdatePauseDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdatePauseDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.Details).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var histories = existingDailyOperation.Details.OrderByDescending(e => e.DateTimeOperation);
            var lastHistory = histories.FirstOrDefault();

            //Validation for Start Status
            var countStartStatus =
                existingDailyOperation
                    .Details
                    .Where(e => e.MachineStatus == DailyOperationMachineStatus.ONSTART)
                    .Count();

            if (countStartStatus == 0)
            {
                throw Validator.ErrorValidation(("StartStatus", "This operation has not started yet"));
            }

            //Validation for Finish Status
            var countFinishStatus =
                existingDailyOperation
                    .Details
                    .Where(e => e.MachineStatus == DailyOperationMachineStatus.ONCOMPLETE)
                    .Count();

            if (countFinishStatus == 1)
            {
                throw Validator.ErrorValidation(("FinishStatus", "This operation's status already COMPLETED"));
            }

            //Reformat DateTime
            var year = request.Details.PauseDate.Year;
            var month = request.Details.PauseDate.Month;
            var day = request.Details.PauseDate.Day;
            var hour = request.Details.PauseTime.Hours;
            var minutes = request.Details.PauseTime.Minutes;
            var seconds = request.Details.PauseTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Pause Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeOperation.Date, new TimeSpan(+7, 0, 0));
            var pauseDateMachineLogUtc = new DateTimeOffset(request.Details.PauseDate.Date, new TimeSpan(+7, 0, 0));

            if (pauseDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("PauseDate", "Pause date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation < lastHistory.DateTimeOperation)
                {
                    throw Validator.ErrorValidation(("PauseTime", "Pause time cannot less than latest operation"));
                }
                else
                {
                    if (histories.FirstOrDefault().MachineStatus == DailyOperationMachineStatus.ONSTART || histories.FirstOrDefault().MachineStatus == DailyOperationMachineStatus.ONRESUME)
                    {
                        var Causes = request.Details.Causes;

                        var newOperation =
                                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                                   new ShiftId(request.Details.ShiftId.Value),
                                                                   new OperatorId(lastHistory.OperatorDocumentId),
                                                                   dateTimeOperation,
                                                                   DailyOperationMachineStatus.ONSTOP,
                                                                   request.Details.Information,
                                                                   new SizingCauseValueObject(Causes.BrokenBeam, Causes.MachineTroubled));

                        existingDailyOperation.AddDailyOperationSizingDetail(newOperation);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                        _storage.Save();

                        return existingDailyOperation;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("Status", "Can't stop, latest status is not on PROCESS or on RESUME"));
                    }
                }
            }

            //Validation for Pause Time
            //var lastTimeMachineLog = lastHistory.DateTimeOperation.TimeOfDay;
            //var pauseTimeMachineLog = request.Details.PauseTime;

            //if (pauseTimeMachineLog < lastTimeMachineLog)
            //{
            //    throw Validator.ErrorValidation(("PauseTime", "Pause time cannot less than latest time log"));
            //}
        }
    }
}
