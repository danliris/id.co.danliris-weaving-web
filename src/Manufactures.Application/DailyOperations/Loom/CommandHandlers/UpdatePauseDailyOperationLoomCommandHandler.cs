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
        private readonly IDailyOperationLoomBeamHistoryRepository _dailyOperationLoomHistoryRepository;
        private readonly IDailyOperationLoomBeamProductRepository _dailyOperationLoomProductRepository;

        public UpdatePauseDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationLoomDocumentRepository = _storage.GetRepository<IDailyOperationLoomRepository>();
            _dailyOperationLoomHistoryRepository = _storage.GetRepository<IDailyOperationLoomBeamHistoryRepository>();
            _dailyOperationLoomProductRepository = _storage.GetRepository<IDailyOperationLoomBeamProductRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(UpdatePauseDailyOperationLoomCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Loom
            //var loomQuery =
            //    _dailyOperationLoomDocumentRepository
            //            .Query
            //            .Include(o => o.LoomBeamHistories)
            //            .Include(o => o.LoomBeamProducts)
            //            .Where(doc => doc.Identity.Equals(request.Id));
            var existingDailyOperationLoomDocument =
                _dailyOperationLoomDocumentRepository
                        .Find(s => s.Identity == request.Id)
                        .FirstOrDefault();
            if (existingDailyOperationLoomDocument == null)
                throw Validator.ErrorValidation(("Id", "Invalid Daily Operation Loom: " + request.Id));

            var loomHistories = _dailyOperationLoomHistoryRepository.Find(s => s.DailyOperationLoomDocumentId == existingDailyOperationLoomDocument.Identity);
            var loomProducts = _dailyOperationLoomProductRepository.Find(s => s.DailyOperationLoomDocumentId == existingDailyOperationLoomDocument.Identity);


            //Get Daily Operation Loom History
            var existingDailyOperationLoomHistories =
                loomHistories
                        .Where(o => o.BeamNumber.Equals(request.PauseBeamNumber))
                        .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingDailyOperationLoomHistories.FirstOrDefault();

            //Get Daily Operation Loom Beam Product
            var existingDailyOperationLoomBeamProducts =
               loomProducts
                        .Where(o => o.BeamDocumentId.Value == request.PauseBeamProductBeamId)
                        .OrderByDescending(o => o.DateTimeProcessed);
            var lastBeamProduct = existingDailyOperationLoomBeamProducts.FirstOrDefault();

            //Reformat DateTime
            var year = request.PauseDateMachine.Year;
            var month = request.PauseDateMachine.Month;
            var day = request.PauseDateMachine.Day;
            var hour = request.PauseTimeMachine.Hours;
            var minutes = request.PauseTimeMachine.Minutes;
            var seconds = request.PauseTimeMachine.Seconds;
            var pauseDateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var pauseDateMachineLogUtc = new DateTimeOffset(request.PauseDateMachine.Date, new TimeSpan(+7, 0, 0));

            if (pauseDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("PauseDate", "Pause date cannot less than latest date log"));
            }
            else
            {
                if (pauseDateTime <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("PauseTime", "Pause time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastHistory.MachineStatus == MachineStatus.ONSTART || lastHistory.MachineStatus == MachineStatus.ONRESUME)
                    {
                        if (string.IsNullOrEmpty(request.ReprocessTo))
                        {
                            var newLoomHistory =
                                new DailyOperationLoomHistory(Guid.NewGuid(),
                                                                  request.PauseBeamNumber,
                                                                  request.PauseMachineNumber,
                                                                  new OperatorId(request.PauseOperatorDocumentId.Value),
                                                                  pauseDateTime,
                                                                  new ShiftId(request.PauseShiftDocumentId.Value),
                                                                  MachineStatus.ONSTOP,
                                                                  existingDailyOperationLoomDocument.Identity);

                            newLoomHistory.SetWarpBrokenThreads(request.WarpBrokenThreads ?? 0);
                            newLoomHistory.SetWeftBrokenThreads(request.WeftBrokenThreads ?? 0);
                            newLoomHistory.SetLenoBrokenThreads(request.LenoBrokenThreads ?? 0);
                            newLoomHistory.SetInformation(request.Information);

                            //existingDailyOperationLoomDocument.AddDailyOperationLoomHistory(newLoomHistory);
                            await _dailyOperationLoomHistoryRepository.Update(newLoomHistory);

                            lastBeamProduct.SetLatestDateTimeBeamProduct(pauseDateTime);

                            await _dailyOperationLoomDocumentRepository.Update(existingDailyOperationLoomDocument);
                            _storage.Save();
                        }
                        else
                        {
                            var newLoomHistory =
                                new DailyOperationLoomHistory(Guid.NewGuid(),
                                                                  request.PauseBeamNumber,
                                                                  request.PauseMachineNumber,
                                                                  new OperatorId(request.PauseOperatorDocumentId.Value),
                                                                  pauseDateTime,
                                                                  new ShiftId(request.PauseShiftDocumentId.Value),
                                                                  MachineStatus.ONCOMPLETE,
                                                                  existingDailyOperationLoomDocument.Identity);

                            newLoomHistory.SetWarpBrokenThreads(request.WarpBrokenThreads ?? 0);
                            newLoomHistory.SetWeftBrokenThreads(request.WeftBrokenThreads ?? 0);
                            newLoomHistory.SetLenoBrokenThreads(request.LenoBrokenThreads ?? 0);
                            newLoomHistory.SetReprocessTo(request.ReprocessTo);
                            newLoomHistory.SetInformation(request.Information);

                            //existingDailyOperationLoomDocument.AddDailyOperationLoomHistory(newLoomHistory);
                            await _dailyOperationLoomHistoryRepository.Update(newLoomHistory);

                            lastBeamProduct.SetLatestDateTimeBeamProduct(pauseDateTime);
                            lastBeamProduct.SetBeamProductStatus(BeamStatus.END);

                            await Task.Yield();
                            var isAllBeamProductProcessed = 0;
                            foreach (var beamProduct in loomProducts)
                            {
                                if (beamProduct.BeamUsedStatus == BeamStatus.ONPROCESS)
                                {
                                    isAllBeamProductProcessed++;
                                }
                            };

                            await Task.Yield();
                            if (isAllBeamProductProcessed == 0)
                            {
                                existingDailyOperationLoomDocument.SetOperationStatus(OperationStatus.ONFINISH);
                            }

                            await _dailyOperationLoomDocumentRepository.Update(existingDailyOperationLoomDocument);
                            _storage.Save();
                        }

                        return existingDailyOperationLoomDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't pause, latest machine status must ONSTART or ONRESUME"));
                    }
                }
            }
        }
    }
}
