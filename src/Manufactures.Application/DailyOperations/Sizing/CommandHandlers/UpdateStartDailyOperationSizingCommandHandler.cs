using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
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
        private readonly IBeamRepository
            _beamDocumentRepository;

        public UpdateStartDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
            _beamDocumentRepository = _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateStartDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query
                                                               .Include(d => d.SizingDetails)
                                                               .Where(detail => detail.Identity.Equals(request.Id))
                                                               .Include(b => b.SizingBeamDocuments)
                                                               .Where(beamDocument => beamDocument.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var existingBeamdocuments = existingDailyOperation.SizingBeamDocuments.OrderByDescending(b => b.DateTimeBeamDocument);
            var lastBeamDocument = existingBeamdocuments.FirstOrDefault();
            var existingDetails = existingDailyOperation.SizingDetails.OrderByDescending(d => d.DateTimeMachine);
            var lastDetail = existingDetails.FirstOrDefault();

            //Validation for Start Status
            var countStartStatus =
                existingDailyOperation
                    .SizingDetails
                    .Where(e => e.MachineStatus == MachineStatus.ONSTART)
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
            var year = request.SizingDetails.StartDate.Year;
            var month = request.SizingDetails.StartDate.Month;
            var day = request.SizingDetails.StartDate.Day;
            var hour = request.SizingDetails.StartTime.Hours;
            var minutes = request.SizingDetails.StartTime.Minutes;
            var seconds = request.SizingDetails.StartTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var entryDateMachineLogUtc = new DateTimeOffset(lastDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var startDateMachineLogUtc = new DateTimeOffset(request.SizingDetails.StartDate.Date, new TimeSpan(+7, 0, 0));

            if (startDateMachineLogUtc < entryDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("StartDate", "Start date cannot less than latest date log"));
            } else
            {
                if (dateTimeOperation < lastDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("StartTime", "Start time cannot less than latest time log"));
                } else
                {
                    if (existingDetails.FirstOrDefault().MachineStatus == MachineStatus.ONENTRY)
                    {
                        var beamDocument = _beamDocumentRepository.Find(b => b.Identity.Equals(request.SizingBeamDocuments.SizingBeamId.Value)).FirstOrDefault();
                        var beamNumber = beamDocument.Number;

                        var newBeamDocument = new DailyOperationSizingBeamDocument(Guid.NewGuid(), 
                                                                                   new BeamId(beamDocument.Identity),
                                                                                   dateTimeOperation,
                                                                                   new DailyOperationSizingCounterValueObject(request.SizingBeamDocuments.Counter.Start, 0),
                                                                                   new DailyOperationSizingWeightValueObject(0, 0, 0),
                                                                                   0,
                                                                                   0,
                                                                                   BeamStatus.ONPROCESS);

                        existingDailyOperation.AddDailyOperationSizingBeamDocument(newBeamDocument);

                        //var newBeamDocument = new DailyOperationSizingBeamDocument(lastBeamDocument.Identity,
                        //                                                           new BeamId(beamDocument.Identity),
                        //                                                           dateTimeOperation,
                        //                                                           new DailyOperationSizingCounterValueObject(request.SizingBeamDocuments.Counter.Start, 0),
                        //                                                           new DailyOperationSizingWeightValueObject(0, 0, 0),
                        //                                                           0,
                        //                                                           0,
                        //                                                           BeamStatus.ONPROCESS);

                        //existingDailyOperation.UpdateSizingBeamDocuments(newBeamDocument);

                        var Causes = JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(lastDetail.Causes);

                        var entryDetailId = lastDetail.Identity;
                        var entryDetailShiftId = lastDetail.ShiftDocumentId;
                        var entryDetailOperatorId = lastDetail.OperatorDocumentId;
                        var entryDetailDateTimeMachine = lastDetail.DateTimeMachine;
                        var entryDetailMachineStatus = lastDetail.MachineStatus;
                        var entryDetailInformation = lastDetail.Information;
                        var entryDetailSizingBeamNumber = beamNumber;
                        var updateOnEntryOperationDetail = new DailyOperationSizingDetail(entryDetailId,
                                                                                          new ShiftId(entryDetailShiftId),
                                                                                          new OperatorId(entryDetailOperatorId),
                                                                                          entryDetailDateTimeMachine,
                                                                                          entryDetailMachineStatus,
                                                                                          entryDetailInformation,
                                                                                          new DailyOperationSizingCauseValueObject(Causes.BrokenBeam,
                                                                                          Causes.MachineTroubled),
                                                                                          entryDetailSizingBeamNumber);

                        existingDailyOperation.UpdateSizingDetail(updateOnEntryOperationDetail);

                        var newOperationDetail =
                                new DailyOperationSizingDetail(Guid.NewGuid(),
                                                               new ShiftId(request.SizingDetails.ShiftId.Value),
                                                               new OperatorId(request.SizingDetails.OperatorDocumentId.Value),
                                                               dateTimeOperation,
                                                               MachineStatus.ONSTART,
                                                               "-",
                                                               new DailyOperationSizingCauseValueObject(Causes.BrokenBeam, Causes.MachineTroubled),
                                                               beamNumber);

                        existingDailyOperation.AddDailyOperationSizingDetail(newOperationDetail);

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
            //var entryTimeMachineLog = lastDetail.DateTimeMachine.TimeOfDay;
            //var startTimeMachineLog = request.SizingDetails.StartTime;

            //if(startTimeMachineLog < entryTimeMachineLog)
            //{
            //    throw Validator.ErrorValidation(("StartTime", "Start time cannot less than latest time log"));
            //}
        }
    }
}
