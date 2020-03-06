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
    public class UpdateStartDailyOperationWarpingCommandHandler : ICommandHandler<UpdateStartDailyOperationWarpingCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IDailyOperationWarpingHistoryRepository
            _dailyOperationWarpingHistoryRepository;
        private readonly IDailyOperationWarpingBeamProductRepository
            _dailyOperationWarpingBeamProductRepository;

        public UpdateStartDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _dailyOperationWarpingHistoryRepository =
                _storage.GetRepository<IDailyOperationWarpingHistoryRepository>();
            _dailyOperationWarpingBeamProductRepository =
                _storage.GetRepository<IDailyOperationWarpingBeamProductRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(UpdateStartDailyOperationWarpingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Warping
            var existingDailyOperationWarpingDocument =
                _dailyOperationWarpingRepository
                    .Find(x => x.Identity == request.Id)
                    .FirstOrDefault();

            //Get Daily Operation History
            var lastWarpingHistory = 
                _dailyOperationWarpingHistoryRepository
                    .Find(o=>o.DailyOperationWarpingDocumentId == existingDailyOperationWarpingDocument.Identity)
                    .OrderByDescending(detail => detail.DateTimeMachine)
                    .FirstOrDefault();
            //var lastWarpingHistory = existingDailyOperationWarpingHistories.FirstOrDefault();

            //Get Daily Operation Beam Product
            var lastWarpingBeamProduct = 
                _dailyOperationWarpingBeamProductRepository
                    .Find(o=>o.DailyOperationWarpingDocumentId == existingDailyOperationWarpingDocument.Identity)
                    .OrderByDescending(beamProduct => beamProduct.LatestDateTimeBeamProduct)
                    .FirstOrDefault();
            //var lastWarpingBeamProduct = existingDailyOperationWarpingBeamProduct.FirstOrDefault();

            //Validation for Operation Status
            var operationStatus = existingDailyOperationWarpingDocument.OperationStatus;
            if (operationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Tidak Dapat Memulai. Operasi Sudah Selesai"));
            }

            //Reformat DateTime
            var year = request.StartDate.Year;
            var month = request.StartDate.Month;
            var day = request.StartDate.Day;
            var hour = request.StartTime.Hours;
            var minutes = request.StartTime.Minutes;
            var seconds = request.StartTime.Seconds;
            var warpingDateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastWarpingDateLogUtc = new DateTimeOffset(lastWarpingHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var warpingStartDateLogUtc = new DateTimeOffset(request.StartDate.Date, new TimeSpan(+7, 0, 0));

            if (warpingStartDateLogUtc < lastWarpingDateLogUtc)
            {
                throw Validator.ErrorValidation(("StartDate", "Tanggal Tidak Boleh Lebih Awal Dari Tanggal Sebelumnya"));
            }
            else
            {
                if (warpingDateTime <= lastWarpingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("StartTime", "Waktu Tidak Boleh Lebih Awal Dari Waktu Sebelumnya"));
                }
                else
                {
                    if (lastWarpingHistory.MachineStatus == MachineStatus.ONENTRY)
                    {
                        existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);
                        await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);

                        //Assign Value to Warping History and Add to Warping Document
                        var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                          request.StartShift,
                                                                          warpingDateTime,
                                                                          MachineStatus.ONSTART,
                                                                          existingDailyOperationWarpingDocument.Identity);
                        newHistory.SetWarpingBeamId(request.WarpingBeamId);
                        newHistory.SetWarpingBeamLengthPerOperatorUomId(request.WarpingBeamLengthUomId);
                        newHistory.SetOperatorDocumentId(request.StartOperator);
                        await _dailyOperationWarpingHistoryRepository.Update(newHistory);

                        //Assign Value to Warping Beam Product and Add to Warping Document
                        var newBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                                  request.WarpingBeamId,
                                                                                  request.WarpingBeamLengthUomId,
                                                                                  request.WarpingBeamLengthUomUnit,
                                                                                  warpingDateTime,
                                                                                  BeamStatus.ONPROCESS,
                                                                                  existingDailyOperationWarpingDocument.Identity);
                        //newBeamProduct.SetWarpingTotalBeamLengthUomId(request.WarpingBeamLengthUomId);
                        await _dailyOperationWarpingBeamProductRepository.Update(newBeamProduct);

                        _storage.Save();

                        return existingDailyOperationWarpingDocument;
                    }
                    else if (lastWarpingHistory.MachineStatus == MachineStatus.ONCOMPLETE)
                    {
                        if (request.WarpingBeamId.Value != lastWarpingBeamProduct.WarpingBeamId.Value)
                        {
                            existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);
                            await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);

                            //Assign Value to Warping History and Add to Warping Document
                            var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                              request.StartShift,
                                                                              warpingDateTime,
                                                                              MachineStatus.ONSTART, 
                                                                              existingDailyOperationWarpingDocument.Identity);
                            newHistory.SetWarpingBeamId(request.WarpingBeamId);
                            newHistory.SetWarpingBeamLengthPerOperatorUomId(request.WarpingBeamLengthUomId);
                            newHistory.SetOperatorDocumentId(request.StartOperator);
                            await _dailyOperationWarpingHistoryRepository.Update(newHistory);

                            //Assign Value to Warping Beam Product and Add to Warping Document
                            var newBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                                      request.WarpingBeamId,
                                                                                      request.WarpingBeamLengthUomId,
                                                                                      request.WarpingBeamLengthUomUnit,
                                                                                      warpingDateTime,
                                                                                      BeamStatus.ONPROCESS,
                                                                                      existingDailyOperationWarpingDocument.Identity);
                            newBeamProduct.SetWarpingTotalBeamLength(0);
                            newBeamProduct.SetWarpingTotalBeamLengthUomId(request.WarpingBeamLengthUomId);
                            await _dailyOperationWarpingBeamProductRepository.Update(newBeamProduct);

                            _storage.Save();

                            return existingDailyOperationWarpingDocument;
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("BeamStatus", "Beam yang Dipilih Telah Selesai Diproses, Harus Input Beam yang Beda"));
                        }
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Tidak Dapat Memulai, Status Mesin Harus ONENTRY atau ONCOMPLETE"));
                    }
                }
            }
        }
    }
}
