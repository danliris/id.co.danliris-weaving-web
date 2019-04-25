using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.DailyOperations.Loom.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class UpdateDailyOperationLoomCommandHandler 
        : ICommandHandler<UpdateDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository 
            _dailyOperationalDocumentRepository;

        public UpdateDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository = _storage.GetRepository<IDailyOperationLoomRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(UpdateDailyOperationLoomCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationalDocumentRepository.Query.Include(d => d.DailyOperationMachineDetails).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationalDocumentRepository.Find(query).FirstOrDefault();

            foreach(var operationDetail in request.DailyOperationMachineDetails)
            {
                if(operationDetail.Identity == null)
                {
                    var newOperation =
                        new DailyOperationLoomDetail(Guid.NewGuid(),
                                                          operationDetail.OrderDocumentId,
                                                          operationDetail.WarpsOrigin,
                                                          operationDetail.WeftsOrigin,
                                                          operationDetail.BeamId,
                                                          new DailyOperationLoomTimeValueObject(operationDetail.DOMTime),
                                                                                 operationDetail.ShiftId,
                                                                                 operationDetail.BeamOperatorId,
                                                                                 operationDetail.SizingOperatorId,
                                                                                 operationDetail.Information,
                                                                                 operationDetail.DetailStatus);

                    existingDailyOperation.AddDailyOperationMachineDetail(newOperation);
                }
            }
            
            await _dailyOperationalDocumentRepository.Update(existingDailyOperation);
            _storage.Save();

            return existingDailyOperation;
        }
    }
}
