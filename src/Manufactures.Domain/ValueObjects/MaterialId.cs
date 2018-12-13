using Moonlay.Domain;
using Newtonsoft.Json;

namespace Manufactures.Domain.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class MaterialId : SingleValueObject<int>
    {
        public MaterialId(int value) : base(value)
        {
        }
    }
}
