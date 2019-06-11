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
            //Add query
            var query =
                _dailyOperationalDocumentRepository
                    .Query
                    .Include(o => o.DailyOperationLoomDetails);
            //Get existing daily operation
            var existingDailyOperation =
                _dailyOperationalDocumentRepository
                    .Find(query)
                    .Where(e => e.Identity.Equals(request.Id))
                    .FirstOrDefault();
            //Break datetime to match timezone
            var year = request.FinishDate.Year;
            var month = request.FinishDate.Month;
            var day = request.FinishDate.Day;
            var hour = request.FinishTime.Hours;
            var minutes = request.FinishTime.Minutes;
            var seconds = request.FinishTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));
            //Count has finish status
            var countFinishStatus =
                existingDailyOperation
                    .DailyOperationMachineDetails
                    .Where(e => e.OperationStatus == DailyOperationMachineStatus.ONCOMPLETE)
                    .Count();
            //Compare if has finish status
            if (countFinishStatus > 0)
            {
                throw Validator.ErrorValidation(("Status", "Finish status has available"));
            }
            //Get Detail [0]
            var firstDetail =
               existingDailyOperation
                   .DailyOperationMachineDetails
                   .OrderByDescending(o => o.DateTimeOperation)
                   .FirstOrDefault();
            //Compare datetime if possible
            if (dateTimeOperation < firstDetail.DateTimeOperation)
            {
                throw Validator.ErrorValidation(("Status", "Date and Time cannot less than latest operation"));
            }
            //Update daily operation status to finish
            existingDailyOperation.SetDailyOperationStatus(DailyOperationMachineStatus.ONFINISH);
            //Add new operation / detail
            var newOperation =
               new DailyOperationLoomDetail(Guid.NewGuid(),
                                            request.ShiftId,
                                            request.OperatorId,
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
