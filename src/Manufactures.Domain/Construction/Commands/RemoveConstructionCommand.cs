using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Construction.Commands
{
    public class RemoveConstructionCommand : ICommand<ConstructionDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
