using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Productions.Commands
{
    public class RemoveDailyOperationMachineSizingCommand : ICommand<DailyOperationMachineSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
