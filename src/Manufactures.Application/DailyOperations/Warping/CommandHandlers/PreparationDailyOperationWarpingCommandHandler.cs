using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
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
    public  class PreparationDailyOperationWarpingCommandHandler :
        ICommandHandler<PreparationDailyOperationWarpingCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IDailyOperationWarpingHistoryRepository
            _dailyOperationWarpingHistoryRepository;

        public PreparationDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _dailyOperationWarpingHistoryRepository =
                _storage.GetRepository<IDailyOperationWarpingHistoryRepository>();
        }

        //Handle request from User request
        public async Task<DailyOperationWarpingDocument> Handle(PreparationDailyOperationWarpingCommand request, 
                                                                CancellationToken cancellationToken)
        {
            //Set date time when user operate
            var year = request.PreparationDate.Year;
            var month = request.PreparationDate.Month;
            var day = request.PreparationDate.Day;
            var hour = request.PreparationTime.Hours;
            var minutes = request.PreparationTime.Minutes;
            var seconds = request.PreparationTime.Seconds;
            var warpingDateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Instantiate new Daily Operation warping
            var newWarpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                       request.PreparationOrder,
                                                                       request.AmountOfCones,
                                                                       request.BeamProductResult,
                                                                       warpingDateTime,
                                                                       OperationStatus.ONPROCESS);

            //Update
            await _dailyOperationWarpingRepository.Update(newWarpingDocument);

            //Add Daily Operation History
            var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                              request.PreparationShift,
                                                              warpingDateTime,
                                                              MachineStatus.ONENTRY,
                                                              newWarpingDocument.Identity);

            //Update
            await _dailyOperationWarpingHistoryRepository.Update(newHistory);

            //Save
            _storage.Save();

            //return as object  daily operation
            return newWarpingDocument;
        }
    }
}
