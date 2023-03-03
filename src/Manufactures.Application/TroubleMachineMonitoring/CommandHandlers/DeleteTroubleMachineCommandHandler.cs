using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.TroubleMachineMonitoring;
using Manufactures.Domain.TroubleMachineMonitoring.Commands;
using Manufactures.Domain.TroubleMachineMonitoring.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.TroubleMachineMonitoring.CommandHandlers
{
    public class DeleteTroubleMachineCommandHandler : ICommandHandler<RemoveExistingTroubleMachineMonitoringCommand, TroubleMachineMonitoringDocument>
    {
        private readonly IStorage _storage;
        private readonly ITroubleMachineMonitoringRepository _troubleMachineMonitoringRepository;

        public DeleteTroubleMachineCommandHandler(IStorage storage)
        {
            _storage = storage;
            _troubleMachineMonitoringRepository =
                _storage.GetRepository<ITroubleMachineMonitoringRepository>();
        }

        public async Task<TroubleMachineMonitoringDocument> Handle(RemoveExistingTroubleMachineMonitoringCommand request, CancellationToken cancellationToken)
        {
            var existingOperator =
                _troubleMachineMonitoringRepository.Find(o => o.Identity.Equals(request.Id))
                                   .FirstOrDefault();

            if (existingOperator == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Operator Id: " + request.Id));
            }

            existingOperator.Remove();

            await _troubleMachineMonitoringRepository.Update(existingOperator);

            _storage.Save();

            return existingOperator;
        }

    }
}
