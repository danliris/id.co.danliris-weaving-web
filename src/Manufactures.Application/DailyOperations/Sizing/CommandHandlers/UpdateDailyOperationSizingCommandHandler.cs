using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class UpdateDailyOperationSizingCommandHandler : ICommandHandler<UpdateDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public UpdateDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.DailyOperationSizingDetails).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();

            foreach (var operationDetail in request.DailyOperationSizingDetails)
            {
                if (operationDetail.Identity == null)
                {
                    var newOperation =
                        new DailyOperationSizingDetail(Guid.NewGuid(),
                                                      operationDetail.BeamDocumentId,
                                                      operationDetail.ConstructionDocumentId,
                                                      operationDetail.PIS,
                                                      operationDetail.Visco,
                                                      new DailyOperationSizingProductionTimeValueObject(operationDetail.ProductionTime),
                                                      new DailyOperationSizingBeamTimeValueObject(operationDetail.BeamTime),
                                                      operationDetail.BrokenBeam,
                                                      operationDetail.TroubledMachine,
                                                      operationDetail.Counter,
                                                      operationDetail.ShiftDocumentId,
                                                      operationDetail.Information);

                    existingDailyOperation.AddDailyOperationSizingDetail(newOperation);
                }
            }

            await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
            _storage.Save();

            return existingDailyOperation;
        }
    }
}
