using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.BeamStockMonitoring.Commands;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.CommandHandlers
{
    public class ProduceBeamDailyOperationWarpingCommandHandler
        : ICommandHandler<ProduceBeamsDailyOperationWarpingCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;

        public ProduceBeamDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(ProduceBeamsDailyOperationWarpingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Warping
            var warpingQuery =
                _dailyOperationWarpingRepository
                    .Query
                    .Include(x => x.WarpingHistories)
                    .Include(x => x.WarpingBeamProducts)
                    .Where(doc => doc.Identity.Equals(request.Id));
            var existingDailyOperationWarpingDocument =
                _dailyOperationWarpingRepository
                    .Find(warpingQuery)
                    .FirstOrDefault();

            //Get Daily Operation Detail
            var existingDailyOperationWarpingHistories = existingDailyOperationWarpingDocument
                    .WarpingHistories
                    .OrderByDescending(detail => detail.DateTimeMachine);
            var lastWarpingHistory = existingDailyOperationWarpingHistories.FirstOrDefault();

            //Get Daily Operation Beam Product
            var existingDailyOperationWarpingBeamProduct = existingDailyOperationWarpingDocument
                    .WarpingBeamProducts
                    .OrderByDescending(beamProduct => beamProduct.LatestDateTimeBeamProduct);
            var lastWarpingBeamProduct = existingDailyOperationWarpingBeamProduct.FirstOrDefault();

            //Reformat DateTime
            var year = request.ProduceBeamsDate.Year;
            var month = request.ProduceBeamsDate.Month;
            var day = request.ProduceBeamsDate.Day;
            var hour = request.ProduceBeamsTime.Hours;
            var minutes = request.ProduceBeamsTime.Minutes;
            var seconds = request.ProduceBeamsTime.Seconds;
            var warpingDateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastWarpingDateLogUtc = new DateTimeOffset(lastWarpingHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var warpingProduceBeamsDateLogUtc = new DateTimeOffset(request.ProduceBeamsDate.Date, new TimeSpan(+7, 0, 0));

            if (warpingProduceBeamsDateLogUtc < lastWarpingDateLogUtc)
            {
                throw Validator.ErrorValidation(("ProduceBeamsDate", "Produce Beams date cannot less than latest date log"));
            }
            else
            {
                if (warpingDateTime <= lastWarpingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ProduceBeamsTime", "Produce Beams time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastWarpingHistory.MachineStatus == MachineStatus.ONSTART || lastWarpingHistory.MachineStatus == MachineStatus.ONPROCESSBEAM)
                    {
                        existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);

                        //Assign Value to Warping History and Add to Warping Document
                        var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                          new ShiftId(request.ProduceBeamsShift.Value),
                                                                          new OperatorId(request.ProduceBeamsOperator.Value),
                                                                          warpingDateTime,
                                                                          MachineStatus.ONPROCESSBEAM);
                        newHistory.SetWarpingBeamId(new BeamId(lastWarpingHistory.WarpingBeamId));
                        newHistory.SetWarpingBeamLengthPerOperator(request.WarpingBeamLengthPerOperator);
                        existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                        //Assign Value to Warping Beam Product and Add to Warping Document
                        lastWarpingBeamProduct.SetLatestDateTimeBeamProduct(warpingDateTime);

                        var totalBeamLength = request.WarpingBeamLengthPerOperator + lastWarpingBeamProduct.WarpingTotalBeamLength;
                        lastWarpingBeamProduct.SetWarpingTotalBeamLength(totalBeamLength);

                        lastWarpingBeamProduct.SetWarpingBeamLengthUomId(request.WarpingBeamLengthUomId);
                        lastWarpingBeamProduct.SetBeamStatus(BeamStatus.ONPROCESS);

                        await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);
                        _storage.Save();

                        //var sizingStock = new SizingBeamStockMonitoringCommand
                        //{
                        //    BeamDocumentId = new BeamId(existingDailyOperationWarpingDocument.Identity),
                        //    SizingEntryDate = request.ProduceBeamsDate,
                        //    OrderDocumentId = existingDailyOperationWarpingDocument.OrderDocumentId,
                        //    SizingLengthStock = request.WarpingBeamLength
                        //};

                        return existingDailyOperationWarpingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't Produce Beam, latest status is not ONSTART or ONRESUME"));
                    }
                }
            }
        }
    }
}

