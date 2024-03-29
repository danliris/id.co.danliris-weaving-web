﻿using ExtCore.Data.Abstractions;
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.CommandHandlers
{
    public class ChangeOperatorCombDailyOperationReachingCommandHandler : ICommandHandler<ChangeOperatorCombDailyOperationReachingCommand, DailyOperationReachingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingDocumentRepository;
        private readonly IDailyOperationReachingHistoryRepository
            _dailyOperationReachingHistoryRepository;

        public ChangeOperatorCombDailyOperationReachingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
            _dailyOperationReachingHistoryRepository = _storage.GetRepository<IDailyOperationReachingHistoryRepository>();
        }

        public async Task<DailyOperationReachingDocument> Handle(ChangeOperatorCombDailyOperationReachingCommand request, CancellationToken cancellationToken)
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
            var year = request.ChangeOperatorCombDate.Year;
            var month = request.ChangeOperatorCombDate.Month;
            var day = request.ChangeOperatorCombDate.Day;
            var hour = request.ChangeOperatorCombTime.Hours;
            var minutes = request.ChangeOperatorCombTime.Minutes;
            var seconds = request.ChangeOperatorCombTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastReachingHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var combChangeOperatorDateMachineLogUtc = new DateTimeOffset(request.ChangeOperatorCombDate.Date, new TimeSpan(+7, 0, 0));

            if (combChangeOperatorDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ChangeOperatorCombDate", "Change Operator date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastReachingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ChangeOperatorCombTime", "Change Operator time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastReachingHistory.MachineStatus.Equals(MachineStatus.CHANGEOPERATORREACHINGIN) ||
                        lastReachingHistory.MachineStatus.Equals(MachineStatus.ONFINISHREACHINGIN) || 
                        lastReachingHistory.MachineStatus.Equals(MachineStatus.ONSTARTCOMB) ||
                        lastReachingHistory.MachineStatus.Equals(MachineStatus.CHANGEOPERATORCOMB))
                    {
                        existingReachingDocument.SetReachingInTypeInput(existingReachingDocument.ReachingInTypeInput);
                        existingReachingDocument.SetReachingInTypeOutput(existingReachingDocument.ReachingInTypeOutput);
                        existingReachingDocument.SetReachingInWidth(existingReachingDocument.ReachingInWidth);

                        existingReachingDocument.SetCombEdgeStitching(existingReachingDocument.CombEdgeStitching);
                        existingReachingDocument.SetCombNumber(existingReachingDocument.CombNumber);

                        existingReachingDocument.SetOperationStatus(OperationStatus.ONPROCESS);

                        var newHistory =
                            new DailyOperationReachingHistory(Guid.NewGuid(),
                                                              new OperatorId(request.OperatorDocumentId.Value),
                                                              request.YarnStrandsProcessed,
                                                              dateTimeOperation,
                                                              new ShiftId(request.ShiftDocumentId.Value),
                                                              MachineStatus.CHANGEOPERATORCOMB,
                                                              existingReachingDocument.Identity);
                        //existingReachingDocument.AddDailyOperationReachingHistory(newHistory);
                        await _dailyOperationReachingHistoryRepository.Update(newHistory);

                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);

                        _storage.Save();

                        return existingReachingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("OperationStatus", "Can't Change Operator. This operation's status not CHANGEOPERATORREACHINGIN, ONSTARTCOMB, CHANGEOPERATORCOMB or ONFINISHREACHINGIN"));
                    }
                }
            }
        }
    }
}
