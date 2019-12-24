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

        public FinishDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(FinishDailyOperationWarpingCommand request, CancellationToken cancellationToken)
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

            //Validation for Beam Status
            //var countBeamStatus =
            //    existingDailyOperationWarpingDocument
            //        .WarpingBeamProducts
            //        .Where(e => e.BeamStatus == BeamStatus.ONPROCESS)
            //        .Count();

            //if (countBeamStatus != 0)
            //{
            //    throw Validator.ErrorValidation(("WarpingBeamProductStatus", "Can's Start. There's ONPROCESS Warping Beam on this Operation"));
            //}

            //Validation for Machine Status
            //var currentMachineStatus = lastWarpingHistory.MachineStatus;

            //if (currentMachineStatus != MachineStatus.ONCOMPLETE)
            //{
            //    throw Validator.ErrorValidation(("MachineStatus", "Can't Finish. This Machine's Operation is not ONCOMPLETE"));
            //}

            //Validation for Finished Operation Status
            //var currentOperationStatus = existingDailyOperationWarpingDocument.OperationStatus;

            //if (currentOperationStatus == OperationStatus.ONFINISH)
            //{
            //    throw Validator.ErrorValidation(("OperationStatus", "Can't Finish. This Operation's status already FINISHED"));
            //}

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
                throw Validator.ErrorValidation(("FinishDate", "Finish date cannot less than latest date log"));
            }
            else
            {
                if (warpingDateTime <= lastWarpingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("FinishTime", "Finish time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastWarpingHistory.MachineStatus != MachineStatus.ONENTRY)
                    {
                        existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);
                        existingDailyOperationWarpingDocument.SetOperationStatus(OperationStatus.ONFINISH);

                        //Assign Value to Warping History and Add to Warping Document
                        var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                          new ShiftId(request.ProduceBeamsShift.Value),
                                                                          new OperatorId(request.ProduceBeamsOperator.Value),
                                                                          warpingDateTime,
                                                                          MachineStatus.ONFINISH);
                        existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                        lastWarpingBeamProduct.SetTention(request.Tention);
                        lastWarpingBeamProduct.SetMachineSpeed(request.MachineSpeed);
                        lastWarpingBeamProduct.SetPressRoll(request.PressRoll);
                        lastWarpingBeamProduct.SetPressRollUom(request.PressRollUom);

                        foreach(var brokenCause in request.BrokenCauses)
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
