using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class MaterialTypeId : SingleValueObject<Guid>
    {
        public MaterialTypeId(Guid id) : base(id) { }

    }
}
