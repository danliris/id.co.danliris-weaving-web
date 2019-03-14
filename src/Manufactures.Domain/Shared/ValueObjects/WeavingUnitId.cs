using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class WeavingUnitId : SingleValueObject<Guid>
    {
        public WeavingUnitId(Guid id) : base(id) { }
    }
}
