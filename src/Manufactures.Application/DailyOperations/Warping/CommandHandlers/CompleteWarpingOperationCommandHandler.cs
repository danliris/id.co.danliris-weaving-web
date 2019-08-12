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

            //Check if any beam has status on process
            if (existingDailyOperation
                    .DailyOperationWarpingBeamProducts
                    .Any(entity => entity.BeamStatus.Equals(BeamStatus.ONPROCESS)))
            {
                //Throw an error doesn't have any operation
                throw Validator
                        .ErrorValidation(("Id",
                                          "Unavailable process, still have beam on processing " + request.Id));
            }

            //Set date time when user operate
            var datetimeOperation =
               request
                   .DateOperation
                   .UtcDateTime
                   .Add(new TimeSpan(+7)) + TimeSpan.Parse(request.TimeOperation);

            //Add daily operation history
            var history = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                           request.OperatorId.Value,
                                                           datetimeOperation,
                                                           MachineStatus.ONCOMPLETE);

            existingDailyOperation.AddDailyOperationWarpingDetailHistory(history);

            //Update status on daily operation
            existingDailyOperation.SetDailyOperationStatus(OperationStatus.ONFINISH);

            //Update existing daily operation
            await _warpingOperationRepository.Update(existingDailyOperation);
            _storage.Save();

            //return existing operation
            return existingDailyOperation;
        }
    }
}
