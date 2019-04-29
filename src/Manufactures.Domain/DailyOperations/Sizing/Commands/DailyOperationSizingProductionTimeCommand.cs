using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingProductionTimeCommand
    {
        [JsonProperty(PropertyName = "Start")]
        public DateTimeOffset Start { get; set; }

        [JsonProperty(PropertyName = "Pause")]
        public DateTimeOffset Pause { get; set; }

        [JsonProperty(PropertyName = "Resume")]
        public DateTimeOffset Resume { get; set; }

        [JsonProperty(PropertyName = "Doff")]
        public DateTimeOffset Doff { get; set; }
    }
}
