using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
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
                                                               .Include(d => d.SizingHistories)
                                                               .Include(b => b.SizingBeamProducts)
                                                               .Where(sizingDoc => sizingDoc.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var existingBeamdocuments = existingDailyOperation.SizingBeamProducts.OrderByDescending(b => b.LatestDateTimeBeamProduct);
            var lastBeamDocument = existingBeamdocuments.FirstOrDefault();
            var existingDetails = existingDailyOperation.SizingHistories.OrderByDescending(d => d.DateTimeMachine);
            var lastDetail = existingDetails.FirstOrDefault();

            //Validation for Beam Status
            var currentBeamStatus = lastBeamDocument.BeamStatus;

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
            var year = request.ProduceBeamDate.Year;
            var month = request.ProduceBeamDate.Month;
            var day = request.ProduceBeamDate.Day;
            var hour = request.ProduceBeamTime.Hours;
            var minutes = request.ProduceBeamTime.Minutes;
            var seconds = request.ProduceBeamTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var produceBeamDateMachineLogUtc = new DateTimeOffset(request.ProduceBeamDate.Date, new TimeSpan(+7, 0, 0));

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
                        //Set Detail Value on Daily Operation Sizing Beam Document
                        var theoritical = Math.Round(request.WeightTheoritical,2);
                        var spu = Math.Round(request.SPU, 2);
                        var updateBeamDocument = new DailyOperationSizingBeamProduct(lastBeamDocument.Identity,
                                                                                   new BeamId(lastBeamDocument.SizingBeamId),
                                                                                   dateTimeOperation,
                                                                                   lastBeamDocument.CounterStart ?? 0, 
                                                                                   request.CounterFinish,
                                                                                   request.WeightNetto,
                                                                                   request.WeightBruto, 
                                                                                   theoritical,
                                                                                   request.PISMeter,
                                                                                   spu,
                                                                                   BeamStatus.ROLLEDUP);
                        existingDailyOperation.UpdateDailyOperationSizingBeamDocument(updateBeamDocument);

                        //Set Detail Value on Daily Operation Sizing Detail
                        var newOperationDetail =
                                new DailyOperationSizingHistory(Guid.NewGuid(),
                                                               request.ProduceBeamShift,
                                                               request.ProduceBeamOperator,
                                                               dateTimeOperation,
                                                               MachineStatus.ONCOMPLETE,
                                                               "-",
                                                               lastDetail.BrokenBeam,
                                                               lastDetail.MachineTroubled,
                                                               lastDetail.SizingBeamNumber);
                        existingDailyOperation.AddDailyOperationSizingDetail(newOperationDetail);

                        //Set YarnStrands Value on Master Beam
                        var beamQuery = _beamDocumentRepository
                                            .Query
                                            .Where(beamDoc => beamDoc.Identity.Equals(lastBeamDocument.SizingBeamId));
                        var sizingBeamDocument = _beamDocumentRepository
                                            .Find(beamQuery)
                                            .FirstOrDefault();
                        sizingBeamDocument.SetLatestYarnStrands(existingDailyOperation.YarnStrands);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                        _storage.Save();

                        return existingDailyOperation;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't Produce Beam, latest status is not ONSTART or ONRESUME"));
                    }
                }
            }
        }
    }
}
