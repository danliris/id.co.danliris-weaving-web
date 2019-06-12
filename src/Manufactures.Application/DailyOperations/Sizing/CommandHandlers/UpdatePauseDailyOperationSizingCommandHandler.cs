using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class UpdatePauseDailyOperationSizingCommandHandler : ICommandHandler<UpdatePauseDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public UpdatePauseDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdatePauseDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.Details).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var histories = existingDailyOperation.Details.OrderByDescending(e => e.DateTimeOperation);
            var lastHistory = histories.FirstOrDefault();

            //if (histories.FirstOrDefault().OperationStatus == DailyOperationMachineStatus.ONSTART ||
            //    histories.FirstOrDefault().OperationStatus == DailyOperationMachineStatus.ONRESUME ||
            //    histories.FirstOrDefault().OperationStatus == DailyOperationMachineStatus.ONCOMPLETE)
            //{
            //    throw Validator.ErrorValidation(("Status", "Can't stop, check your latest status"));
            //}

            if (histories.FirstOrDefault().OperationStatus == DailyOperationMachineStatus.ONSTART || histories.FirstOrDefault().OperationStatus == DailyOperationMachineStatus.ONRESUME)
            {
                var dateTimeOperation =
                request.Details.PauseDate.ToUniversalTime().AddHours(7).Date + request.Details.PauseTime;

                //var History = request.Details.History;
                var Causes = request.Details.Causes;

                var newOperation =
                            new DailyOperationSizingDetail(Guid.NewGuid(),
                                                           new ShiftId(request.Details.ShiftDocumentId.Value),
                                                           new OperatorId(lastHistory.OperatorDocumentId),
                                                           dateTimeOperation,
                                                           DailyOperationMachineStatus.ONSTOP,
                                                           request.Details.Information,
                                                           //new DailyOperationSizingHistoryValueObject(History.MachineDate, History.MachineTime, DailyOperationMachineStatus.ONSTOP, History.Information),
                                                           new DailyOperationSizingCausesValueObject(Causes.BrokenBeam, Causes.MachineTroubled));

                existingDailyOperation.AddDailyOperationSizingDetail(newOperation);

                await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                _storage.Save();

                return existingDailyOperation;
            }else
            {
                throw Validator.ErrorValidation(("Status", "Can't stop, latest status is not on PROCESS or on RESUME"));
            }
        }
    }
}
