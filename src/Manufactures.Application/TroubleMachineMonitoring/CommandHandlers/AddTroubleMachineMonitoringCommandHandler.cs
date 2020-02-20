using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.TroubleMachineMonitoring;
using Manufactures.Domain.TroubleMachineMonitoring.Commands;
using Manufactures.Domain.TroubleMachineMonitoring.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.TroubleMachineMonitoring.CommandHandlers
{
    public class AddTroubleMachineMonitoringCommandHandler
        : ICommandHandler<AddTroubleMachineMonitoringCommand, TroubleMachineMonitoringDocument>
    {
        private readonly IStorage _storage;
        private readonly ITroubleMachineMonitoringRepository
            _troubleMachineMonitoringRepository;

        public AddTroubleMachineMonitoringCommandHandler(IStorage storage)
        {
            _storage = storage;
            _troubleMachineMonitoringRepository =
                _storage.GetRepository<ITroubleMachineMonitoringRepository>();
        }

        public async Task<TroubleMachineMonitoringDocument> Handle(AddTroubleMachineMonitoringCommand request, CancellationToken cancellationToken)
        {
            //Reformat Date Time
            var continueYear = request.ContinueDate.Year;
            var continueMonth = request.ContinueDate.Month;
            var continueDay = request.ContinueDate.Day;
            var continueHour = request.ContinueDate.Hour;
            var continueMinutes = request.ContinueDate.Minute;
            var continueSeconds = request.ContinueDate.Second;
            var contiuneDateTime =
                new DateTimeOffset(continueYear, continueMonth, continueDay, continueHour, continueMinutes, continueSeconds, new TimeSpan(+7, 0, 0));

            var stopYear = request.StopDate.Year;
            var stopMonth = request.StopDate.Month;
            var stopDay = request.StopDate.Day;
            var stopHour = request.StopDate.Hour;
            var stopMinutes = request.StopDate.Minute;
            var stopSeconds = request.StopDate.Second;
            var stopDateTime =
                new DateTimeOffset(stopYear, stopMonth, stopDay, stopHour, stopMinutes, stopSeconds, new TimeSpan(+7, 0, 0));

            Guid operatorGuid = Guid.Parse(request.OperatorDocument);
            Guid orderDocument = Guid.Parse(request.OrderDocument);
            Guid machineNumber = Guid.Parse(request.MachineNumber);

            //Instantiate Trouble Machine
            var troubleMachine = new TroubleMachineMonitoringDocument(Guid.NewGuid(),
                                                              contiuneDateTime,
                                                              stopDateTime,
                                                              new OrderId (orderDocument),
                                                              request.Process,
                                                              new MachineId (machineNumber),
                                                              new OperatorId (operatorGuid),
                                                              request.Trouble,
                                                              request.Description);

            //Update and Save
            await _troubleMachineMonitoringRepository.Update(troubleMachine);
            _storage.Save();

            //return as object 
            return troubleMachine;
        }
    }
}
