using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class ShiftId : SingleValueObject<Guid>
    {
        public ShiftId(Guid id) : base(id) { }
    }
}
