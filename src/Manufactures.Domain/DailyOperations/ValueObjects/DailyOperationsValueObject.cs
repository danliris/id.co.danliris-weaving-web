using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class DailyOperationsValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "DateOperated")]
        public DateTimeOffset DateOperated { get; private set; }

        [JsonProperty(PropertyName = "MachineId")]
        public string MachineId { get; private set; }

        [JsonProperty(PropertyName = "UnitId")]
        public int UnitId { get; private set; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; private set; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; private set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        [JsonProperty(PropertyName = "Shift")]
        public string Shift { get; private set; }

        [JsonProperty(PropertyName = "DOMTime")]
        public DOMTimeValueObject DOMTime { get; private set; }

        [JsonProperty(PropertyName = "BeamOperator")]
        public string BeamOperator { get; private set; }

        [JsonProperty(PropertyName = "LoomGroup")]
        public string LoomGroup { get; private set; }

        [JsonProperty(PropertyName = "SizingNumber")]
        public string SizingNumber { get; private set; }

        [JsonProperty(PropertyName = "SizingOperator")]
        public string SizingOperator { get; private set; }

        [JsonProperty(PropertyName = "SizingGroup")]
        public string SizingGroup { get; private set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; private set; }

        public DailyOperationsValueObject(DateTimeOffset dateOperated, string machineId, int unitId, string orderNumber, string warpOrigin, string weftOrigin, string constructionNumber, string shift, DOMTimeValueObject dOMTime, string beamOperator, string loomGroup, string sizingNumber, string sizingOperator, string sizingGroup, string information)
        {
            DateOperated = dateOperated;
            MachineId = machineId;
            UnitId = unitId;
            OrderNumber = orderNumber;
            WarpOrigin = warpOrigin;
            WeftOrigin = weftOrigin;
            ConstructionNumber = constructionNumber;
            Shift = shift;
            DOMTime = dOMTime;
            BeamOperator = beamOperator;
            LoomGroup = loomGroup;
            SizingNumber = sizingNumber;
            SizingOperator = sizingOperator;
            SizingGroup = sizingGroup;
            Information = information;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DateOperated;
            yield return MachineId;
            yield return UnitId;
            yield return OrderNumber;
            yield return WarpOrigin;
            yield return WeftOrigin;
            yield return ConstructionNumber;
            yield return Shift;
            yield return DOMTime;
            yield return BeamOperator;
            yield return LoomGroup;
            yield return SizingNumber;
            yield return SizingOperator;
            yield return SizingGroup;
            yield return Information;
        }
    }
}
