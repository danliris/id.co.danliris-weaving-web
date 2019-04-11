using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class DailyOperationalMachineDocumentValueObject : ValueObject
    {
        //Self Properties (Document)
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DateOperated")]
        public DateTimeOffset DateOperated { get; private set; }

        [JsonProperty(PropertyName = "MachineId")]
        public string MachineId { get; private set; }

        [JsonProperty(PropertyName = "UnitId")]
        public int UnitId { get; private set; }

        [JsonProperty(PropertyName = "Status")]
        public string Status { get; private set; }

        public IReadOnlyCollection<DailyOperationalMachineDetailsValueObject> DailyOperationMachineDetails { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DateOperated;
            yield return MachineId;
            yield return UnitId;
            yield return Status;
            yield return DailyOperationMachineDetails;
        }
    }
}
