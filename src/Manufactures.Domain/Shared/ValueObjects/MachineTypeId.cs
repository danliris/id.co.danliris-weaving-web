using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class MachineTypeId : SingleValueObject<Guid>
    {
        public MachineTypeId(Guid id): base(id) { }
    }
}
