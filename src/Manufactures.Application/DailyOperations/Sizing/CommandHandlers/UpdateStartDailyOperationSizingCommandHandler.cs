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
                                                               .Include(b => b.SizingBeamDocuments)
                                                               .Where(sizingDoc => sizingDoc.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var existingBeamdocuments = existingDailyOperation.SizingBeamDocuments.OrderByDescending(b => b.DateTimeBeamDocument);
            var lastBeamDocument = existingBeamdocuments.FirstOrDefault();
            var existingDetails = existingDailyOperation.SizingDetails.OrderByDescending(d => d.DateTimeMachine);
            var lastDetail = existingDetails.FirstOrDefault();

            //Validation for Beam Status
            var countBeamStatus =
                existingDailyOperation
                    .SizingBeamDocuments
                    .Where(e => e.SizingBeamStatus == BeamStatus.ONPROCESS)
                    .Count();

            if (!countBeamStatus.Equals(0))
            {
                throw Validator.ErrorValidation(("BeamStatus", "Can's Start. There's ONPROCESS Sizing Beam on this Operation"));
            }

            //Validation for Operation Status
            var operationCompleteStatus =
                existingDailyOperation.OperationStatus;

            if (operationCompleteStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can's Start. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.StartDate.Year;
            var month = request.StartDate.Month;
            var day = request.StartDate.Day;
            var hour = request.StartTime.Hours;
            var minutes = request.StartTime.Minutes;
            var seconds = request.StartTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var startDateMachineLogUtc = new DateTimeOffset(request.StartDate.Date, new TimeSpan(+7, 0, 0));

            if (startDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("StartDate", "Start date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("StartTime", "Start time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastDetail.MachineStatus == MachineStatus.ONENTRY)
                    {
                        var sizingBeamDocument = _beamDocumentRepository.Find(b => b.Identity.Equals(request.SizingBeamId.Value)).FirstOrDefault();
                        var sizingBeamNumber = sizingBeamDocument.Number;

                        var newBeamDocument = new DailyOperationSizingBeamDocument(Guid.NewGuid(),
                                                                                   new BeamId(sizingBeamDocument.Identity),
                                                                                   dateTimeOperation,
                                                                                   new DailyOperationSizingCounterValueObject(request.Start, 0),
                                                                                   new DailyOperationSizingWeightValueObject(0, 0, 0),
                                                                                   0,
                                                                                   0,
                                                                                   BeamStatus.ONPROCESS);
                        existingDailyOperation.AddDailyOperationSizingBeamDocument(newBeamDocument);

                        var causes = JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(lastDetail.Causes);
                        var newOperationDetail =
                                new DailyOperationSizingDetail(Guid.NewGuid(),
                                                               new ShiftId(request.StartShift.Value),
                                                               new OperatorId(request.StartOperator.Value),
                                                               dateTimeOperation,
                                                               MachineStatus.ONSTART,
                                                               "-",
                                                               new DailyOperationSizingCauseValueObject(causes.BrokenBeam, causes.MachineTroubled),
                                                               sizingBeamNumber);
                        existingDailyOperation.AddDailyOperationSizingDetail(newOperationDetail);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                        _storage.Save();

                        return existingDailyOperation;
                    }
                    else if (lastDetail.MachineStatus == MachineStatus.ONCOMPLETE)
                    {
                        if (lastBeamDocument.SizingBeamStatus == BeamStatus.ROLLEDUP)
                        {
                            var beamDocument = _beamDocumentRepository.Find(b => b.Identity.Equals(request.SizingBeamId.Value)).FirstOrDefault();
                            var beamNumber = beamDocument.Number;

                            var newBeamDocument = new DailyOperationSizingBeamDocument(Guid.NewGuid(),
                                                                                       new BeamId(beamDocument.Identity),
                                                                                       dateTimeOperation,
                                                                                       new DailyOperationSizingCounterValueObject(request.Start, 0),
                                                                                       new DailyOperationSizingWeightValueObject(0, 0, 0),
                                                                                       0,
                                                                                       0,
                                                                                       BeamStatus.ONPROCESS);
                            existingDailyOperation.AddDailyOperationSizingBeamDocument(newBeamDocument);

                            var causes = JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(lastDetail.Causes);
                            var newOperationDetail =
                                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                                   new ShiftId(request.StartShift.Value),
                                                                   new OperatorId(request.StartOperator.Value),
                                                                   dateTimeOperation,
                                                                   MachineStatus.ONSTART,
                                                                   "-",
                                                                   new DailyOperationSizingCauseValueObject(causes.BrokenBeam, causes.MachineTroubled),
                                                                   beamNumber);
                            existingDailyOperation.AddDailyOperationSizingDetail(newOperationDetail);

                            await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                            _storage.Save();

                            return existingDailyOperation;
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("BeamStatus", "Can't start, latest beam status must ROLLED-UP"));
                        }
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't start, latest machine status must ONENTRY or ONCOMPLETE"));
                    }
                }
            }
        }
    }
}
