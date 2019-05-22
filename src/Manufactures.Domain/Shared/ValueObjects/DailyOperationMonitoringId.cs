using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class DailyOperationMonitoringId : SingleValueObject<Guid>
    {
        public DailyOperationMonitoringId(Guid id) : base(id) { }
    }
}
