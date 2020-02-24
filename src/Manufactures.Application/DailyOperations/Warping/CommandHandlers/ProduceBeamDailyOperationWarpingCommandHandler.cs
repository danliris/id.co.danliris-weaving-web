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
        private readonly IDailyOperationWarpingHistoryRepository
            _dailyOperationWarpingHistoryRepository;
        private readonly IDailyOperationWarpingBeamProductRepository
            _dailyOperationWarpingBeamProductRepository;

        public ProduceBeamDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _dailyOperationWarpingHistoryRepository =
                _storage.GetRepository<IDailyOperationWarpingHistoryRepository>();
            _dailyOperationWarpingBeamProductRepository =
                _storage.GetRepository<IDailyOperationWarpingBeamProductRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(ProduceBeamsDailyOperationWarpingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Warping
            var existingDailyOperationWarpingDocument =
                _dailyOperationWarpingRepository
                    .Find(x => x.Identity == request.Id)
                    .FirstOrDefault();

            //Get Daily Operation History
            var existingDailyOperationWarpingHistories =
                _dailyOperationWarpingHistoryRepository
                    .Find(o=>o.DailyOperationWarpingDocumentId == existingDailyOperationWarpingDocument.Identity)
                    .OrderByDescending(detail => detail.DateTimeMachine);
            var lastWarpingHistory = existingDailyOperationWarpingHistories.FirstOrDefault();

            //Get Daily Operation Beam Product
            
            var lastWarpingBeamProduct = 
                _dailyOperationWarpingBeamProductRepository
                    .Find(x => x.DailyOperationWarpingDocumentId == existingDailyOperationWarpingDocument.Identity)
                    .OrderByDescending(x => x.LatestDateTimeBeamProduct)
                    .FirstOrDefault();

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
                throw Validator.ErrorValidation(("ProduceBeamsDate", "Tanggal Tidak Boleh Lebih Awal Dari Tanggal Sebelumnya"));
            }
            else
            {
                if (warpingDateTime <= lastWarpingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ProduceBeamsTime", "Waktu Tidak Boleh Lebih Awal Dari Waktu Sebelumnya"));
                }
                else
                {
                    if (lastWarpingHistory.MachineStatus == MachineStatus.ONSTART || lastWarpingHistory.MachineStatus == MachineStatus.ONPROCESSBEAM)
                    {
                        existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);
                        await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);

                        //Assign Value to Warping History and Add to Warping Document
                        var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                          new ShiftId(request.ProduceBeamsShift.Value),
                                                                          new OperatorId(request.ProduceBeamsOperator.Value),
                                                                          warpingDateTime,
                                                                          MachineStatus.ONPROCESSBEAM,
                                                                          existingDailyOperationWarpingDocument.Identity);
                        newHistory.SetWarpingBeamId(lastWarpingHistory.WarpingBeamId);
                        newHistory.SetWarpingBeamLengthPerOperator(request.WarpingBeamLengthPerOperator);
                        newHistory.SetWarpingBeamLengthPerOperatorUomId(lastWarpingHistory.WarpingBeamLengthPerOperatorUomId);
                        await _dailyOperationWarpingHistoryRepository.Update(newHistory);

                        //Assign Value to Warping Beam Product and Add to Warping Document
                        lastWarpingBeamProduct.SetLatestDateTimeBeamProduct(warpingDateTime);                        
                        var totalBeamLength = request.WarpingBeamLengthPerOperator + lastWarpingBeamProduct.WarpingTotalBeamLength;
                        lastWarpingBeamProduct.SetWarpingTotalBeamLength(totalBeamLength);
                        //lastWarpingBeamProduct.SetWarpingTotalBeamLengthUomId(lastWarpingBeamProduct.WarpingTotalBeamLengthUomId);
                        lastWarpingBeamProduct.SetBeamStatus(BeamStatus.ONPROCESS);
                        await _dailyOperationWarpingBeamProductRepository.Update(lastWarpingBeamProduct);                        

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
                        throw Validator.ErrorValidation(("MachineStatus", "Tidak Dapat Produksi Beam, Status Mesin Harus Mulai"));
                    }
                }
            }
        }
    }
}

