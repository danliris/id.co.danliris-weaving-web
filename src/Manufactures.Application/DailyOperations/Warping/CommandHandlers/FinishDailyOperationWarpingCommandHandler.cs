using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
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
    public class FinishDailyOperationWarpingCommandHandler
        : ICommandHandler<FinishDailyOperationWarpingCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IDailyOperationWarpingHistoryRepository
            _dailyOperationWarpingHistoryRepository;
        private readonly IDailyOperationWarpingBeamProductRepository
            _dailyOperationWarpingBeamProductRepository;
        private readonly IDailyOperationWarpingBrokenCauseRepository
            _dailyOperationWarpingBrokenCauseRepository;

        public FinishDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _dailyOperationWarpingHistoryRepository =
                _storage.GetRepository<IDailyOperationWarpingHistoryRepository>();
            _dailyOperationWarpingBeamProductRepository =
                _storage.GetRepository<IDailyOperationWarpingBeamProductRepository>();
            _dailyOperationWarpingBrokenCauseRepository =
                _storage.GetRepository<IDailyOperationWarpingBrokenCauseRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(FinishDailyOperationWarpingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Warping
            var existingWarpingDocument =
                _dailyOperationWarpingRepository
                    .Find(x => x.Identity == request.Id)
                    .FirstOrDefault();

            //Get Daily Operation History
            var lastWarpingHistory =
                _dailyOperationWarpingHistoryRepository
                    .Find(o=>o.DailyOperationWarpingDocumentId == existingWarpingDocument.Identity)
                    .OrderByDescending(detail => detail.DateTimeMachine)
                    .FirstOrDefault();

            //Get Daily Operation Beam Product
            
            var lastWarpingBeamProduct = 
                _dailyOperationWarpingBeamProductRepository
                    .Find(x => x.DailyOperationWarpingDocumentId == existingWarpingDocument.Identity)
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
            var warpingFinishDateLogUtc = new DateTimeOffset(request.ProduceBeamsDate.Date, new TimeSpan(+7, 0, 0));

            if (warpingFinishDateLogUtc < lastWarpingDateLogUtc)
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
                    if (lastWarpingHistory.MachineStatus != MachineStatus.ONENTRY)
                    {
                        existingWarpingDocument.SetDateTimeOperation(warpingDateTime);
                        if (request.IsFinishFlag == true)
                        {
                            existingWarpingDocument.SetOperationStatus(OperationStatus.ONFINISH);
                        }
                        else
                        {
                            existingWarpingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                        }
                        await _dailyOperationWarpingRepository.Update(existingWarpingDocument);

                        //Assign Value to Warping History and Add to Warping Document
                        var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                          request.ProduceBeamsShift,
                                                                          warpingDateTime,
                                                                          MachineStatus.ONCOMPLETE,
                                                                          existingWarpingDocument.Identity);
                        newHistory.SetWarpingBeamId(lastWarpingHistory.WarpingBeamId);
                        newHistory.SetWarpingBeamLengthPerOperator(request.WarpingBeamLengthPerOperator);
                        newHistory.SetOperatorDocumentId(request.ProduceBeamsOperator);
                        //newHistory.SetWarpingBeamLengthPerOperatorUomId(lastWarpingHistory.WarpingBeamLengthPerOperatorUomId);
                        await _dailyOperationWarpingHistoryRepository.Update(newHistory);

                        lastWarpingBeamProduct.SetLatestDateTimeBeamProduct(warpingDateTime);
                        lastWarpingBeamProduct.SetBeamStatus(BeamStatus.ROLLEDUP);

                        var totalBeamLength = request.WarpingBeamLengthPerOperator + lastWarpingBeamProduct.WarpingTotalBeamLength;
                        lastWarpingBeamProduct.SetWarpingTotalBeamLength(totalBeamLength);

                        lastWarpingBeamProduct.SetTention(request.Tention);
                        lastWarpingBeamProduct.SetMachineSpeed(request.MachineSpeed);
                        lastWarpingBeamProduct.SetPressRoll(request.PressRoll);
                        lastWarpingBeamProduct.SetPressRollUom(request.PressRollUom);
                        await _dailyOperationWarpingBeamProductRepository.Update(lastWarpingBeamProduct);

                        foreach (var brokenCause in request.BrokenCauses)
                        {
                            var newBrokenCause = new DailyOperationWarpingBrokenCause(Guid.NewGuid(),
                                                                                      new BrokenCauseId(brokenCause.WarpingBrokenCauseId),
                                                                                      brokenCause.TotalBroken,
                                                                                      lastWarpingBeamProduct.Identity);
                            await _dailyOperationWarpingBrokenCauseRepository.Update(newBrokenCause);
                        }

                        await _dailyOperationWarpingRepository.Update(existingWarpingDocument);
                        _storage.Save();

                        return existingWarpingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Tidak Dapat Menyelesaikan Proses, Status Mesin Harus ONSTART atau ON-PROCESS"));
                    }
                }
            }
        }
    }
}
