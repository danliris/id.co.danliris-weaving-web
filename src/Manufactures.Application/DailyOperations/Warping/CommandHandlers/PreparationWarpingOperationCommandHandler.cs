using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.CommandHandlers
{
    /**
     * Command handle for daily operation warping preparation
     * **/
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

        //Handle request from User request
        public async Task<DailyOperationWarpingDocument> Handle(PreparationWarpingOperationCommand request, 
                                                          CancellationToken cancellationToken)
        {
            //Set date time when user operate
            var datetimeOperation =
                request.DateOperation.UtcDateTime.Add(new TimeSpan(+7)) + TimeSpan.Parse(request.TimeOperation);

            //Instantiate new Daily operation warping
            var dailyOperationWarping = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                              "",
                                                              request.ConstructionId,
                                                              request.MaterialTypeId,
                                                              request.AmountOfCones,
                                                              request.ColourOfCone,
                                                              datetimeOperation,
                                                              request.OperatorId);

            dailyOperationWarping.SetDailyOperationStatus(OperationStatus.ONPROCESS);

            //Add daily operation history
            var history = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                           request.OperatorId.Value, 
                                                           datetimeOperation,
                                                           MachineStatus.ONENTRY);
            dailyOperationWarping.AddDailyOperationWarpingDetailHistory(history);
            
            //Update and save
            await _warpingOperationRepository.Update(dailyOperationWarping);
            _storage.Save();

            //return as object  daily operation
            return dailyOperationWarping;
        }
    }
}
