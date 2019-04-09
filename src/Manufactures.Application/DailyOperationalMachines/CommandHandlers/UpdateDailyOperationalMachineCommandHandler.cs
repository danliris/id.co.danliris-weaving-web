using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.DailyOperations;
using Manufactures.Domain.DailyOperations.Commands;
using Manufactures.Domain.DailyOperations.Repositories;
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
            //_weavingOrderDocumentRepository = _storage.GetRepository<IWeavingOrderDocumentRepository>();
            //_constructionDocumentRepository = _storage.GetRepository<IConstructionDocumentRepository>();
            //_machineRepository = _storage.GetRepository<IMachineRepository>();
        }

        public async Task<DailyOperationalMachineDocument> Handle(UpdateDailyOperationalMachineCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationalDocumentRepository.Query.Include(d => d.DailyOperationMachineDetails);
            var existingOperation = _dailyOperationalDocumentRepository.Find(query).Where(entity => entity.Identity.Equals(request.Id)).FirstOrDefault();

            //if (existingOperation == null)
            //{
            //    Validator.ErrorValidation(("Daily Production Document", "Unavailable existing Daily Production Document with Id " + request.Id));
            //}

            foreach (var operation in existingOperation.DailyOperationMachineDetails)
            {
                var updateOperation = request.DailyOperationMachineDetails.Where(e => e.Id.Equals(operation.Identity)).FirstOrDefault();
                if(updateOperation != null)
                {
                    operation.SetShift(updateOperation.Shift);
                    operation.SetBeamNumber(updateOperation.BeamNumber);
                    operation.SetBeamOperator(updateOperation.BeamOperator);
                    operation.SetLoomGroup(updateOperation.LoomGroup);
                    operation.SetSizingNumber(updateOperation.SizingNumber);
                    operation.SetSizingOperator(updateOperation.SizingOperator);
                    operation.SetSizingGroup(updateOperation.SizingGroup);
                    operation.SetInformation(updateOperation.Information);
                }
            }
            
            await _dailyOperationalDocumentRepository.Update(existingOperation);
            _storage.Save();

            return existingOperation;
        }
    }
}
