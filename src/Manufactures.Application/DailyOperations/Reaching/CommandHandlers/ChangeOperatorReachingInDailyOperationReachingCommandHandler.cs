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
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.CommandHandlers
{
    public class ChangeOperatorReachingInDailyOperationReachingCommandHandler : ICommandHandler<ChangeOperatorReachingInDailyOperationReachingCommand, DailyOperationReachingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingDocumentRepository;
        private readonly IDailyOperationReachingHistoryRepository
            _dailyOperationReachingHistoryRepository;

        public ChangeOperatorReachingInDailyOperationReachingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
            _dailyOperationReachingHistoryRepository = _storage.GetRepository<IDailyOperationReachingHistoryRepository>();
        }

        public async Task<DailyOperationReachingDocument> Handle(ChangeOperatorReachingInDailyOperationReachingCommand request, CancellationToken cancellationToken)
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
                throw Validator.ErrorValidation(("OperationStatus", "Can't Change Operator. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.ChangeOperatorReachingInDate.Year;
            var month = request.ChangeOperatorReachingInDate.Month;
            var day = request.ChangeOperatorReachingInDate.Day;
            var hour = request.ChangeOperatorReachingInTime.Hours;
            var minutes = request.ChangeOperatorReachingInTime.Minutes;
            var seconds = request.ChangeOperatorReachingInTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastReachingHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var reachingChangeOperatorDateMachineLogUtc = new DateTimeOffset(request.ChangeOperatorReachingInDate.Date, new TimeSpan(+7, 0, 0));

            if (reachingChangeOperatorDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ReachingChangeOperator", "Change Operator date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastReachingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ReachingChangeOperator", "Change Operator time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastReachingHistory.MachineStatus.Equals(MachineStatus.ONSTARTREACHINGIN) || lastReachingHistory.MachineStatus.Equals(MachineStatus.CHANGEOPERATORREACHINGIN))
                    {
                        existingReachingDocument.SetReachingInTypeInput(existingReachingDocument.ReachingInTypeInput);
                        existingReachingDocument.SetReachingInTypeOutput(existingReachingDocument.ReachingInTypeOutput);
                        existingReachingDocument.SetReachingInWidth(existingReachingDocument.ReachingInWidth);

                        var newHistory =
                            new DailyOperationReachingHistory(Guid.NewGuid(),
                                                                  new OperatorId(request.OperatorDocumentId.Value),
                                                                  request.YarnStrandsProcessed,
                                                                  dateTimeOperation,
                                                                  new ShiftId(request.ShiftDocumentId.Value),
                                                                  MachineStatus.CHANGEOPERATORREACHINGIN,
                                                                  existingReachingDocument.Identity);
                        //existingReachingDocument.AddDailyOperationReachingHistory(newHistory);

                        await _dailyOperationReachingHistoryRepository.Update(newHistory);

                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);

                        _storage.Save();

                        return existingReachingDocument;
                    }
                    else if (lastReachingHistory.MachineStatus.Equals(MachineStatus.ONSTARTCOMB) || lastReachingHistory.MachineStatus.Equals(MachineStatus.CHANGEOPERATORCOMB))
                    {
                        existingReachingDocument.SetReachingInTypeInput(existingReachingDocument.ReachingInTypeInput);
                        existingReachingDocument.SetReachingInTypeOutput(existingReachingDocument.ReachingInTypeOutput);
                        existingReachingDocument.SetReachingInWidth(existingReachingDocument.ReachingInWidth);

                        existingReachingDocument.SetCombEdgeStitching(existingReachingDocument.CombEdgeStitching);
                        existingReachingDocument.SetCombNumber(existingReachingDocument.CombNumber);

                        var newHistory =
                            new DailyOperationReachingHistory(Guid.NewGuid(),
                                                                  new OperatorId(request.OperatorDocumentId.Value),
                                                                  request.YarnStrandsProcessed,
                                                                  dateTimeOperation,
                                                                  new ShiftId(request.ShiftDocumentId.Value),
                                                                  MachineStatus.CHANGEOPERATORREACHINGIN,
                                                                  existingReachingDocument.Identity);
                        //existingReachingDocument.AddDailyOperationReachingHistory(newHistory);

                        await _dailyOperationReachingHistoryRepository.Update(newHistory);

                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);

                        _storage.Save();

                        return existingReachingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("OperationStatus", "Can't Change Operator. This operation's status not ONSTARTREACHINGIN or CHANGEOPERATORREACHINGIN"));
                    }
                }
            }
        }
    }
}
