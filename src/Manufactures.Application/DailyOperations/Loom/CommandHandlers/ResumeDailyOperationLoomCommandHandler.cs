using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class ResumeDailyOperationLoomCommandHandler 
        : ICommandHandler<ResumeDailyOperationLoomCommand,
                          DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationalDocumentRepository;
        private readonly IWeavingOrderDocumentRepository 
            _weavingOrderDocumentRepository;

        public ResumeDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _weavingOrderDocumentRepository = 
                _storage.GetRepository<IWeavingOrderDocumentRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(ResumeDailyOperationLoomCommand request, 
                                                       CancellationToken cancellationToken)
        {
            //Add query
            var query =
                _dailyOperationalDocumentRepository
                    .Query
                    .Include(o => o.DailyOperationLoomDetails);
            //Get existing daily operation
            var existingDailyOperation =
                _dailyOperationalDocumentRepository
                    .Find(query)
                    .Where(e => e.Identity.Equals(request.Id))
                    .FirstOrDefault();
            //Get[0] detail
            var detail =
                existingDailyOperation
                    .DailyOperationMachineDetails
                    .OrderByDescending(e => e.DateTimeOperation)
                    .FirstOrDefault();
            //Compare if has status stop
            if (!detail.OperationStatus.Equals(DailyOperationMachineStatus.ONSTOP))
            {
                throw Validator.ErrorValidation(("Status", "Can't continue, check your latest status"));
            }
            //Get existing order from Weaving Order Document
            var existingOrder = 
                _weavingOrderDocumentRepository
                    .Find(e => e.Identity.Equals(existingDailyOperation.OrderId.Value))
                    .FirstOrDefault();
            //Break datetime to match timezone
            var year = request.ResumeDate.Year;
            var month = request.ResumeDate.Month;
            var day = request.ResumeDate.Day;
            var hour = request.ResumeTime.Hours;
            var minutes = request.ResumeTime.Minutes;
            var seconds = request.ResumeTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));
            //Compare datetime if possible
            if (dateTimeOperation < detail.DateTimeOperation)
            {
                throw Validator.ErrorValidation(("Status", "Date and Time cannot less than latest operation"));
            }
            //Add origin if has change
            var warpOrigin = existingOrder.WarpOrigin;
            var weftOrigin = existingOrder.WeftOrigin;
            //Add new operation / detail
            var newOperation =
                new DailyOperationLoomDetail(Guid.NewGuid(),
                                             request.ShiftId,
                                             request.OperatorId,
                                             dateTimeOperation,
                                             DailyOperationMachineStatus.ONRESUME,
                                             true,
                                             false);
            //Compare if has change warp origin
            if (warpOrigin != request.WarpOrigin)
            {
                newOperation.AddWarpOrigin(request.WarpOrigin);
            }
            //Compare if has change weft origin
            if (weftOrigin != request.WeftOrigin)
            {
                newOperation.AddWeftOrigin(request.WeftOrigin);
            }

            existingDailyOperation.AddDailyOperationMachineDetail(newOperation);

            await _dailyOperationalDocumentRepository.Update(existingDailyOperation);

            _storage.Save();

            return existingDailyOperation;
        }
    }
}
