using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class FinishDailyOperationLoomCommandHandler 
        : ICommandHandler<FinishDailyOperationLoomCommand,
                          DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationalDocumentRepository;

        public FinishDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(FinishDailyOperationLoomCommand request, 
                                                             CancellationToken cancellationToken)
        {
            var query =
                _dailyOperationalDocumentRepository
                    .Query
                    .Include(o => o.DailyOperationLoomDetails);
            var existingDailyOperation =
                _dailyOperationalDocumentRepository
                    .Find(query)
                    .Where(e => e.Identity.Equals(request.Id))
                    .FirstOrDefault();
            var dateTimeOperation = 
                request.FinishDate.ToUniversalTime().AddHours(7).Date + request.FinishTime;
            var countFinishStatus =
                existingDailyOperation
                    .DailyOperationMachineDetails
                    .Where(e => e.OperationStatus == DailyOperationMachineStatus.ONFINISH)
                    .Count();

            if (countFinishStatus > 0)
            {
                throw Validator.ErrorValidation(("Status", "Start status has available"));
            }

            var firstDetail =
               existingDailyOperation
                   .DailyOperationMachineDetails
                   .OrderByDescending(o => o.DateTimeOperation)
                   .FirstOrDefault();

            if (dateTimeOperation < firstDetail.DateTimeOperation)
            {
                throw Validator.ErrorValidation(("Status", "Date and Time cannot less than latest operation"));
            }

            existingDailyOperation.SetDailyOperationStatus(DailyOperationMachineStatus.ONFINISH);
            var newOperation =
               new DailyOperationLoomDetail(Guid.NewGuid(),
                                            request.ShiftId,
                                            request.OperatorId,
                                            Constants.EMPTYvALUE,
                                            Constants.EMPTYvALUE, 
                                            dateTimeOperation, 
                                            DailyOperationMachineStatus.ONCOMPLETE, 
                                            false, 
                                            true);

            existingDailyOperation.AddDailyOperationMachineDetail(newOperation);

            await _dailyOperationalDocumentRepository.Update(existingDailyOperation);

            _storage.Save();

            return existingDailyOperation;
        }
    }
}
