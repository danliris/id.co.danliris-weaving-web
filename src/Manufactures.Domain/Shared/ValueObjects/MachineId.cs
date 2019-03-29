using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class MachineId : SingleValueObject<Guid>
    {
        public MachineId(Guid id) : base(id) { }
    }
}
