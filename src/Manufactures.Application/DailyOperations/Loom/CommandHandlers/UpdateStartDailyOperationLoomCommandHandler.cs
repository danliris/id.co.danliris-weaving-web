using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class UpdateStartDailyOperationLoomCommandHandler : ICommandHandler<UpdateStartDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage
            _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomDocumentRepository;
        private readonly IDailyOperationLoomHistoryRepository
            _dailyOperationLoomHistoryRepository;
        private readonly IDailyOperationLoomBeamUsedRepository
            _dailyOperationLoomBeamUsedRepository;
        //private readonly IDailyOperationLoomBeamProductRepository 
        //    _dailyOperationLoomProductRepository;

        public UpdateStartDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage =
                storage;
            _dailyOperationLoomDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _dailyOperationLoomHistoryRepository =
                _storage.GetRepository<IDailyOperationLoomHistoryRepository>();
            _dailyOperationLoomBeamUsedRepository =
                _storage.GetRepository<IDailyOperationLoomBeamUsedRepository>();
            //_dailyOperationLoomProductRepository = 
            //    _storage.GetRepository<IDailyOperationLoomBeamProductRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(UpdateStartDailyOperationLoomCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Loom
            var loomDocument =
                _dailyOperationLoomDocumentRepository
                        .Find(s => s.Identity == request.Id)
                        .FirstOrDefault();

            if (loomDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Daily Operation Loom: " + request.Id));
            }

            var lastLoomHistory =
                _dailyOperationLoomHistoryRepository
                    .Find(s => s.DailyOperationLoomDocumentId == loomDocument.Identity)
                    .Where(o => o.BeamDocumentId == request.StartBeamDocumentId)
                    .OrderByDescending(d => d.DateTimeMachine)
                    .FirstOrDefault();

            var lastLoomBeamUsed =
                _dailyOperationLoomBeamUsedRepository
                    .Find(s => s.DailyOperationLoomDocumentId == loomDocument.Identity)
                    .Where(o => o.BeamDocumentId == request.StartBeamDocumentId)
                    .OrderByDescending(d => d.LastDateTimeProcessed)
                    .FirstOrDefault();

            //Get Daily Operation Loom History
            //var loomHistories =
            //    loomHistories
            //            .Where(o => o.BeamNumber.Equals(request.StartBeamNumber))
            //            .OrderByDescending(o => o.DateTimeMachine);
            //var lastHistory = loomHistories.FirstOrDefault();

            ////Get Daily Operation Loom Beam Product
            //var loomBeamsUsed =
            //    loomBeamsUsed
            //            .Where(o => o.Identity.Equals(request.StartBeamDocumentId))
            //            .OrderByDescending(o => o.LastDateTimeProcessed);
            //var lastBeamProduct = loomBeamsUsed.FirstOrDefault();

            //Validation for Prevent Beam Product with End Status Processed Again
            if (lastLoomBeamUsed.BeamUsedStatus.Equals(BeamStatus.END))
            {
                throw Validator.ErrorValidation(("StartBeamNumber", "Status Beam ini Reproses, Tidak Dapat Diproses Kembali"));
            }

            //Reformat DateTime
            var year = request.StartDateMachine.Year;
            var month = request.StartDateMachine.Month;
            var day = request.StartDateMachine.Day;
            var hour = request.StartTimeMachine.Hours;
            var minutes = request.StartTimeMachine.Minutes;
            var seconds = request.StartTimeMachine.Seconds;
            var dateTimeLoom =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastLoomHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var startDateMachineLogUtc = new DateTimeOffset(request.StartDateMachine.Date, new TimeSpan(+7, 0, 0));

            if (startDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("StartDate", "Start date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeLoom <= lastLoomHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("StartTime", "Start time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastLoomHistory.MachineStatus == MachineStatus.ONENTRY || lastLoomHistory.MachineStatus == MachineStatus.ONCOMPLETE)
                    {
                        var newLoomHistory =
                                new DailyOperationLoomHistory(Guid.NewGuid(),
                                                              request.StartBeamDocumentId,
                                                              request.StartBeamNumber,
                                                              lastLoomHistory.LoomMachineId,
                                                              request.StartLoomOperatorDocumentId,
                                                              request.StartCounterPerOperator,
                                                              dateTimeLoom,
                                                              request.StartShiftDocumentId,
                                                              MachineStatus.ONSTART,
                                                              loomDocument.Identity);

                        await _dailyOperationLoomHistoryRepository.Update(newLoomHistory);

                        lastLoomBeamUsed.SetStartCounter(request.StartCounterPerOperator);
                        lastLoomBeamUsed.SetLastDateTimeProcessed(dateTimeLoom);

                        await _dailyOperationLoomBeamUsedRepository.Update(lastLoomBeamUsed);

                        await _dailyOperationLoomDocumentRepository.Update(loomDocument);
                        _storage.Save();

                        return loomDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't start, latest machine status must ONENTRY or ONCOMPLETE"));
                    }
                }
            }
        }
    }
}
