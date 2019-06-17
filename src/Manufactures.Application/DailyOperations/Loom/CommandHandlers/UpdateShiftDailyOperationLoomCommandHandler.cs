using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class UpdateShiftDailyOperationLoomCommandHandler
        : ICommandHandler<UpdateShiftDailyOperationLoomCommand,
                          string>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationalDocumentRepository;

        public UpdateShiftDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
        }
        public async Task<string>
            Handle(UpdateShiftDailyOperationLoomCommand request,
                   CancellationToken cancellationToken)
        {
            //Add query
            var query =
                _dailyOperationalDocumentRepository
                    .Query
                    .Include(o => o.DailyOperationLoomDetails);
            //Get existing daily operation
            var existingDailyOperations =
                _dailyOperationalDocumentRepository
                    .Find(query)
                    .Where(o => o.DailyOperationStatus.Equals(DailyOperationMachineStatus.ONPROCESS));
            //Generate DateTimeOffset
            var dateTimeOperation = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(+7, 0, 0));
            var timeOperation = dateTimeOperation.TimeOfDay;

            //update all daily operation when status = process
            foreach (var dailyOperation in existingDailyOperations)
            {
                var checkTime = timeOperation;
                var defaultTime = checkTime;
                //Check latest operation
                var isUp = false;
                var isDown = false;
                var detail =
                    dailyOperation
                        .DailyOperationMachineDetails
                        .OrderByDescending(e => e.DateTimeOperation)
                        .FirstOrDefault();
                //get latest status of machine
                if (!detail.OperationStatus.Equals(DailyOperationMachineStatus.ONSTOP) ||
                    !detail.OperationStatus.Equals(DailyOperationMachineStatus.ONCOMPLETE))
                {
                    isUp = true;
                    isDown = false;
                }
                else
                {
                    isUp = false;
                    isDown = true;
                }

                //Add new operation / detail
                var newOperation =
                    new DailyOperationLoomDetail(Guid.NewGuid(),
                                                 request.ShiftId,
                                                 request.OperatorId,
                                                 dateTimeOperation,
                                                 DailyOperationMachineStatus.ONCHANGESHIFT,
                                                 isUp,
                                                 isDown);

                dailyOperation.AddDailyOperationMachineDetail(newOperation);
                await _dailyOperationalDocumentRepository.Update(dailyOperation);
            }

            _storage.Save();

            return "Updated";
        }
    }
}
