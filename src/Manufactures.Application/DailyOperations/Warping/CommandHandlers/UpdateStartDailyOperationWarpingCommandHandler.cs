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

        public UpdateStartDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(UpdateStartDailyOperationWarpingCommand request,
                                                          CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Warping
            var warpingQuery =
                _dailyOperationWarpingRepository
                    .Query
                    .Include(x => x.WarpingHistories)
                    .Include(x => x.WarpingBeamProducts);
            var existingDailyOperationWarpingDocument =
                _dailyOperationWarpingRepository
                    .Find(warpingQuery)
                    .Where(doc => doc.Identity.Equals(request.Id))
                    .FirstOrDefault();

            //Get Daily Operation Detail
            var existingDailyOperationWarpingHistories = existingDailyOperationWarpingDocument
                .WarpingHistories
                .OrderByDescending(detail => detail.DateTimeMachine);
            var lastWarpingHistory = existingDailyOperationWarpingHistories.FirstOrDefault();

            //Get Daily Operation Beam Product
            var existingDailyOperationWarpingBeamProduct = existingDailyOperationWarpingDocument
                .WarpingBeamProducts
                .OrderByDescending(beamProduct => beamProduct.DateTimeBeamProduct);
            var lastWarpingBeamProduct = existingDailyOperationWarpingBeamProduct.FirstOrDefault();

            //Validation for Beam Status
            var countBeamStatus =
                existingDailyOperationWarpingDocument
                    .WarpingBeamProducts
                    .Where(beamProduct => beamProduct.BeamStatus == BeamStatus.ONPROCESS)
                    .Count();

            if (!countBeamStatus.Equals(0))
            {
                throw Validator.ErrorValidation(("WarpingBeamProductStatus", "Can's Start. There's ONPROCESS Warping Beam on this Operation"));
            }

            //Validation for Operation Status
            var operationStatus = existingDailyOperationWarpingDocument.OperationStatus;
            if (operationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Start. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.WarpingStartDate.Year;
            var month = request.WarpingStartDate.Month;
            var day = request.WarpingStartDate.Day;
            var hour = request.WarpingStartTime.Hours;
            var minutes = request.WarpingStartTime.Minutes;
            var seconds = request.WarpingStartTime.Seconds;
            var warpingDateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastWarpingDateLogUtc = new DateTimeOffset(lastWarpingHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var warpingStartDateLogUtc = new DateTimeOffset(request.WarpingStartDate.Date, new TimeSpan(+7, 0, 0));

            if (warpingStartDateLogUtc < lastWarpingDateLogUtc)
            {
                throw Validator.ErrorValidation(("WarpingStartDate", "Start date cannot less than latest date log"));
            }
            else
            {
                if (warpingDateTime <= lastWarpingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("WarpingStartTime", "Start time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastWarpingHistory.MachineStatus == MachineStatus.ONENTRY)
                    {
                        existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);

                        //Assign Value to Warping History and Add to Warping Document
                        var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                          new ShiftId(request.ShiftId.Value),
                                                                          new OperatorId(request.OperatorId.Value),
                                                                          warpingDateTime,
                                                                          MachineStatus.ONSTART);
                        existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                        //Assign Value to Warping Beam Product and Add to Warping Document
                        var newBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                                  new BeamId(request.WarpingBeamId.Value));
                        existingDailyOperationWarpingDocument.AddDailyOperationWarpingBeamProduct(newBeamProduct);

                        await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);
                        _storage.Save();

                        return existingDailyOperationWarpingDocument;
                    }
                    else if (lastWarpingHistory.MachineStatus == MachineStatus.ONCOMPLETE)
                    {
                        if (lastWarpingBeamProduct.BeamStatus.Equals(BeamStatus.ROLLEDUP))
                        {
                            existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);

                            //Assign Value to Warping History and Add to Warping Document
                            var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                              new ShiftId(request.ShiftId.Value),
                                                                              new OperatorId(request.OperatorId.Value),
                                                                              warpingDateTime,
                                                                              MachineStatus.ONSTART);
                            existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                            //Assign Value to Warping Beam Product and Add to Warping Document
                            var newBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                                      new BeamId(request.WarpingBeamId.Value));
                            existingDailyOperationWarpingDocument.AddDailyOperationWarpingBeamProduct(newBeamProduct);

                            await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);
                            _storage.Save();

                            return existingDailyOperationWarpingDocument;
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("WarpingBeamStatus", "Can't start, latest beam status must ROLLED-UP"));
                        }
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
