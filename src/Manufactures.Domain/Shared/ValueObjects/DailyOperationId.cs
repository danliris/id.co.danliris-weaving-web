using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class DailyOperationId : SingleValueObject<Guid>
    {
        public DailyOperationId(Guid id) : base(id) { }
    }
}
