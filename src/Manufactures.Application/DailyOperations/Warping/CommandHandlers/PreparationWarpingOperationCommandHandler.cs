using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.CommandHandlers
{
    public  class PreparationWarpingOperationCommandHandler :
        ICommandHandler<PreparationWarpingOperationCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository 
            _warpingOperationRepository;

        public PreparationWarpingOperationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _warpingOperationRepository = 
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(PreparationWarpingOperationCommand request, 
                                                          CancellationToken cancellationToken)
        {
            var datetimeOperation =
                request.DateOperation.UtcDateTime.Add(new TimeSpan(+7)) + TimeSpan.Parse(request.TimeOperation);

            var operation = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                              request.ConstructionId,
                                                              request.MaterialTypeId,
                                                              request.AmountOfCones,
                                                              request.ColourOfCone,
                                                              datetimeOperation,
                                                              request.OperatorId);

            await _warpingOperationRepository.Update(operation);
            _storage.Save();
            return operation;
        }
    }
}
