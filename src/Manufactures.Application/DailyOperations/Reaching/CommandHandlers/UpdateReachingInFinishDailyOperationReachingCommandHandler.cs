using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Manufactures.Application.DailyOperations.Reaching.CommandHandlers
{
    public class UpdateReachingInFinishDailyOperationReachingCommandHandler : ICommandHandler<UpdateReachingInFinishDailyOperationReachingCommand, DailyOperationReachingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingDocumentRepository;

        public UpdateReachingInFinishDailyOperationReachingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
        }

        public async Task<DailyOperationReachingDocument>
            Handle(UpdateReachingInFinishDailyOperationReachingCommand request, CancellationToken cancellationToken)
        {
            var query =
                _dailyOperationReachingDocumentRepository.Query
                                                         .Include(d => d.ReachingHistories)
                                                         .Where(doc => doc.Identity.Equals(request.Id));
            var existingReachingDocument = _dailyOperationReachingDocumentRepository.Find(query).FirstOrDefault();
            var existingReachingHistories =
                existingReachingDocument.ReachingHistories
                .OrderByDescending(d => d.DateTimeMachine);
            var lastReachingHistory = existingReachingHistories.FirstOrDefault();

            //Validation for Operation Status
            var operationStatus = existingReachingDocument.OperationStatus;
            if (operationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Finish. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.ReachingInFinishDate.Year;
            var month = request.ReachingInFinishDate.Month;
            var day = request.ReachingInFinishDate.Day;
            var hour = request.ReachingInFinishTime.Hours;
            var minutes = request.ReachingInFinishTime.Minutes;
            var seconds = request.ReachingInFinishTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastReachingHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var reachingInFinishDateMachineLogUtc = new DateTimeOffset(request.ReachingInFinishDate.Date, new TimeSpan(+7, 0, 0));

            if (reachingInFinishDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ReachingInFinishDate", "Finish date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastReachingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ReachingInFinishTime", "Finish time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastReachingHistory.MachineStatus.Equals(MachineStatus.ONSTARTREACHINGIN) || lastReachingHistory.MachineStatus.Equals(MachineStatus.CHANGEOPERATORREACHINGIN))
                    {
                        existingReachingDocument.SetReachingInTypeInput(existingReachingDocument.ReachingInTypeInput);
                        existingReachingDocument.SetReachingInTypeOutput(existingReachingDocument.ReachingInTypeOutput);
                        existingReachingDocument.SetReachingInWidth(request.ReachingInWidth);

                        var newHistory =
                            new DailyOperationReachingHistory(Guid.NewGuid(),
                                                                  new OperatorId(request.OperatorDocumentId.Value),
                                                                  request.YarnStrandsProcessed,
                                                                  dateTimeOperation,
                                                                  new ShiftId(request.ShiftDocumentId.Value),
                                                                  MachineStatus.ONFINISHREACHINGIN);
                        existingReachingDocument.AddDailyOperationReachingHistory(newHistory);

                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);

                        _storage.Save();

                        return existingReachingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("OperationStatus", "Can't Finish. This operation's status not ONSTARTREACHINGIN or CHANGEOPERATORREACHINGIN"));
                    }
                }
            }
        }
    }
}
