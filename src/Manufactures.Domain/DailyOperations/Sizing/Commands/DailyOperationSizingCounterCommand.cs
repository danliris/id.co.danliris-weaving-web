using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingCounterCommand
    {
        [JsonProperty(PropertyName = "Start")]
        public string Start { get; set; }

        [JsonProperty(PropertyName = "Finish")]
        public string Finish { get; set; }
    }
}
