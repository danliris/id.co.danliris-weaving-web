using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
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

            //foreach (var operation in existingDailyOperation.DailyOperationSizingDetails)
            //{
                var newOperation =
                        new DailyOperationSizingDetail(Guid.NewGuid(),
                                                       new ConstructionId(operation.ConstructionDocumentId),
                                                       operation.ShiftId,
                                                       operation.OperatorDocumentId,
                                                       new DailyOperationSizingProductionTimeValueObject(operation.ProductionTime, request.UpdatePauseDailyOperationSizingDetails.ProductionTime.Pause, DateTimeOffset.MinValue, DateTimeOffset.MinValue)
                                                       operation.PIS,
                                                       operation.Visco,
                                                       operation.ProductionTime.Deserialize<DailyOperationSizingProductionTimeValueObject>(),
                                                       operation.BeamTime.Deserialize<DailyOperationSizingBeamTimeValueObject>(),
                                                       request.DailyOperationSizingDetails.BrokenBeam,
                                                       request.DailyOperationSizingDetails.TroubledMachine,
                                                       operation.Counter,
                                                       new ShiftId(operation.ShiftDocumentId.Value),
                                                       request.DailyOperationSizingDetails.Information);

                await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                _storage.Save();

            //}

            return existingDailyOperation;
        }
    }
}
