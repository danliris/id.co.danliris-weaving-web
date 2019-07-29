using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;

namespace Manufactures.Application.DailyOperations.Warping.CommandHandlers
{
    public class ResumeWarpingOperationCommandHandler
        : ICommandHandler<ResumeWarpingOperationCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _warpingOperationRepository;

        public ResumeWarpingOperationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _warpingOperationRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }
        public async Task<DailyOperationWarpingDocument> Handle(ResumeWarpingOperationCommand request, CancellationToken cancellationToken)
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
            var datetimeOperation =
                request
                    .DateOperation
                    .UtcDateTime
                    .Add(new TimeSpan(+7)) + TimeSpan.Parse(request.TimeOperation);

            //Add daily operation history
            var history = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                           request.OperatorId.Value,
                                                           datetimeOperation,
                                                           MachineStatus.ONRESUME);
            existingDailyOperation.AddDailyOperationWarpingDetailHistory(history);

            //Update existing daily operation
            await _warpingOperationRepository.Update(existingDailyOperation);
            _storage.Save();

            //return existing operation
            return existingDailyOperation;
        }
    }
}
