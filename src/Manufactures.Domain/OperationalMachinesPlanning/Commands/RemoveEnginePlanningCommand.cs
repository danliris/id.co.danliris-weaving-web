using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.OperationalMachinesPlanning.Commands
{
    public class RemoveEnginePlanningCommand : ICommand<MachinesPlanningDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        public void SetId(Guid Id) { this.Id = Id; }
    }
}
