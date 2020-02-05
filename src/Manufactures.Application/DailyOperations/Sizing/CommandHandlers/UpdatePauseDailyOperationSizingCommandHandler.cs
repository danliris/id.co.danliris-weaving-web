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
    public class UpdatePauseDailyOperationSizingCommandHandler : ICommandHandler<UpdatePauseDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IBeamRepository
            _beamDocumentRepository;

        public UpdatePauseDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _beamDocumentRepository = _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdatePauseDailyOperationSizingCommand request, CancellationToken cancellationToken)
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
            //if (lastBeamProduct.BeamStatus.Equals(null) || !lastBeamProduct.BeamStatus.Equals(BeamStatus.ONPROCESS))
            //{
            //    throw Validator.ErrorValidation(("BeamStatus", "Can't Pause. There isn't ONPROCESS Sizing Beam on this Operation"));
            //}

            //Validation for Operation Status
            var currentOperationStatus =
                existingDailyOperationSizingDocument.OperationStatus;

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
                    if (existingDailyOperationSizingHistories.FirstOrDefault().MachineStatus == MachineStatus.ONSTART || 
                        existingDailyOperationSizingHistories.FirstOrDefault().MachineStatus == MachineStatus.ONRESUME)
                    {
                        //Get Daily Operation Beam Product
                        var existingDailyOperationBeamProducts =
                            existingDailyOperationSizingDocument
                                    .SizingBeamProducts
                                    .OrderByDescending(o => o.LatestDateTimeBeamProduct);
                        var lastBeamProduct = existingDailyOperationBeamProducts.FirstOrDefault();

                        var updateBeamDocument = new DailyOperationSizingBeamProduct(lastBeamProduct.Identity,
                                                                                     new BeamId(lastBeamProduct.SizingBeamId),
                                                                                     dateTimeOperation,
                                                                                     lastBeamProduct.CounterStart ?? 0,
                                                                                     lastBeamProduct.CounterFinish ?? 0,
                                                                                     lastBeamProduct.WeightNetto ?? 0,
                                                                                     lastBeamProduct.WeightBruto ?? 0,
                                                                                     lastBeamProduct.WeightTheoritical ?? 0,
                                                                                     lastBeamProduct.PISMeter ?? 0,
                                                                                     lastBeamProduct.SPU ?? 0,
                                                                                     BeamStatus.ONPROCESS);

                        existingDailyOperationSizingDocument.UpdateDailyOperationSizingBeamProduct(updateBeamDocument);

                        var newOperation =
                                    new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                   new ShiftId(request.PauseShift.Value),
                                                                   new OperatorId(request.PauseOperator.Value),
                                                                   dateTimeOperation,
                                                                   MachineStatus.ONSTOP,
                                                                   request.Information,
                                                                   request.BrokenBeam,
                                                                   request.MachineTroubled,
                                                                   lastHistory.SizingBeamNumber);

                        existingDailyOperationSizingDocument.AddDailyOperationSizingHistory(newOperation);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperationSizingDocument);
                        _storage.Save();

                        return existingDailyOperationSizingDocument;
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
