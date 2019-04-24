using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class BeamId : SingleValueObject<Guid>
    {
        public BeamId(Guid id) : base(id) { }
    }
}
