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
    public class CompleteWarpingOperationCommandHandler
        : ICommandHandler<CompleteWarpingOperationCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _warpingOperationRepository;

        public CompleteWarpingOperationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _warpingOperationRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(CompleteWarpingOperationCommand request, CancellationToken cancellationToken)
        {
            //Check if has existing daily operation
            var warpingQuery =
                _warpingOperationRepository
                    .Query
                    .Include(x => x.WarpingDetails)
                    .Include(x => x.WarpingBeamProducts);
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

            //Check if any beam has status on process
            if (existingDailyOperation
                    .WarpingBeamProducts
                    .Any(entity => entity.BeamStatus.Equals(BeamStatus.ONPROCESS)))
            {
                //Throw an error doesn't have any operation
                throw Validator
                        .ErrorValidation(("Id",
                                          "Unavailable process, still have beam on processing " + request.Id));
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
            var history = new DailyOperationWarpingDetail(Guid.NewGuid(),
                                                           request.ShiftId,
                                                           request.OperatorId,
                                                           dateTimeOperation,
                                                           MachineStatus.ONCOMPLETE);

            existingDailyOperation.AddDailyOperationWarpingDetail(history);

            //Update status on daily operation
            existingDailyOperation.SetOperationStatus(OperationStatus.ONFINISH);

            //Update existing daily operation
            await _warpingOperationRepository.Update(existingDailyOperation);
            _storage.Save();

            //return existing operation
            return existingDailyOperation;
        }
    }
}
