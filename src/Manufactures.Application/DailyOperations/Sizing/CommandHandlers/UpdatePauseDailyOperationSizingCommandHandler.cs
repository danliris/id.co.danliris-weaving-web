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
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.DailyOperationSizingDetails).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var lastHistory = existingDailyOperation.DailyOperationSizingDetails.Last();

                var newOperation =
                        new DailyOperationSizingDetail(Guid.NewGuid(), 
                                                       new ShiftId(lastHistory.ShiftId), 
                                                       new OperatorId(lastHistory.OperatorDocumentId),
                                                       new DailyOperationSizingHistoryValueObject(request.UpdatePauseDailyOperationSizingDetails.History.TimeOnMachine, DailyOperationMachineStatus.ONSTOP, request.UpdatePauseDailyOperationSizingDetails.History.Information),
                                                       new DailyOperationSizingCausesValueObject(request.UpdatePauseDailyOperationSizingDetails.Causes.BrokenBeam,request.UpdatePauseDailyOperationSizingDetails.Causes.MachineTroubled));

                await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                _storage.Save();
            

            return existingDailyOperation;
        }
    }
}
