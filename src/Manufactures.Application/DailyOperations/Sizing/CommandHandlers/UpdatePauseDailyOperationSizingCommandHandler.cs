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
                throw Validator.ErrorValidation(("BeamStatus", "Can't Pause. There isn't ONPROCESS Sizing Beam on this Operation"));
            }

            //Validation for Operation Status
            var currentOperationStatus =
                existingDailyOperation.OperationStatus;

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
            var lastDateMachineLogUtc = new DateTimeOffset(lastDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var pauseDateMachineLogUtc = new DateTimeOffset(request.PauseDate.Date, new TimeSpan(+7, 0, 0));

            if (pauseDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("PauseDate", "Pause date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("PauseTime", "Pause time cannot less than or equal latest operation"));
                }
                else
                {
                    if (existingDetails.FirstOrDefault().MachineStatus == MachineStatus.ONSTART || existingDetails.FirstOrDefault().MachineStatus == MachineStatus.ONRESUME)
                    {

                        var updateBeamDocument = new DailyOperationSizingBeamProduct(lastBeamDocument.Identity,
                                                                                      new BeamId(lastBeamDocument.SizingBeamId),
                                                                                      dateTimeOperation,
                                                                                      lastBeamDocument.CounterStart??0,
                                                                                      lastBeamDocument.CounterFinish ?? 0,
                                                                                      lastBeamDocument.WeightNetto ?? 0,
                                                                                      lastBeamDocument.WeightBruto ?? 0,
                                                                                      lastBeamDocument.WeightTheoritical ?? 0,
                                                                                      lastBeamDocument.PISMeter ?? 0,
                                                                                      lastBeamDocument.SPU ?? 0,
                                                                                      BeamStatus.ONPROCESS);

                        existingDailyOperation.UpdateDailyOperationSizingBeamProduct(updateBeamDocument);

                        var newOperation =
                                    new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                   new ShiftId(request.PauseShift.Value),
                                                                   new OperatorId(request.PauseOperator.Value),
                                                                   dateTimeOperation,
                                                                   MachineStatus.ONSTOP,
                                                                   request.Information,
                                                                   request.BrokenBeam, 
                                                                   request.MachineTroubled,
                                                                   lastDetail.SizingBeamNumber);

                        existingDailyOperation.AddDailyOperationSizingHistory(newOperation);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                        _storage.Save();

                        return existingDailyOperation;
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
