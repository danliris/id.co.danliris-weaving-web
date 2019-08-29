using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.ReachingTying;
using Manufactures.Domain.DailyOperations.ReachingTying.Command;
using Manufactures.Domain.DailyOperations.ReachingTying.Entities;
using Manufactures.Domain.DailyOperations.ReachingTying.Repositories;
using Manufactures.Domain.DailyOperations.ReachingTying.ValueObjects;
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
                                                         .Where(doc => doc.Identity.Equals(request.Id));
            var existingReachingTyingDocument = _dailyOperationReachingTyingDocumentRepository.Find(query).FirstOrDefault();
            var existingReachingTyingDetail =
                existingReachingTyingDocument.ReachingTyingDetails
                .OrderByDescending(d => d.DateTimeMachine);
            var lastReachingTyingDetail = existingReachingTyingDetail.FirstOrDefault();

            //Validation for Operation Status
            var operationStatus = existingReachingTyingDocument.OperationStatus;
            if (operationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Start. This operation's status already FINISHED"));
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
            var lastDateMachineLogUtc = new DateTimeOffset(lastReachingTyingDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var reachingStartDateMachineLogUtc = new DateTimeOffset(request.ReachingStartDate.Date, new TimeSpan(+7, 0, 0));

            if (reachingStartDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ReachingStartDate", "Start date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation < lastReachingTyingDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ReachingStartTime", "Start time cannot less than latest time log"));
                }
                else
                {
                    if (lastReachingTyingDetail.MachineStatus.Equals(MachineStatus.ONENTRY))
                    {
                        existingReachingTyingDocument.SetReachingValueObjects(new DailyOperationReachingValueObject(request.ReachingTypeInput, request.ReachingTypeOutput));

                        var newOperationDetail = 
                            new DailyOperationReachingTyingDetail(Guid.NewGuid(),
                                                                  new OperatorId(request.OperatorDocumentId.Value),
                                                                  request.YarnStrandsProcessed,
                                                                  dateTimeOperation,
                                                                  new ShiftId(request.ShiftDocumentId.Value),
                                                                  MachineStatus.ONSTARTREACHING);
                        existingReachingTyingDocument.AddDailyOperationReachingTyingDetail(newOperationDetail);

                        await _dailyOperationReachingTyingDocumentRepository.Update(existingReachingTyingDocument);

                        _storage.Save();

                        return existingReachingTyingDocument;
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
