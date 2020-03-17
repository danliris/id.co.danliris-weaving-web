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
    public class FinishDailyOperationLoomCommandHandler : ICommandHandler<FinishDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository _dailyOperationLoomDocumentRepository;
        private readonly IDailyOperationLoomBeamHistoryRepository _dailyOperationLoomHistoryRepository;
        private readonly IDailyOperationLoomBeamProductRepository _dailyOperationLoomProductRepository;

        public FinishDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationLoomDocumentRepository = _storage.GetRepository<IDailyOperationLoomRepository>();
            _dailyOperationLoomHistoryRepository = _storage.GetRepository<IDailyOperationLoomBeamHistoryRepository>();
            _dailyOperationLoomProductRepository = _storage.GetRepository<IDailyOperationLoomBeamProductRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(FinishDailyOperationLoomCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Loom
            var existingDailyOperationLoomDocument =
                _dailyOperationLoomDocumentRepository
                        .Find(x => x.Identity == request.Id)
                        .FirstOrDefault();

            if (existingDailyOperationLoomDocument == null)
                throw Validator.ErrorValidation(("Id", "Invalid Daily Operation Loom: " + request.Id));

            var loomHistories = _dailyOperationLoomHistoryRepository.Find(s => s.DailyOperationLoomDocumentId == existingDailyOperationLoomDocument.Identity);
            var loomProducts = _dailyOperationLoomProductRepository.Find(s => s.DailyOperationLoomDocumentId == existingDailyOperationLoomDocument.Identity);


            //Get Daily Operation Loom History
            //var existingDailyOperationLoomHistories =
            //    existingDailyOperationLoomDocument
            //            .LoomBeamHistories
            //            .Where(o => o.BeamNumber.Equals(request.FinishBeamNumber))
            //            .OrderByDescending(o => o.DateTimeMachine);

            var existingDailyOperationLoomHistories = loomHistories.Where(o => o.BeamNumber.Equals(request.FinishBeamNumber)).OrderByDescending(x => x.DateTimeMachine);
            var lastHistory = existingDailyOperationLoomHistories.FirstOrDefault();

            //Get Daily Operation Loom Beam Product
            //var existingDailyOperationLoomBeamProducts =
            //    existingDailyOperationLoomDocument
            //            .LoomBeamProducts
            //            .Where(o => o.BeamDocumentId.Equals(request.FinishBeamProductBeamId))
            //            .OrderByDescending(o => o.LatestDateTimeBeamProduct);
            var existingDailyOperationLoomBeamProducts = loomProducts.Where(o => o.BeamDocumentId.Value == request.FinishBeamProductBeamId).OrderByDescending(x => x.DateTimeProcessed);
            var lastBeamProduct = existingDailyOperationLoomBeamProducts.FirstOrDefault();

            //Reformat DateTime
            var year = request.FinishDateMachine.Year;
            var month = request.FinishDateMachine.Month;
            var day = request.FinishDateMachine.Day;
            var hour = request.FinishTimeMachine.Hours;
            var minutes = request.FinishTimeMachine.Minutes;
            var seconds = request.FinishTimeMachine.Seconds;
            var finishDateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var finishDateMachineLogUtc = new DateTimeOffset(request.FinishDateMachine.Date, new TimeSpan(+7, 0, 0));

            if (finishDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("FinishDate", "Finish date cannot less than latest date log"));
            }
            else
            {
                if (finishDateTime <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("FinishTime", "Finish time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastHistory.MachineStatus == MachineStatus.ONSTART || lastHistory.MachineStatus == MachineStatus.ONRESUME)
                    {
                        var newLoomHistory =
                            new DailyOperationLoomHistory(Guid.NewGuid(),
                                                              request.FinishBeamNumber,
                                                              request.FinishMachineNumber,
                                                              new OperatorId(request.FinishOperatorDocumentId.Value),
                                                              finishDateTime,
                                                              new ShiftId(request.FinishShiftDocumentId.Value),
                                                              MachineStatus.ONCOMPLETE,
                                                              existingDailyOperationLoomDocument.Identity);

                        newLoomHistory.SetWarpBrokenThreads(lastHistory.WarpBrokenThreads ?? 0);
                        newLoomHistory.SetWeftBrokenThreads(lastHistory.WeftBrokenThreads ?? 0);
                        newLoomHistory.SetLenoBrokenThreads(lastHistory.LenoBrokenThreads ?? 0);

                        //existingDailyOperationLoomDocument.AddDailyOperationLoomHistory(newLoomHistory);
                        await _dailyOperationLoomHistoryRepository.Update(newLoomHistory);

                        lastBeamProduct.SetLatestDateTimeBeamProduct(finishDateTime);
                        lastBeamProduct.SetBeamProductStatus(BeamStatus.COMPLETED);

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

                        return existingDailyOperationLoomDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't finish, latest machine status must ONSTART or ONRESUME"));
                    }
                }
            }
        }
    }
}
