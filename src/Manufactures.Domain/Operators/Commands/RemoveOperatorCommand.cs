using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Operators.Commands
{
    public class RemoveOperatorCommand : ICommand<OperatorDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
