using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.CommandHandlers
{
    public class StartWarpingOperationCommandHandler :
        ICommandHandler<StartWarpingOperationCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _warpingOperationRepository;

        public StartWarpingOperationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _warpingOperationRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }

        public Task<DailyOperationWarpingDocument> Handle(StartWarpingOperationCommand request, 
                                                          CancellationToken cancellationToken)
        {
            //Check if has existing daily operation
            var existingDailyOperation = 
                _warpingOperationRepository
                    .Find(x => x.Identity.Equals(request.Id))
                    .FirstOrDefault();

            //Set date time when user operate
            var datetimeOperation =
                request.DateOperation.UtcDateTime.Add(new TimeSpan(+7)) + TimeSpan.Parse(request.TimeOperation);

            //Add daily operation history
            var history = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                           request.OperatorId.Value,
                                                           datetimeOperation,
                                                           MachineStatus.ONSTART);

            existingDailyOperation.AddDailyOperationWarpingDetailHistory(history);

            throw new NotImplementedException();
        }
    }
}
