using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.DailyOperations.Reaching.ValueObjects;
using Manufactures.Domain.DailyOperations.ReachingTying.Command;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.ReachingTying.CommandHandlers
{
    public class UpdateReachingFinishDailyOperationReachingTyingCommandHandler : ICommandHandler<UpdateReachingFinishDailyOperationReachingTyingCommand, DailyOperationReachingTyingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingTyingRepository
            _dailyOperationReachingTyingDocumentRepository;

        public UpdateReachingFinishDailyOperationReachingTyingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingTyingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingTyingRepository>();
        }

        public async Task<DailyOperationReachingTyingDocument>
            Handle(UpdateReachingFinishDailyOperationReachingTyingCommand request, CancellationToken cancellationToken)
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
            var year = request.ReachingFinishDate.Year;
            var month = request.ReachingFinishDate.Month;
            var day = request.ReachingFinishDate.Day;
            var hour = request.ReachingFinishTime.Hours;
            var minutes = request.ReachingFinishTime.Minutes;
            var seconds = request.ReachingFinishTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastReachingDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var startDateMachineLogUtc = new DateTimeOffset(request.ReachingFinishDate.Date, new TimeSpan(+7, 0, 0));

            if (startDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ReachingFinishDate", "Finish date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation < lastReachingDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ReachingFinishTime", "Finish time cannot less than latest time log"));
                }
                else
                {
                    if (lastReachingDetail.MachineStatus.Equals(MachineStatus.ONSTARTREACHING))
                    {
                        existingReachingTyingDocument.SetReachingValueObjects(new DailyOperationReachingValueObject(request.ReachingWidth));

                        var newOperationDetail = new DailyOperationReachingTyingDetail(
                            Guid.NewGuid(),
                            new OperatorId(request.OperatorDocumentId.Value),
                            dateTimeOperation,
                            new ShiftId(request.ShiftDocumentId.Value),
                            MachineStatus.ONFINISHREACHING);
                        existingReachingTyingDocument.AddDailyOperationReachingDetail(newOperationDetail);

                        await _dailyOperationReachingTyingDocumentRepository.Update(existingReachingTyingDocument);

                        _storage.Save();

                        return existingReachingTyingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("OperationStatus", "Can's Finish. This operation's status not ONSTARTREACHING"));
                    }
                }
            }
        }
    }
}
