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
    public class StartDailyOperationLoomCommandHandler
        : ICommandHandler<StartDailyOperationLoomCommand,
                          DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationalDocumentRepository;

        public StartDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(StartDailyOperationLoomCommand request,
                                                             CancellationToken cancellationToken)
        {
            //Define query
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
            var year = request.StartDate.Year;
            var month = request.StartDate.Month;
            var day = request.StartDate.Day;
            var hour = request.StartTime.Hours;
            var minutes = request.StartTime.Minutes;
            var seconds = request.StartTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));
            //Check if any available start status
            var countStartStatus = 
                existingDailyOperation
                    .DailyOperationMachineDetails
                    .Where( e => e.OperationStatus == DailyOperationMachineStatus.ONSTART)
                    .Count();
            //Get latest detail
            var firstDetail = 
                existingDailyOperation
                    .DailyOperationMachineDetails
                    .OrderByDescending(o => o.DateTimeOperation)
                    .FirstOrDefault();
            //Comparing to check if time not possible
            if (dateTimeOperation < firstDetail.DateTimeOperation )
            {
                throw Validator.ErrorValidation(("Status", "Date and Time cannot less than latest operation"));
            }
            //Comparing to check start status
            if (countStartStatus > 0)
            {
                throw Validator.ErrorValidation(("Status", "Start status has available"));
            }
            //Create new operation / detail
            var newOperation =
                new DailyOperationLoomDetail(Guid.NewGuid(),
                                             request.ShiftId,
                                             request.OperatorId,
                                             dateTimeOperation,
                                             DailyOperationMachineStatus.ONSTART,
                                             true,
                                             false);

            existingDailyOperation.AddDailyOperationMachineDetail(newOperation);

            await _dailyOperationalDocumentRepository.Update(existingDailyOperation);

            _storage.Save();

            return existingDailyOperation;
        }
    }
}
