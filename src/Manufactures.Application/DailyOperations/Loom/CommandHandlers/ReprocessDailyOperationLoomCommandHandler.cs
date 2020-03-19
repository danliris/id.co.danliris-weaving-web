using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class ReprocessDailyOperationLoomCommandHandler : ICommandHandler<ReprocessDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage
            _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomDocumentRepository;
        private readonly IDailyOperationLoomHistoryRepository
            _dailyOperationLoomHistoryRepository;
        private readonly IDailyOperationLoomBeamUsedRepository
            _dailyOperationLoomBeamUsedRepository;

        public ReprocessDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage =
                storage;
            _dailyOperationLoomDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _dailyOperationLoomHistoryRepository =
                _storage.GetRepository<IDailyOperationLoomHistoryRepository>();
            _dailyOperationLoomBeamUsedRepository =
                _storage.GetRepository<IDailyOperationLoomBeamUsedRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(ReprocessDailyOperationLoomCommand request, CancellationToken cancellationToken)
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
                    .Where(o => o.BeamDocumentId.Value == request.ReprocessBeamDocumentId)
                    .OrderByDescending(d => d.DateTimeMachine)
                    .FirstOrDefault();

            var lastLoomBeamUsed =
                _dailyOperationLoomBeamUsedRepository
                    .Find(s => s.DailyOperationLoomDocumentId == loomDocument.Identity)
                    .Where(o => o.BeamDocumentId.Value == request.ReprocessBeamDocumentId)
                    .OrderByDescending(d => d.LastDateTimeProcessed)
                    .FirstOrDefault();

            //Reformat DateTime
            var year = request.ReprocessDateMachine.Year;
            var month = request.ReprocessDateMachine.Month;
            var day = request.ReprocessDateMachine.Day;
            var hour = request.ReprocessTimeMachine.Hours;
            var minutes = request.ReprocessTimeMachine.Minutes;
            var seconds = request.ReprocessTimeMachine.Seconds;
            var dateTimeLoom =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastLoomHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var reprocessDateMachineLogUtc = new DateTimeOffset(request.ReprocessDateMachine.Date, new TimeSpan(+7, 0, 0));

            if (reprocessDateMachineLogUtc < lastDateMachineLogUtc)
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
                    if (lastLoomHistory.MachineStatus == MachineStatus.ONSTART || lastLoomHistory.MachineStatus == MachineStatus.ONPROCESSBEAM)
                    {

                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}
