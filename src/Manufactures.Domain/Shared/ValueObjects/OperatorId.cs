using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class OperatorId : SingleValueObject<Guid>
    {
        public OperatorId(Guid id) : base(id) { }
    }
}
