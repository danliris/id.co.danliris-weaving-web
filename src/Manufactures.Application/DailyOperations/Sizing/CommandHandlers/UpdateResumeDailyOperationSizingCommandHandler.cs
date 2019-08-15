using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
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
    public class UpdateResumeDailyOperationSizingCommandHandler : ICommandHandler<UpdateResumeDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public UpdateResumeDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateResumeDailyOperationSizingCommand request, CancellationToken cancellationToken)
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
            var currentBeamStatus = lastBeamDocument.SizingBeamStatus;

            if (!currentBeamStatus.Equals(BeamStatus.ONPROCESS))
            {
                throw Validator.ErrorValidation(("BeamStatus", "Can't Resume. There isn't ONPROCESS Sizing Beam on this Operation"));
            }

            //Validation for Machine Status
            var currentMachineStatus = lastDetail.MachineStatus;

            if (!currentMachineStatus.Equals(MachineStatus.ONSTOP))
            {
                throw Validator.ErrorValidation(("MachineStatus", "Can't Resume. This current Operation status isn't ONSTOP"));
            }

            //Validation for Operation Status
            var currentOperationStatus =
                existingDailyOperation.OperationStatus;

            if (currentOperationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Resume. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.Details.ResumeDate.Year;
            var month = request.Details.ResumeDate.Month;
            var day = request.Details.ResumeDate.Day;
            var hour = request.Details.ResumeTime.Hours;
            var minutes = request.Details.ResumeTime.Minutes;
            var seconds = request.Details.ResumeTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Resume Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var resumeDateMachineLogUtc = new DateTimeOffset(request.Details.ResumeDate.Date, new TimeSpan(+7, 0, 0));

            if (resumeDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ResumeDate", "Resume date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation < lastDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ResumeTime", "Resume time cannot less than latest operation"));
                }
                else
                {
                    if (existingDetails.FirstOrDefault().MachineStatus == MachineStatus.ONSTOP)
                    {
                        var counter = JsonConvert.DeserializeObject<DailyOperationSizingCounterCommand>(lastBeamDocument.Counter);
                        var weight = JsonConvert.DeserializeObject<DailyOperationSizingWeightCommand>(lastBeamDocument.Weight);

                        var updateBeamDocument = new DailyOperationSizingBeamDocument(lastBeamDocument.Identity,
                                                                                      new BeamId(lastBeamDocument.SizingBeamId),
                                                                                      dateTimeOperation,
                                                                                      new DailyOperationSizingCounterValueObject(counter.Start, counter.Finish),
                                                                                      new DailyOperationSizingWeightValueObject(weight.Netto, weight.Bruto, weight.Theoritical),
                                                                                      lastBeamDocument.PISMeter,
                                                                                      lastBeamDocument.SPU,
                                                                                      BeamStatus.ONPROCESS);

                        existingDailyOperation.UpdateDailyOperationSizingBeamDocument(updateBeamDocument);

                        var causes = JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(lastDetail.Causes);

                        var newOperation =
                                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                                   new ShiftId(request.Details.ShiftId.Value),
                                                                   new OperatorId(request.Details.OperatorDocumentId.Value),
                                                                   dateTimeOperation,
                                                                   MachineStatus.ONRESUME,
                                                                   "-",
                                                                   new DailyOperationSizingCauseValueObject(causes.BrokenBeam, causes.MachineTroubled),
                                                                   lastDetail.SizingBeamNumber);

                        existingDailyOperation.AddDailyOperationSizingDetail(newOperation);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                        _storage.Save();

                        return existingDailyOperation;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("Status", "Can't continue, latest status is not on STOP"));
                    }
                }
            }
        }
    }
}
