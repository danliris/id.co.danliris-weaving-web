using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.TroubleMachineMonitoring;
using Manufactures.Domain.TroubleMachineMonitoring.Commands;
using Manufactures.Domain.TroubleMachineMonitoring.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.TroubleMachineMonitoring.CommandHandlers
{
    public class UpdateTroubleMachineMonitoringCommandHandler
        : ICommandHandler<UpdateExistingTroubleMachineMonitoringCommand, TroubleMachineMonitoringDocument>
    {
        private readonly IStorage _storage;
        private readonly ITroubleMachineMonitoringRepository _troubleMachineMonitoringRepository;

        public UpdateTroubleMachineMonitoringCommandHandler(IStorage storage)
        {
            _storage = storage;
            _troubleMachineMonitoringRepository =
                _storage.GetRepository<ITroubleMachineMonitoringRepository>();
        }

        public async Task<TroubleMachineMonitoringDocument> Handle(UpdateExistingTroubleMachineMonitoringCommand request,
                                                   CancellationToken cancellationToken)
        {
            var existingTroubleMachine =
                _troubleMachineMonitoringRepository.Find(o => o.Identity.Equals(request.Id))
                                   .FirstOrDefault();

            existingTroubleMachine.SetContinueDate(request.ContinueDate);
            existingTroubleMachine.SetStopDate(request.StopDate);
            existingTroubleMachine.SetDescription(request.Description);
            existingTroubleMachine.SetMachineId(new MachineId(Guid.Parse(request.MachineNumber)));
            existingTroubleMachine.SetOrderId(new OrderId(Guid.Parse(request.OrderDocument)));
            existingTroubleMachine.SetOperatorId(new OperatorId(Guid.Parse(request.OperatorDocument)));
            existingTroubleMachine.SetProcess(request.Process);
            existingTroubleMachine.SetTrouble(request.Trouble);
            existingTroubleMachine.SetDescription(request.Description);

            await _troubleMachineMonitoringRepository.Update(existingTroubleMachine);
            _storage.Save();

            return existingTroubleMachine;
        }
    }
}
