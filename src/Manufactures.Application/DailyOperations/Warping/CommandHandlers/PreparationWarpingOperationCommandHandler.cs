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
            var existingDailyOperation = _warpingOperationRepository
                                         .Find(o => o.OrderDocumentId.Equals(request.OrderDocumentId.Value))
                                         .Any();

            if (existingDailyOperation == true)
            {
                throw Validator.ErrorValidation(("OrderId", "Please input daily operation with different Order "));
            }

            //Set date time when user operate
            var year = request.PreparationDate.Year;
            var month = request.PreparationDate.Month;
            var day = request.PreparationDate.Day;
            var hour = request.PreparationTime.Hours;
            var minutes = request.PreparationTime.Minutes;
            var seconds = request.PreparationTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Instantiate new Daily operation warping
            var dailyOperationWarping = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                          request.OrderDocumentId,
                                                                          request.MaterialTypeId,
                                                                          request.AmountOfCones,
                                                                          request.ColourOfCone,
                                                                          dateTimeOperation,
                                                                          OperationStatus.ONPROCESS);

            //Add daily operation history
            var history = new DailyOperationWarpingDetail(Guid.NewGuid(),
                                                          request.ShiftDocumentId,
                                                          request.OperatorDocumentId, 
                                                          dateTimeOperation,
                                                          MachineStatus.ONENTRY);
            dailyOperationWarping.AddDailyOperationWarpingDetail(history);
            
            //Update and save
            await _warpingOperationRepository.Update(dailyOperationWarping);
            _storage.Save();

            //return as object  daily operation
            return dailyOperationWarping;
        }
    }
}
