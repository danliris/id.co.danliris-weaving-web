using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class UpdatePauseDailyOperationSizingCommandHandler : ICommandHandler<UpdatePauseDailyOperationSizingCommand, DailyOperationSizingDocument>
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

        public UpdatePauseDailyOperationSizingCommandHandler(IStorage storage)
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

        public async Task<DailyOperationSizingDocument> Handle(UpdatePauseDailyOperationSizingCommand request, CancellationToken cancellationToken)
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
            //if (lastBeamProduct.BeamStatus.Equals(null) || !lastBeamProduct.BeamStatus.Equals(BeamStatus.ONPROCESS))
            //{
            //    throw Validator.ErrorValidation(("BeamStatus", "Can't Pause. There isn't ONPROCESS Sizing Beam on this Operation"));
            //}

            //Validation for Operation Status
            var currentOperationStatus =
                existingSizingDocument.OperationStatus;

            if (currentOperationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Pause. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.PauseDate.Year;
            var month = request.PauseDate.Month;
            var day = request.PauseDate.Day;
            var hour = request.PauseTime.Hours;
            var minutes = request.PauseTime.Minutes;
            var seconds = request.PauseTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Pause Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var pauseDateMachineLogUtc = new DateTimeOffset(request.PauseDate.Date, new TimeSpan(+7, 0, 0));

            if (pauseDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("PauseDate", "Pause date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("PauseTime", "Pause time cannot less than or equal latest operation"));
                }
                else
                {
                    if (existingSizingHistories.FirstOrDefault().MachineStatus == MachineStatus.ONSTART ||
                        existingSizingHistories.FirstOrDefault().MachineStatus == MachineStatus.ONRESUME)
                    {
                        //Get Daily Operation Beam Product
                        var existingSizingBeamProducts =
                            _dailyOperationSizingBeamProductRepository
                                .Find(o => o.DailyOperationSizingDocumentId == existingSizingDocument.Identity)
                                .OrderByDescending(o => o.LatestDateTimeBeamProduct);
                        var lastBeamProduct = existingSizingBeamProducts.FirstOrDefault();

                        var updateBeamProduct = 
                            new DailyOperationSizingBeamProduct(lastBeamProduct.Identity,
                                                                lastBeamProduct.SizingBeamId,
                                                                lastBeamProduct.CounterStart,
                                                                BeamStatus.ONPROCESS,
                                                                dateTimeOperation,
                                                                existingSizingDocument.Identity);
                        updateBeamProduct.SetCounterFinish(lastBeamProduct.CounterFinish);
                        updateBeamProduct.SetWeightNetto(lastBeamProduct.WeightNetto);
                        updateBeamProduct.SetWeightBruto(lastBeamProduct.WeightBruto);
                        updateBeamProduct.SetWeightTheoritical(lastBeamProduct.WeightTheoritical);
                        updateBeamProduct.SetPISMeter(lastBeamProduct.PISMeter);
                        updateBeamProduct.SetSPU(lastBeamProduct.SPU);

                        await _dailyOperationSizingBeamProductRepository.Update(updateBeamProduct);

                        var newHistory = 
                            new DailyOperationSizingHistory(Guid.NewGuid(),
                                                            request.PauseShift,
                                                            request.PauseOperator,
                                                            dateTimeOperation,
                                                            MachineStatus.ONSTOP,
                                                            request.Information,
                                                            request.BrokenBeam,
                                                            request.MachineTroubled,
                                                            lastHistory.SizingBeamNumber,
                                                            existingSizingDocument.Identity);

                        await _dailyOperationSizingHistoryRepository.Update(newHistory);

                        _storage.Save();

                        return existingSizingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't stop, latest status is not on START or on RESUME"));
                    }
                }
            }
        }
    }
}
