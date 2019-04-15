using Newtonsoft.Json;

namespace Manufactures.Domain.Shared.Commands
{
    public class CoreAccountCommand
    {
        [JsonProperty(propertyName: "MongoId")]
        public string MongoId { get; private set; }

        [JsonProperty(propertyName: "Id")]
        public int? Id { get; private set; }
    }
}
