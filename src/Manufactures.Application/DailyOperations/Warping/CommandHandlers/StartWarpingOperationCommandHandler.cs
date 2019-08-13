using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
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

        public async Task<DailyOperationWarpingDocument> Handle(StartWarpingOperationCommand request,
                                                          CancellationToken cancellationToken)
        {
            //Check if has existing daily operation
            var warpingQuery =
                _warpingOperationRepository
                    .Query
                    .Include(x => x.DailyOperationWarpingDetailHistory)
                    .Include(x => x.DailyOperationWarpingBeamProducts);
            var existingDailyOperation =
                _warpingOperationRepository
                    .Find(warpingQuery)
                    .Where(x => x.Identity.Equals(request.Id))
                    .FirstOrDefault();

            //Check if has existing daily operation
            if (existingDailyOperation == null)
            {
                //Throw an error doesn't have any operation
                throw Validator
                        .ErrorValidation(("Id",
                                          "Unavailable exsisting daily operation warping Document with Id " + request.Id));
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

            //Add daily operation history
            var history = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                           request.ShiftId,
                                                           request.OperatorId.Value,
                                                           dateTimeOperation,
                                                           MachineStatus.ONSTART);

            existingDailyOperation.AddDailyOperationWarpingDetailHistory(history);
            
            //Check if any beam on process
            if (existingDailyOperation
                    .DailyOperationWarpingBeamProducts
                    .Any(x => !x.BeamStatus.Equals(BeamStatus.ONPROCESS)))
            {
                //Add new beam product
                var warpingBeamProduct =
                new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                     request.BeamId);
                warpingBeamProduct.UpdateBeamStatus(BeamStatus.ONPROCESS);

                existingDailyOperation.AddDailyOperationWarpingBeamProduct(warpingBeamProduct);
            }
            else
            {
                //Throw an error has beam on process
                throw Validator
                        .ErrorValidation(("BeamId",
                                          "Still have another beam on process" + request.BeamId));
            }

            //Update existing daily operation
            await _warpingOperationRepository.Update(existingDailyOperation);
            _storage.Save();

            //return existing operation
            return existingDailyOperation;
        }
    }
}
