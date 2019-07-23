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
                                                               .Where(detail => detail.Identity.Equals(request.Id))
                                                               .Include(b => b.SizingBeamDocuments)
                                                               .Where(beamDocument => beamDocument.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var existingBeamdocuments = existingDailyOperation.SizingBeamDocuments.OrderByDescending(b => b.DateTimeBeamDocument);
            var lastBeamDocument = existingBeamdocuments.FirstOrDefault();
            var existingDetails = existingDailyOperation.SizingDetails.OrderByDescending(d => d.DateTimeMachine);
            var lastDetail = existingDetails.FirstOrDefault();

            ////Validation for Start Status
            //var countStartStatus =
            //    existingDailyOperation
            //        .SizingDetails
            //        .Where(e => e.MachineStatus == MachineStatus.ONSTART)
            //        .Count();

            //if (countStartStatus == 1)
            //{
            //    throw Validator.ErrorValidation(("StartStatus", "This operation already has START status"));
            //}

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
                if (dateTimeOperation < lastDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ProduceBeamTime", "Produce Beam time cannot less than latest time log"));
                }
                else
                {
                    if (existingDetails.FirstOrDefault().MachineStatus == MachineStatus.ONSTART || existingDetails.FirstOrDefault().MachineStatus == MachineStatus.ONRESUME)
                    {
                        //var sizingBeamDocument = _beamDocumentRepository.Find(b => b.Identity.Equals(request.SizingBeamDocuments.SizingBeamId.Value)).FirstOrDefault();
                        //var sizingBeamNumber = sizingBeamDocument.Number;
                        var counter = JsonConvert.DeserializeObject<DailyOperationSizingCounterValueObject>(lastBeamDocument.Counter);
                        var weight = request.SizingBeamDocuments.Weight;

                        var updateBeamDocument = new DailyOperationSizingBeamDocument(lastBeamDocument.Identity,
                                                                                   new BeamId(lastBeamDocument.SizingBeamId),
                                                                                   dateTimeOperation,
                                                                                   new DailyOperationSizingCounterValueObject(counter.Start, request.SizingBeamDocuments.Counter.Finish),
                                                                                   new DailyOperationSizingWeightValueObject(weight.Netto, weight.Bruto, weight.Theoritical),
                                                                                   request.SizingBeamDocuments.PISMeter,
                                                                                   request.SizingBeamDocuments.SPU,
                                                                                   BeamStatus.ROLLEDUP);

                        existingDailyOperation.UpdateSizingBeamDocuments(updateBeamDocument);

                        //var entryDetailId = lastDetail.Identity;
                        //var entryDetailShiftId = lastDetail.ShiftDocumentId;
                        //var entryDetailOperatorId = lastDetail.OperatorDocumentId;
                        //var entryDetailDateTimeMachine = lastDetail.DateTimeMachine;
                        //var entryDetailMachineStatus = lastDetail.MachineStatus;
                        //var entryDetailInformation = lastDetail.Information;
                        //var entryDetailSizingBeamNumber = sizingBeamNumber;
                        //var updateOnEntryOperationDetail = new DailyOperationSizingDetail(entryDetailId,
                        //                                                                  new ShiftId(entryDetailShiftId),
                        //                                                                  new OperatorId(entryDetailOperatorId),
                        //                                                                  entryDetailDateTimeMachine,
                        //                                                                  entryDetailMachineStatus,
                        //                                                                  entryDetailInformation,
                        //                                                                  new DailyOperationSizingCauseValueObject(causes.BrokenBeam,
                        //                                                                  causes.MachineTroubled),
                        //                                                                  entryDetailSizingBeamNumber);

                        //existingDailyOperation.UpdateSizingDetail(updateOnEntryOperationDetail);

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
