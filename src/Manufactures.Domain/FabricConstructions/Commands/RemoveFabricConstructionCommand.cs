using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.FabricConstructions.Commands
{
    public class RemoveFabricConstructionCommand : ICommand<FabricConstructionDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
