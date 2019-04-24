using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.DailyOperations;
using Manufactures.Domain.DailyOperations.Commands;
using Manufactures.Domain.DailyOperations.Entities;
using Manufactures.Domain.DailyOperations.Repositories;
using Manufactures.Domain.DailyOperations.ValueObjects;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperationalMachines.CommandHandlers
{
    public class UpdateDailyOperationalMachineCommandHandler : ICommandHandler<UpdateDailyOperationalMachineCommand, DailyOperationalMachineDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationalMachineRepository _dailyOperationalDocumentRepository;
        private readonly IWeavingOrderDocumentRepository _weavingOrderDocumentRepository;
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;
        private readonly IMachineRepository _machineRepository;

        public UpdateDailyOperationalMachineCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository = _storage.GetRepository<IDailyOperationalMachineRepository>();
        }

        public async Task<DailyOperationalMachineDocument> Handle(UpdateDailyOperationalMachineCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationalDocumentRepository.Query.Include(d => d.DailyOperationMachineDetails);
            var existingDailyOperation = _dailyOperationalDocumentRepository.Find(query).Where(entity => entity.Identity.Equals(request.Id)).FirstOrDefault();

            foreach(var operationDetail in request.DailyOperationMachineDetails)
            {
                if(operationDetail.Identity == null)
                {
                    var newOperation =
                        new DailyOperationalMachineDetail(Guid.NewGuid(),
                                                          operationDetail.OrderDocumentId,
                                                          operationDetail.WarpsOrigin,
                                                          operationDetail.WeftsOrigin,
                                                          operationDetail.BeamId,
                                                          new DOMTimeValueObject(operationDetail.DOMTime),
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
