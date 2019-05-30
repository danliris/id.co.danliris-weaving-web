using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Orders.Repositories;
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
            var existingDailyOperation =
                _dailyOperationalDocumentRepository.Find(e => e.Identity.Equals(request.Id))
                                                   .FirstOrDefault();
            var existingOrder = 
                _weavingOrderDocumentRepository.Find(e => e.Identity.Equals(existingDailyOperation.OrderId))
                                               .FirstOrDefault();
            var dateTimeOperation = request.ResumeDate.Date + request.ResumeTime;
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
