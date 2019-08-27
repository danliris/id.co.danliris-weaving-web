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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.ReachingTying.CommandHandlers
{
    public class UpdateTyingFinishDailyOperationReachingTyingCommandHandler : ICommandHandler<UpdateTyingFinishDailyOperationReachingTyingCommand, DailyOperationReachingTyingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingTyingRepository
            _dailyOperationReachingTyingDocumentRepository;

        public UpdateTyingFinishDailyOperationReachingTyingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingTyingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingTyingRepository>();
        }

        public async Task<DailyOperationReachingTyingDocument> Handle(UpdateTyingFinishDailyOperationReachingTyingCommand request, CancellationToken cancellationToken)
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
                throw Validator.ErrorValidation(("OperationStatus", "Can's Finish. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.TyingFinishDate.Year;
            var month = request.TyingFinishDate.Month;
            var day = request.TyingFinishDate.Day;
            var hour = request.TyingFinishTime.Hours;
            var minutes = request.TyingFinishTime.Minutes;
            var seconds = request.TyingFinishTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastReachingTyingDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var tyingFinishDateMachineLogUtc = new DateTimeOffset(request.TyingFinishDate.Date, new TimeSpan(+7, 0, 0));

            if (tyingFinishDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("TyingFinishDate", "Finish date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation < lastReachingTyingDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("TyingFinishTime", "Finish time cannot less than latest time log"));
                }
                else
                {
                    if (lastReachingTyingDetail.MachineStatus.Equals(MachineStatus.ONSTARTTYING))
                    {
                        var reachingValueObjects = JsonConvert.DeserializeObject<DailyOperationReachingValueObject>(existingReachingTyingDocument.ReachingValueObjects);
                        existingReachingTyingDocument.SetReachingValueObjects(new DailyOperationReachingValueObject(reachingValueObjects.ReachingTypeInput, 
                                                                                                                    reachingValueObjects.ReachingTypeOutput, 
                                                                                                                    reachingValueObjects.ReachingWidth));

                        var tyingValueObjects = JsonConvert.DeserializeObject<DailyOperationTyingValueObject>(existingReachingTyingDocument.TyingValueObjects);
                        existingReachingTyingDocument.SetTyingValueObjects(new DailyOperationTyingValueObject(tyingValueObjects.TyingEdgeStitching, 
                                                                                                              tyingValueObjects.TyingNumber, 
                                                                                                              request.TyingWidth));
                        existingReachingTyingDocument.SetOperationStatus(OperationStatus.ONFINISH);

                        var newOperationDetail = new DailyOperationReachingTyingDetail(
                            Guid.NewGuid(),
                            new OperatorId(request.OperatorDocumentId.Value),
                            dateTimeOperation,
                            new ShiftId(request.ShiftDocumentId.Value),
                            MachineStatus.ONFINISHTYING);
                        existingReachingTyingDocument.AddDailyOperationReachingTyingDetail(newOperationDetail);

                        await _dailyOperationReachingTyingDocumentRepository.Update(existingReachingTyingDocument);

                        _storage.Save();

                        return existingReachingTyingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("OperationStatus", "Can's Finish. This operation's status not ONSTARTTYING"));
                    }
                }
            }
        }
    }
}
