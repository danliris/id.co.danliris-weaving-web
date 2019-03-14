using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Suppliers.Commands
{
    public class RemoveSupplierCommand : ICommand<WeavingSupplierDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        public void SetId(Guid Id) { this.Id = Id; }
    }
}
