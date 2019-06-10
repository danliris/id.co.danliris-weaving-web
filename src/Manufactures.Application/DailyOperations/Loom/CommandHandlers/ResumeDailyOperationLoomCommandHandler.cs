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
            var query =
                _dailyOperationalDocumentRepository
                    .Query
                    .Include(o => o.DailyOperationLoomDetails);
            var existingDailyOperation =
                _dailyOperationalDocumentRepository
                    .Find(query)
                    .Where(e => e.Identity.Equals(request.Id))
                    .FirstOrDefault();
            var detail =
                existingDailyOperation
                    .DailyOperationMachineDetails
                    .OrderByDescending(e => e.DateTimeOperation);

            if (detail.FirstOrDefault().OperationStatus != DailyOperationMachineStatus.ONSTOP)
            {
                throw Validator.ErrorValidation(("Status", "Can't continue, check your latest status"));
            }

            var existingOrder = 
                _weavingOrderDocumentRepository
                    .Find(e => e.Identity.Equals(existingDailyOperation.OrderId.Value))
                    .FirstOrDefault();
            var dateTimeOperation = 
                request.ResumeDate.ToUniversalTime().AddHours(7).Date + request.ResumeTime;
            var firstDetail =
               existingDailyOperation
                   .DailyOperationMachineDetails
                   .OrderByDescending(o => o.DateTimeOperation)
                   .FirstOrDefault();

            if (dateTimeOperation < firstDetail.DateTimeOperation)
            {
                throw Validator.ErrorValidation(("Status", "Date and Time cannot less than latest operation"));
            }

            var warpOrigin = existingOrder.WarpOrigin;
            var weftOrigin = existingOrder.WeftOrigin;

            if (warpOrigin != request.WarpOrigin)
            {
                warpOrigin = request.WarpOrigin;
            }

            if (weftOrigin != request.WeftOrigin)
            {
                weftOrigin = request.WeftOrigin;
            }

            var newOperation =
                new DailyOperationLoomDetail(Guid.NewGuid(),
                                             request.ShiftId,
                                             request.OperatorId,
                                             warpOrigin,
                                             weftOrigin,
                                             dateTimeOperation,
                                             DailyOperationMachineStatus.ONRESUME,
                                             true,
                                             false);

            existingDailyOperation.AddDailyOperationMachineDetail(newOperation);

            await _dailyOperationalDocumentRepository.Update(existingDailyOperation);

            _storage.Save();

            return existingDailyOperation;
        }
    }
}
