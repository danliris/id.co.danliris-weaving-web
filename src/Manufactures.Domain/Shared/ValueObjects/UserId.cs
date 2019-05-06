using Moonlay.Domain;
using Newtonsoft.Json;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class UserId : SingleValueObject<string>
    {
        public UserId(string id) : base(id) { }
    }
}
