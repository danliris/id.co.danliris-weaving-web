using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class YarnId : SingleValueObject<Guid>
    {
        public YarnId(Guid id) : base(id) { }
    }
}
