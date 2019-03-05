using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Machines.Commands
{
    public class RemoveExistingMachineCommand : ICommand<MachineDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
