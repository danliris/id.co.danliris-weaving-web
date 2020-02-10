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
        private readonly IDailyOperationSizingHistoryRepository
            _dailyOperationSizingHistoryRepository;
        private readonly IDailyOperationSizingBeamProductRepository
            _dailyOperationSizingBeamProductRepository;
        private readonly IBeamRepository
            _beamDocumentRepository;

        public ProduceBeamDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = 
                storage;
            _dailyOperationSizingDocumentRepository = 
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationSizingHistoryRepository =
                _storage.GetRepository<IDailyOperationSizingHistoryRepository>();
            _dailyOperationSizingBeamProductRepository =
                _storage.GetRepository<IDailyOperationSizingBeamProductRepository>();
            _beamDocumentRepository = 
                _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(ProduceBeamDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Sizing
            var existingSizingDocument =
                _dailyOperationSizingDocumentRepository
                        .Find(o=>o.Identity == request.Id)
                        .FirstOrDefault();

            //Get Daily Operation History
            var existingSizingHistories =
                _dailyOperationSizingHistoryRepository
                    .Find(o=>o.DailyOperationSizingDocumentId == existingSizingDocument.Identity)
                    .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingSizingHistories.FirstOrDefault();

            //Validation for Beam Status
            //var currentBeamStatus = lastBeamProduct.BeamStatus;

            //if (!currentBeamStatus.Equals(BeamStatus.ONPROCESS))
            //{
            //    throw Validator.ErrorValidation(("BeamStatus", "Can't Produce Beam. There isn't ONPROCESS Sizing Beam on this Operation"));
            //}

            //Validation for Operation Status
            var currentOperationStatus =
                existingSizingDocument.OperationStatus;

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
                    if (existingSizingHistories.FirstOrDefault().MachineStatus == MachineStatus.ONSTART || 
                        existingSizingHistories.FirstOrDefault().MachineStatus == MachineStatus.ONRESUME)
                    {
                        //Get Daily Operation Beam Product
                        var existingDailyOperationBeamProducts =
                            _dailyOperationSizingBeamProductRepository
                                .Find(o=>o.DailyOperationSizingDocumentId == existingSizingDocument.Identity)
                                .OrderByDescending(o => o.LatestDateTimeBeamProduct);
                        var lastBeamProduct = existingDailyOperationBeamProducts.FirstOrDefault();

                        //Set Beam Product Value on Daily Operation Sizing Beam Document
                        var theoriticalLimit = Math.Round(request.WeightTheoritical, 2);
                        var spuLimit = Math.Round(request.SPU, 2);

                        var updateBeamProduct = 
                            new DailyOperationSizingBeamProduct(lastBeamProduct.Identity,
                                                                lastBeamProduct.SizingBeamId,
                                                                lastBeamProduct.CounterStart,
                                                                BeamStatus.ROLLEDUP,
                                                                dateTimeOperation,
                                                                existingSizingDocument.Identity);
                        updateBeamProduct.SetCounterFinish(request.CounterFinish);
                        updateBeamProduct.SetWeightNetto(request.WeightNetto);
                        updateBeamProduct.SetWeightBruto(request.WeightBruto);
                        updateBeamProduct.SetWeightTheoritical(theoriticalLimit);
                        updateBeamProduct.SetPISMeter(request.PISMeter);
                        updateBeamProduct.SetSPU(spuLimit);

                        await _dailyOperationSizingBeamProductRepository.Update(updateBeamProduct);

                        //Set History Value on Daily Operation Sizing Detail
                        var newHistory =
                                new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                request.ProduceBeamShift,
                                                                request.ProduceBeamOperator,
                                                                dateTimeOperation,
                                                                MachineStatus.ONCOMPLETE,
                                                                "",
                                                                lastHistory.BrokenPerShift,
                                                                lastHistory.MachineTroubled,
                                                                lastHistory.SizingBeamNumber,
                                                                existingSizingDocument.Identity);

                        await _dailyOperationSizingHistoryRepository.Update(newHistory);

                        //Set YarnStrands Value on Master Beam
                        var beamQuery = 
                            _beamDocumentRepository
                                .Query
                                .Where(beam => beam.Identity.Equals(lastBeamProduct.SizingBeamId));
                        var beamDocument = 
                            _beamDocumentRepository
                                .Find(beamQuery)
                                .FirstOrDefault();
                        beamDocument.SetLatestYarnStrands(existingSizingDocument.YarnStrands);

                        await _beamDocumentRepository.Update(beamDocument);

                        _storage.Save();

                        return existingSizingDocument;
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
