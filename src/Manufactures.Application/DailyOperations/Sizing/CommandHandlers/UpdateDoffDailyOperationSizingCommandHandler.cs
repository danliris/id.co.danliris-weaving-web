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
    public class UpdateDoffDailyOperationSizingCommandHandler : ICommandHandler<UpdateDoffFinishDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public UpdateDoffDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateDoffFinishDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.DailyOperationSizingDetails).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();

            foreach (var operation in existingDailyOperation.DailyOperationSizingDetails)
            {
                var newOperation =
                        new DailyOperationSizingDetail(Guid.NewGuid(),
                                                       request.DailyOperationSizingDetails.BeamDocumentId,
                                                       new ConstructionId(operation.ConstructionDocumentId.Value),
                                                       request.DailyOperationSizingDetails.PIS,
                                                       request.DailyOperationSizingDetails.Visco,
                                                       operation.ProductionTime.Deserialize<DailyOperationSizingProductionTimeValueObject>(),
                                                       operation.BeamTime.Deserialize<DailyOperationSizingBeamTimeValueObject>(),
                                                       operation.BrokenBeam,
                                                       operation.TroubledMachine,
                                                       request.DailyOperationSizingDetails.Counter,
                                                       request.DailyOperationSizingDetails.ShiftDocumentId,
                                                       operation.Information);

                await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                _storage.Save();

            }

            return existingDailyOperation;
        }
    }
}
