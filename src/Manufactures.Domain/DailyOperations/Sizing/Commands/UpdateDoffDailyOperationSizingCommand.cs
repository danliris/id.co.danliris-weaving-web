using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateDoffDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationSizingDetails")]
        public UpdateDoffDailyOperationSizingDetailCommand DailyOperationSizingDetails { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
