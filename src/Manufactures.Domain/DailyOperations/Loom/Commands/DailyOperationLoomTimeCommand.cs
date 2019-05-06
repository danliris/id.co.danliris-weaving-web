using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class DailyOperationLoomTimeCommand
    {
        [JsonProperty(PropertyName = "Pause")]
        public DateTimeOffset Pause { get; set; }

        [JsonProperty(PropertyName = "Resume")]
        public DateTimeOffset Resume { get; set; }

        [JsonProperty(PropertyName = "Difference")]
        public DateTimeOffset Difference { get; set; }
    }
}
