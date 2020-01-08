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
    public class CompletedDailyOperationWarpingCommandHandler
        : ICommandHandler<CompletedDailyOperationWarpingCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;

        public CompletedDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(CompletedDailyOperationWarpingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Warping
            var warpingQuery =
                _dailyOperationWarpingRepository
                    .Query
                    .Include(o => o.WarpingHistories)
                    .Include(o => o.WarpingBeamProducts)
                    .ThenInclude(o => o.WarpingBrokenThreadsCauses)
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
            var warpingFinishDateLogUtc = new DateTimeOffset(request.ProduceBeamsDate.Date, new TimeSpan(+7, 0, 0));

            if (warpingFinishDateLogUtc < lastWarpingDateLogUtc)
            {
                throw Validator.ErrorValidation(("ProduceBeamsDate", "Tanggal Tidak Boleh Melebihi Tanggal Sebelumnya"));
            }
            else
            {
                if (warpingDateTime <= lastWarpingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ProduceBeamsTime", "Waktu Tidak Boleh Melebihi Waktu Sebelumnya"));
                }
                else
                {
                    if (lastWarpingHistory.MachineStatus != MachineStatus.ONENTRY)
                    {
                        existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);
                        if (request.IsFinishFlag == true)
                        {
                            existingDailyOperationWarpingDocument.SetOperationStatus(OperationStatus.ONFINISH);
                        }
                        else
                        {
                            existingDailyOperationWarpingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                        }

                        //Assign Value to Warping History and Add to Warping Document
                        var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                          new ShiftId(request.ProduceBeamsShift.Value),
                                                                          new OperatorId(request.ProduceBeamsOperator.Value),
                                                                          warpingDateTime,
                                                                          MachineStatus.ONCOMPLETE);
                        newHistory.SetWarpingBeamId(new BeamId(lastWarpingHistory.WarpingBeamId));
                        newHistory.SetWarpingBeamLengthPerOperator(request.WarpingBeamLengthPerOperator);
                        existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                        var totalBeamLength = request.WarpingBeamLengthPerOperator + lastWarpingBeamProduct.WarpingTotalBeamLength;
                        lastWarpingBeamProduct.SetWarpingTotalBeamLength(totalBeamLength);

                        lastWarpingBeamProduct.SetTention(request.Tention);
                        lastWarpingBeamProduct.SetMachineSpeed(request.MachineSpeed);
                        lastWarpingBeamProduct.SetPressRoll(request.PressRoll);
                        lastWarpingBeamProduct.SetPressRollUom(request.PressRollUom);
                        lastWarpingBeamProduct.SetBeamStatus(BeamStatus.ROLLEDUP);

                        foreach (var brokenCause in request.BrokenCauses)
                        {
                            var newBrokenCause = new DailyOperationWarpingBrokenCause(Guid.NewGuid(),
                                                                                      new BrokenCauseId(brokenCause.WarpingBrokenCauseId),
                                                                                      brokenCause.TotalBroken);
                            lastWarpingBeamProduct.AddWarpingBrokenThreadsCause(newBrokenCause);
                        }

                        await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);
                        _storage.Save();

                        return existingDailyOperationWarpingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't Finish, latest status is not ONSTART or ONPROCESSBEAM"));
                    }
                }
            }
        }
    }
}
