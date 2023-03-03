using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.CommandHandlers
{
    public class UpdateReachingInStartDailyOperationReachingCommandHandler : ICommandHandler<UpdateReachingInStartDailyOperationReachingCommand, DailyOperationReachingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingDocumentRepository;
        private readonly IDailyOperationReachingHistoryRepository
            _dailyOperationReachingHistoryRepository;

        public UpdateReachingInStartDailyOperationReachingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
            _dailyOperationReachingHistoryRepository = _storage.GetRepository<IDailyOperationReachingHistoryRepository>();
        }

        public async Task<DailyOperationReachingDocument>
            Handle(UpdateReachingInStartDailyOperationReachingCommand request, CancellationToken cancellationToken)
        {
            var existingReachingDocument = _dailyOperationReachingDocumentRepository.Find(s => s.Identity == request.Id).FirstOrDefault();
            if (existingReachingDocument == null)
                throw Validator.ErrorValidation(("Id", "Invalid Daily Operation Reaching: " + request.Id));

            var histories = _dailyOperationReachingHistoryRepository.Find(s => s.DailyOperationReachingDocumentId == existingReachingDocument.Identity);
            var existingReachingHistories =
                histories
                .OrderByDescending(d => d.DateTimeMachine);
            var lastReachingHistory = existingReachingHistories.FirstOrDefault();

            //Validation for Operation Status
            var operationStatus = existingReachingDocument.OperationStatus;
            if (operationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Start. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.ReachingInStartDate.Year;
            var month = request.ReachingInStartDate.Month;
            var day = request.ReachingInStartDate.Day;
            var hour = request.ReachingInStartTime.Hours;
            var minutes = request.ReachingInStartTime.Minutes;
            var seconds = request.ReachingInStartTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastReachingHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var reachingStartDateMachineLogUtc = new DateTimeOffset(request.ReachingInStartDate.Date, new TimeSpan(+7, 0, 0));

            if (reachingStartDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ReachingInStartDate", "Start date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastReachingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ReachingInStartTime", "Start time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastReachingHistory.MachineStatus.Equals(MachineStatus.ONENTRY))
                    {
                        existingReachingDocument.SetReachingInTypeInput(request.ReachingInTypeInput);
                        existingReachingDocument.SetReachingInTypeOutput(request.ReachingInTypeOutput);

                        var newHistory = 
                            new DailyOperationReachingHistory(Guid.NewGuid(),
                                                                  new OperatorId(request.OperatorDocumentId.Value),
                                                                  request.YarnStrandsProcessed,
                                                                  dateTimeOperation,
                                                                  new ShiftId(request.ShiftDocumentId.Value),
                                                                  MachineStatus.ONSTARTREACHINGIN,
                                                                  existingReachingDocument.Identity);
                        //existingReachingDocument.AddDailyOperationReachingHistory(newHistory);
                        await _dailyOperationReachingHistoryRepository.Update(newHistory);
                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);

                        _storage.Save();

                        return existingReachingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("OperationStatus", "Can't Start. This operation's status not ONENTRY"));
                    }
                }
            }
        }
    }
}
