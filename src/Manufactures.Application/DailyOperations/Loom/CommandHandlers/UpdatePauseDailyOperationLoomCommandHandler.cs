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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class UpdatePauseDailyOperationLoomCommandHandler : ICommandHandler<UpdatePauseDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomDocumentRepository;

        public UpdatePauseDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationLoomDocumentRepository = _storage.GetRepository<IDailyOperationLoomRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(UpdatePauseDailyOperationLoomCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Loom
            var loomQuery =
                _dailyOperationLoomDocumentRepository
                        .Query
                        .Include(o => o.LoomBeamHistories)
                        .Include(o => o.LoomBeamProducts)
                        .Where(doc => doc.Identity.Equals(request.Id));
            var existingDailyOperationLoomDocument =
                _dailyOperationLoomDocumentRepository
                        .Find(loomQuery)
                        .FirstOrDefault();

            //Get Daily Operation Loom History
            var existingDailyOperationLoomHistories =
                existingDailyOperationLoomDocument
                        .LoomBeamHistories
                        .Where(o => o.BeamNumber.Equals(request.PauseBeamNumber))
                        .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingDailyOperationLoomHistories.FirstOrDefault();

            //Get Daily Operation Loom History
            var existingDailyOperationLoomBeamProducts =
                existingDailyOperationLoomDocument
                        .LoomBeamProducts
                        .Where(o => o.Identity.Equals(request.PauseBeamNumber))
                        .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingDailyOperationSizingHistories.FirstOrDefault();

            //Reformat DateTime
            var year = request.PauseDateMachine.Year;
            var month = request.PauseDateMachine.Month;
            var day = request.PauseDateMachine.Day;
            var hour = request.PauseTimeMachine.Hours;
            var minutes = request.PauseTimeMachine.Minutes;
            var seconds = request.PauseTimeMachine.Seconds;
            var dateTimeHistory =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var pauseDateMachineLogUtc = new DateTimeOffset(request.PauseDateMachine.Date, new TimeSpan(+7, 0, 0));

            if (pauseDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("PauseDate", "Start date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeHistory <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("PauseTime", "Start time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastHistory.MachineStatus == MachineStatus.ONSTART || lastHistory.MachineStatus == MachineStatus.ONRESUME)
                    {
                        if (string.IsNullOrEmpty(request.ReprocessTo))
                        {
                            var newLoomHistory =
                                new DailyOperationLoomBeamHistory(Guid.NewGuid(),
                                                                  request.PauseBeamNumber,
                                                                  request.PauseMachineNumber,
                                                                  new OperatorId(request.PauseOperatorDocumentId.Value),
                                                                  dateTimeHistory,
                                                                  new ShiftId(request.PauseShiftDocumentId.Value),
                                                                  MachineStatus.ONSTART);

                            newLoomHistory.SetWarpBrokenThreads(request.WarpBrokenThreads ?? 0);
                            newLoomHistory.SetWeftBrokenThreads(request.WeftBrokenThreads ?? 0);
                            newLoomHistory.SetLenoBrokenThreads(request.LenoBrokenThreads ?? 0);
                            newLoomHistory.SetReprocessTo(request.ReprocessTo);
                            newLoomHistory.SetInformation(request.Information ?? "");

                            existingDailyOperationLoomDocument.AddDailyOperationLoomHistory(newLoomHistory);

                            await _dailyOperationLoomDocumentRepository.Update(existingDailyOperationLoomDocument);
                            _storage.Save();
                        }
                        else
                        {
                            var newLoomHistory =
                                                            new DailyOperationLoomBeamHistory(Guid.NewGuid(),
                                                                                              request.PauseBeamNumber,
                                                                                              request.PauseMachineNumber,
                                                                                              new OperatorId(request.PauseOperatorDocumentId.Value),
                                                                                              dateTimeHistory,
                                                                                              new ShiftId(request.PauseShiftDocumentId.Value),
                                                                                              MachineStatus.ONSTART);

                            newLoomHistory.SetWarpBrokenThreads(request.WarpBrokenThreads ?? 0);
                            newLoomHistory.SetWeftBrokenThreads(request.WeftBrokenThreads ?? 0);
                            newLoomHistory.SetLenoBrokenThreads(request.LenoBrokenThreads ?? 0);
                            newLoomHistory.SetReprocessTo(request.ReprocessTo??"");
                            newLoomHistory.SetInformation(request.Information ?? "");

                            existingDailyOperationLoomDocument.AddDailyOperationLoomHistory(newLoomHistory);

                            await _dailyOperationLoomDocumentRepository.Update(existingDailyOperationLoomDocument);
                            _storage.Save();
                        }

                        return existingDailyOperationLoomDocument;
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
