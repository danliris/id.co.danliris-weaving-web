using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class StopDailyOperationLoomCommandHandler 
        : ICommandHandler<StopDailyOperationLoomCommand,
                          DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationalDocumentRepository;

        public StopDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(StopDailyOperationLoomCommand request, 
                                                       CancellationToken cancellationToken)
        {
            var existingDailyOperation =
                _dailyOperationalDocumentRepository.Find(e => e.Identity.Equals(request.Id))
                                                   .FirstOrDefault();
            var dateTimeOperation = request.StopDate.Date + request.StopTime;
            var newOperation =
                new DailyOperationLoomDetail(Guid.NewGuid(),
                                             request.ShiftId,
                                             request.OperatorId,
                                             string.Empty,
                                             string.Empty,
                                             dateTimeOperation,
                                             DailyOperationMachineStatus.ONSTOP,
                                             false,
                                             true);

            existingDailyOperation.AddDailyOperationMachineDetail(newOperation);

            await _dailyOperationalDocumentRepository.Update(existingDailyOperation);

            _storage.Save();

            return existingDailyOperation;
        }
    }
}
