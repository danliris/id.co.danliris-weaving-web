using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.DailyOperations.Reaching.ValueObjects;
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
    public class UpdateReachingStartDailyOperationReachingTyingCommandHandler : ICommandHandler<UpdateReachingStartDailyOperationReachingTyingCommand, DailyOperationReachingTyingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingTyingRepository
            _dailyOperationReachingTyingDocumentRepository;

        public UpdateReachingStartDailyOperationReachingTyingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingTyingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingTyingRepository>();
        }

        public async Task<DailyOperationReachingTyingDocument>
            Handle(UpdateReachingStartDailyOperationReachingTyingCommand request, CancellationToken cancellationToken)
        {
            var query =
                _dailyOperationReachingTyingDocumentRepository.Query
                                                         .Include(d => d.ReachingTyingDetails)
                                                         .Where(reachingDoc => reachingDoc
                                                         .Identity.Equals(request.Id));
            var existingReachingTyingDocument = _dailyOperationReachingTyingDocumentRepository.Find(query).FirstOrDefault();
            var existingReachingTyingDetail =
                existingReachingTyingDocument.ReachingTyingDetails
                .OrderByDescending(d => d.DateTimeMachine);
            var lastReachingDetail = existingReachingTyingDetail.FirstOrDefault();

            //Validation for Operation Status
            var operationStatus = existingReachingTyingDocument.OperationStatus;
            if (operationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can's Start. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.ReachingStartDate.Year;
            var month = request.ReachingStartDate.Month;
            var day = request.ReachingStartDate.Day;
            var hour = request.ReachingStartTime.Hours;
            var minutes = request.ReachingStartTime.Minutes;
            var seconds = request.ReachingStartTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastReachingDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var entryDateMachineLogUtc = new DateTimeOffset(request.ReachingStartDate.Date, new TimeSpan(+7, 0, 0));

            if (entryDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ReachingStartDate", "Start date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation < lastReachingDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ReachingStartTime", "Start time cannot less than latest time log"));
                }
                else
                {
                    if (lastReachingDetail.MachineStatus.Equals(MachineStatus.ONENTRY))
                    {
                        existingReachingTyingDocument.SetReachingValueObjects(new DailyOperationReachingValueObject(request.ReachingTypeInput, request.ReachingTypeOutput));

                        var newOperationDetail = new DailyOperationReachingTyingDetail(
                            Guid.NewGuid(),
                            new OperatorId(request.OperatorDocumentId.Value),
                            dateTimeOperation,
                            new ShiftId(request.ShiftDocumentId.Value),
                            MachineStatus.ONSTARTREACHING);
                        existingReachingTyingDocument.AddDailyOperationReachingDetail(newOperationDetail);

                        await _dailyOperationReachingTyingDocumentRepository.Update(existingReachingTyingDocument);

                        _storage.Save();

                        return existingReachingTyingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("OperationStatus", "Can's Start. This operation's status not ONENTRY"));
                    }
                }
            }
        }
    }
}
