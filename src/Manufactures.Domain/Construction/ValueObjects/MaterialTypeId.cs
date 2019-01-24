using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Construction.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class MaterialTypeId : SingleValueObject<Guid>
    {
        public MaterialTypeId(Guid value) : base(value)
        {
        }
    }
}
