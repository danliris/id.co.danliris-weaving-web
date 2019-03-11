using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class ConstructionId : SingleValueObject<Guid>
    {
        public ConstructionId(Guid id) : base(id) { }
    }
}
