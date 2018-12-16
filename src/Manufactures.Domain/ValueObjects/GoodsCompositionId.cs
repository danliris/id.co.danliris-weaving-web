using Moonlay.Domain;
using Newtonsoft.Json;

namespace Manufactures.Domain.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class GoodsCompositionId : SingleValueObject<string>
    {
        public GoodsCompositionId(string value) : base(value)
        {
        }
    }
}