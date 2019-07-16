using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
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
            var existingOperation = 
                _warpingOperationRepository
                    .Find(x => x.Identity.Equals(request.Id))
                    .FirstOrDefault();

            throw new NotImplementedException();
        }
    }
}
