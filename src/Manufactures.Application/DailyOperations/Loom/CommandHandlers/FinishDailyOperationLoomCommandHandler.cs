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
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomDocumentRepository;

        public FinishDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationLoomDocumentRepository = _storage.GetRepository<IDailyOperationLoomRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(FinishDailyOperationLoomCommand request, CancellationToken cancellationToken)
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
                        .Where(o => o.BeamNumber.Equals(request.FinishBeamNumber))
                        .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingDailyOperationLoomHistories.FirstOrDefault();

            //Get Daily Operation Loom Beam Product
            var existingDailyOperationLoomBeamProducts =
                existingDailyOperationLoomDocument
                        .LoomBeamProducts
                        .Where(o => o.BeamDocumentId.Equals(request.FinishBeamProductBeamId))
                        .OrderByDescending(o => o.LatestDateTimeBeamProduct);
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
                throw Validator.ErrorValidation(("FinishDate", "Resume date cannot less than latest date log"));
            }
            else
            {
                if (finishDateTime <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("FinishTime", "Resume time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastHistory.MachineStatus == MachineStatus.ONSTART || lastHistory.MachineStatus == MachineStatus.ONRESUME)
                    {
                        var newLoomHistory =
                            new DailyOperationLoomBeamHistory(Guid.NewGuid(),
                                                              request.FinishBeamNumber,
                                                              request.FinishMachineNumber,
                                                              new OperatorId(request.FinishOperatorDocumentId.Value),
                                                              finishDateTime,
                                                              new ShiftId(request.FinishShiftDocumentId.Value),
                                                              MachineStatus.ONCOMPLETE);

                        newLoomHistory.SetWarpBrokenThreads(lastHistory.WarpBrokenThreads ?? 0);
                        newLoomHistory.SetWeftBrokenThreads(lastHistory.WeftBrokenThreads ?? 0);
                        newLoomHistory.SetLenoBrokenThreads(lastHistory.LenoBrokenThreads ?? 0);

                        existingDailyOperationLoomDocument.AddDailyOperationLoomHistory(newLoomHistory);

                        lastBeamProduct.SetLatestDateTimeBeamProduct(finishDateTime);
                        lastBeamProduct.SetBeamProductStatus(BeamStatus.COMPLETED);

                        await Task.Yield();
                        var isAllBeamProductProcessed = 0;
                        foreach (var beamProduct in existingDailyOperationLoomDocument.LoomBeamProducts)
                        {
                            if (beamProduct.BeamProductStatus == BeamStatus.ONPROCESS) {
                                isAllBeamProductProcessed++;
                            }
                        };

                        await Task.Yield();
                        if (isAllBeamProductProcessed==0)
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
