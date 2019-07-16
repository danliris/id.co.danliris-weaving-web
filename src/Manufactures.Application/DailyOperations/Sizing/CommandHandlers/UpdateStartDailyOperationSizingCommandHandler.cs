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
            var histories = existingDailyOperation.SizingDetails.OrderByDescending(e => e.DateTimeOperation);
            var lastHistory = histories.FirstOrDefault();

            //Validation for Start Status
            var countStartStatus =
                existingDailyOperation
                    .SizingDetails
                    .Where(e => e.MachineStatus == DailyOperationMachineStatus.ONSTART)
                    .Count();

            if (countStartStatus == 1)
            {
                throw Validator.ErrorValidation(("StartStatus", "This operation already has START status"));
            }

            //Validation for Finish Status
            //var countFinishStatus =
            //    existingDailyOperation
            //        .SizingDetails
            //        .Where(e => e.MachineStatus == DailyOperationMachineStatus.ONCOMPLETE)
            //        .Count();

            //if (countFinishStatus == 1)
            //{
            //    throw Validator.ErrorValidation(("FinishStatus", "This operation's status already COMPLETED"));
            //}

            //Reformat DateTime
            var year = request.Details.StartDate.Year;
            var month = request.Details.StartDate.Month;
            var day = request.Details.StartDate.Day;
            var hour = request.Details.StartTime.Hours;
            var minutes = request.Details.StartTime.Minutes;
            var seconds = request.Details.StartTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var entryDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeOperation.Date, new TimeSpan(+7, 0, 0));
            var startDateMachineLogUtc = new DateTimeOffset(request.Details.StartDate.Date, new TimeSpan(+7, 0, 0));

            if (startDateMachineLogUtc < entryDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("StartDate", "Start date cannot less than latest date log"));
            } else
            {
                if (dateTimeOperation < lastHistory.DateTimeOperation)
                {
                    throw Validator.ErrorValidation(("StartTime", "Start time cannot less than latest time log"));
                } else
                {
                    if (histories.FirstOrDefault().MachineStatus == DailyOperationMachineStatus.ONENTRY)
                    {
                        var Causes = JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(lastHistory.Causes);

                        var newOperation =
                                new DailyOperationSizingDetail(Guid.NewGuid(),
                                                               new ShiftId(request.Details.ShiftId.Value),
                                                               new OperatorId(request.Details.OperatorDocumentId.Value),
                                                               dateTimeOperation,
                                                               DailyOperationMachineStatus.ONSTART,
                                                               "-",
                                                               new DailyOperationSizingCauseValueObject(Causes.BrokenBeam, Causes.MachineTroubled),
                                                               request.Details.SizingBeamNumber);

                        existingDailyOperation.AddDailyOperationSizingDetail(newOperation);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                        _storage.Save();


                        return existingDailyOperation;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("Status", "Can't start, latest status is not on ENTRY"));
                    }
                }
            }

            //Validation for Start Time
            //var entryTimeMachineLog = lastHistory.DateTimeOperation.TimeOfDay;
            //var startTimeMachineLog = request.SizingDetails.StartTime;

            //if(startTimeMachineLog < entryTimeMachineLog)
            //{
            //    throw Validator.ErrorValidation(("StartTime", "Start time cannot less than latest time log"));
            //}
        }
    }
}
