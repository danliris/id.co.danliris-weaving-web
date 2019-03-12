using Newtonsoft.Json;

namespace Manufactures.Domain.Orders.Commands
{
    public class FabricConstructionCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; set; }
    }
}
