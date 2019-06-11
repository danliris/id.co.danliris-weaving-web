using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class StopDailyOperationLoomCommandHandler 
        : ICommandHandler<StopDailyOperationLoomCommand,
                          DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationalDocumentRepository;

        public StopDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(StopDailyOperationLoomCommand request, 
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
            //Get[0] detail from existing daily operation
            var detail = 
                existingDailyOperation
                    .DailyOperationMachineDetails
                    .OrderByDescending(e => e.DateTimeOperation)
                    .FirstOrDefault();
            //Compare if has status Start or Resume
            if (!detail.OperationStatus.Equals(DailyOperationMachineStatus.ONSTART) ||
                !detail.OperationStatus.Equals(DailyOperationMachineStatus.ONRESUME))
            {
                throw Validator.ErrorValidation(("Status", "Can't stop, check your latest status"));
            }
            //Break datetime to match timezone
            var year = request.StopDate.Year;
            var month = request.StopDate.Month;
            var day = request.StopDate.Day;
            var hour = request.StopTime.Hours;
            var minutes = request.StopTime.Minutes;
            var seconds = request.StopTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));
            //Compare to check datetime if possible
            if (dateTimeOperation < detail.DateTimeOperation)
            {
                throw Validator.ErrorValidation(("Status", "Date and Time cannot less than latest operation"));
            }
            //Add new operation / detail
            var newOperation =
                new DailyOperationLoomDetail(Guid.NewGuid(),
                                             request.ShiftId,
                                             request.OperatorId,
                                             dateTimeOperation,
                                             DailyOperationMachineStatus.ONSTOP,
                                             false,
                                             true);

            existingDailyOperation.AddDailyOperationMachineDetail(newOperation);

            await _dailyOperationalDocumentRepository.Update(existingDailyOperation);

            _storage.Save();

            return existingDailyOperation;
        }
    }
}
