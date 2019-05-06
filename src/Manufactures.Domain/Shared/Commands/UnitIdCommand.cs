using Newtonsoft.Json;

namespace Manufactures.Domain.Shared.Commands
{
    public class UnitIdCommand
    {
        [JsonProperty(propertyName: "Id")]
        public int Id { get; set; }
    }
}
