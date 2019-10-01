using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

namespace Manufactures.Application.DailyOperations.Warping.CommandHandlers
{
    public class UpdateResumeDailyOperationWarpingCommandHandler
        : ICommandHandler<UpdateResumeDailyOperationWarpingCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;

        public UpdateResumeDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }
        public async Task<DailyOperationWarpingDocument> Handle(UpdateResumeDailyOperationWarpingCommand request, CancellationToken cancellationToken)
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
            var year = request.ResumeDate.Year;
            var month = request.ResumeDate.Month;
            var day = request.ResumeDate.Day;
            var hour = request.ResumeTime.Hours;
            var minutes = request.ResumeTime.Minutes;
            var seconds = request.ResumeTime.Seconds;
            var warpingDateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastWarpingDateLogUtc = new DateTimeOffset(lastWarpingHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var warpingResumeDateLogUtc = new DateTimeOffset(request.ResumeDate.Date, new TimeSpan(+7, 0, 0));

            if (warpingResumeDateLogUtc < lastWarpingDateLogUtc)
            {
                throw Validator.ErrorValidation(("ResumeDate", "Resume date cannot less than latest date log"));
            }
            else
            {
                if (warpingDateTime <= lastWarpingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ResumeTime", "Resume time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastWarpingHistory.MachineStatus == MachineStatus.ONSTOP)
                    {
                        existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);
                        
                        //Assign Value to Warping History and Add to Warping Document
                        var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                          new ShiftId(request.ResumeShift.Value),
                                                                          new OperatorId(request.ResumeOperator.Value),
                                                                          warpingDateTime,
                                                                          MachineStatus.ONRESUME,
                                                                          lastWarpingHistory.WarpingBeamNumber);
                        existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                        await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);
                        _storage.Save();

                        return existingDailyOperationWarpingDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't Resume. This current Operation status isn't ONSTOP"));
                    }
                }
            }
        }
    }
}
