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
    public class UpdateCombStartDailyOperationReachingCommandHandler : ICommandHandler<UpdateCombStartDailyOperationReachingCommand, DailyOperationReachingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingDocumentRepository;

        public UpdateCombStartDailyOperationReachingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
        }

        public async Task<DailyOperationReachingDocument> Handle(UpdateCombStartDailyOperationReachingCommand request, CancellationToken cancellationToken)
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
                throw Validator.ErrorValidation(("OperationStatus", "Can't Start. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.CombStartDate.Year;
            var month = request.CombStartDate.Month;
            var day = request.CombStartDate.Day;
            var hour = request.CombStartTime.Hours;
            var minutes = request.CombStartTime.Minutes;
            var seconds = request.CombStartTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastReachingHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var combStartDateMachineLogUtc = new DateTimeOffset(request.CombStartDate.Date, new TimeSpan(+7, 0, 0));

            if (combStartDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("CombStartDate", "Start date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastReachingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("CombStartTime", "Start time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastReachingHistory.MachineStatus.Equals(MachineStatus.ONFINISHREACHINGIN))
                    {
                        existingReachingDocument.SetReachingInTypeInput(existingReachingDocument.ReachingInTypeInput);
                        existingReachingDocument.SetReachingInTypeOutput(existingReachingDocument.ReachingInTypeOutput);
                        existingReachingDocument.SetReachingInWidth(existingReachingDocument.ReachingInWidth);

                        existingReachingDocument.SetCombEdgeStitching(request.CombEdgeStitching);
                        existingReachingDocument.SetCombNumber(request.CombNumber);

                        var newHistory =
                            new DailyOperationReachingHistory(Guid.NewGuid(),
                                                              new OperatorId(request.OperatorDocumentId.Value),
                                                              request.YarnStrandsProcessed,
                                                              dateTimeOperation,
                                                              new ShiftId(request.ShiftDocumentId.Value),
                                                              MachineStatus.ONSTARTCOMB);
                        existingReachingDocument.AddDailyOperationReachingHistory(newHistory);

                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);

                        _storage.Save();

                        return existingReachingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("OperationStatus", "Can't Start. This operation's status not ONFINISHREACHINGIN"));
                    }
                }
            }
        }
    }
}
