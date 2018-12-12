using Moonlay.Domain;
using Newtonsoft.Json;

namespace Weaving.Domain.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class MaterialId : SingleValueObject<int>
    {
        public MaterialId(int value) : base(value)
        {
        }
    }
}
