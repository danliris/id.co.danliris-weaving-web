using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class YarnNumberId : SingleValueObject<Guid>
    {
        public YarnNumberId(Guid id) : base(id) { }
    }
}
