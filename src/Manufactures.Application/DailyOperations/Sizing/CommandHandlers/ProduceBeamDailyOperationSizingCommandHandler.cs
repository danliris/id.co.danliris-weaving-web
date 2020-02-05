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
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IBeamRepository
            _beamDocumentRepository;

        public ProduceBeamDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _beamDocumentRepository = _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(ProduceBeamDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Sizing
            var sizingQuery =
                _dailyOperationSizingDocumentRepository
                        .Query
                        .Include(d => d.SizingHistories)
                        .Include(b => b.SizingBeamProducts)
                        .Where(doc => doc.Identity.Equals(request.Id));
            var existingDailyOperationSizingDocument =
                _dailyOperationSizingDocumentRepository
                        .Find(sizingQuery)
                        .FirstOrDefault();

            //Get Daily Operation History
            var existingDailyOperationSizingHistories =
                existingDailyOperationSizingDocument
                        .SizingHistories
                        .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingDailyOperationSizingHistories.FirstOrDefault();

            //Validation for Beam Status
            //var currentBeamStatus = lastBeamProduct.BeamStatus;

            //if (!currentBeamStatus.Equals(BeamStatus.ONPROCESS))
            //{
            //    throw Validator.ErrorValidation(("BeamStatus", "Can't Produce Beam. There isn't ONPROCESS Sizing Beam on this Operation"));
            //}

            //Validation for Operation Status
            var currentOperationStatus =
                existingDailyOperationSizingDocument.OperationStatus;

            if (currentOperationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Produce Beam. This operation's status already FINISHED"));
            }

            //Validation for Machine Status
            var currentMachineStatus = lastHistory.MachineStatus;

            if (currentMachineStatus.Equals(MachineStatus.ONCOMPLETE))
            {
                throw Validator.ErrorValidation(("MachineStatus", "Can't Produce Beam. This current Operation status already ONCOMPLETE"));
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
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var produceBeamDateMachineLogUtc = new DateTimeOffset(request.ProduceBeamDate.Date, new TimeSpan(+7, 0, 0));

            if (produceBeamDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ProduceBeamDate", "Produce Beam date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ProduceBeamTime", "Produce Beam time cannot less than or equal latest time log"));
                }
                else
                {
                    if (existingDailyOperationSizingHistories.FirstOrDefault().MachineStatus == MachineStatus.ONSTART || 
                        existingDailyOperationSizingHistories.FirstOrDefault().MachineStatus == MachineStatus.ONRESUME)
                    {
                        //Get Daily Operation Beam Product
                        var existingDailyOperationBeamProducts =
                            existingDailyOperationSizingDocument
                                    .SizingBeamProducts
                                    .OrderByDescending(o => o.LatestDateTimeBeamProduct);
                        var lastBeamProduct = existingDailyOperationBeamProducts.FirstOrDefault();

                        //Set Beam Product Value on Daily Operation Sizing Beam Document
                        var theoritical = Math.Round(request.WeightTheoritical,2);
                        var spu = Math.Round(request.SPU, 2);
                        var updateBeamDocument = new DailyOperationSizingBeamProduct(lastBeamProduct.Identity,
                                                                                     new BeamId(lastBeamProduct.SizingBeamId),
                                                                                     dateTimeOperation,
                                                                                     lastBeamProduct.CounterStart ?? 0, 
                                                                                     request.CounterFinish,
                                                                                     request.WeightNetto,
                                                                                     request.WeightBruto, 
                                                                                     theoritical,
                                                                                     request.PISMeter,
                                                                                     spu,
                                                                                     BeamStatus.ROLLEDUP);
                        existingDailyOperationSizingDocument.UpdateDailyOperationSizingBeamProduct(updateBeamDocument);

                        //Set History Value on Daily Operation Sizing Detail
                        var newHistory =
                                new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                request.ProduceBeamShift,
                                                                request.ProduceBeamOperator,
                                                                dateTimeOperation,
                                                                MachineStatus.ONCOMPLETE,
                                                                "-",
                                                                lastHistory.BrokenBeam,
                                                                lastHistory.MachineTroubled,
                                                                lastHistory.SizingBeamNumber);
                        existingDailyOperationSizingDocument.AddDailyOperationSizingHistory(newHistory);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperationSizingDocument);

                        //Set YarnStrands Value on Master Beam
                        var beamQuery = 
                            _beamDocumentRepository
                                .Query
                                .Where(beam => beam.Identity.Equals(lastBeamProduct.SizingBeamId));
                        var beamDocument = 
                            _beamDocumentRepository
                                .Find(beamQuery)
                                .FirstOrDefault();
                        beamDocument.SetLatestYarnStrands(existingDailyOperationSizingDocument.YarnStrands);

                        await _beamDocumentRepository.Update(beamDocument);

                        _storage.Save();

                        return existingDailyOperationSizingDocument;
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
