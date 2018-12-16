using Moonlay.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Domain.Orders.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class MaterialId : SingleValueObject<int>
    {
        public MaterialId(int value) : base(value)
        {
        }
    }

    public class MaterialIds : ListX<MaterialId>
    {
        public MaterialIds(IEnumerable<MaterialId> collection) : base(collection)
        {
        }
    }
}