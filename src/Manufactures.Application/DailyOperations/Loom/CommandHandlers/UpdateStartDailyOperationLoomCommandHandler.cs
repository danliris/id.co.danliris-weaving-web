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
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomDocumentRepository;

        public UpdateStartDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationLoomDocumentRepository = _storage.GetRepository<IDailyOperationLoomRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(UpdateStartDailyOperationLoomCommand request, CancellationToken cancellationToken)
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
            var existingDailyOperationSizingHistories =
                existingDailyOperationLoomDocument
                        .LoomBeamHistories
                        .Where(o=>o.BeamNumber.Equals(request.StartBeamNumber))
                        .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingDailyOperationSizingHistories.FirstOrDefault();

            //Get Daily Operation Loom Beam Product
            var existingDailyOperationLoomBeamProducts =
                existingDailyOperationLoomDocument
                        .LoomBeamProducts
                        .Where(o => o.Identity.Equals(request.StartBeamProductId))
                        .OrderByDescending(o => o.LatestDateTimeBeamProduct);
            var lastBeamProduct = existingDailyOperationLoomBeamProducts.FirstOrDefault();

            //Validation for Prevent Beam Product with End Status Processed Again
            if (lastBeamProduct.BeamProductStatus.Equals(BeamStatus.END))
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
            var startDateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var startDateMachineLogUtc = new DateTimeOffset(request.StartDateMachine.Date, new TimeSpan(+7, 0, 0));

            if (startDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("StartDate", "Start date cannot less than latest date log"));
            }
            else
            {
                if (startDateTime <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("StartTime", "Start time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastHistory.MachineStatus == MachineStatus.ONENTRY || lastHistory.MachineStatus == MachineStatus.ONCOMPLETE)
                    {
                        var newLoomHistory =
                                new DailyOperationLoomBeamHistory(Guid.NewGuid(),
                                                                  request.StartBeamNumber,
                                                                  request.StartMachineNumber,
                                                                  new OperatorId(request.StartOperatorDocumentId.Value),
                                                                  startDateTime,
                                                                  new ShiftId(request.StartShiftDocumentId.Value),
                                                                  MachineStatus.ONSTART);

                        newLoomHistory.SetWarpBrokenThreads(lastHistory.WarpBrokenThreads ?? 0);
                        newLoomHistory.SetWeftBrokenThreads(lastHistory.WeftBrokenThreads ?? 0);
                        newLoomHistory.SetLenoBrokenThreads(lastHistory.LenoBrokenThreads ?? 0);

                        existingDailyOperationLoomDocument.AddDailyOperationLoomHistory(newLoomHistory);

                        lastBeamProduct.SetLatestDateTimeBeamProduct(startDateTime);

                        await _dailyOperationLoomDocumentRepository.Update(existingDailyOperationLoomDocument);
                        _storage.Save();

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
