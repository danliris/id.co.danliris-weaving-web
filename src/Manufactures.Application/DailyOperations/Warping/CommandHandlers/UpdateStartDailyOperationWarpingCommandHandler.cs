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

        public async Task<DailyOperationWarpingDocument> Handle(UpdateStartDailyOperationWarpingCommand request, CancellationToken cancellationToken)
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

            //Get Daily Operation History
            var existingDailyOperationWarpingHistories = existingDailyOperationWarpingDocument
                .WarpingHistories
                .OrderByDescending(detail => detail.DateTimeMachine);
            var lastWarpingHistory = existingDailyOperationWarpingHistories.FirstOrDefault();

            //Get Daily Operation Beam Product
            var existingDailyOperationWarpingBeamProduct = existingDailyOperationWarpingDocument
                .WarpingBeamProducts
                .OrderByDescending(beamProduct => beamProduct.LatestDateTimeBeamProduct);
            var lastWarpingBeamProduct = existingDailyOperationWarpingBeamProduct.FirstOrDefault();

            //Validation for Operation Status
            var operationStatus = existingDailyOperationWarpingDocument.OperationStatus;
            if (operationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Start. This operation's status already FINISHED"));
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
                throw Validator.ErrorValidation(("StartDate", "Start date cannot less than latest date log"));
            }
            else
            {
                if (warpingDateTime <= lastWarpingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("StartTime", "Start time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastWarpingHistory.MachineStatus == MachineStatus.ONENTRY)
                    {
                        existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);

                        //Assign Value to Warping History and Add to Warping Document
                        var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                          new ShiftId(request.StartShift.Value),
                                                                          new OperatorId(request.StartOperator.Value),
                                                                          warpingDateTime,
                                                                          MachineStatus.ONSTART);
                        newHistory.SetWarpingBeamId(request.WarpingBeamId);
                        existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                        //Assign Value to Warping Beam Product and Add to Warping Document
                        var newBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                                  new BeamId(request.WarpingBeamId.Value),
                                                                                  warpingDateTime,
                                                                                  BeamStatus.ONPROCESS);
                        existingDailyOperationWarpingDocument.AddDailyOperationWarpingBeamProduct(newBeamProduct);

                        await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);
                        _storage.Save();

                        return existingDailyOperationWarpingDocument;
                    }
                    else if (lastWarpingHistory.MachineStatus == MachineStatus.ONCOMPLETE)
                    {
                        if (request.WarpingBeamId.Value == lastWarpingBeamProduct.WarpingBeamId)
                        {
                            existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);

                            //Assign Value to Warping History and Add to Warping Document
                            var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                              new ShiftId(request.StartShift.Value),
                                                                              new OperatorId(request.StartOperator.Value),
                                                                              warpingDateTime,
                                                                              MachineStatus.ONSTART);
                            newHistory.SetWarpingBeamId(request.WarpingBeamId);
                            existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                            //Assign Value to Warping Beam Product and Add to Warping Document
                            var newBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                                      new BeamId(request.WarpingBeamId.Value),
                                                                                      warpingDateTime,
                                                                                      BeamStatus.ONPROCESS);
                            existingDailyOperationWarpingDocument.AddDailyOperationWarpingBeamProduct(newBeamProduct);

                            await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);
                            _storage.Save();

                            return existingDailyOperationWarpingDocument;
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("BeamStatus", "Beam Sebelumnya masih diproses, harus Input Beam yang sama"));
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
