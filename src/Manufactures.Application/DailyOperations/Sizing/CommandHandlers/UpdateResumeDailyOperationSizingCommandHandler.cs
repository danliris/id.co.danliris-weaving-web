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
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.SizingDetails).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var histories = existingDailyOperation.SizingDetails.OrderByDescending(e => e.DateTimeMachine);
            var lastHistory = histories.FirstOrDefault();

            //Validation for Start Status
            var countStartStatus =
                existingDailyOperation
                    .SizingDetails
                    .Where(e => e.MachineStatus == DailyOperationMachineStatus.ONSTART)
                    .Count();

            if (countStartStatus == 0)
            {
                throw Validator.ErrorValidation(("StartStatus", "This operation has not started yet"));
            }

            //Validation for Finish Status
            var countFinishStatus =
                existingDailyOperation
                    .SizingDetails
                    .Where(e => e.MachineStatus == DailyOperationMachineStatus.ONCOMPLETE)
                    .Count();

            if (countFinishStatus == 1)
            {
                throw Validator.ErrorValidation(("FinishStatus", "This operation's status already COMPLETED"));
            }

            //Reformat DateTime
            var year = request.Details.ResumeDate.Year;
            var month = request.Details.ResumeDate.Month;
            var day = request.Details.ResumeDate.Day;
            var hour = request.Details.ResumeTime.Hours;
            var minutes = request.Details.ResumeTime.Minutes;
            var seconds = request.Details.ResumeTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Resume Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var resumeDateMachineLogUtc = new DateTimeOffset(request.Details.ResumeDate.Date, new TimeSpan(+7, 0, 0));

            if (resumeDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ResumeDate", "Resume date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation < lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ResumeTime", "Resume time cannot less than latest operation"));
                }
                else
                {
                    if (histories.FirstOrDefault().MachineStatus == DailyOperationMachineStatus.ONSTOP)
                    {
                        var Causes = JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(lastHistory.Causes);

                        var newOperation =
                                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                                   new ShiftId(request.Details.ShiftId.Value),
                                                                   new OperatorId(request.Details.OperatorDocumentId.Value),
                                                                   dateTimeOperation,
                                                                   DailyOperationMachineStatus.ONRESUME,
                                                                   "-",
                                                                   new DailyOperationSizingCauseValueObject(Causes.BrokenBeam, Causes.MachineTroubled),
                                                                   lastHistory.SizingBeamNumber);

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

            //Validation for Resume Time
            //var lastTimeMachineLog = lastHistory.DateTimeMachine.TimeOfDay;
            //var resumeTimeMachineLog = request.SizingDetails.ResumeTime;

            //if (resumeTimeMachineLog < lastTimeMachineLog)
            //{
            //    throw Validator.ErrorValidation(("ResumeTime", "Resume time cannot less than latest time log"));
            //}
        }
    }
}
