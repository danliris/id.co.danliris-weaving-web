using Moonlay.Domain;
using Newtonsoft.Json;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class UserId : SingleValueObject<int>
    {
        public UserId(int id) : base(id) { }
    }
}
