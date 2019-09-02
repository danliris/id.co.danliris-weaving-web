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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class ProduceBeamDailyOperationSizingCommandHandler : ICommandHandler<ProduceBeamDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IBeamRepository
            _beamDocumentRepository;

        public ProduceBeamDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
            _beamDocumentRepository = _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(ProduceBeamDailyOperationSizingCommand request, CancellationToken cancellationToken)
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
                throw Validator.ErrorValidation(("BeamStatus", "Can't Produce Beam. There isn't ONPROCESS Sizing Beam on this Operation"));
            }

            //Validation for Machine Status
            var currentMachineStatus = lastDetail.MachineStatus;

            if (currentMachineStatus.Equals(MachineStatus.ONCOMPLETE))
            {
                throw Validator.ErrorValidation(("MachineStatus", "Can't Produce Beam. This current Operation status already ONCOMPLETE"));
            }

            //Validation for Operation Status
            var currentOperationStatus =
                existingDailyOperation.OperationStatus;

            if (currentOperationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Produce Beam. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.SizingDetails.ProduceBeamDate.Year;
            var month = request.SizingDetails.ProduceBeamDate.Month;
            var day = request.SizingDetails.ProduceBeamDate.Day;
            var hour = request.SizingDetails.ProduceBeamTime.Hours;
            var minutes = request.SizingDetails.ProduceBeamTime.Minutes;
            var seconds = request.SizingDetails.ProduceBeamTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var produceBeamDateMachineLogUtc = new DateTimeOffset(request.SizingDetails.ProduceBeamDate.Date, new TimeSpan(+7, 0, 0));

            if (produceBeamDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ProduceBeamDate", "Produce Beam date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ProduceBeamTime", "Produce Beam time cannot less than or equal latest time log"));
                }
                else
                {
                    if (existingDetails.FirstOrDefault().MachineStatus == MachineStatus.ONSTART || existingDetails.FirstOrDefault().MachineStatus == MachineStatus.ONRESUME)
                    {
                        var counter = JsonConvert.DeserializeObject<DailyOperationSizingCounterValueObject>(lastBeamDocument.Counter);
                        var weight = request.SizingBeamDocuments.Weight;
                        var theoritical = Math.Round(weight.Theoritical,2);
                        var spu = Math.Round(request.SizingBeamDocuments.SPU, 2);

                        var updateBeamDocument = new DailyOperationSizingBeamDocument(lastBeamDocument.Identity,
                                                                                   new BeamId(lastBeamDocument.SizingBeamId),
                                                                                   dateTimeOperation,
                                                                                   new DailyOperationSizingCounterValueObject(counter.Start, request.SizingBeamDocuments.FinishCounter),
                                                                                   new DailyOperationSizingWeightValueObject(weight.Netto, weight.Bruto, theoritical),
                                                                                   request.SizingBeamDocuments.PISMeter,
                                                                                   spu,
                                                                                   BeamStatus.ROLLEDUP);

                        existingDailyOperation.UpdateDailyOperationSizingBeamDocument(updateBeamDocument);

                        var causes = JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(lastDetail.Causes);

                        var newOperationDetail =
                                new DailyOperationSizingDetail(Guid.NewGuid(),
                                                               new ShiftId(request.SizingDetails.ShiftId.Value),
                                                               new OperatorId(request.SizingDetails.OperatorDocumentId.Value),
                                                               dateTimeOperation,
                                                               MachineStatus.ONCOMPLETE,
                                                               "-",
                                                               new DailyOperationSizingCauseValueObject(causes.BrokenBeam, causes.MachineTroubled),
                                                               lastDetail.SizingBeamNumber);

                        existingDailyOperation.AddDailyOperationSizingDetail(newOperationDetail);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                        _storage.Save();

                        return existingDailyOperation;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("Status", "Can't Produce Beam, latest status is not ONSTART or ONRESUME"));
                    }
                }
            }
        }
    }
}
