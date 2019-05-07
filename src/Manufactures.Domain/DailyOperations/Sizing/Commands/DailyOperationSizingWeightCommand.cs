using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingWeightCommand
    {
        [JsonProperty(PropertyName = "Netto")]
        public string Netto { get; set; }

        [JsonProperty(PropertyName = "Bruto")]
        public string Bruto { get; set; }
    }
}
