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
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class UpdateResumeDailyOperationSizingCommandHandler : ICommandHandler<UpdateResumeDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public UpdateResumeDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateResumeDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.DailyOperationSizingDetails).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var lastHistory = existingDailyOperation.DailyOperationSizingDetails.Last();

            var newOperation =
                        new DailyOperationSizingDetail(Guid.NewGuid(),
                                                       new ShiftId(lastHistory.ShiftDocumentId),
                                                       new OperatorId (request.Details.OperatorDocumentId.Value),
                                                       new DailyOperationSizingHistoryValueObject(request.Details.History.MachineDate, request.Details.History.MachineTime, DailyOperationMachineStatus.ONRESUME, request.Details.History.Information),
                                                       new DailyOperationSizingCausesValueObject(lastHistory.Causes.Deserialize<DailyOperationSizingCausesCommand>()));

                await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                _storage.Save();
            

            return existingDailyOperation;
        }
    }
}
