using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Construction.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class MaterialType : SingleValueObject<Guid>
    {
        public MaterialType(Guid identity) : base(identity) { }
    }
}
