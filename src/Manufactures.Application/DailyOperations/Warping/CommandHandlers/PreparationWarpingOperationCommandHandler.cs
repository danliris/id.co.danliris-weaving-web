using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Text;
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
            //Check if any sop/order id has define on daily operation
            var existingDailyOperation =
                _warpingOperationRepository
                    .Find(o => o.OrderId.Equals(request.OrderId.Value))
                    .Any();

            if (existingDailyOperation == true)
            {
                throw Validator.ErrorValidation(("OrderId", "Please input daily operation with different Order "));
            }

            //Set date time when user operate
            var year = request.DateOperation.Year;
            var month = request.DateOperation.Month;
            var day = request.DateOperation.Day;
            var hour = request.TimeOperation.Hours;
            var minutes = request.TimeOperation.Minutes;
            var seconds = request.TimeOperation.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Instantiate new Daily operation warping
            var dailyOperationWarping = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                              request.OrderId,
                                                              request.ConstructionId,
                                                              request.MaterialTypeId,
                                                              request.AmountOfCones,
                                                              request.ColourOfCone,
                                                              dateTimeOperation,
                                                              request.OperatorId);

            dailyOperationWarping.SetDailyOperationStatus(OperationStatus.ONPROCESS);

            //Add daily operation history
            var history = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                           request.ShiftId,
                                                           request.OperatorId.Value, 
                                                           dateTimeOperation,
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
