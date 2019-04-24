using Manufactures.Domain.DailyOperations.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Commands
{
    public class DailyOperationMachineTimeCommand
    {
        [JsonProperty(PropertyName = "Pause")]
        public DateTimeOffset Pause { get; set; }

        [JsonProperty(PropertyName = "Resume")]
        public DateTimeOffset Resume { get; set; }

        [JsonProperty(PropertyName = "Difference")]
        public DateTimeOffset Difference { get; set; }
    }
}
