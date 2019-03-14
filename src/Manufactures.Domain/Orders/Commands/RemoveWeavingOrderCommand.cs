using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Orders.Commands
{
    public class RemoveWeavingOrderCommand : ICommand<WeavingOrderDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
