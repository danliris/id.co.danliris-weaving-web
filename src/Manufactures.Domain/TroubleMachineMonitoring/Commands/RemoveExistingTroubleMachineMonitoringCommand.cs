using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.TroubleMachineMonitoring.Commands
{
    public class RemoveExistingTroubleMachineMonitoringCommand : ICommand<TroubleMachineMonitoringDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
