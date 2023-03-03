using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class DailyOperationSizingId : SingleValueObject<Guid>
    {
        public DailyOperationSizingId(Guid id) : base(id) { }
    }
}
