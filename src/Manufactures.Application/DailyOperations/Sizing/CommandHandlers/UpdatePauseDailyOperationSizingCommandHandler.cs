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
    public class UpdatePauseDailyOperationSizingCommandHandler : ICommandHandler<UpdatePauseDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IBeamRepository
            _beamDocumentRepository;

        public UpdatePauseDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
            _beamDocumentRepository = _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdatePauseDailyOperationSizingCommand request, CancellationToken cancellationToken)
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

            if (countStartStatus == 0)
            {
                throw Validator.ErrorValidation(("StartStatus", "This operation has not started yet"));
            }

            //Validation for Finish Status
            var countFinishStatus =
                existingDailyOperation
                    .SizingDetails
                    .Where(e => e.MachineStatus == MachineStatus.ONCOMPLETE)
                    .Count();

            if (countFinishStatus == 1)
            {
                throw Validator.ErrorValidation(("FinishStatus", "This operation's status already COMPLETED"));
            }

            //Reformat DateTime
            var year = request.Details.PauseDate.Year;
            var month = request.Details.PauseDate.Month;
            var day = request.Details.PauseDate.Day;
            var hour = request.Details.PauseTime.Hours;
            var minutes = request.Details.PauseTime.Minutes;
            var seconds = request.Details.PauseTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Pause Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var pauseDateMachineLogUtc = new DateTimeOffset(request.Details.PauseDate.Date, new TimeSpan(+7, 0, 0));

            if (pauseDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("PauseDate", "Pause date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation < lastDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("PauseTime", "Pause time cannot less than latest operation"));
                }
                else
                {
                    if (existingDetails.FirstOrDefault().MachineStatus == MachineStatus.ONSTART || existingDetails.FirstOrDefault().MachineStatus == MachineStatus.ONRESUME)
                    {
                        //var beamDocument = _beamDocumentRepository.Find(b => b.Identity.Equals(request.SizingBeamDocuments.SizingBeamId.Value)).FirstOrDefault();
                        //var beamNumber = beamDocument.Number;

                        var counter = JsonConvert.DeserializeObject<DailyOperationSizingCounterCommand>(lastBeamDocument.Counter);
                        var weight = JsonConvert.DeserializeObject<DailyOperationSizingWeightCommand>(lastBeamDocument.Weight);

                        var newBeamDocument = new DailyOperationSizingBeamDocument(lastBeamDocument.Identity,
                                                                                   new BeamId(lastBeamDocument.SizingBeamId),
                                                                                   dateTimeOperation,
                                                                                   new DailyOperationSizingCounterValueObject(counter.Start, counter.Finish),
                                                                                   new DailyOperationSizingWeightValueObject(weight.Netto, weight.Bruto, weight.Theoritical),
                                                                                   lastBeamDocument.PISMeter,
                                                                                   lastBeamDocument.SPU,
                                                                                   BeamStatus.ONPROCESS);

                        existingDailyOperation.UpdateSizingBeamDocuments(newBeamDocument);

                        var Causes = request.Details.Causes;

                        var newOperation =
                                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                                   new ShiftId(request.Details.ShiftId.Value),
                                                                   new OperatorId(request.Details.OperatorDocumentId.Value),
                                                                   dateTimeOperation,
                                                                   MachineStatus.ONSTOP,
                                                                   request.Details.Information,
                                                                   new DailyOperationSizingCauseValueObject(Causes.BrokenBeam, Causes.MachineTroubled),
                                                                   lastDetail.SizingBeamNumber);

                        existingDailyOperation.AddDailyOperationSizingDetail(newOperation);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                        _storage.Save();

                        return existingDailyOperation;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("Status", "Can't stop, latest status is not on PROCESS or on RESUME"));
                    }
                }
            }

            //Validation for Pause Time
            //var lastTimeMachineLog = lastDetail.DateTimeMachine.TimeOfDay;
            //var pauseTimeMachineLog = request.SizingDetails.PauseTime;

            //if (pauseTimeMachineLog < lastTimeMachineLog)
            //{
            //    throw Validator.ErrorValidation(("PauseTime", "Pause time cannot less than latest time log"));
            //}
        }
    }
}
